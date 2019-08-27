﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pwiz.Skyline.Model.Prosit {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class PrositResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PrositResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("pwiz.Skyline.Model.Prosit.PrositResources", typeof(PrositResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Intensity model {0} does not exist.
        /// </summary>
        internal static string PrositIntensityModel_PrositIntensityModel_Intensity_model__0__does_not_exist {
            get {
                return ResourceManager.GetString("PrositIntensityModel_PrositIntensityModel_Intensity_model__0__does_not_exist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Constructing Prosit Inputs.
        /// </summary>
        internal static string PrositModel_BatchPredict_Constructing_Prosit_Inputs {
            get {
                return ResourceManager.GetString("PrositModel_BatchPredict_Constructing_Prosit_Inputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Requesting predictions from Prosit.
        /// </summary>
        internal static string PrositModel_BatchPredict_Requesting_predictions_from_Prosit {
            get {
                return ResourceManager.GetString("PrositModel_BatchPredict_Requesting_predictions_from_Prosit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Peptide Sequence &apos;{0}&apos; ({1}) is longer than the maximum supported length by Prosit ({2}).
        /// </summary>
        internal static string PrositPeptideTooLongException_PrositPeptideTooLongException_Peptide_Sequence___0_____1___is_longer_than_the_maximum_supported_length_by_Prosit___2__ {
            get {
                return ResourceManager.GetString("PrositPeptideTooLongException_PrositPeptideTooLongException_Peptide_Sequence___0_" +
                        "____1___is_longer_than_the_maximum_supported_length_by_Prosit___2__", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Amino acid &apos;{0}&apos; in &apos;{1}&apos; is not supported by Prosit.
        /// </summary>
        internal static string PrositPeptideTooLongException_PrositUnsupportedAminoAcidException_Amino_acid___0___in___1___is_not_supported_by_Prosit {
            get {
                return ResourceManager.GetString("PrositPeptideTooLongException_PrositUnsupportedAminoAcidException_Amino_acid___0_" +
                        "__in___1___is_not_supported_by_Prosit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Prosit server set.
        /// </summary>
        internal static string PrositPredictionClient_Current_No_Prosit_server_set {
            get {
                return ResourceManager.GetString("PrositPredictionClient_Current_No_Prosit_server_set", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Modifcation &apos;{0}&apos; at index &apos;{1}&apos; in &apos;{2}&apos; is not supported by Prosit.
        /// </summary>
        internal static string PrositUnsupportedModificationException_PrositUnsupportedModificationException_Modifcation___0___at_index___1___in___2___is_not_supported_by_Prosit {
            get {
                return ResourceManager.GetString("PrositUnsupportedModificationException_PrositUnsupportedModificationException_Mod" +
                        "ifcation___0___at_index___1___in___2___is_not_supported_by_Prosit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server offline.
        /// </summary>
        internal static string ToolOptionsUI_ToolOptionsUI_Server_offline {
            get {
                return ResourceManager.GetString("ToolOptionsUI_ToolOptionsUI_Server_offline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server online.
        /// </summary>
        internal static string ToolOptionsUI_ToolOptionsUI_Server_online {
            get {
                return ResourceManager.GetString("ToolOptionsUI_ToolOptionsUI_Server_online", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server unavailable.
        /// </summary>
        internal static string ToolOptionsUI_UpdateServerStatus_Server_unavailable {
            get {
                return ResourceManager.GetString("ToolOptionsUI_UpdateServerStatus_Server_unavailable", resourceCulture);
            }
        }
    }
}
