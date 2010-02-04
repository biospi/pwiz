﻿namespace ScanRanker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblInputFileDir = new System.Windows.Forms.Label();
            this.tbSrcDir = new System.Windows.Forms.TextBox();
            this.btnSrcFileBrowse = new System.Windows.Forms.Button();
            this.gbAssessment = new System.Windows.Forms.GroupBox();
            this.tbOutputMetricsSuffix = new System.Windows.Forms.TextBox();
            this.lblOutputMetricsSurfix = new System.Windows.Forms.Label();
            this.gbDtgConfig = new System.Windows.Forms.GroupBox();
            this.cbWriteOutTags = new System.Windows.Forms.CheckBox();
            this.tbTagLength = new System.Windows.Forms.TextBox();
            this.lblTagLength = new System.Windows.Forms.Label();
            this.cbUseMultipleProcessors = new System.Windows.Forms.CheckBox();
            this.cbUseChargeStateFromMS = new System.Windows.Forms.CheckBox();
            this.rbMono = new System.Windows.Forms.RadioButton();
            this.tbNumChargeStates = new System.Windows.Forms.TextBox();
            this.rbAverage = new System.Windows.Forms.RadioButton();
            this.lblUseMass = new System.Windows.Forms.Label();
            this.lblNumChargeStates = new System.Windows.Forms.Label();
            this.tbStaticMods = new System.Windows.Forms.TextBox();
            this.lblStaticMods = new System.Windows.Forms.Label();
            this.tbFragmentTolerance = new System.Windows.Forms.TextBox();
            this.lblFragmentTolerance = new System.Windows.Forms.Label();
            this.tbPrecursorTolerance = new System.Windows.Forms.TextBox();
            this.lblPrecursorTolerance = new System.Windows.Forms.Label();
            this.cbAssessement = new System.Windows.Forms.CheckBox();
            this.gbRemoval = new System.Windows.Forms.GroupBox();
            this.cmbOutputFileFormat = new System.Windows.Forms.ComboBox();
            this.tbOutFileNameSuffixForRemoval = new System.Windows.Forms.TextBox();
            this.lblOutFileNameSurrfixForRemoval = new System.Windows.Forms.Label();
            this.lblOutFileFormat = new System.Windows.Forms.Label();
            this.lblPercentSpectra = new System.Windows.Forms.Label();
            this.tbRemovalCutoff = new System.Windows.Forms.TextBox();
            this.lblRetain = new System.Windows.Forms.Label();
            this.tbMetricsFileSuffixForRemoval = new System.Windows.Forms.TextBox();
            this.lblMetricsFileSuffixForRemoval = new System.Windows.Forms.Label();
            this.cbRemoval = new System.Windows.Forms.CheckBox();
            this.gbRecovery = new System.Windows.Forms.GroupBox();
            this.cbOptimizeScoreWeights = new System.Windows.Forms.CheckBox();
            this.cmbScoreWeights = new System.Windows.Forms.ComboBox();
            this.cbNormSearchScores = new System.Windows.Forms.CheckBox();
            this.tbDecoyPrefix = new System.Windows.Forms.TextBox();
            this.tbMaxFDR = new System.Windows.Forms.TextBox();
            this.lblDecoyPrefix = new System.Windows.Forms.Label();
            this.lblSearchScoreWeights = new System.Windows.Forms.Label();
            this.lblMaxFDR = new System.Windows.Forms.Label();
            this.btnDBBrowse = new System.Windows.Forms.Button();
            this.tbDBFile = new System.Windows.Forms.TextBox();
            this.lblDBFile = new System.Windows.Forms.Label();
            this.tbPepXMLDir = new System.Windows.Forms.TextBox();
            this.btnPepXMLBrowse = new System.Windows.Forms.Button();
            this.lblPepXMLDir = new System.Windows.Forms.Label();
            this.tbOutFileNameSuffixForRecovery = new System.Windows.Forms.TextBox();
            this.lblOutFileNameSuffixForRecovery = new System.Windows.Forms.Label();
            this.cbRecovery = new System.Windows.Forms.CheckBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblOutputDir = new System.Windows.Forms.Label();
            this.tbOutputDir = new System.Windows.Forms.TextBox();
            this.btnOutputDirBrowse = new System.Windows.Forms.Button();
            this.bgDirectagRun = new System.ComponentModel.BackgroundWorker();
            this.bgWriteSpectra = new System.ComponentModel.BackgroundWorker();
            this.bgAddLabels = new System.ComponentModel.BackgroundWorker();
            this.tvSelDirs = new System.Windows.Forms.TreeView();
            this.lblInputFileFilters = new System.Windows.Forms.Label();
            this.tbInputFileFilters = new System.Windows.Forms.TextBox();
            this.btnListFiles = new System.Windows.Forms.Button();
            this.gbAssessment.SuspendLayout();
            this.gbDtgConfig.SuspendLayout();
            this.gbRemoval.SuspendLayout();
            this.gbRecovery.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInputFileDir
            // 
            this.lblInputFileDir.AutoSize = true;
            this.lblInputFileDir.Location = new System.Drawing.Point(23, 23);
            this.lblInputFileDir.Name = "lblInputFileDir";
            this.lblInputFileDir.Size = new System.Drawing.Size(79, 13);
            this.lblInputFileDir.TabIndex = 0;
            this.lblInputFileDir.Text = "Input Directory:";
            // 
            // tbSrcDir
            // 
            this.tbSrcDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSrcDir.Location = new System.Drawing.Point(115, 19);
            this.tbSrcDir.Name = "tbSrcDir";
            this.tbSrcDir.Size = new System.Drawing.Size(496, 20);
            this.tbSrcDir.TabIndex = 1;
            // 
            // btnSrcFileBrowse
            // 
            this.btnSrcFileBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSrcFileBrowse.Location = new System.Drawing.Point(617, 19);
            this.btnSrcFileBrowse.Name = "btnSrcFileBrowse";
            this.btnSrcFileBrowse.Size = new System.Drawing.Size(73, 21);
            this.btnSrcFileBrowse.TabIndex = 1;
            this.btnSrcFileBrowse.Text = "Browse";
            this.btnSrcFileBrowse.UseVisualStyleBackColor = true;
            this.btnSrcFileBrowse.Click += new System.EventHandler(this.btnSrcFileBrowse_Click);
            // 
            // gbAssessment
            // 
            this.gbAssessment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbAssessment.Controls.Add(this.tbOutputMetricsSuffix);
            this.gbAssessment.Controls.Add(this.lblOutputMetricsSurfix);
            this.gbAssessment.Controls.Add(this.gbDtgConfig);
            this.gbAssessment.Controls.Add(this.cbAssessement);
            this.gbAssessment.Location = new System.Drawing.Point(38, 280);
            this.gbAssessment.Name = "gbAssessment";
            this.gbAssessment.Size = new System.Drawing.Size(264, 374);
            this.gbAssessment.TabIndex = 3;
            this.gbAssessment.TabStop = false;
            // 
            // tbOutputMetricsSuffix
            // 
            this.tbOutputMetricsSuffix.Location = new System.Drawing.Point(139, 339);
            this.tbOutputMetricsSuffix.Name = "tbOutputMetricsSuffix";
            this.tbOutputMetricsSuffix.Size = new System.Drawing.Size(109, 20);
            this.tbOutputMetricsSuffix.TabIndex = 4;
            // 
            // lblOutputMetricsSurfix
            // 
            this.lblOutputMetricsSurfix.AutoSize = true;
            this.lblOutputMetricsSurfix.Location = new System.Drawing.Point(6, 342);
            this.lblOutputMetricsSurfix.Name = "lblOutputMetricsSurfix";
            this.lblOutputMetricsSurfix.Size = new System.Drawing.Size(127, 13);
            this.lblOutputMetricsSurfix.TabIndex = 2;
            this.lblOutputMetricsSurfix.Text = "Quality Metrics File Suffix:";
            // 
            // gbDtgConfig
            // 
            this.gbDtgConfig.Controls.Add(this.cbWriteOutTags);
            this.gbDtgConfig.Controls.Add(this.tbTagLength);
            this.gbDtgConfig.Controls.Add(this.lblTagLength);
            this.gbDtgConfig.Controls.Add(this.cbUseMultipleProcessors);
            this.gbDtgConfig.Controls.Add(this.cbUseChargeStateFromMS);
            this.gbDtgConfig.Controls.Add(this.rbMono);
            this.gbDtgConfig.Controls.Add(this.tbNumChargeStates);
            this.gbDtgConfig.Controls.Add(this.rbAverage);
            this.gbDtgConfig.Controls.Add(this.lblUseMass);
            this.gbDtgConfig.Controls.Add(this.lblNumChargeStates);
            this.gbDtgConfig.Controls.Add(this.tbStaticMods);
            this.gbDtgConfig.Controls.Add(this.lblStaticMods);
            this.gbDtgConfig.Controls.Add(this.tbFragmentTolerance);
            this.gbDtgConfig.Controls.Add(this.lblFragmentTolerance);
            this.gbDtgConfig.Controls.Add(this.tbPrecursorTolerance);
            this.gbDtgConfig.Controls.Add(this.lblPrecursorTolerance);
            this.gbDtgConfig.Location = new System.Drawing.Point(18, 23);
            this.gbDtgConfig.Name = "gbDtgConfig";
            this.gbDtgConfig.Size = new System.Drawing.Size(229, 306);
            this.gbDtgConfig.TabIndex = 1;
            this.gbDtgConfig.TabStop = false;
            this.gbDtgConfig.Text = "Sequence Tagging Configuration";
            // 
            // cbWriteOutTags
            // 
            this.cbWriteOutTags.AutoSize = true;
            this.cbWriteOutTags.Location = new System.Drawing.Point(25, 274);
            this.cbWriteOutTags.Name = "cbWriteOutTags";
            this.cbWriteOutTags.Size = new System.Drawing.Size(98, 17);
            this.cbWriteOutTags.TabIndex = 17;
            this.cbWriteOutTags.Text = "Write Out Tags";
            this.cbWriteOutTags.UseVisualStyleBackColor = true;
            // 
            // tbTagLength
            // 
            this.tbTagLength.Location = new System.Drawing.Point(152, 84);
            this.tbTagLength.Name = "tbTagLength";
            this.tbTagLength.Size = new System.Drawing.Size(45, 20);
            this.tbTagLength.TabIndex = 16;
            this.tbTagLength.Text = "3";
            // 
            // lblTagLength
            // 
            this.lblTagLength.AutoSize = true;
            this.lblTagLength.Location = new System.Drawing.Point(22, 87);
            this.lblTagLength.Name = "lblTagLength";
            this.lblTagLength.Size = new System.Drawing.Size(117, 13);
            this.lblTagLength.TabIndex = 15;
            this.lblTagLength.Text = "Sequence Tag Length:";
            // 
            // cbUseMultipleProcessors
            // 
            this.cbUseMultipleProcessors.AutoSize = true;
            this.cbUseMultipleProcessors.Location = new System.Drawing.Point(25, 247);
            this.cbUseMultipleProcessors.Name = "cbUseMultipleProcessors";
            this.cbUseMultipleProcessors.Size = new System.Drawing.Size(142, 17);
            this.cbUseMultipleProcessors.TabIndex = 10;
            this.cbUseMultipleProcessors.Text = "Use Multiple Processors ";
            this.cbUseMultipleProcessors.UseVisualStyleBackColor = true;
            // 
            // cbUseChargeStateFromMS
            // 
            this.cbUseChargeStateFromMS.AutoSize = true;
            this.cbUseChargeStateFromMS.Location = new System.Drawing.Point(25, 218);
            this.cbUseChargeStateFromMS.Name = "cbUseChargeStateFromMS";
            this.cbUseChargeStateFromMS.Size = new System.Drawing.Size(152, 17);
            this.cbUseChargeStateFromMS.TabIndex = 7;
            this.cbUseChargeStateFromMS.Text = "Use Charge State from MS";
            this.cbUseChargeStateFromMS.UseVisualStyleBackColor = true;
            // 
            // rbMono
            // 
            this.rbMono.AutoSize = true;
            this.rbMono.Location = new System.Drawing.Point(150, 185);
            this.rbMono.Name = "rbMono";
            this.rbMono.Size = new System.Drawing.Size(52, 17);
            this.rbMono.TabIndex = 6;
            this.rbMono.Text = "Mono";
            this.rbMono.UseVisualStyleBackColor = true;
            // 
            // tbNumChargeStates
            // 
            this.tbNumChargeStates.Location = new System.Drawing.Point(157, 146);
            this.tbNumChargeStates.Name = "tbNumChargeStates";
            this.tbNumChargeStates.Size = new System.Drawing.Size(40, 20);
            this.tbNumChargeStates.TabIndex = 12;
            this.tbNumChargeStates.Text = "3";
            // 
            // rbAverage
            // 
            this.rbAverage.AutoSize = true;
            this.rbAverage.Checked = true;
            this.rbAverage.Location = new System.Drawing.Point(87, 184);
            this.rbAverage.Name = "rbAverage";
            this.rbAverage.Size = new System.Drawing.Size(65, 17);
            this.rbAverage.TabIndex = 5;
            this.rbAverage.TabStop = true;
            this.rbAverage.Text = "Average";
            this.rbAverage.UseVisualStyleBackColor = true;
            // 
            // lblUseMass
            // 
            this.lblUseMass.AutoSize = true;
            this.lblUseMass.Location = new System.Drawing.Point(22, 186);
            this.lblUseMass.Name = "lblUseMass";
            this.lblUseMass.Size = new System.Drawing.Size(57, 13);
            this.lblUseMass.TabIndex = 4;
            this.lblUseMass.Text = "Use Mass:";
            // 
            // lblNumChargeStates
            // 
            this.lblNumChargeStates.AutoSize = true;
            this.lblNumChargeStates.Location = new System.Drawing.Point(22, 152);
            this.lblNumChargeStates.Name = "lblNumChargeStates";
            this.lblNumChargeStates.Size = new System.Drawing.Size(129, 13);
            this.lblNumChargeStates.TabIndex = 11;
            this.lblNumChargeStates.Text = "Number of Charge States:";
            // 
            // tbStaticMods
            // 
            this.tbStaticMods.Location = new System.Drawing.Point(94, 117);
            this.tbStaticMods.Name = "tbStaticMods";
            this.tbStaticMods.Size = new System.Drawing.Size(103, 20);
            this.tbStaticMods.TabIndex = 9;
            this.tbStaticMods.Text = "C 57.0215";
            // 
            // lblStaticMods
            // 
            this.lblStaticMods.AutoSize = true;
            this.lblStaticMods.Location = new System.Drawing.Point(22, 121);
            this.lblStaticMods.Name = "lblStaticMods";
            this.lblStaticMods.Size = new System.Drawing.Size(66, 13);
            this.lblStaticMods.TabIndex = 8;
            this.lblStaticMods.Text = "Static Mods:";
            // 
            // tbFragmentTolerance
            // 
            this.tbFragmentTolerance.Location = new System.Drawing.Point(152, 55);
            this.tbFragmentTolerance.Name = "tbFragmentTolerance";
            this.tbFragmentTolerance.Size = new System.Drawing.Size(45, 20);
            this.tbFragmentTolerance.TabIndex = 3;
            this.tbFragmentTolerance.Text = "0.5";
            // 
            // lblFragmentTolerance
            // 
            this.lblFragmentTolerance.AutoSize = true;
            this.lblFragmentTolerance.Location = new System.Drawing.Point(22, 58);
            this.lblFragmentTolerance.Name = "lblFragmentTolerance";
            this.lblFragmentTolerance.Size = new System.Drawing.Size(126, 13);
            this.lblFragmentTolerance.TabIndex = 2;
            this.lblFragmentTolerance.Text = "Fragment m/z Tolerance:";
            // 
            // tbPrecursorTolerance
            // 
            this.tbPrecursorTolerance.Location = new System.Drawing.Point(152, 25);
            this.tbPrecursorTolerance.Name = "tbPrecursorTolerance";
            this.tbPrecursorTolerance.Size = new System.Drawing.Size(45, 20);
            this.tbPrecursorTolerance.TabIndex = 1;
            this.tbPrecursorTolerance.Text = "1.25";
            // 
            // lblPrecursorTolerance
            // 
            this.lblPrecursorTolerance.AutoSize = true;
            this.lblPrecursorTolerance.Location = new System.Drawing.Point(22, 28);
            this.lblPrecursorTolerance.Name = "lblPrecursorTolerance";
            this.lblPrecursorTolerance.Size = new System.Drawing.Size(127, 13);
            this.lblPrecursorTolerance.TabIndex = 0;
            this.lblPrecursorTolerance.Text = "Precursor m/z Tolerance:";
            // 
            // cbAssessement
            // 
            this.cbAssessement.AutoSize = true;
            this.cbAssessement.Checked = true;
            this.cbAssessement.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAssessement.Location = new System.Drawing.Point(0, 0);
            this.cbAssessement.Name = "cbAssessement";
            this.cbAssessement.Size = new System.Drawing.Size(159, 17);
            this.cbAssessement.TabIndex = 0;
            this.cbAssessement.Text = "Spectral Quality Assessment";
            this.cbAssessement.UseVisualStyleBackColor = true;
            this.cbAssessement.CheckedChanged += new System.EventHandler(this.cbAssessement_CheckedChanged);
            // 
            // gbRemoval
            // 
            this.gbRemoval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRemoval.Controls.Add(this.cmbOutputFileFormat);
            this.gbRemoval.Controls.Add(this.tbOutFileNameSuffixForRemoval);
            this.gbRemoval.Controls.Add(this.lblOutFileNameSurrfixForRemoval);
            this.gbRemoval.Controls.Add(this.lblOutFileFormat);
            this.gbRemoval.Controls.Add(this.lblPercentSpectra);
            this.gbRemoval.Controls.Add(this.tbRemovalCutoff);
            this.gbRemoval.Controls.Add(this.lblRetain);
            this.gbRemoval.Controls.Add(this.tbMetricsFileSuffixForRemoval);
            this.gbRemoval.Controls.Add(this.lblMetricsFileSuffixForRemoval);
            this.gbRemoval.Controls.Add(this.cbRemoval);
            this.gbRemoval.Location = new System.Drawing.Point(329, 280);
            this.gbRemoval.Name = "gbRemoval";
            this.gbRemoval.Size = new System.Drawing.Size(361, 156);
            this.gbRemoval.TabIndex = 4;
            this.gbRemoval.TabStop = false;
            // 
            // cmbOutputFileFormat
            // 
            this.cmbOutputFileFormat.FormattingEnabled = true;
            this.cmbOutputFileFormat.Items.AddRange(new object[] {
            "mzML",
            "mzXML",
            "mgf"});
            this.cmbOutputFileFormat.Location = new System.Drawing.Point(149, 85);
            this.cmbOutputFileFormat.Name = "cmbOutputFileFormat";
            this.cmbOutputFileFormat.Size = new System.Drawing.Size(121, 21);
            this.cmbOutputFileFormat.TabIndex = 15;
            this.cmbOutputFileFormat.Text = "mzML";
            // 
            // tbOutFileNameSuffixForRemoval
            // 
            this.tbOutFileNameSuffixForRemoval.Location = new System.Drawing.Point(149, 118);
            this.tbOutFileNameSuffixForRemoval.Name = "tbOutFileNameSuffixForRemoval";
            this.tbOutFileNameSuffixForRemoval.Size = new System.Drawing.Size(205, 20);
            this.tbOutFileNameSuffixForRemoval.TabIndex = 14;
            // 
            // lblOutFileNameSurrfixForRemoval
            // 
            this.lblOutFileNameSurrfixForRemoval.AutoSize = true;
            this.lblOutFileNameSurrfixForRemoval.Location = new System.Drawing.Point(16, 121);
            this.lblOutFileNameSurrfixForRemoval.Name = "lblOutFileNameSurrfixForRemoval";
            this.lblOutFileNameSurrfixForRemoval.Size = new System.Drawing.Size(121, 13);
            this.lblOutFileNameSurrfixForRemoval.TabIndex = 13;
            this.lblOutFileNameSurrfixForRemoval.Text = "Output File Name Suffix:";
            // 
            // lblOutFileFormat
            // 
            this.lblOutFileFormat.AutoSize = true;
            this.lblOutFileFormat.Location = new System.Drawing.Point(16, 88);
            this.lblOutFileFormat.Name = "lblOutFileFormat";
            this.lblOutFileFormat.Size = new System.Drawing.Size(96, 13);
            this.lblOutFileFormat.TabIndex = 10;
            this.lblOutFileFormat.Text = "Output File Format:";
            // 
            // lblPercentSpectra
            // 
            this.lblPercentSpectra.AutoSize = true;
            this.lblPercentSpectra.Location = new System.Drawing.Point(120, 58);
            this.lblPercentSpectra.Name = "lblPercentSpectra";
            this.lblPercentSpectra.Size = new System.Drawing.Size(180, 13);
            this.lblPercentSpectra.TabIndex = 9;
            this.lblPercentSpectra.Text = "% High Quality Spectra in Output File";
            // 
            // tbRemovalCutoff
            // 
            this.tbRemovalCutoff.Location = new System.Drawing.Point(80, 55);
            this.tbRemovalCutoff.Name = "tbRemovalCutoff";
            this.tbRemovalCutoff.Size = new System.Drawing.Size(36, 20);
            this.tbRemovalCutoff.TabIndex = 8;
            this.tbRemovalCutoff.Text = "60";
            this.tbRemovalCutoff.TextChanged += new System.EventHandler(this.tbRemovalCutoff_TextChanged);
            // 
            // lblRetain
            // 
            this.lblRetain.AutoSize = true;
            this.lblRetain.Location = new System.Drawing.Point(16, 58);
            this.lblRetain.Name = "lblRetain";
            this.lblRetain.Size = new System.Drawing.Size(60, 13);
            this.lblRetain.TabIndex = 7;
            this.lblRetain.Text = "Retain Top";
            // 
            // tbMetricsFileSuffixForRemoval
            // 
            this.tbMetricsFileSuffixForRemoval.Location = new System.Drawing.Point(149, 26);
            this.tbMetricsFileSuffixForRemoval.Name = "tbMetricsFileSuffixForRemoval";
            this.tbMetricsFileSuffixForRemoval.Size = new System.Drawing.Size(205, 20);
            this.tbMetricsFileSuffixForRemoval.TabIndex = 6;
            // 
            // lblMetricsFileSuffixForRemoval
            // 
            this.lblMetricsFileSuffixForRemoval.AutoSize = true;
            this.lblMetricsFileSuffixForRemoval.Location = new System.Drawing.Point(16, 29);
            this.lblMetricsFileSuffixForRemoval.Name = "lblMetricsFileSuffixForRemoval";
            this.lblMetricsFileSuffixForRemoval.Size = new System.Drawing.Size(127, 13);
            this.lblMetricsFileSuffixForRemoval.TabIndex = 5;
            this.lblMetricsFileSuffixForRemoval.Text = "Quality Metrics File Suffix:";
            // 
            // cbRemoval
            // 
            this.cbRemoval.AutoSize = true;
            this.cbRemoval.Checked = true;
            this.cbRemoval.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRemoval.Location = new System.Drawing.Point(0, 0);
            this.cbRemoval.Name = "cbRemoval";
            this.cbRemoval.Size = new System.Drawing.Size(108, 17);
            this.cbRemoval.TabIndex = 0;
            this.cbRemoval.Text = "Spectra Removal";
            this.cbRemoval.UseVisualStyleBackColor = true;
            this.cbRemoval.CheckedChanged += new System.EventHandler(this.cbRemoval_CheckedChanged);
            // 
            // gbRecovery
            // 
            this.gbRecovery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRecovery.Controls.Add(this.cbOptimizeScoreWeights);
            this.gbRecovery.Controls.Add(this.cmbScoreWeights);
            this.gbRecovery.Controls.Add(this.cbNormSearchScores);
            this.gbRecovery.Controls.Add(this.tbDecoyPrefix);
            this.gbRecovery.Controls.Add(this.tbMaxFDR);
            this.gbRecovery.Controls.Add(this.lblDecoyPrefix);
            this.gbRecovery.Controls.Add(this.lblSearchScoreWeights);
            this.gbRecovery.Controls.Add(this.lblMaxFDR);
            this.gbRecovery.Controls.Add(this.btnDBBrowse);
            this.gbRecovery.Controls.Add(this.tbDBFile);
            this.gbRecovery.Controls.Add(this.lblDBFile);
            this.gbRecovery.Controls.Add(this.tbPepXMLDir);
            this.gbRecovery.Controls.Add(this.btnPepXMLBrowse);
            this.gbRecovery.Controls.Add(this.lblPepXMLDir);
            this.gbRecovery.Controls.Add(this.tbOutFileNameSuffixForRecovery);
            this.gbRecovery.Controls.Add(this.lblOutFileNameSuffixForRecovery);
            this.gbRecovery.Controls.Add(this.cbRecovery);
            this.gbRecovery.Location = new System.Drawing.Point(329, 448);
            this.gbRecovery.Name = "gbRecovery";
            this.gbRecovery.Size = new System.Drawing.Size(361, 206);
            this.gbRecovery.TabIndex = 5;
            this.gbRecovery.TabStop = false;
            this.gbRecovery.Text = " ";
            // 
            // cbOptimizeScoreWeights
            // 
            this.cbOptimizeScoreWeights.AutoSize = true;
            this.cbOptimizeScoreWeights.Checked = true;
            this.cbOptimizeScoreWeights.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOptimizeScoreWeights.Location = new System.Drawing.Point(206, 139);
            this.cbOptimizeScoreWeights.Name = "cbOptimizeScoreWeights";
            this.cbOptimizeScoreWeights.Size = new System.Drawing.Size(142, 17);
            this.cbOptimizeScoreWeights.TabIndex = 16;
            this.cbOptimizeScoreWeights.Text = "Optimize Score Weights ";
            this.cbOptimizeScoreWeights.UseVisualStyleBackColor = true;
            // 
            // cmbScoreWeights
            // 
            this.cmbScoreWeights.FormattingEnabled = true;
            this.cmbScoreWeights.Items.AddRange(new object[] {
            "mvh 1 mzFidelity 1",
            "xcorr 1 deltacn 1",
            "hyperscore 1 expect -1",
            "ionscore 1 identityscore -1"});
            this.cmbScoreWeights.Location = new System.Drawing.Point(17, 137);
            this.cmbScoreWeights.Name = "cmbScoreWeights";
            this.cmbScoreWeights.Size = new System.Drawing.Size(166, 21);
            this.cmbScoreWeights.TabIndex = 17;
            // 
            // cbNormSearchScores
            // 
            this.cbNormSearchScores.AutoSize = true;
            this.cbNormSearchScores.Checked = true;
            this.cbNormSearchScores.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNormSearchScores.Location = new System.Drawing.Point(206, 120);
            this.cbNormSearchScores.Name = "cbNormSearchScores";
            this.cbNormSearchScores.Size = new System.Drawing.Size(148, 17);
            this.cbNormSearchScores.TabIndex = 15;
            this.cbNormSearchScores.Text = "Normalize Search Scores ";
            this.cbNormSearchScores.UseVisualStyleBackColor = true;
            // 
            // tbDecoyPrefix
            // 
            this.tbDecoyPrefix.Location = new System.Drawing.Point(282, 85);
            this.tbDecoyPrefix.Name = "tbDecoyPrefix";
            this.tbDecoyPrefix.Size = new System.Drawing.Size(72, 20);
            this.tbDecoyPrefix.TabIndex = 23;
            this.tbDecoyPrefix.Text = "rev_";
            // 
            // tbMaxFDR
            // 
            this.tbMaxFDR.Location = new System.Drawing.Point(75, 85);
            this.tbMaxFDR.Name = "tbMaxFDR";
            this.tbMaxFDR.Size = new System.Drawing.Size(72, 20);
            this.tbMaxFDR.TabIndex = 22;
            this.tbMaxFDR.Text = "0.05";
            // 
            // lblDecoyPrefix
            // 
            this.lblDecoyPrefix.AutoSize = true;
            this.lblDecoyPrefix.Location = new System.Drawing.Point(203, 88);
            this.lblDecoyPrefix.Name = "lblDecoyPrefix";
            this.lblDecoyPrefix.Size = new System.Drawing.Size(70, 13);
            this.lblDecoyPrefix.TabIndex = 21;
            this.lblDecoyPrefix.Text = "Decoy Prefix:";
            // 
            // lblSearchScoreWeights
            // 
            this.lblSearchScoreWeights.AutoSize = true;
            this.lblSearchScoreWeights.Location = new System.Drawing.Point(14, 118);
            this.lblSearchScoreWeights.Name = "lblSearchScoreWeights";
            this.lblSearchScoreWeights.Size = new System.Drawing.Size(169, 13);
            this.lblSearchScoreWeights.TabIndex = 14;
            this.lblSearchScoreWeights.Text = "Database Search Score Weights: ";
            // 
            // lblMaxFDR
            // 
            this.lblMaxFDR.AutoSize = true;
            this.lblMaxFDR.Location = new System.Drawing.Point(14, 88);
            this.lblMaxFDR.Name = "lblMaxFDR";
            this.lblMaxFDR.Size = new System.Drawing.Size(55, 13);
            this.lblMaxFDR.TabIndex = 20;
            this.lblMaxFDR.Text = "Max FDR:";
            // 
            // btnDBBrowse
            // 
            this.btnDBBrowse.Location = new System.Drawing.Point(281, 50);
            this.btnDBBrowse.Name = "btnDBBrowse";
            this.btnDBBrowse.Size = new System.Drawing.Size(73, 21);
            this.btnDBBrowse.TabIndex = 19;
            this.btnDBBrowse.Text = "Browse";
            this.btnDBBrowse.UseVisualStyleBackColor = true;
            this.btnDBBrowse.Click += new System.EventHandler(this.btnDBBrowse_Click);
            // 
            // tbDBFile
            // 
            this.tbDBFile.Location = new System.Drawing.Point(120, 51);
            this.tbDBFile.Name = "tbDBFile";
            this.tbDBFile.Size = new System.Drawing.Size(155, 20);
            this.tbDBFile.TabIndex = 18;
            // 
            // lblDBFile
            // 
            this.lblDBFile.AutoSize = true;
            this.lblDBFile.Location = new System.Drawing.Point(14, 58);
            this.lblDBFile.Name = "lblDBFile";
            this.lblDBFile.Size = new System.Drawing.Size(75, 13);
            this.lblDBFile.TabIndex = 17;
            this.lblDBFile.Text = "Database File:";
            // 
            // tbPepXMLDir
            // 
            this.tbPepXMLDir.Location = new System.Drawing.Point(120, 25);
            this.tbPepXMLDir.Name = "tbPepXMLDir";
            this.tbPepXMLDir.Size = new System.Drawing.Size(155, 20);
            this.tbPepXMLDir.TabIndex = 14;
            // 
            // btnPepXMLBrowse
            // 
            this.btnPepXMLBrowse.Location = new System.Drawing.Point(281, 25);
            this.btnPepXMLBrowse.Name = "btnPepXMLBrowse";
            this.btnPepXMLBrowse.Size = new System.Drawing.Size(73, 21);
            this.btnPepXMLBrowse.TabIndex = 14;
            this.btnPepXMLBrowse.Text = "Browse";
            this.btnPepXMLBrowse.UseVisualStyleBackColor = true;
            this.btnPepXMLBrowse.Click += new System.EventHandler(this.btnPepXMLBrowse_Click);
            // 
            // lblPepXMLDir
            // 
            this.lblPepXMLDir.AutoSize = true;
            this.lblPepXMLDir.Location = new System.Drawing.Point(14, 29);
            this.lblPepXMLDir.Name = "lblPepXMLDir";
            this.lblPepXMLDir.Size = new System.Drawing.Size(100, 13);
            this.lblPepXMLDir.TabIndex = 16;
            this.lblPepXMLDir.Text = "pepXMLs Directory:";
            // 
            // tbOutFileNameSuffixForRecovery
            // 
            this.tbOutFileNameSuffixForRecovery.Location = new System.Drawing.Point(147, 171);
            this.tbOutFileNameSuffixForRecovery.Name = "tbOutFileNameSuffixForRecovery";
            this.tbOutFileNameSuffixForRecovery.Size = new System.Drawing.Size(207, 20);
            this.tbOutFileNameSuffixForRecovery.TabIndex = 15;
            // 
            // lblOutFileNameSuffixForRecovery
            // 
            this.lblOutFileNameSuffixForRecovery.AutoSize = true;
            this.lblOutFileNameSuffixForRecovery.Location = new System.Drawing.Point(14, 174);
            this.lblOutFileNameSuffixForRecovery.Name = "lblOutFileNameSuffixForRecovery";
            this.lblOutFileNameSuffixForRecovery.Size = new System.Drawing.Size(121, 13);
            this.lblOutFileNameSuffixForRecovery.TabIndex = 15;
            this.lblOutFileNameSuffixForRecovery.Text = "Output File Name Suffix:";
            // 
            // cbRecovery
            // 
            this.cbRecovery.AutoSize = true;
            this.cbRecovery.Location = new System.Drawing.Point(0, 0);
            this.cbRecovery.Name = "cbRecovery";
            this.cbRecovery.Size = new System.Drawing.Size(112, 17);
            this.cbRecovery.TabIndex = 0;
            this.cbRecovery.Text = "Spectra Recovery";
            this.cbRecovery.UseVisualStyleBackColor = true;
            this.cbRecovery.CheckedChanged += new System.EventHandler(this.cbRecovery_CheckedChanged);
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(515, 660);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(80, 23);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(608, 660);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblOutputDir
            // 
            this.lblOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOutputDir.AutoSize = true;
            this.lblOutputDir.Location = new System.Drawing.Point(23, 245);
            this.lblOutputDir.Name = "lblOutputDir";
            this.lblOutputDir.Size = new System.Drawing.Size(87, 13);
            this.lblOutputDir.TabIndex = 8;
            this.lblOutputDir.Text = "Output Directory:";
            // 
            // tbOutputDir
            // 
            this.tbOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputDir.Location = new System.Drawing.Point(113, 242);
            this.tbOutputDir.Name = "tbOutputDir";
            this.tbOutputDir.Size = new System.Drawing.Size(496, 20);
            this.tbOutputDir.TabIndex = 9;
            // 
            // btnOutputDirBrowse
            // 
            this.btnOutputDirBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOutputDirBrowse.Location = new System.Drawing.Point(615, 241);
            this.btnOutputDirBrowse.Name = "btnOutputDirBrowse";
            this.btnOutputDirBrowse.Size = new System.Drawing.Size(73, 21);
            this.btnOutputDirBrowse.TabIndex = 10;
            this.btnOutputDirBrowse.Text = "Browse";
            this.btnOutputDirBrowse.UseVisualStyleBackColor = true;
            this.btnOutputDirBrowse.Click += new System.EventHandler(this.btnOutputDirBrowse_Click);
            // 
            // bgDirectagRun
            // 
            this.bgDirectagRun.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDirectagRun_DoWork);
            // 
            // bgWriteSpectra
            // 
            this.bgWriteSpectra.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWriteSpectra_DoWork);
            // 
            // bgAddLabels
            // 
            this.bgAddLabels.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgAddLabels_DoWork);
            // 
            // tvSelDirs
            // 
            this.tvSelDirs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvSelDirs.CheckBoxes = true;
            this.tvSelDirs.Location = new System.Drawing.Point(26, 77);
            this.tvSelDirs.Name = "tvSelDirs";
            this.tvSelDirs.Size = new System.Drawing.Size(662, 158);
            this.tvSelDirs.TabIndex = 11;
            this.tvSelDirs.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvSelDirs_AfterCheck);
            // 
            // lblInputFileFilters
            // 
            this.lblInputFileFilters.AutoSize = true;
            this.lblInputFileFilters.Location = new System.Drawing.Point(23, 52);
            this.lblInputFileFilters.Name = "lblInputFileFilters";
            this.lblInputFileFilters.Size = new System.Drawing.Size(83, 13);
            this.lblInputFileFilters.TabIndex = 12;
            this.lblInputFileFilters.Text = "Input File Filters:";
            // 
            // tbInputFileFilters
            // 
            this.tbInputFileFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInputFileFilters.Location = new System.Drawing.Point(115, 49);
            this.tbInputFileFilters.Name = "tbInputFileFilters";
            this.tbInputFileFilters.Size = new System.Drawing.Size(496, 20);
            this.tbInputFileFilters.TabIndex = 13;
            this.tbInputFileFilters.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbInputFileFilters_KeyPress);
            // 
            // btnListFiles
            // 
            this.btnListFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnListFiles.Location = new System.Drawing.Point(617, 48);
            this.btnListFiles.Name = "btnListFiles";
            this.btnListFiles.Size = new System.Drawing.Size(73, 21);
            this.btnListFiles.TabIndex = 1;
            this.btnListFiles.Text = "List Files";
            this.btnListFiles.UseVisualStyleBackColor = true;
            this.btnListFiles.Click += new System.EventHandler(this.btnListFiles_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 695);
            this.Controls.Add(this.btnListFiles);
            this.Controls.Add(this.tbInputFileFilters);
            this.Controls.Add(this.lblInputFileFilters);
            this.Controls.Add(this.tvSelDirs);
            this.Controls.Add(this.btnOutputDirBrowse);
            this.Controls.Add(this.tbOutputDir);
            this.Controls.Add(this.lblOutputDir);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.gbRecovery);
            this.Controls.Add(this.gbRemoval);
            this.Controls.Add(this.gbAssessment);
            this.Controls.Add(this.btnSrcFileBrowse);
            this.Controls.Add(this.tbSrcDir);
            this.Controls.Add(this.lblInputFileDir);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "ScanRanker";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbAssessment.ResumeLayout(false);
            this.gbAssessment.PerformLayout();
            this.gbDtgConfig.ResumeLayout(false);
            this.gbDtgConfig.PerformLayout();
            this.gbRemoval.ResumeLayout(false);
            this.gbRemoval.PerformLayout();
            this.gbRecovery.ResumeLayout(false);
            this.gbRecovery.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInputFileDir;
        private System.Windows.Forms.TextBox tbSrcDir;
        private System.Windows.Forms.Button btnSrcFileBrowse;
        private System.Windows.Forms.GroupBox gbAssessment;
        private System.Windows.Forms.CheckBox cbAssessement;
        private System.Windows.Forms.GroupBox gbDtgConfig;
        private System.Windows.Forms.TextBox tbOutputMetricsSuffix;
        private System.Windows.Forms.Label lblOutputMetricsSurfix;
        private System.Windows.Forms.GroupBox gbRemoval;
        private System.Windows.Forms.CheckBox cbRemoval;
        private System.Windows.Forms.GroupBox gbRecovery;
        private System.Windows.Forms.Label lblRetain;
        private System.Windows.Forms.TextBox tbMetricsFileSuffixForRemoval;
        private System.Windows.Forms.Label lblMetricsFileSuffixForRemoval;
        private System.Windows.Forms.CheckBox cbRecovery;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblOutFileFormat;
        private System.Windows.Forms.Label lblPercentSpectra;
        private System.Windows.Forms.TextBox tbRemovalCutoff;
        private System.Windows.Forms.Label lblOutFileNameSurrfixForRemoval;
        private System.Windows.Forms.TextBox tbOutFileNameSuffixForRemoval;
        private System.Windows.Forms.TextBox tbOutFileNameSuffixForRecovery;
        private System.Windows.Forms.Label lblOutFileNameSuffixForRecovery;
        private System.Windows.Forms.Label lblPrecursorTolerance;
        private System.Windows.Forms.Label lblUseMass;
        private System.Windows.Forms.TextBox tbFragmentTolerance;
        private System.Windows.Forms.Label lblFragmentTolerance;
        private System.Windows.Forms.TextBox tbPrecursorTolerance;
        private System.Windows.Forms.CheckBox cbUseChargeStateFromMS;
        private System.Windows.Forms.RadioButton rbMono;
        private System.Windows.Forms.RadioButton rbAverage;
        private System.Windows.Forms.TextBox tbNumChargeStates;
        private System.Windows.Forms.Label lblNumChargeStates;
        private System.Windows.Forms.CheckBox cbUseMultipleProcessors;
        private System.Windows.Forms.TextBox tbStaticMods;
        private System.Windows.Forms.Label lblStaticMods;
        private System.Windows.Forms.TextBox tbTagLength;
        private System.Windows.Forms.Label lblTagLength;
        private System.Windows.Forms.ComboBox cmbOutputFileFormat;
        private System.Windows.Forms.Label lblOutputDir;
        private System.Windows.Forms.TextBox tbOutputDir;
        private System.Windows.Forms.Button btnOutputDirBrowse;
        private System.Windows.Forms.CheckBox cbWriteOutTags;
        public System.ComponentModel.BackgroundWorker bgDirectagRun;
        public System.ComponentModel.BackgroundWorker bgWriteSpectra;
        public System.ComponentModel.BackgroundWorker bgAddLabels;
        private System.Windows.Forms.TreeView tvSelDirs;
        private System.Windows.Forms.Label lblInputFileFilters;
        private System.Windows.Forms.TextBox tbInputFileFilters;
        private System.Windows.Forms.Button btnListFiles;
        private System.Windows.Forms.Button btnPepXMLBrowse;
        private System.Windows.Forms.Label lblPepXMLDir;
        private System.Windows.Forms.TextBox tbPepXMLDir;
        private System.Windows.Forms.Label lblDBFile;
        private System.Windows.Forms.Button btnDBBrowse;
        private System.Windows.Forms.TextBox tbDBFile;
        private System.Windows.Forms.TextBox tbMaxFDR;
        private System.Windows.Forms.Label lblDecoyPrefix;
        private System.Windows.Forms.Label lblMaxFDR;
        private System.Windows.Forms.TextBox tbDecoyPrefix;
        private System.Windows.Forms.CheckBox cbOptimizeScoreWeights;
        private System.Windows.Forms.ComboBox cmbScoreWeights;
        private System.Windows.Forms.CheckBox cbNormSearchScores;
        private System.Windows.Forms.Label lblSearchScoreWeights;


        
    }
}

