﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using pwiz.Common.Collections;
using pwiz.Common.SystemUtil;
using pwiz.Skyline.Controls.Graphs;
using pwiz.Skyline.Model.DocSettings;
using pwiz.Skyline.Properties;
using pwiz.Skyline.Util;
using pwiz.Skyline.Util.Extensions;

namespace pwiz.Skyline.Model
{

    public partial class AreaCVGraphData : Immutable
    {
        public readonly AreaCVGraphSettings _graphSettings;
        public readonly AreaCVRefinementSettings _refineSettings;
        private readonly bool _isGraphData;

        public AreaCVGraphData(SrmDocument document, AreaCVGraphSettings graphSettings = null,
            CancellationToken? token = null, AreaCVRefinementSettings refineSettings = null)
        {
            if (graphSettings != null)
            {
                _graphSettings = graphSettings;
                _refineSettings = null;
                _isGraphData = true;
            }
            else
            {
                _refineSettings = refineSettings;
                _graphSettings = null;
                _isGraphData = false;
            }

            if (document == null || !document.Settings.HasResults)
            {
                IsValid = false;
                return;
            }

            int? minDetections = null;
            if (!_isGraphData)
            {
                if (_refineSettings.MinimumDetections != -1)
                    minDetections = _refineSettings.MinimumDetections;
            }

            var annotations = new string[] { null };
            if (_isGraphData)
            {
                annotations = AnnotationHelper.GetPossibleAnnotations(document.Settings, _graphSettings.Group, AnnotationDef.AnnotationTarget.replicate);
                if (!annotations.Any() && AreaGraphController.GroupByGroup == null)
                    annotations = new string[] { null };
            }

            var data = new List<InternalData>();
            double? qvalueCutoff = null;
            if (ShouldUseQValues(document))
            {
                if (_isGraphData)
                    qvalueCutoff = _graphSettings.QValueCutoff;
                else
                    qvalueCutoff = _refineSettings.QValueCutoff;
            }

            var hasHeavyMods = document.Settings.PeptideSettings.Modifications.HasHeavyModifications;
            var hasGlobalStandards = document.Settings.HasGlobalStandardArea;

            var ms1 = false;
            var best = false;
            if (_isGraphData)
            {
                ms1 = _graphSettings.MsLevel == AreaCVMsLevel.precursors;
                best = _graphSettings.Transitions == AreaCVTransitions.best;
            }

            var replicates = document.MeasuredResults.Chromatograms.Count;
            var areas = new List<AreaInfo>(replicates);

            MedianInfo medianInfo = null;

            if ((_isGraphData && _graphSettings.NormalizationMethod == AreaCVNormalizationMethod.medians) ||
                (!_isGraphData && _refineSettings.NormalizationMethod == AreaCVNormalizationMethod.medians))
                medianInfo = CalculateMedianAreas(document);

            foreach (var peptideGroup in document.MoleculeGroups)
            {
                foreach (var peptide in peptideGroup.Molecules)
                {
                    foreach (var transitionGroupDocNode in peptide.TransitionGroups)
                    {
                        if ((_isGraphData && _graphSettings.PointsType == PointsTypePeakArea.decoys != transitionGroupDocNode.IsDecoy) ||
                            (!_isGraphData && transitionGroupDocNode.IsDecoy))
                            continue;

                        foreach (var a in annotations)
                        {
                            areas.Clear();

                            if (_isGraphData && a != _graphSettings.Annotation && (_graphSettings.Group == null || _graphSettings.Annotation != null))
                                continue;

                            foreach (var i in AnnotationHelper.GetReplicateIndices(document.Settings,
                                _isGraphData ? _graphSettings.Group : null, a))
                            {
                                if (_isGraphData && token.HasValue && token.Value.IsCancellationRequested)
                                {
                                    IsValid = false;
                                    return;
                                }

                                var groupChromInfo = transitionGroupDocNode.GetSafeChromInfo(i)
                                    .FirstOrDefault(c => c.OptimizationStep == 0);
                                if (groupChromInfo == null)
                                    continue;

                                if (qvalueCutoff.HasValue)
                                {
                                    if (!(groupChromInfo.QValue.HasValue && groupChromInfo.QValue.Value < qvalueCutoff.Value))
                                        continue;
                                }

                                if (!groupChromInfo.Area.HasValue)
                                    continue;

                                var index = i;
                                var sumArea = transitionGroupDocNode.Transitions.Where(t =>
                                {
                                    if (ms1 != t.IsMs1 || !t.ExplicitQuantitative)
                                        return false;

                                    var chromInfo = t.GetSafeChromInfo(index).FirstOrDefault(c => c.OptimizationStep == 0);
                                    return chromInfo != null && (!best || chromInfo.RankByLevel == 1);
                                    // ReSharper disable once PossibleNullReferenceException
                                }).Sum(t => (double)t.GetSafeChromInfo(index).FirstOrDefault(c => c.OptimizationStep == 0).Area);

                                double area = sumArea;
                                var normalizedArea = area;
                                if ((_isGraphData && _graphSettings.NormalizationMethod == AreaCVNormalizationMethod.medians) ||
                                    (!_isGraphData && _refineSettings.NormalizationMethod == AreaCVNormalizationMethod.medians))
                                    normalizedArea /= medianInfo.Medians[i] / medianInfo.MedianMedian;
                                else if (((_isGraphData && _graphSettings.NormalizationMethod == AreaCVNormalizationMethod.global_standards) ||
                                         (!_isGraphData && _refineSettings.NormalizationMethod == AreaCVNormalizationMethod.global_standards)) &&
                                         hasGlobalStandards)
                                    normalizedArea = NormalizeToGlobalStandard(document, transitionGroupDocNode, i, area);
                                else if (((_isGraphData && _graphSettings.NormalizationMethod == AreaCVNormalizationMethod.ratio && _graphSettings.RatioIndex >= 0) ||
                                    (!_isGraphData && _refineSettings.NormalizationMethod == AreaCVNormalizationMethod.ratio && _refineSettings.RatioIndex >= 0)) &&
                                    hasHeavyMods)
                                {
                                    var ci = transitionGroupDocNode.GetSafeChromInfo(i).FirstOrDefault(c => c.OptimizationStep == 0);
                                    if (ci != null)
                                    {
                                        var ratioValue = ci.GetRatio(_isGraphData ? _graphSettings.RatioIndex : _refineSettings.RatioIndex);
                                        if (ratioValue != null)
                                            normalizedArea /= ratioValue.Ratio;
                                    }
                                }

                                areas.Add(new AreaInfo(area, normalizedArea));
                            }

                            if (qvalueCutoff.HasValue &&
                                (_isGraphData && areas.Count < _graphSettings.MinimumDetections) ||
                                (!_isGraphData && minDetections.HasValue && areas.Count < minDetections.Value))
                                continue;

                            AddToInternalData(data, areas, peptideGroup, peptide, transitionGroupDocNode, a);
                        }
                    }
                }
            }

            Data = ImmutableList<CVData>.ValueOf(data.GroupBy(i => i, (key, grouped) =>
            {
                var groupedArray = grouped.ToArray();
                return new CVData(
                        groupedArray.Select(idata => 
                            new PeptideAnnotationPair(idata.PeptideGroup, idata.Peptide, idata.TransitionGroup, _isGraphData ? idata.Annotation : null, idata.CV)), 
                        _isGraphData ? key.CVBucketed.Value : key.CV, key.Area, groupedArray.Length);
            }).OrderBy(d => d.CV));

            if (_isGraphData)
            {
                CalculateStats();

                if (IsValid)
                    MedianCV = new Statistics(data.Select(d => d.CV)).Median();
            }
        }

