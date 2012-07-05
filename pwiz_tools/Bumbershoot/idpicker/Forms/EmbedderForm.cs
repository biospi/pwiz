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
// Copyright 2012 Vanderbilt University
//
// Contributor(s): 
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using NHibernate.Linq;
using IDPicker.DataModel;
using pwiz.CLI.util;

namespace IDPicker.Forms
{
    public partial class EmbedderForm : Form
    {
        private NHibernate.ISession session;
        private bool hasEmbeddedSources, hasNonEmbeddedSources;

        public EmbedderForm (NHibernate.ISession session)
        {
            this.session = session.SessionFactory.OpenSession();

            InitializeComponent();
        }

        protected override void OnLoad (EventArgs e)
        {
            searchPathTextBox.Text = String.Join(";", Util.StringCollectionToStringArray(Properties.Settings.Default.SourcePaths));
            extensionsTextBox.Text = Properties.Settings.Default.SourceExtensions;

            base.OnLoad(e);

            Refresh();
        }

        public override void Refresh ()
        {
            Text = "Embed Subset Spectra";
            Application.UseWaitCursor = false;

            dataGridView.SuspendLayout();
            dataGridView.Rows.Clear();

            var rows = session.CreateSQLQuery("SELECT Name, COUNT(s.Id), IFNULL((SELECT LENGTH(MSDataBytes) FROM SpectrumSource WHERE Id=ss.Id), 0), MAX(s.ScanTimeInSeconds) " +
                                              "FROM SpectrumSource ss " +
                                              "JOIN UnfilteredSpectrum s ON ss.Id=Source " +
                                              "GROUP BY ss.Id")
                              .List<object[]>()
                              .Select(o => new { Name = (string) o[0], Spectra = Convert.ToInt32(o[1]), EmbeddedSize = Convert.ToInt32(o[2]), MaxScanTime = Convert.ToDouble(o[3]) });

            hasEmbeddedSources = hasNonEmbeddedSources = false;

            foreach (var row in rows)
                if (row.EmbeddedSize > 0)
                {
                    dataGridView.Rows.Add(row.Name, String.Format("{0} spectra embedded ({1} bytes)", row.Spectra, row.EmbeddedSize));
                    hasEmbeddedSources = true;
                }
                else if (row.MaxScanTime > 0)
                {
                    dataGridView.Rows.Add(row.Name, String.Format("{0} spectra with scan times", row.Spectra));
                    hasNonEmbeddedSources = true;
                }
                else
                {
                    dataGridView.Rows.Add(row.Name, "not embedded");
                    hasNonEmbeddedSources = true;
                }

            dataGridView.ResumeLayout();

            deleteAllButton.Enabled = hasEmbeddedSources;
            embedAllButton.Enabled = hasNonEmbeddedSources;
            okButton.Enabled = true;

            if (TaskbarManager.IsPlatformSupported)
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);

            base.Refresh();
        }

        protected override void OnClosing (CancelEventArgs e)
        {
            if (Application.UseWaitCursor)
                e.Cancel = true;
            else
                session.Dispose();

            base.OnClosing(e);
        }

        private class EmbedderIterationListener : IterationListener
        {
            Form form;
            public EmbedderIterationListener (Form form) { this.form = form; }

            public override Status update (UpdateMessage updateMessage)
            {
                var title = new StringBuilder(updateMessage.message);
                title[0] = Char.ToUpper(title[0]);
                title.AppendFormat(" ({0}/{1})", updateMessage.iterationIndex + 1, updateMessage.iterationCount);
                form.BeginInvoke(new MethodInvoker(() => form.Text = title.ToString()));

                if (TaskbarManager.IsPlatformSupported)
                {
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                    TaskbarManager.Instance.SetProgressValue(updateMessage.iterationIndex + 1, updateMessage.iterationCount);
                }

                return IterationListener.Status.Ok;

                // TODO: support cancel
            }
        }

        private void embedAllButton_Click (object sender, EventArgs e)
        {
            var searchPath = new StringBuilder(searchPathTextBox.Text);
            string extensions = extensionsTextBox.Text;
            Application.UseWaitCursor = true;
            deleteAllButton.Enabled = embedAllButton.Enabled = okButton.Enabled = false;

            try
            {
                // add location of original idpDBs to the search path
                var mergedFilepaths = session.CreateSQLQuery("SELECT DISTINCT Filepath FROM MergedFiles").List<string>();
                foreach (var filepath in mergedFilepaths)
                    searchPath.AppendFormat(";{0}", System.IO.Path.GetDirectoryName(filepath));
            }
            catch
            {
                // ignore if MergedFiles does not exist
            }

            new Thread(() =>
            {
                try
                {
                    var ilr = new IterationListenerRegistry();
                    ilr.addListener(new EmbedderIterationListener(this), 1);

                    string idpDbFilepath = session.Connection.GetDataSource();
                    if (scanTimeOnlyCheckBox.Checked)
                        Embedder.EmbedScanTime(idpDbFilepath, searchPath.ToString(), extensions, ilr);
                    else
                        Embedder.Embed(idpDbFilepath, searchPath.ToString(), extensions, ilr);
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("no filepath"))
                        Program.HandleException(ex);

                    bool multipleMissingFilepaths = ex.Message.Contains("\n");
                    string missingFilepaths = ex.Message.Replace("[embed] no", "No").Replace("\n", "\r\n");
                    MessageBox.Show(missingFilepaths + "\r\n\r\nCheck that " +
                                    (multipleMissingFilepaths ? "these source files" : "this source file") +
                                    " can be found in the search path with one of the specified extensions.");
                }
                BeginInvoke(new MethodInvoker(() => Refresh()));
            }).Start();
        }

        private void deleteAllButton_Click (object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all embedded spectra?", "Confirm",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Exclamation,
                                MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            Text = "Deleting embedded spectra (this could take a few minutes)";
            Application.UseWaitCursor = true;
            deleteAllButton.Enabled = embedAllButton.Enabled = okButton.Enabled = false;

            if (TaskbarManager.IsPlatformSupported)
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);

            new Thread(() =>
            {
                try
                {
                    var tls = session.SessionFactory.OpenStatelessSession();
                    tls.CreateSQLQuery("UPDATE SpectrumSource SET MSDataBytes = NULL; VACUUM").ExecuteUpdate();
                }
                catch (Exception ex)
                {
                    Program.HandleException(ex);
                }
                BeginInvoke(new MethodInvoker(() => Refresh()));
            }).Start();
        }

        private void okButton_Click (object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
