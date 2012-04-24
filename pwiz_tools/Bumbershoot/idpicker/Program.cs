﻿//
// $Id$
//
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
//
// The Original Code is the IDPicker project.
//
// The Initial Developer of the Original Code is Matt Chambers.
//
// Copyright 2010 Vanderbilt University
//
// Contributor(s): Surendra Dasari
//

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using IDPicker.Forms;
using SharpSvn;

namespace IDPicker
{
    static class Program
    {
        public static bool IsHeadless { get; private set; }
        public static IDPickerForm MainWindow { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main (string[] args)
        {
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(UIThread_UnhandledException);

            // Set the unhandled exception mode to force all Windows Forms errors to go through
            // our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            var singleInstanceHandler = new SingleInstanceHandler(Application.ExecutablePath) { Timeout = 200 };
            singleInstanceHandler.Launching += (sender, e) =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var singleInstanceArgs = e.Args.ToList();
                IsHeadless = singleInstanceArgs.Contains("--headless");
                if (IsHeadless)
                    singleInstanceArgs.Remove("--headless");

                // initialize webClient asynchronously
                initializeWebClient();

                automaticCheckForUpdates();

                //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

                MainWindow = new IDPickerForm(singleInstanceArgs);
                Application.Run(MainWindow);
            };

            try
            {
                singleInstanceHandler.Connect(args);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        #region Exception handling
        public static void HandleUserError (Exception e)
        {
            if (IsHeadless)
            {
                Console.Error.WriteLine("Error: {0}\r\n\r\nDetails:\r\n{1}", e.Message, e.ToString());
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            if (MainWindow == null)
            {
                MessageBox.Show(e.ToString(), "Error");
                return;
            }

            if (MainWindow.InvokeRequired)
            {
                MainWindow.Invoke(new MethodInvoker(() => HandleUserError(e)));
                return;
            }

            using (var userErrorForm = new UserErrorForm(e.Message))
            {
                userErrorForm.StartPosition = FormStartPosition.CenterParent;
                userErrorForm.ShowDialog(MainWindow);
            }
        }

        public static void HandleException (Exception e)
        {
            if (MainWindow == null)
            {
                if (IsHeadless)
                {
                    Console.Error.WriteLine("Error: {0}\r\n\r\nDetails:\r\n{1}", e.Message, e.ToString());
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                else
                    MessageBox.Show(e.ToString(), "Error");
                return;
            }

            if (MainWindow.InvokeRequired)
            {
                MainWindow.Invoke(new MethodInvoker(() => HandleException(e)));
                return;
            }

            // for certain exception types, the InnerException is a better representative of the real error
            if (e is NHibernate.ADOException &&
                e.InnerException != null)
                e = e.InnerException;

            using (var reportForm = new ReportErrorDlg(e, ReportErrorDlg.ReportChoice.choice))
            {
                reportForm.StartPosition = FormStartPosition.CenterParent;

                if (IsHeadless)
                {
                    Console.Error.WriteLine("Error: {0}\r\n\r\nDetails:\r\n{1}", reportForm.ExceptionType, reportForm.MessageBody);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }

                if (MainWindow.IsDisposed)
                {
                    if (reportForm.ShowDialog() == DialogResult.OK)
                        SendErrorReport(reportForm.MessageBody, reportForm.ExceptionType, reportForm.Email, reportForm.Username);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }

                if (reportForm.ShowDialog(MainWindow) == DialogResult.OK)
                    SendErrorReport(reportForm.MessageBody, reportForm.ExceptionType, reportForm.Email, reportForm.Username);
                if (reportForm.ForceClose)
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private static void UIThread_UnhandledException (object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }
        #endregion

        #region Update checking and error reporting
        private static WebClient webClient = new WebClient();
        private static void initializeWebClient ()
        {
            new Thread(() => { try { lock (webClient) webClient.DownloadString("http://www.google.com"); } catch {/* TODO: log warning */} }).Start();
        }

        private static void automaticCheckForUpdates ()
        {
            if (!Properties.GUI.Settings.Default.AutomaticCheckForUpdates)
                return;

            var timeSinceLastCheckForUpdates = DateTime.UtcNow - Properties.GUI.Settings.Default.LastCheckForUpdates;
            if (timeSinceLastCheckForUpdates.TotalDays < 1)
                return;

            // ignore development builds
            if (Application.ExecutablePath.Contains("build-nt-x86"))
                return;

            new Thread(() => { try { CheckForUpdates(); } catch {/* TODO: log warning */} }).Start();
        }

        /// <summary>
        /// Filters out log lines that do not start with '-'
        /// </summary>
        private static string filterRevisionLog (string log)
        {
            var lines = log.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return String.Join("\r\n", lines.Where(o=> o.TrimStart().StartsWith("-")).Select(o=> o.TrimEnd('\r')).ToArray());
        }

        public static bool CheckForUpdates ()
        {
            Properties.GUI.Settings.Default.LastCheckForUpdates = DateTime.UtcNow;
            Properties.GUI.Settings.Default.Save();

            string teamcityURL = "http://teamcity.fenchurch.mc.vanderbilt.edu";
            string buildsURL = teamcityURL + "/httpAuth/app/rest/buildTypes/id:bt31/builds?status=SUCCESS&count=1&guest=1";
            string latestArtifactURL;
            string versionArtifactFormatURL = teamcityURL + "/repository/download/bt31/{0}:id/VERSION?guest=1";

            Version latestVersion;

            lock (webClient)
            {
                string xml = webClient.DownloadString(buildsURL);
                int startIndex = xml.IndexOf("id=");
                if (startIndex < 0) throw new InvalidDataException("build id not found in:\r\n" + xml);
                int endIndex = xml.IndexOfAny("\"'".ToCharArray(), startIndex + 4);
                if (endIndex < 0) throw new InvalidDataException("not well formed xml:\r\n" + xml);
                startIndex += 4; // skip the attribute name, equals, and opening quote
                string buildId = xml.Substring(startIndex, endIndex - startIndex);

                latestArtifactURL = String.Format("{0}/repository/download/bt31/{1}:id", teamcityURL, buildId);
                latestVersion = new Version(webClient.DownloadString(latestArtifactURL + "/VERSION?guest=1"));
            }

            Version currentVersion = new Version(Util.Version);

            if (currentVersion < latestVersion)
            {
                System.Collections.ObjectModel.Collection<SvnLogEventArgs> logItems = null;

                using (var client = new SvnClient())
                {
                    try
                    {
                        client.GetLog(new Uri("http://forge.fenchurch.mc.vanderbilt.edu/svn/idpicker/branches/IDPicker-3/"),
                                      new SvnLogArgs(new SvnRevisionRange(currentVersion.Build, latestVersion.Build)),
                                      out logItems);
                    }
                    catch
                    {
                    }
                }

                IEnumerable<SvnLogEventArgs> filteredLogItems = logItems;

                string changeLog;
                if (logItems.IsNullOrEmpty())
                    changeLog = "<unable to get change log>";
                else
                {
                    // return if no important revisions have happened
                    filteredLogItems = logItems.Where(o => !filterRevisionLog(o.LogMessage).Trim().IsNullOrEmpty());
                    if (filteredLogItems.IsNullOrEmpty())
                        return false;

                    var logEntries = filteredLogItems.Select(o => String.Format("Revision {0}:\r\n{1}",
                                                                                o.Revision,
                                                                                filterRevisionLog(o.LogMessage)));
                    changeLog = String.Join("\r\n\r\n", logEntries.ToArray());
                }

                var form = new NewVersionForm(Application.ProductName,
                                              currentVersion.ToString(),
                                              latestVersion.ToString(),
                                              changeLog)
                                              {
                                                  Owner = MainWindow,
                                                  StartPosition = FormStartPosition.CenterParent
                                              };

                if (form.ShowDialog() == DialogResult.Yes)
                {
                    string installerURL = String.Format("{0}/IDPicker-{1}.msi?guest=1", latestArtifactURL, latestVersion);
                    System.Diagnostics.Process.Start(installerURL);
                }
                return true;
            }
            return false;
        }

        private static void SendErrorReport (string messageBody, string exceptionType, string email, string username)
        {
            const string address = "http://forge.fenchurch.mc.vanderbilt.edu/tracker/index.php?func=add&group_id=10&atid=149";

            lock (webClient)
            {
                string html = webClient.DownloadString(address);
                Match m = Regex.Match(html, "name=\"form_key\" value=\"(?<key>\\S+)\"");
                if (!m.Groups["key"].Success)
                {
                    MessageBox.Show("Unable to find form_key for exception tracker.", "Error submitting report",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                exceptionType = exceptionType.Replace("System.", "");
                string errorMessage = Regex.Match(messageBody, "Error message: (.+?)\\r").Groups[1].Value;
                errorMessage = errorMessage.Length > 60 ? errorMessage.Substring(0, 60) + "..." : errorMessage;
                username = String.IsNullOrEmpty(username) ? "unknown" : username;

                NameValueCollection form = new NameValueCollection
                                               {
                                                   {"form_key", m.Groups["key"].Value},
                                                   {"func", "postadd"},
                                                   {"summary", exceptionType + " (User: " + username + "; Message: " + errorMessage + ")"},
                                                   {"details", messageBody},
                                                   {"user_email", email},
                                               };

                webClient.UploadValues(address, form);
            }
        }
        #endregion
    }
}
