﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IFS_Thesis.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int PopulationSize {
            get {
                return ((int)(this["PopulationSize"]));
            }
            set {
                this["PopulationSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.1")]
        public float N2IndividualsPercentage {
            get {
                return ((float)(this["N2IndividualsPercentage"]));
            }
            set {
                this["N2IndividualsPercentage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.15")]
        public float N3IndividualsPercentage {
            get {
                return ((float)(this["N3IndividualsPercentage"]));
            }
            set {
                this["N3IndividualsPercentage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.55")]
        public float N4IndividualsPercentage {
            get {
                return ((float)(this["N4IndividualsPercentage"]));
            }
            set {
                this["N4IndividualsPercentage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.5")]
        public float MutationProbability {
            get {
                return ((float)(this["MutationProbability"]));
            }
            set {
                this["MutationProbability"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.01")]
        public float AverageFitnessThreshold {
            get {
                return ((float)(this["AverageFitnessThreshold"]));
            }
            set {
                this["AverageFitnessThreshold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.2")]
        public float N1IndividualsPercentage {
            get {
                return ((float)(this["N1IndividualsPercentage"]));
            }
            set {
                this["N1IndividualsPercentage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:/tmp/IFS Images")]
        public string WorkingDirectory {
            get {
                return ((string)(this["WorkingDirectory"]));
            }
            set {
                this["WorkingDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4000")]
        public int NumberOfGenerations {
            get {
                return ((int)(this["NumberOfGenerations"]));
            }
            set {
                this["NumberOfGenerations"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int PrcFitness {
            get {
                return ((int)(this["PrcFitness"]));
            }
            set {
                this["PrcFitness"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public int ProFitness {
            get {
                return ((int)(this["ProFitness"]));
            }
            set {
                this["ProFitness"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int DrawPointsMultiplier {
            get {
                return ((int)(this["DrawPointsMultiplier"]));
            }
            set {
                this["DrawPointsMultiplier"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.7")]
        public float ArithmeticCrossoverProbability {
            get {
                return ((float)(this["ArithmeticCrossoverProbability"]));
            }
            set {
                this["ArithmeticCrossoverProbability"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.3")]
        public float OnePointCrossoverProbability {
            get {
                return ((float)(this["OnePointCrossoverProbability"]));
            }
            set {
                this["OnePointCrossoverProbability"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IgnoreProbabilities {
            get {
                return ((bool)(this["IgnoreProbabilities"]));
            }
            set {
                this["IgnoreProbabilities"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.05")]
        public float RandomMutationProbability {
            get {
                return ((float)(this["RandomMutationProbability"]));
            }
            set {
                this["RandomMutationProbability"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.95")]
        public float ControlledMutationProbability {
            get {
                return ((float)(this["ControlledMutationProbability"]));
            }
            set {
                this["ControlledMutationProbability"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("256")]
        public int ImageX {
            get {
                return ((int)(this["ImageX"]));
            }
            set {
                this["ImageX"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("256")]
        public int ImageY {
            get {
                return ((int)(this["ImageY"]));
            }
            set {
                this["ImageY"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50000")]
        public int InitialSingelPoolSize {
            get {
                return ((int)(this["InitialSingelPoolSize"]));
            }
            set {
                this["InitialSingelPoolSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int DrawImageEveryNthGeneration {
            get {
                return ((int)(this["DrawImageEveryNthGeneration"]));
            }
            set {
                this["DrawImageEveryNthGeneration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int EliteIndividualsPerDegree {
            get {
                return ((int)(this["EliteIndividualsPerDegree"]));
            }
            set {
                this["EliteIndividualsPerDegree"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExtremeDebugging {
            get {
                return ((bool)(this["ExtremeDebugging"]));
            }
            set {
                this["ExtremeDebugging"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.15")]
        public float MutationRange {
            get {
                return ((float)(this["MutationRange"]));
            }
            set {
                this["MutationRange"] = value;
            }
        }
    }
}