        public static readonly AreaCVGraphData INVALID = new AreaCVGraphData(null,
            new AreaCVGraphSettings((GraphTypeSummary)~0, (AreaCVNormalizationMethod)~0, -1, string.Empty,
                string.Empty, (PointsTypePeakArea)~0, double.NaN, double.NaN, -1, double.NaN, (AreaCVMsLevel)~0,
                (AreaCVTransitions)~0));

        public SrmDocument RemoveAboveCVCuttoff(SrmDocument document)
        {
            var cutoff = _refineSettings.CVCutoff / 100.0;

            var ids = new HashSet<int>(Data.Where(d => d.CV < cutoff)
                .SelectMany(d => d.PeptideAnnotationPairs)
                .Select(pair => pair.TransitionGroup.Id.GlobalIndex));

            var setRemove = IndicesToRemove(document, ids);

            return (SrmDocument)document.RemoveAll(setRemove, null, (int)SrmDocument.Level.Molecules);
        }

        public static HashSet<int> IndicesToRemove(SrmDocument document, HashSet<int> ids)
        {
            var setRemove = new HashSet<int>();
            foreach (var nodeMolecule in document.Molecules)
            {
                if (nodeMolecule.GlobalStandardType != null)
                    continue;
                foreach (var nodeGroup in nodeMolecule.TransitionGroups)
                {
                    if (!ids.Contains(nodeGroup.Id.GlobalIndex) || nodeGroup.AveragePeakArea == null)
                        setRemove.Add(nodeGroup.Id.GlobalIndex);
                    foreach (var trans in nodeGroup.Transitions)
                    {
                        if (trans.AveragePeakArea == null)
                            setRemove.Add(trans.Id.GlobalIndex);
                    }
                }
            }

            return setRemove;
        }

