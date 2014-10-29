﻿/*
 * Original author: Don Marsh <donmarsh .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2014 University of Washington - Seattle, WA
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Diagnostics;

namespace SkylineTool
{
    public interface ITestTool
    {
        void TestSelect(string link);
        void TestSelectReplicate(string link);
        string TestVersion();
        string TestDocumentPath();
        string GetDocumentChangeCount();
        string Quit();
    }

    public class TestToolClient : RemoteClient, ITestTool
    {
        public TestToolClient(string connectionName)
            : base(connectionName)
        {
        }

        public void Exit()
        {
            var processId = int.Parse(Quit());
            try
            {
                // Wait for tool to exit.  If it has already quit, this
                // will cause a harmless exception.
                Process.GetProcessById(processId).WaitForExit();
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch (ArgumentException)
            {
            }
        }

        public void TestSelect(string link)
        {
            RemoteCall(TestSelect, link);
        }

        public void TestSelectReplicate(string link)
        {
            RemoteCall(TestSelectReplicate, link);
        }

        public string TestVersion()
        {
            return (string) RemoteCallFunction(TestVersion);
        }

        public string TestDocumentPath()
        {
            return (string) RemoteCallFunction(TestDocumentPath);
        }

        public string GetDocumentChangeCount()
        {
            return (string) RemoteCallFunction(GetDocumentChangeCount);
        }

        public string Quit()
        {
            return (string) RemoteCallFunction(Quit);
        }
    }
}
