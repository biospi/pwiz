/*
 * Original author: Brendan MacLean <brendanx .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2009 University of Washington - Seattle, WA
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
using System.Collections.Generic;
using System.Windows.Forms;
using DigitalRune.Windows.Docking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pwiz.Skyline.Controls.Graphs;
using pwiz.Skyline.EditUI;
using pwiz.Skyline.FileUI;
using pwiz.Skyline.Model;
using pwiz.Skyline.Model.Results;
using pwiz.Skyline.Util;

namespace pwiz.SkylineTestFunctional
{
    /// <summary>
    /// Functional test for CE Optimization.
    /// </summary>
    [TestClass]
    public class ManageResultsTest : AbstractFunctionalTest
    {
        [TestMethod]
        public void TestManageResults()
        {
            TestFilesZip = @"TestFunctional\ManageResultsTest.zip";
            RunFunctionalTest();
        }

        /// <summary>
        /// Test CE optimization.  Creates optimization transition lists,
        /// imports optimization data, shows graphs, recalculates linear equations,
        /// and exports optimized method.
        /// </summary>
        protected override void DoTest()
        {
            // Open the .sky file
            string documentPath = TestFilesDir.GetTestPath("160109_Mix1_calcurve.sky");
            RunUI(() => SkylineWindow.OpenFile(documentPath));

            var listGraphChroms = new List<GraphChromatogram>(SkylineWindow.GraphChromatograms);
            Assert.AreEqual(4, listGraphChroms.Count);
            var dictGraphPositions = new Dictionary<Point, GraphChromatogram>();
            var dictChromPositions = new Dictionary<Point, int>();
            var docLoading = SkylineWindow.Document;
            foreach (var graphChrom in listGraphChroms)
            {
                // Graphs should all be showing in panes
                Point ptLeftTop = GetTopLeft(graphChrom.Parent);
                Assert.IsFalse(dictGraphPositions.ContainsKey(ptLeftTop));
                Assert.IsTrue(graphChrom.Visible);
                Assert.IsTrue(graphChrom.VisibleState == DockState.Document);

                dictGraphPositions.Add(ptLeftTop, graphChrom);

                int index;
                ChromatogramSet chromSet;
                Assert.IsTrue(docLoading.Settings.MeasuredResults.TryGetChromatogramSet(
                    graphChrom.NameSet, out chromSet, out index));
                dictChromPositions.Add(ptLeftTop, index);
            }
            WaitForConditionUI(() => SkylineWindow.DocumentUI.Settings.MeasuredResults.IsLoaded);

            var docOrig = SkylineWindow.Document;   // 70, 73, 75, 78

            RunDlg<ManageResultsDlg>(SkylineWindow.ManageResults, dlg =>
                {
                    dlg.MoveDown();
                    dlg.MoveDown();
                    dlg.OkDialog();
                });

            CheckResultsEquivalent(docOrig, false);

            var docMove1 = SkylineWindow.Document;  // 73, 75, 70, 78

            // Make sure the moved chromatogram set ended up in the right place.
            Assert.AreSame(docOrig.Settings.MeasuredResults.Chromatograms[0],
                docMove1.Settings.MeasuredResults.Chromatograms[2]);

            RunDlg<ManageResultsDlg>(SkylineWindow.ManageResults, dlg =>
            {
                var chromatograms = docMove1.Settings.MeasuredResults.Chromatograms;
                dlg.SelectedChromatograms = new[] {chromatograms[1], chromatograms[3]};
                dlg.MoveUp();
                dlg.OkDialog();
            });

            CheckResultsEquivalent(docMove1, false);

            var docMove2 = SkylineWindow.Document;  // 75, 78, 73, 70

            // Make sure the moved chromatogram sets ended up in the right place.
            Assert.AreSame(docMove1.Settings.MeasuredResults.Chromatograms[1],
                docMove2.Settings.MeasuredResults.Chromatograms[0]);
            Assert.AreSame(docMove1.Settings.MeasuredResults.Chromatograms[3],
                docMove2.Settings.MeasuredResults.Chromatograms[1]);

            // Rename a chromatogram set
            var manageResultsDlg = ShowDialog<ManageResultsDlg>(SkylineWindow.ManageResults);
            RunUI(() =>
            {
                var chromatograms = docMove2.Settings.MeasuredResults.Chromatograms;
                manageResultsDlg.SelectedChromatograms = new[] { chromatograms[2] };
                manageResultsDlg.MoveUp();
            });

            var renameDlg = ShowDialog<RenameResultDlg>(manageResultsDlg.RenameResult);
            const string newName = "Test this name";
            RunUI(() =>
            {
                renameDlg.ReplicateName = newName;
                renameDlg.OkDialog();
            });
            WaitForClosedForm(renameDlg);

            RunUI(manageResultsDlg.OkDialog);
            WaitForClosedForm(manageResultsDlg);

            CheckResultsEquivalent(docMove2, true);

            // Make sure the desired rename happened
            var docRename = SkylineWindow.Document; // 75, Test this name, 78, 70
            Assert.AreEqual(newName, docRename.Settings.MeasuredResults.Chromatograms[1].Name);
            Assert.AreEqual(docMove2.Settings.MeasuredResults.Chromatograms[2].Id.GlobalIndex,
                docRename.Settings.MeasuredResults.Chromatograms[1].Id.GlobalIndex);
            
            // Make sure the tab text of one of the graphs changed
            Assert.IsTrue(SkylineWindow.GraphChromatograms.Contains(graph =>
                graph.TabText == newName && graph.NameSet == newName));
            // Graphs should not have moved
            listGraphChroms = new List<GraphChromatogram>(SkylineWindow.GraphChromatograms);
            Assert.AreEqual(4, listGraphChroms.Count);
            foreach (var graphChrom in listGraphChroms)
            {
                Point ptLeftTop = GetTopLeft(graphChrom.Parent);
                Assert.AreEqual(graphChrom.NameSet, dictGraphPositions[ptLeftTop].NameSet);
            }

            var firstGraphChrom = listGraphChroms[3];
            Point ptLeftTopFirst = GetTopLeft(firstGraphChrom.Parent);

            RunDlg<ArrangeGraphsGroupedDlg>(SkylineWindow.ArrangeGraphsGrouped, dlg =>
            {
                dlg.Groups = 4;
                dlg.GroupType = GroupGraphsType.distributed;
                dlg.GroupOrder = GroupGraphsOrder.Document;
                dlg.OkDialog();
            });
            
            WaitForConditionUI(() => !Equals(ptLeftTopFirst, GetTopLeft(firstGraphChrom.Parent)));

            // Check that graphs were rearranged correctly
            var dictGraphPositionsNew = new Dictionary<Point, GraphChromatogram>();
            listGraphChroms = new List<GraphChromatogram>(SkylineWindow.GraphChromatograms);
            foreach (var graphChrom in listGraphChroms)
            {
                int index;
                ChromatogramSet chromSet;
                Assert.IsTrue(docRename.Settings.MeasuredResults.TryGetChromatogramSet(
                    graphChrom.NameSet, out chromSet, out index));

                Point ptLeftTop = GetTopLeft(graphChrom.Parent);
                // Pane order started out in reversed document order
                Assert.AreEqual(listGraphChroms.Count - 1 - dictChromPositions[ptLeftTop], index);

                dictGraphPositionsNew.Add(ptLeftTop, graphChrom);
            }

            // Remove the last 2 replicates
            RunDlg<ManageResultsDlg>(SkylineWindow.ManageResults, dlg =>
            {
                var chromatograms = docRename.Settings.MeasuredResults.Chromatograms;
                dlg.SelectedChromatograms = new[] { chromatograms[2], chromatograms[3] };
                dlg.Remove();
                dlg.OkDialog();
            });

            CheckResultsEquivalent(docRename, false);

            var docRemoved = SkylineWindow.Document;
            Assert.AreEqual(2, docRemoved.Settings.MeasuredResults.Chromatograms.Count);

            // First two graphs should not have moved
            listGraphChroms = new List<GraphChromatogram>(SkylineWindow.GraphChromatograms);
            Assert.AreEqual(4, listGraphChroms.Count);
            int countVisible = 0;
            foreach (var graphChrom in listGraphChroms)
            {
                if (!graphChrom.Visible)
                    continue;

                Point ptLeftTop = GetTopLeft(graphChrom.Parent);
                Assert.AreEqual(graphChrom.NameSet, dictGraphPositionsNew[ptLeftTop].NameSet);
                countVisible++;
            }
            Assert.AreEqual(2, countVisible);

            // Remove the last 2 replicates
            RunDlg<ManageResultsDlg>(SkylineWindow.ManageResults, dlg =>
            {
                dlg.RemoveAll();
                dlg.OkDialog();
            });

            // Wait for the document to be different from what it was before
            WaitForDocumentChange(docRemoved);

            var docClear = SkylineWindow.Document;

            Assert.IsFalse(docClear.Settings.HasResults);
        }

        private static Point GetTopLeft(Control control)
        {
            return new Point(control.Left, control.Top);
        }

        private static void CheckResultsEquivalent(SrmDocument docOrig, bool allowNameChanges)
        {
            // Wait for the document to be different from what it was before
            WaitForDocumentChange(docOrig);

            var docNew = SkylineWindow.Document;

            // Should have only required a single revision.  No reloading should
            // occurred.
            Assert.AreEqual(docOrig.RevisionIndex + 1, docNew.RevisionIndex);

            Assert.IsTrue(docOrig.Settings.HasResults);
            Assert.IsTrue(docNew.Settings.HasResults);

            // Fill dictionary to map from chromatogram set to its index in the
            // original document
            var resultsOrig = docOrig.Settings.MeasuredResults;
            var dictIdToIndex = new Dictionary<int, int>();
            for (int i = 0; i < resultsOrig.Chromatograms.Count; i++)
                dictIdToIndex.Add(resultsOrig.Chromatograms[i].Id.GlobalIndex, i);

            // Verify that the chromatogram sets have changed, but still have
            // equivalent entries in the original document
            var resultsNew = docNew.Settings.MeasuredResults;
            Assert.IsFalse(ArrayUtil.ReferencesEqual(resultsOrig.Chromatograms,
                resultsNew.Chromatograms));
            int countChrom = resultsNew.Chromatograms.Count;
            var arrayIndexOld = new int[countChrom];
            for (int i = 0; i < countChrom; i++)
            {
                var chromSet = resultsNew.Chromatograms[i];

                int iOld;
                Assert.IsTrue(dictIdToIndex.TryGetValue(chromSet.Id.GlobalIndex, out iOld),
                    string.Format("Missing result {0}", chromSet.Name));
                arrayIndexOld[i] = iOld;

                var chromSetOld = resultsOrig.Chromatograms[iOld];
                if (!allowNameChanges || Equals(chromSet.Name, chromSetOld.Name))
                    Assert.AreSame(chromSet, chromSetOld);
            }

            // Verify that the transition information has been moved around
            // as expected, but nothing newly created
            var enumTranOrig = docOrig.Transitions.GetEnumerator();
            foreach (var nodeTran in docNew.Transitions)
            {
                Assert.IsTrue(enumTranOrig.MoveNext());
                
                var nodeTranOrig = enumTranOrig.Current;
                Assert.AreNotSame(nodeTran, nodeTranOrig);
                Assert.AreSame(nodeTran.Id, nodeTranOrig.Id);

                // Results should have just moved, but otherwise they should be the same
                for (int i = 0; i < countChrom; i++)
                {
                    // For the most part everything should be reference equal with old values
                    if (!ArrayUtil.ReferencesEqual(nodeTran.Results[i], nodeTranOrig.Results[arrayIndexOld[i]]))
                    {
                        // But, also allow it to be reference equal with its previous value, as long
                        // as that value is content equal with the desired value.  Code in TransitionGroupDocNode
                        // may cause this, because it tries to keep new copies of chromInfo to a minimum.
                        if (!ArrayUtil.ReferencesEqual(nodeTran.Results[i], nodeTranOrig.Results[i]) ||
                            !ArrayUtil.EqualsDeep(nodeTran.Results[i], nodeTranOrig.Results[arrayIndexOld[i]]))
                        {
                            Assert.Fail("Transition chromatogram information changed.");
                        }
                    }
                }
            }

            // Make sure goup nodes have equal chromatogram info to ensure user modifications
            // are preserved.
            var enumTranGroupOrig = docOrig.TransitionGroups.GetEnumerator();
            foreach (var nodeGroup in docNew.TransitionGroups)
            {
                Assert.IsTrue(enumTranGroupOrig.MoveNext());

                var nodeGroupOrig = enumTranGroupOrig.Current;
                Assert.AreNotSame(nodeGroup, nodeGroupOrig);
                Assert.AreSame(nodeGroup.Id, nodeGroupOrig.Id);

                for (int i = 0; i < countChrom; i++)
                {
                    if (!ArrayUtil.EqualsDeep(nodeGroup.Results[i], nodeGroupOrig.Results[arrayIndexOld[i]]))
                    {
                        Assert.Fail("Transition chromatogram information changed.");
                    }
                }
            }
        }

        private struct Point
        {
            private readonly int _x;
            private readonly int _y;

            public Point(int x, int y)
            {
                _x = x;
                _y = y;
            }

            #region object overrides

            private bool Equals(Point other)
            {
                return other._x == _x && other._y == _y;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (obj.GetType() != typeof (Point)) return false;
                return Equals((Point) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_x*397) ^ _y;
                }
            }

            public override string ToString()
            {
                return string.Format("({0}, {1})", _x, _y);
            }

            #endregion
        }
    }
}