        private void AddToInternalData(ICollection<InternalData> data, List<AreaInfo> areas,
            PeptideGroupDocNode peptideGroup, PeptideDocNode peptide, TransitionGroupDocNode tranGroup,
            string annotation)
        {
            var normalizedStatistics = new Statistics(areas.Select(a => a.NormalizedArea));
            var normalizedMean = normalizedStatistics.Mean();
            var normalizedStdDev = normalizedStatistics.StdDev();

            // If the mean is 0 or NaN or the standard deviaiton is NaN the cv would also be NaN
            if (normalizedMean == 0.0 || double.IsNaN(normalizedMean) || double.IsNaN(normalizedStdDev))
                return;

            var cv = normalizedStdDev / normalizedMean;
            double? cvBucketed = null;
            var log10Mean = 0.0;

            if (_graphSettings != null)
            {
                var unnormalizedStatistics = new Statistics(areas.Select(a => a.Area));
                var unnomarlizedMean = unnormalizedStatistics.Mean();

                // Round cvs so that the smallest difference between two cv's is BinWidth
                cvBucketed = Math.Floor(cv / _graphSettings.BinWidth) * _graphSettings.BinWidth;
                log10Mean = _graphSettings.GraphType == GraphTypeSummary.histogram2d
                    ? Math.Floor(Math.Log10(unnomarlizedMean) / 0.05) * 0.05
                    : 0.0;
            }

            data.Add(new InternalData
            {
                Peptide = peptide,
                PeptideGroup = peptideGroup,
                TransitionGroup = tranGroup,
                Annotation = annotation,
                CV = cv,
                CVBucketed = cvBucketed,
                Area = log10Mean
            });
        }

        private static double NormalizeToGlobalStandard(SrmDocument document, TransitionGroupDocNode transitionGroupDocNode, int replicateIndex, double area)
        {
            var chromSet = document.MeasuredResults.Chromatograms[replicateIndex];
            var groupChromSet = transitionGroupDocNode.Results[replicateIndex];
            var fileInfoFirst = chromSet.GetFileInfo(groupChromSet.First(c => c.OptimizationStep == 0).FileId);
            var globalStandard = document.Settings.CalcGlobalStandardArea(replicateIndex, fileInfoFirst);
            if (globalStandard != 0.0)
                area /= globalStandard;
            return area;
        }

