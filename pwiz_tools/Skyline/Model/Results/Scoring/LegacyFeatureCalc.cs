﻿/*
 * Original author: Brendan MacLean <brendanx .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2012 University of Washington - Seattle, WA
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
using System.Linq;

namespace pwiz.Skyline.Model.Results.Scoring
{
    public class LegacyLogUnforcedAreaCalc : SummaryPeakFeatureCalculator
    {
        /// <summary>
        /// Standard peaks are assigned ^1.2 the value of analyte peaks, since standards
        /// are intended to be spiked in at a constant concentration, while analyte peaks
        /// are expected to vary, and may even be missing altogether.
        /// </summary>
        public const double STANDARD_MULTIPLIER = 1.2;

        public static double Score(double area, double areaStandard)
        {
            return Math.Log(area + Math.Pow(areaStandard, STANDARD_MULTIPLIER));
        }

        protected override double Calculate(PeakScoringContext context, IPeptidePeakData<ISummaryPeakData> summaryPeakData)
        {
            return Score(SummedArea(summaryPeakData, false), SummedArea(summaryPeakData, true));
        }

        private double SummedArea(IPeptidePeakData<ISummaryPeakData> summaryPeakData, bool isStandard)
        {
            return summaryPeakData.TransitionGroupPeakData
                .Where(pd => pd.IsStandard == isStandard)
                .SelectMany(pd => pd.TranstionPeakData)
                .Where(p => !p.Peak.IsForcedIntegration)
                .Sum(p => p.Peak.Area);
        }
    }

    public abstract class LegacyCountScoreCalc : SummaryPeakFeatureCalculator
    {
        protected abstract bool IsIncludedGroup(ITransitionGroupPeakData<ISummaryPeakData> transitionGroupPeakData);

        protected override double Calculate(PeakScoringContext context, IPeptidePeakData<ISummaryPeakData> summaryPeakData)
        {
            return summaryPeakData.TransitionGroupPeakData.Where(IsIncludedGroup).Sum(pd => CalcCountScore(pd));
        }

        private double CalcCountScore(ITransitionGroupPeakData<ISummaryPeakData> transitionGroupPeakData)
        {
            return GetPeakCountScore(transitionGroupPeakData.TranstionPeakData.Count(p => !p.Peak.IsForcedIntegration),
                                     transitionGroupPeakData.TranstionPeakData.Count);
        }

        public static double GetPeakCountScore(double peakCount, double totalCount)
        {
            return totalCount > 4
                       ? 4.0 * peakCount / totalCount
                       : peakCount;
        }
    }

    class LegacyUnforcedCountScoreCalc : LegacyCountScoreCalc
    {
        protected override bool IsIncludedGroup(ITransitionGroupPeakData<ISummaryPeakData> transitionGroupPeakData)
        {
            return !transitionGroupPeakData.IsStandard;
        }
    }

    class LegacyUnforcedCountScoreStandardCalc : LegacyCountScoreCalc
    {
        protected override bool IsIncludedGroup(ITransitionGroupPeakData<ISummaryPeakData> transitionGroupPeakData)
        {
            return transitionGroupPeakData.IsStandard;
        }
    }

    class LegacyIdentifiedCountStandardCalc : SummaryPeakFeatureCalculator
    {
        protected override double Calculate(PeakScoringContext context, IPeptidePeakData<ISummaryPeakData> summaryPeakData)
        {
            return summaryPeakData.TransitionGroupPeakData.Count(
                pd => pd.TranstionPeakData.Any(p => p.Peak.IsIdentified));
        }
    }
}
