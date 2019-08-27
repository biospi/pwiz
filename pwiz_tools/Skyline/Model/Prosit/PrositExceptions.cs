﻿/*
 * Original author: Tobias Rohde <tobiasr .at. uw.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2019 University of Washington - Seattle, WA
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
using pwiz.Skyline.Model.DocSettings;

namespace pwiz.Skyline.Model.Prosit
{
    /// <summary>
    /// Simple class to distinguish Prosit exceptions.
    /// </summary>
    public class PrositException : Exception
    {
        public PrositException(string message) : base(message)
        {
        }
    }

    public class PrositPeptideTooLongException : PrositException
    {
        public PrositPeptideTooLongException(Target target)
            : base(string.Format(
                PrositResources.PrositPeptideTooLongException_PrositPeptideTooLongException_Peptide_Sequence___0_____1___is_longer_than_the_maximum_supported_length_by_Prosit___2__,
                target.Sequence, FastaSequence.StripModifications(target.Sequence).Length, Constants.PEPTIDE_SEQ_LEN))
        {
        }
    }

    public class PrositUnsupportedAminoAcidException : PrositException
    {
        public PrositUnsupportedAminoAcidException(Target target, int index)
            : base(string.Format(
                PrositResources.PrositPeptideTooLongException_PrositUnsupportedAminoAcidException_Amino_acid___0___in___1___is_not_supported_by_Prosit,
                target.Sequence[index], target))
        {
        }
    }

    public class PrositUnsupportedModificationException : PrositException
    {
        public PrositUnsupportedModificationException(Target target, StaticMod mod, int index)
            : base(string.Format(PrositResources.PrositUnsupportedModificationException_PrositUnsupportedModificationException_Modifcation___0___at_index___1___in___2___is_not_supported_by_Prosit, mod.Name,
                index, target.Sequence))
        {
        }
    }
}