        private void CalculateStats()
        {
            MedianCV = 0.0;
            MinMeanArea = double.MaxValue;
            MaxMeanArea = 0.0;
            MaxFrequency = 0;
            MaxCV = 0.0;
            Total = 0;

            IsValid = Data.Any();
            if (IsValid)
            {
                var belowCvCutoffCount = 0;

                // Calculate max frequency, total number of cv's and number of cv's below the cutoff
                MinCV = Data.First().CV;
                MaxCV = Data.Last().CV;

                foreach (var d in Data)
                {
                    MinMeanArea = Math.Min(MinMeanArea, d.MeanArea);
                    MaxMeanArea = Math.Max(MaxMeanArea, d.MeanArea);
                    MaxFrequency = Math.Max(MaxFrequency, d.Frequency);

                    Total += d.Frequency;
                    if (d.CV < _graphSettings.CVCutoff)
                        belowCvCutoffCount += d.Frequency;
                }

                MeanCV = Data.Sum(d => d.CV * d.Frequency) / Total;

                BelowCVCutoff = (double)belowCvCutoffCount / Total;
            }
        }

        private MedianInfo CalculateMedianAreas(SrmDocument document)
        {
            double? qvalueCutoff = null;
            if (_isGraphData &&
                 _graphSettings.PointsType == PointsTypePeakArea.targets &&
                 !double.IsNaN(Settings.Default.AreaCVQValueCutoff) &&
                 Settings.Default.AreaCVQValueCutoff < 1.0 &&
                document.Settings.PeptideSettings.Integration.PeakScoringModel.IsTrained)
                qvalueCutoff = _graphSettings.QValueCutoff;

            else if (!_isGraphData && !double.IsNaN(_refineSettings.QValueCutoff) &&
                _refineSettings.QValueCutoff < 1.0 &&
                document.Settings.PeptideSettings.Integration.PeakScoringModel.IsTrained)
                qvalueCutoff = _refineSettings.QValueCutoff;

            var replicates = document.MeasuredResults.Chromatograms.Count;
            var allAreas = new List<List<double?>>(document.MoleculeTransitionGroupCount);

            foreach (var transitionGroupDocNode in document.MoleculeTransitionGroups)
            {
                if ((_isGraphData && (_graphSettings.PointsType == PointsTypePeakArea.decoys) != transitionGroupDocNode.IsDecoy) ||
                    transitionGroupDocNode.IsDecoy)
                    continue;

                var detections = 0;
                var areas = new List<double?>(replicates);

                for (var i = 0; i < replicates; ++i)
                {
                    double? area = transitionGroupDocNode.GetPeakArea(i, qvalueCutoff);
                    if (area.HasValue)
                        ++detections;
                    areas.Add(area);
                }

                var useRefineDetections = !_isGraphData && _refineSettings.MinimumDetections != -1 && detections < _refineSettings.MinimumDetections;
                var useGraphDetections = _isGraphData && detections < _graphSettings.MinimumDetections;
                if (qvalueCutoff.HasValue && (useRefineDetections || useGraphDetections))
                    continue;

                allAreas.Add(areas);
            }

            var medians = new List<double>(replicates);
            for (var i = 0; i < replicates; ++i)
                medians.Add(new Statistics(GetReplicateAreas(allAreas, i)).Median());

            return new MedianInfo(medians, new Statistics(medians).Median());
        }

        private IEnumerable<double> GetReplicateAreas(List<List<double?>> allAreas, int replicateIndex)
        {
            foreach (var areas in allAreas)
            {
                var area = areas[replicateIndex];
                if (area.HasValue)
                    yield return area.Value;
            }
        }

        private bool ShouldUseQValues(SrmDocument document)
        {
            if (_isGraphData)
            {
                return _graphSettings.PointsType == PointsTypePeakArea.targets &&
                              document.Settings.PeptideSettings.Integration.PeakScoringModel.IsTrained &&
                              !double.IsNaN(Settings.Default.AreaCVQValueCutoff) &&
                              Settings.Default.AreaCVQValueCutoff < 1.0;
            }

            return document.Settings.PeptideSettings.Integration.PeakScoringModel.IsTrained &&
                   !double.IsNaN(_refineSettings.QValueCutoff) &&
                   _refineSettings.QValueCutoff < 1.0;
        }

        public IList<CVData> Data { get; private set; }

        public bool IsValid { get; private set; }
        public double MinMeanArea { get; private set; } // Smallest mean area
        public double MaxMeanArea { get; private set; } // Largest mean area
        public int Total { get; private set; } // Total number of CV's
        public double MaxCV { get; private set; } // Highest CV
        public double MinCV { get; private set; } // Smallest CV
        public int MaxFrequency { get; private set; } // Highest count of CV's
        public double MedianCV { get; private set; } // Median CV
        public double MeanCV { get; private set; } // Mean CV
        public double BelowCVCutoff { get; private set; } // Fraction/Percentage of CV's below cutoff

        private class InternalData
        {
            public PeptideGroupDocNode PeptideGroup;
            public PeptideDocNode Peptide;
            public TransitionGroupDocNode TransitionGroup;
            public string Annotation;
            public double CV;
            public double? CVBucketed;
            public double Area;

            #region object overrides

            protected bool Equals(InternalData other)
            {
                if (CVBucketed.HasValue)
                    return CVBucketed.Equals(other.CVBucketed) && Area.Equals(other.Area);

                return CV.Equals(other.CV) && Area.Equals(other.Area);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((InternalData)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    if (CVBucketed.HasValue)
                        return (CVBucketed.GetHashCode() * 397) ^ Area.GetHashCode();

                    return (CV.GetHashCode() * 397) ^ Area.GetHashCode();
                }
            }

            #endregion
        }

        public class AreaCVGraphSettings : AreaCVRefinementSettings
        {
            public AreaCVGraphSettings(GraphTypeSummary graphType, bool convertToDecimal = true)
            {
                var factor = !Settings.Default.AreaCVShowDecimals && convertToDecimal ? 0.01 : 1.0;
                GraphType = graphType;
                MsLevel = Helpers.ParseEnum(Settings.Default.AreaCVMsLevel, AreaCVMsLevel.products);
                Transitions = Helpers.ParseEnum(Settings.Default.AreaCVTransitions, AreaCVTransitions.all);
                NormalizationMethod = Helpers.ParseEnum(Settings.Default.AreaCVNormalizationMethod, AreaCVNormalizationMethod.none);
                RatioIndex = Settings.Default.AreaCVRatioIndex;
                Group = AreaGraphController.GroupByGroup != null ? string.Copy(AreaGraphController.GroupByGroup) : null;
                Annotation = AreaGraphController.GroupByAnnotation != null ? string.Copy(AreaGraphController.GroupByAnnotation) : null;
                PointsType = Helpers.ParseEnum(Settings.Default.AreaCVPointsType, PointsTypePeakArea.targets);
                QValueCutoff = Settings.Default.AreaCVQValueCutoff;
                CVCutoff = Settings.Default.AreaCVCVCutoff * factor;
                MinimumDetections = AreaGraphController.MinimumDetections;
                BinWidth = Settings.Default.AreaCVHistogramBinWidth * factor;
            }

            public AreaCVGraphSettings(GraphTypeSummary graphType, AreaCVNormalizationMethod normalizationMethod, int ratioIndex, string group, string annotation, PointsTypePeakArea pointsType, double qValueCutoff,
                double cvCutoff, int minimumDetections, double binwidth, AreaCVMsLevel msLevel, AreaCVTransitions transitions)
            {
                GraphType = graphType;
                NormalizationMethod = normalizationMethod;
                RatioIndex = ratioIndex;
                Group = group;
                Annotation = annotation;
                PointsType = pointsType;
                QValueCutoff = qValueCutoff;
                CVCutoff = cvCutoff;
                MinimumDetections = minimumDetections;
                BinWidth = binwidth;
                MsLevel = msLevel;
                Transitions = transitions;
            }

            public static bool CacheEqual(AreaCVGraphSettings a, AreaCVGraphSettings b)
            {
                return a.GraphType == b.GraphType && a.Group == b.Group &&
                       a.PointsType == b.PointsType && (a.QValueCutoff == b.QValueCutoff || double.IsNaN(a.QValueCutoff) && double.IsNaN(b.QValueCutoff)) &&
                       a.CVCutoff == b.CVCutoff && a.BinWidth == b.BinWidth && a.MsLevel == b.MsLevel && a.Transitions == b.Transitions;
            }

            public GraphTypeSummary GraphType { get; private set; }
            public AreaCVNormalizationMethod NormalizationMethod { get; private set; }
            public AreaCVMsLevel MsLevel { get; private set; }
            public AreaCVTransitions Transitions { get; private set; }
            public int RatioIndex { get; private set; }
            public string Group { get; private set; }
            public string Annotation { get; private set; }
            public PointsTypePeakArea PointsType { get; private set; }
            public double QValueCutoff { get; private set; }
            public double CVCutoff { get; private set; }
            public int MinimumDetections { get; private set; }
            public double BinWidth { get; private set; }
        }

        public class AreaCVRefinementSettings
        {
            public AreaCVRefinementSettings(double cvCutoff, double qValueCutoff, int minimumDetections, AreaCVNormalizationMethod normalizationMethod, int ratioIndex)
            {
                CVCutoff = cvCutoff;
                QValueCutoff = qValueCutoff;
                MinimumDetections = minimumDetections;
                NormalizationMethod = normalizationMethod;
                RatioIndex = ratioIndex;
            }

            public double QValueCutoff { get; private set; }
            public double CVCutoff { get; private set; }
            public int MinimumDetections { get; private set; }
            public AreaCVNormalizationMethod NormalizationMethod { get; private set; }
            public int RatioIndex { get; private set; }
        }
    }

    public class PeptideAnnotationPair
    {
        public PeptideAnnotationPair(PeptideGroupDocNode peptideGroup, PeptideDocNode peptide, TransitionGroupDocNode tranGroup, string annotation, double cvRaw)
        {
            PeptideGroup = peptideGroup;
            Peptide = peptide;
            TransitionGroup = tranGroup;
            Annotation = annotation;
            CVRaw = cvRaw;
        }

        public PeptideGroupDocNode PeptideGroup { get; private set; }
        public PeptideDocNode Peptide { get; private set; }
        public TransitionGroupDocNode TransitionGroup { get; private set; }
        public string Annotation { get; private set; }
        public double CVRaw { get; private set; }

    }


    public class MedianInfo
    {
        public MedianInfo(List<double> medians, double medianMedian)
        {
            Medians = medians;
            MedianMedian = medianMedian;
        }

        public IList<double> Medians { get; private set; }
        public double MedianMedian { get; private set; }
    }

    public class AreaInfo
    {
        public AreaInfo(double area, double normalizedArea)
        {
            Area = area;
            NormalizedArea = normalizedArea;
        }

        public double Area { get; private set; }
        public double NormalizedArea { get; private set; }
    }

    public class CVData
    {
        public CVData(IEnumerable<PeptideAnnotationPair> peptideAnnotationPairs, double cv, double meanArea, int frequency)
        {
            PeptideAnnotationPairs = peptideAnnotationPairs;
            CV = cv;
            MeanArea = meanArea;
            Frequency = frequency;
        }

        public IEnumerable<PeptideAnnotationPair> PeptideAnnotationPairs { get; private set; }
        public double CV { get; private set; }
        public double MeanArea { get; private set; }
        public int Frequency { get; private set; }

        public override string ToString()
        {
            return CV + TextUtil.LineSeparate(PeptideAnnotationPairs.Select(p => @" " + p.Peptide.TextId));
        }
    }

    #region Functional test support

    public class AreaCVGraphDataStatistics
    {
        public AreaCVGraphDataStatistics(int dataCount, int items, double minMeanArea, double maxMeanArea, int total, double maxCv, double minCv, int maxFrequency, double medianCv, double meanCv, double belowCvCutoff)
        {
            DataCount = dataCount;
            Items = items;
            MinMeanArea = minMeanArea;
            MaxMeanArea = maxMeanArea;
            Total = total;
            MaxCV = maxCv;
            MinCV = minCv;
            MaxFrequency = maxFrequency;
            MedianCV = medianCv;
            MeanCV = meanCv;
            BelowCVCutoff = belowCvCutoff;
        }

        public AreaCVGraphDataStatistics(AreaCVGraphData data, int items)
        {
            DataCount = data.Data.Count;
            Items = items;
            MinMeanArea = data.MinMeanArea;
            MaxMeanArea = data.MaxMeanArea;
            Total = data.Total;
            MaxCV = data.MaxCV;
            MinCV = data.MinCV;
            MaxFrequency = data.MaxFrequency;
            MedianCV = data.MedianCV;
            MeanCV = data.MeanCV;
            BelowCVCutoff = data.BelowCVCutoff;
        }

        protected bool Equals(AreaCVGraphDataStatistics other)
        {
            return DataCount == other.DataCount &&
                   Items == other.Items &&
                   MinMeanArea.Equals(other.MinMeanArea) &&
                   MaxMeanArea.Equals(other.MaxMeanArea) &&
                   Total == other.Total &&
                   MaxCV.Equals(other.MaxCV) &&
                   MinCV.Equals(other.MinCV) &&
                   MaxFrequency == other.MaxFrequency &&
                   MedianCV.Equals(other.MedianCV) &&
                   MeanCV.Equals(other.MeanCV) &&
                   BelowCVCutoff.Equals(other.BelowCVCutoff);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AreaCVGraphDataStatistics)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DataCount;
                hashCode = (hashCode * 397) ^ Items.GetHashCode();
                hashCode = (hashCode * 397) ^ MinMeanArea.GetHashCode();
                hashCode = (hashCode * 397) ^ MaxMeanArea.GetHashCode();
                hashCode = (hashCode * 397) ^ Total;
                hashCode = (hashCode * 397) ^ MaxCV.GetHashCode();
                hashCode = (hashCode * 397) ^ MinCV.GetHashCode();
                hashCode = (hashCode * 397) ^ MaxFrequency;
                hashCode = (hashCode * 397) ^ MedianCV.GetHashCode();
                hashCode = (hashCode * 397) ^ MeanCV.GetHashCode();
                hashCode = (hashCode * 397) ^ BelowCVCutoff.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format(@"{0}, {1}, {2:R}, {3:R}, {4}, {5:R}, {6:R}, {7}, {8:R}, {9:R}, {10:R}",
                DataCount, Items, MinMeanArea, MaxMeanArea, Total, MaxCV, MinCV, MaxFrequency, MedianCV, MeanCV, BelowCVCutoff);
        }

        public string ToCode()
        {
            return string.Format(@"new {0}({1}),", GetType().Name, ToString());
        }

        private int DataCount { get; set; }
        private int Items { get; set; }
        private double MinMeanArea { get; set; } // Smallest mean area
        private double MaxMeanArea { get; set; } // Largest mean area
        private int Total { get; set; } // Total number of CV's
        private double MaxCV { get; set; } // Highest CV
        private double MinCV { get; set; } // Smallest CV
        private int MaxFrequency { get; set; } // Highest count of CV's
        private double MedianCV { get; set; } // Median CV
        private double MeanCV { get; set; } // Mean CV
        private double BelowCVCutoff { get; set; } // Fraction/Percentage of CV's below cutoff
    }

    #endregion
}