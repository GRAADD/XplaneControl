using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ControlPadTest
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TcpDataFound = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.WeatherPage = new System.Windows.Forms.TabPage();
            this.SendWxr = new System.Windows.Forms.Button();
            this.WeatherControlTabs = new System.Windows.Forms.TabControl();
            this.WeatherParamsPage = new System.Windows.Forms.TabPage();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.DayLabel = new System.Windows.Forms.Label();
            this.timeBar = new System.Windows.Forms.TrackBar();
            this.dayBar = new System.Windows.Forms.TrackBar();
            this.ArmospherePage = new System.Windows.Forms.TabPage();
            this.ThermalStrLabel = new System.Windows.Forms.Label();
            this.ThermCovLabel = new System.Windows.Forms.Label();
            this.BaroLabel = new System.Windows.Forms.Label();
            this.TempLabel = new System.Windows.Forms.Label();
            this.StormLabel = new System.Windows.Forms.Label();
            this.RainLabel = new System.Windows.Forms.Label();
            this.VisibilityLabel = new System.Windows.Forms.Label();
            this.ThermalStrength = new System.Windows.Forms.TrackBar();
            this.ThermalCover = new System.Windows.Forms.TrackBar();
            this.Baro = new System.Windows.Forms.TrackBar();
            this.Temperature = new System.Windows.Forms.TrackBar();
            this.PatchyCheckBox = new System.Windows.Forms.CheckBox();
            this.WetRadio = new System.Windows.Forms.RadioButton();
            this.DampRadio = new System.Windows.Forms.RadioButton();
            this.DryRadio = new System.Windows.Forms.RadioButton();
            this.StormBar = new System.Windows.Forms.TrackBar();
            this.RainBar = new System.Windows.Forms.TrackBar();
            this.VisibilityBar = new System.Windows.Forms.TrackBar();
            this.WindPage = new System.Windows.Forms.TabPage();
            this.Wind1StrengthLabel = new System.Windows.Forms.Label();
            this.Wind1DirLabel = new System.Windows.Forms.Label();
            this.Wind1GustsLabel = new System.Windows.Forms.Label();
            this.Wind1AltitudeLabel = new System.Windows.Forms.Label();
            this.Wind1TurbLabel = new System.Windows.Forms.Label();
            this.Wind2AltitudeLabel = new System.Windows.Forms.Label();
            this.Wind2Altitude = new System.Windows.Forms.TrackBar();
            this.Wind3TurbLabel = new System.Windows.Forms.Label();
            this.Wind1Altitude = new System.Windows.Forms.TrackBar();
            this.Wind3GustsLabel = new System.Windows.Forms.Label();
            this.Wind2DirLabel = new System.Windows.Forms.Label();
            this.Wind3Gust = new System.Windows.Forms.TrackBar();
            this.Wind3StrengthLabel = new System.Windows.Forms.Label();
            this.Wind3Strength = new System.Windows.Forms.TrackBar();
            this.Wind1Turb = new System.Windows.Forms.TrackBar();
            this.Wind3DirLabel = new System.Windows.Forms.Label();
            this.Wind1Direction = new System.Windows.Forms.TrackBar();
            this.Wind2StrengthLabel = new System.Windows.Forms.Label();
            this.Wind2TurbLabel = new System.Windows.Forms.Label();
            this.Wind3AltitudeLabel = new System.Windows.Forms.Label();
            this.Wind1Strength = new System.Windows.Forms.TrackBar();
            this.Wind1Gust = new System.Windows.Forms.TrackBar();
            this.Wind2GustsLabel = new System.Windows.Forms.Label();
            this.Wind3Turb = new System.Windows.Forms.TrackBar();
            this.Wind3Altitude = new System.Windows.Forms.TrackBar();
            this.Wind2Turb = new System.Windows.Forms.TrackBar();
            this.Wind2Gust = new System.Windows.Forms.TrackBar();
            this.Wind2Strength = new System.Windows.Forms.TrackBar();
            this.Wind2Direction = new System.Windows.Forms.TrackBar();
            this.Wind3Direction = new System.Windows.Forms.TrackBar();
            this.CloudsPage = new System.Windows.Forms.TabPage();
            this.Cloud1TopLabel = new System.Windows.Forms.Label();
            this.Cloud1BaseLabel = new System.Windows.Forms.Label();
            this.Cloud3TopLabel = new System.Windows.Forms.Label();
            this.Cloud3BaseLabel = new System.Windows.Forms.Label();
            this.Cloud1Top = new System.Windows.Forms.TrackBar();
            this.Cloud2BaseLabel = new System.Windows.Forms.Label();
            this.Cloud1Base = new System.Windows.Forms.TrackBar();
            this.Cloud2TopLabel = new System.Windows.Forms.Label();
            this.Cloud3Base = new System.Windows.Forms.TrackBar();
            this.Cloud3Top = new System.Windows.Forms.TrackBar();
            this.Cloud2Base = new System.Windows.Forms.TrackBar();
            this.Cloud2Top = new System.Windows.Forms.TrackBar();
            this.airportsTextBox = new System.Windows.Forms.TextBox();
            this.CmdButtonsTabs = new System.Windows.Forms.TabControl();
            this.WgtLabel = new System.Windows.Forms.Label();
            this.DimBox = new System.Windows.Forms.ListBox();
            this.DimLabel = new System.Windows.Forms.Label();
            this.RadLabel = new System.Windows.Forms.Label();
            this.FailsListBox = new System.Windows.Forms.ListBox();
            this.AcfsListBox = new System.Windows.Forms.ListBox();
            this.Message_TextBox = new System.Windows.Forms.TextBox();
            this.UDP_TextBox = new System.Windows.Forms.TextBox();
            this.SendUDP = new System.Windows.Forms.Button();
            this.SendTCP = new System.Windows.Forms.Button();
            this.CentralControl = new System.Windows.Forms.TabControl();
            this.MapPage = new System.Windows.Forms.TabPage();
            this.Map = new GMap.NET.WindowsForms.GMapControl();
            this.TextBoxSpd = new System.Windows.Forms.TextBox();
            this.TextBoxHdg = new System.Windows.Forms.TextBox();
            this.TextBoxAgl = new System.Windows.Forms.TextBox();
            this.TextBoxAsl = new System.Windows.Forms.TextBox();
            this.TextBoxLat = new System.Windows.Forms.TextBox();
            this.TextBoxLng = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mapProviderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleMapsSatteliteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloudMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arcGISTerrainBaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FailTabs = new System.Windows.Forms.TabControl();
            this.CommandsTabControl = new System.Windows.Forms.TabControl();
            this.ConnectPage = new System.Windows.Forms.TabPage();
            this.ConnectNoMasterBtn = new System.Windows.Forms.Button();
            this.MasterIpTxtBx = new System.Windows.Forms.TextBox();
            this.FailsPage = new System.Windows.Forms.TabPage();
            this.AircraftsPage = new System.Windows.Forms.TabPage();
            this.CmdButtons = new System.Windows.Forms.TabPage();
            this.MiscCommands = new System.Windows.Forms.TabPage();
            this.WriteLogs = new System.Windows.Forms.Button();
            this.LogsBox = new System.Windows.Forms.TextBox();
            this.MainControl = new System.Windows.Forms.TabControl();
            this.AirportsPage = new System.Windows.Forms.TabPage();
            this.AirportControls = new System.Windows.Forms.Panel();
            this.AirportChoose = new System.Windows.Forms.TextBox();
            this.ConnectionStatusPage = new System.Windows.Forms.TabPage();
            this.ReleaseNotesPage = new System.Windows.Forms.TabPage();
            this.ToDoTextBox = new System.Windows.Forms.TextBox();
            this.RealesNotesTextBox = new System.Windows.Forms.TextBox();
            this.Cloud1Type = new System.Windows.Forms.TrackBar();
            this.Cloud2Type = new System.Windows.Forms.TrackBar();
            this.Cloud3Type = new System.Windows.Forms.TrackBar();
            this.statusStrip1.SuspendLayout();
            this.WeatherPage.SuspendLayout();
            this.WeatherControlTabs.SuspendLayout();
            this.WeatherParamsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dayBar)).BeginInit();
            this.ArmospherePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThermalStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThermalCover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Baro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Temperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StormBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RainBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VisibilityBar)).BeginInit();
            this.WindPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Altitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Altitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Gust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Strength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Turb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Direction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Strength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Gust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Turb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Altitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Turb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Gust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Strength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Direction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Direction)).BeginInit();
            this.CloudsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud1Top)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud1Base)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud3Base)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud3Top)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud2Base)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud2Top)).BeginInit();
            this.CentralControl.SuspendLayout();
            this.MapPage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.CommandsTabControl.SuspendLayout();
            this.ConnectPage.SuspendLayout();
            this.FailsPage.SuspendLayout();
            this.CmdButtons.SuspendLayout();
            this.MiscCommands.SuspendLayout();
            this.MainControl.SuspendLayout();
            this.AirportsPage.SuspendLayout();
            this.ReleaseNotesPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud1Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud2Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud3Type)).BeginInit();
            this.SuspendLayout();
            // 
            // TcpDataFound
            // 
            this.TcpDataFound.AutoEllipsis = true;
            this.TcpDataFound.AutoSize = true;
            this.TcpDataFound.Location = new System.Drawing.Point(6, 85);
            this.TcpDataFound.Name = "TcpDataFound";
            this.TcpDataFound.Size = new System.Drawing.Size(85, 13);
            this.TcpDataFound.TabIndex = 7;
            this.TcpDataFound.Text = "Connection data";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar,
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 597);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1439, 24);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(500, 18);
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(13, 19);
            this.StatusLabel.Text = "0";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "csv file| *.csv";
            // 
            // WeatherPage
            // 
            this.WeatherPage.Controls.Add(this.SendWxr);
            this.WeatherPage.Controls.Add(this.WeatherControlTabs);
            this.WeatherPage.Location = new System.Drawing.Point(4, 22);
            this.WeatherPage.Name = "WeatherPage";
            this.WeatherPage.Size = new System.Drawing.Size(534, 558);
            this.WeatherPage.TabIndex = 2;
            this.WeatherPage.Text = "Weather";
            this.WeatherPage.UseVisualStyleBackColor = true;
            // 
            // SendWxr
            // 
            this.SendWxr.Location = new System.Drawing.Point(4, 532);
            this.SendWxr.Name = "SendWxr";
            this.SendWxr.Size = new System.Drawing.Size(524, 23);
            this.SendWxr.TabIndex = 50;
            this.SendWxr.Text = "Send params";
            this.SendWxr.UseVisualStyleBackColor = true;
            this.SendWxr.Click += new System.EventHandler(this.SendWxr_Click);
            // 
            // WeatherControlTabs
            // 
            this.WeatherControlTabs.Controls.Add(this.WeatherParamsPage);
            this.WeatherControlTabs.Controls.Add(this.ArmospherePage);
            this.WeatherControlTabs.Controls.Add(this.WindPage);
            this.WeatherControlTabs.Controls.Add(this.CloudsPage);
            this.WeatherControlTabs.Location = new System.Drawing.Point(4, 6);
            this.WeatherControlTabs.Name = "WeatherControlTabs";
            this.WeatherControlTabs.SelectedIndex = 0;
            this.WeatherControlTabs.Size = new System.Drawing.Size(525, 520);
            this.WeatherControlTabs.TabIndex = 50;
            // 
            // WeatherParamsPage
            // 
            this.WeatherParamsPage.Controls.Add(this.TimeLabel);
            this.WeatherParamsPage.Controls.Add(this.DayLabel);
            this.WeatherParamsPage.Controls.Add(this.timeBar);
            this.WeatherParamsPage.Controls.Add(this.dayBar);
            this.WeatherParamsPage.Location = new System.Drawing.Point(4, 22);
            this.WeatherParamsPage.Name = "WeatherParamsPage";
            this.WeatherParamsPage.Padding = new System.Windows.Forms.Padding(3);
            this.WeatherParamsPage.Size = new System.Drawing.Size(517, 494);
            this.WeatherParamsPage.TabIndex = 1;
            this.WeatherParamsPage.Text = "Time and presets";
            this.WeatherParamsPage.UseVisualStyleBackColor = true;
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Location = new System.Drawing.Point(217, 89);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(35, 13);
            this.TimeLabel.TabIndex = 7;
            this.TimeLabel.Text = "Timex";
            // 
            // DayLabel
            // 
            this.DayLabel.AutoSize = true;
            this.DayLabel.Location = new System.Drawing.Point(220, 38);
            this.DayLabel.Name = "DayLabel";
            this.DayLabel.Size = new System.Drawing.Size(26, 13);
            this.DayLabel.TabIndex = 9;
            this.DayLabel.Text = "Day";
            // 
            // timeBar
            // 
            this.timeBar.Location = new System.Drawing.Point(6, 57);
            this.timeBar.Maximum = 240;
            this.timeBar.Minimum = 1;
            this.timeBar.Name = "timeBar";
            this.timeBar.Size = new System.Drawing.Size(505, 45);
            this.timeBar.TabIndex = 0;
            this.timeBar.Value = 120;
            this.timeBar.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // dayBar
            // 
            this.dayBar.Location = new System.Drawing.Point(6, 6);
            this.dayBar.Maximum = 365;
            this.dayBar.Minimum = 1;
            this.dayBar.Name = "dayBar";
            this.dayBar.Size = new System.Drawing.Size(505, 45);
            this.dayBar.TabIndex = 1;
            this.dayBar.TickFrequency = 7;
            this.dayBar.Value = 182;
            this.dayBar.Scroll += new System.EventHandler(this.dayBar_Scroll);
            this.dayBar.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // ArmospherePage
            // 
            this.ArmospherePage.Controls.Add(this.ThermalStrLabel);
            this.ArmospherePage.Controls.Add(this.ThermCovLabel);
            this.ArmospherePage.Controls.Add(this.BaroLabel);
            this.ArmospherePage.Controls.Add(this.TempLabel);
            this.ArmospherePage.Controls.Add(this.StormLabel);
            this.ArmospherePage.Controls.Add(this.RainLabel);
            this.ArmospherePage.Controls.Add(this.VisibilityLabel);
            this.ArmospherePage.Controls.Add(this.ThermalStrength);
            this.ArmospherePage.Controls.Add(this.ThermalCover);
            this.ArmospherePage.Controls.Add(this.Baro);
            this.ArmospherePage.Controls.Add(this.Temperature);
            this.ArmospherePage.Controls.Add(this.PatchyCheckBox);
            this.ArmospherePage.Controls.Add(this.WetRadio);
            this.ArmospherePage.Controls.Add(this.DampRadio);
            this.ArmospherePage.Controls.Add(this.DryRadio);
            this.ArmospherePage.Controls.Add(this.StormBar);
            this.ArmospherePage.Controls.Add(this.RainBar);
            this.ArmospherePage.Controls.Add(this.VisibilityBar);
            this.ArmospherePage.Location = new System.Drawing.Point(4, 22);
            this.ArmospherePage.Name = "ArmospherePage";
            this.ArmospherePage.Padding = new System.Windows.Forms.Padding(3);
            this.ArmospherePage.Size = new System.Drawing.Size(517, 494);
            this.ArmospherePage.TabIndex = 0;
            this.ArmospherePage.Text = "Armosphere params";
            this.ArmospherePage.UseVisualStyleBackColor = true;
            // 
            // ThermalStrLabel
            // 
            this.ThermalStrLabel.AutoSize = true;
            this.ThermalStrLabel.Location = new System.Drawing.Point(221, 317);
            this.ThermalStrLabel.Name = "ThermalStrLabel";
            this.ThermalStrLabel.Size = new System.Drawing.Size(35, 13);
            this.ThermalStrLabel.TabIndex = 19;
            this.ThermalStrLabel.Text = "Timex";
            // 
            // ThermCovLabel
            // 
            this.ThermCovLabel.AutoSize = true;
            this.ThermCovLabel.Location = new System.Drawing.Point(221, 266);
            this.ThermCovLabel.Name = "ThermCovLabel";
            this.ThermCovLabel.Size = new System.Drawing.Size(35, 13);
            this.ThermCovLabel.TabIndex = 18;
            this.ThermCovLabel.Text = "Timex";
            // 
            // BaroLabel
            // 
            this.BaroLabel.AutoSize = true;
            this.BaroLabel.Location = new System.Drawing.Point(221, 215);
            this.BaroLabel.Name = "BaroLabel";
            this.BaroLabel.Size = new System.Drawing.Size(35, 13);
            this.BaroLabel.TabIndex = 17;
            this.BaroLabel.Text = "Timex";
            // 
            // TempLabel
            // 
            this.TempLabel.AutoSize = true;
            this.TempLabel.Location = new System.Drawing.Point(221, 164);
            this.TempLabel.Name = "TempLabel";
            this.TempLabel.Size = new System.Drawing.Size(35, 13);
            this.TempLabel.TabIndex = 16;
            this.TempLabel.Text = "Timex";
            // 
            // StormLabel
            // 
            this.StormLabel.AutoSize = true;
            this.StormLabel.Location = new System.Drawing.Point(221, 89);
            this.StormLabel.Name = "StormLabel";
            this.StormLabel.Size = new System.Drawing.Size(35, 13);
            this.StormLabel.TabIndex = 15;
            this.StormLabel.Text = "Timex";
            // 
            // RainLabel
            // 
            this.RainLabel.AutoSize = true;
            this.RainLabel.Location = new System.Drawing.Point(221, 38);
            this.RainLabel.Name = "RainLabel";
            this.RainLabel.Size = new System.Drawing.Size(35, 13);
            this.RainLabel.TabIndex = 14;
            this.RainLabel.Text = "Timex";
            // 
            // VisibilityLabel
            // 
            this.VisibilityLabel.AutoSize = true;
            this.VisibilityLabel.Location = new System.Drawing.Point(54, 475);
            this.VisibilityLabel.Name = "VisibilityLabel";
            this.VisibilityLabel.Size = new System.Drawing.Size(35, 13);
            this.VisibilityLabel.TabIndex = 13;
            this.VisibilityLabel.Text = "Timex";
            // 
            // ThermalStrength
            // 
            this.ThermalStrength.Location = new System.Drawing.Point(57, 285);
            this.ThermalStrength.Maximum = 150000;
            this.ThermalStrength.Name = "ThermalStrength";
            this.ThermalStrength.Size = new System.Drawing.Size(454, 45);
            this.ThermalStrength.TabIndex = 12;
            this.ThermalStrength.TickFrequency = 1000;
            this.ThermalStrength.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // ThermalCover
            // 
            this.ThermalCover.Location = new System.Drawing.Point(57, 234);
            this.ThermalCover.Maximum = 25;
            this.ThermalCover.Name = "ThermalCover";
            this.ThermalCover.Size = new System.Drawing.Size(454, 45);
            this.ThermalCover.TabIndex = 11;
            this.ThermalCover.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Baro
            // 
            this.Baro.Location = new System.Drawing.Point(57, 183);
            this.Baro.Maximum = 3050;
            this.Baro.Minimum = 2900;
            this.Baro.Name = "Baro";
            this.Baro.Size = new System.Drawing.Size(454, 45);
            this.Baro.TabIndex = 10;
            this.Baro.Value = 2992;
            this.Baro.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Temperature
            // 
            this.Temperature.Location = new System.Drawing.Point(57, 132);
            this.Temperature.Maximum = 40;
            this.Temperature.Minimum = -40;
            this.Temperature.Name = "Temperature";
            this.Temperature.Size = new System.Drawing.Size(454, 45);
            this.Temperature.TabIndex = 9;
            this.Temperature.Value = 19;
            this.Temperature.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // PatchyCheckBox
            // 
            this.PatchyCheckBox.AutoSize = true;
            this.PatchyCheckBox.Location = new System.Drawing.Point(334, 108);
            this.PatchyCheckBox.Name = "PatchyCheckBox";
            this.PatchyCheckBox.Size = new System.Drawing.Size(110, 17);
            this.PatchyCheckBox.TabIndex = 8;
            this.PatchyCheckBox.Text = "Water/Ice patchy";
            this.PatchyCheckBox.UseVisualStyleBackColor = true;
            this.PatchyCheckBox.Click += new System.EventHandler(this.WeatherChanged);
            // 
            // WetRadio
            // 
            this.WetRadio.AutoSize = true;
            this.WetRadio.Location = new System.Drawing.Point(243, 107);
            this.WetRadio.Name = "WetRadio";
            this.WetRadio.Size = new System.Drawing.Size(45, 17);
            this.WetRadio.TabIndex = 7;
            this.WetRadio.Text = "Wet";
            this.WetRadio.UseVisualStyleBackColor = true;
            this.WetRadio.Click += new System.EventHandler(this.WeatherChanged);
            // 
            // DampRadio
            // 
            this.DampRadio.AutoSize = true;
            this.DampRadio.Location = new System.Drawing.Point(152, 107);
            this.DampRadio.Name = "DampRadio";
            this.DampRadio.Size = new System.Drawing.Size(53, 17);
            this.DampRadio.TabIndex = 6;
            this.DampRadio.Text = "Damp";
            this.DampRadio.UseVisualStyleBackColor = true;
            this.DampRadio.Click += new System.EventHandler(this.WeatherChanged);
            // 
            // DryRadio
            // 
            this.DryRadio.AutoSize = true;
            this.DryRadio.Checked = true;
            this.DryRadio.Location = new System.Drawing.Point(61, 107);
            this.DryRadio.Name = "DryRadio";
            this.DryRadio.Size = new System.Drawing.Size(41, 17);
            this.DryRadio.TabIndex = 5;
            this.DryRadio.TabStop = true;
            this.DryRadio.Text = "Dry";
            this.DryRadio.UseVisualStyleBackColor = true;
            this.DryRadio.Click += new System.EventHandler(this.WeatherChanged);
            // 
            // StormBar
            // 
            this.StormBar.Location = new System.Drawing.Point(57, 57);
            this.StormBar.Maximum = 100;
            this.StormBar.Name = "StormBar";
            this.StormBar.Size = new System.Drawing.Size(454, 45);
            this.StormBar.TabIndex = 4;
            this.StormBar.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // RainBar
            // 
            this.RainBar.Location = new System.Drawing.Point(57, 6);
            this.RainBar.Maximum = 100;
            this.RainBar.Name = "RainBar";
            this.RainBar.Size = new System.Drawing.Size(454, 45);
            this.RainBar.TabIndex = 3;
            this.RainBar.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // VisibilityBar
            // 
            this.VisibilityBar.Location = new System.Drawing.Point(6, 6);
            this.VisibilityBar.Maximum = 50000;
            this.VisibilityBar.Name = "VisibilityBar";
            this.VisibilityBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.VisibilityBar.Size = new System.Drawing.Size(45, 482);
            this.VisibilityBar.TabIndex = 2;
            this.VisibilityBar.TickFrequency = 1000;
            this.VisibilityBar.Value = 20000;
            this.VisibilityBar.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // WindPage
            // 
            this.WindPage.Controls.Add(this.Wind1StrengthLabel);
            this.WindPage.Controls.Add(this.Wind1DirLabel);
            this.WindPage.Controls.Add(this.Wind1GustsLabel);
            this.WindPage.Controls.Add(this.Wind1AltitudeLabel);
            this.WindPage.Controls.Add(this.Wind1TurbLabel);
            this.WindPage.Controls.Add(this.Wind2AltitudeLabel);
            this.WindPage.Controls.Add(this.Wind2Altitude);
            this.WindPage.Controls.Add(this.Wind3TurbLabel);
            this.WindPage.Controls.Add(this.Wind1Altitude);
            this.WindPage.Controls.Add(this.Wind3GustsLabel);
            this.WindPage.Controls.Add(this.Wind2DirLabel);
            this.WindPage.Controls.Add(this.Wind3Gust);
            this.WindPage.Controls.Add(this.Wind3StrengthLabel);
            this.WindPage.Controls.Add(this.Wind3Strength);
            this.WindPage.Controls.Add(this.Wind1Turb);
            this.WindPage.Controls.Add(this.Wind3DirLabel);
            this.WindPage.Controls.Add(this.Wind1Direction);
            this.WindPage.Controls.Add(this.Wind2StrengthLabel);
            this.WindPage.Controls.Add(this.Wind2TurbLabel);
            this.WindPage.Controls.Add(this.Wind3AltitudeLabel);
            this.WindPage.Controls.Add(this.Wind1Strength);
            this.WindPage.Controls.Add(this.Wind1Gust);
            this.WindPage.Controls.Add(this.Wind2GustsLabel);
            this.WindPage.Controls.Add(this.Wind3Turb);
            this.WindPage.Controls.Add(this.Wind3Altitude);
            this.WindPage.Controls.Add(this.Wind2Turb);
            this.WindPage.Controls.Add(this.Wind2Gust);
            this.WindPage.Controls.Add(this.Wind2Strength);
            this.WindPage.Controls.Add(this.Wind2Direction);
            this.WindPage.Controls.Add(this.Wind3Direction);
            this.WindPage.Location = new System.Drawing.Point(4, 22);
            this.WindPage.Name = "WindPage";
            this.WindPage.Size = new System.Drawing.Size(517, 494);
            this.WindPage.TabIndex = 2;
            this.WindPage.Text = "Wind";
            this.WindPage.UseVisualStyleBackColor = true;
            // 
            // Wind1StrengthLabel
            // 
            this.Wind1StrengthLabel.AutoSize = true;
            this.Wind1StrengthLabel.Location = new System.Drawing.Point(68, 338);
            this.Wind1StrengthLabel.Name = "Wind1StrengthLabel";
            this.Wind1StrengthLabel.Size = new System.Drawing.Size(47, 13);
            this.Wind1StrengthLabel.TabIndex = 32;
            this.Wind1StrengthLabel.Text = "Strength";
            // 
            // Wind1DirLabel
            // 
            this.Wind1DirLabel.AutoSize = true;
            this.Wind1DirLabel.Location = new System.Drawing.Point(379, 284);
            this.Wind1DirLabel.Name = "Wind1DirLabel";
            this.Wind1DirLabel.Size = new System.Drawing.Size(90, 13);
            this.Wind1DirLabel.TabIndex = 31;
            this.Wind1DirLabel.Text = "Strength direction";
            // 
            // Wind1GustsLabel
            // 
            this.Wind1GustsLabel.AutoSize = true;
            this.Wind1GustsLabel.Location = new System.Drawing.Point(222, 338);
            this.Wind1GustsLabel.Name = "Wind1GustsLabel";
            this.Wind1GustsLabel.Size = new System.Drawing.Size(34, 13);
            this.Wind1GustsLabel.TabIndex = 33;
            this.Wind1GustsLabel.Text = "Gusts";
            // 
            // Wind1AltitudeLabel
            // 
            this.Wind1AltitudeLabel.AutoSize = true;
            this.Wind1AltitudeLabel.Location = new System.Drawing.Point(63, 287);
            this.Wind1AltitudeLabel.Name = "Wind1AltitudeLabel";
            this.Wind1AltitudeLabel.Size = new System.Drawing.Size(84, 13);
            this.Wind1AltitudeLabel.TabIndex = 30;
            this.Wind1AltitudeLabel.Text = "Strength altitude";
            // 
            // Wind1TurbLabel
            // 
            this.Wind1TurbLabel.AutoSize = true;
            this.Wind1TurbLabel.Location = new System.Drawing.Point(366, 338);
            this.Wind1TurbLabel.Name = "Wind1TurbLabel";
            this.Wind1TurbLabel.Size = new System.Drawing.Size(61, 13);
            this.Wind1TurbLabel.TabIndex = 34;
            this.Wind1TurbLabel.Text = "Turbulence";
            // 
            // Wind2AltitudeLabel
            // 
            this.Wind2AltitudeLabel.AutoSize = true;
            this.Wind2AltitudeLabel.Location = new System.Drawing.Point(63, 163);
            this.Wind2AltitudeLabel.Name = "Wind2AltitudeLabel";
            this.Wind2AltitudeLabel.Size = new System.Drawing.Size(84, 13);
            this.Wind2AltitudeLabel.TabIndex = 20;
            this.Wind2AltitudeLabel.Text = "Strength altitude";
            // 
            // Wind2Altitude
            // 
            this.Wind2Altitude.Location = new System.Drawing.Point(6, 131);
            this.Wind2Altitude.Maximum = 50000;
            this.Wind2Altitude.Minimum = 1;
            this.Wind2Altitude.Name = "Wind2Altitude";
            this.Wind2Altitude.Size = new System.Drawing.Size(250, 45);
            this.Wind2Altitude.TabIndex = 19;
            this.Wind2Altitude.Value = 8000;
            this.Wind2Altitude.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind3TurbLabel
            // 
            this.Wind3TurbLabel.AutoSize = true;
            this.Wind3TurbLabel.Location = new System.Drawing.Point(366, 96);
            this.Wind3TurbLabel.Name = "Wind3TurbLabel";
            this.Wind3TurbLabel.Size = new System.Drawing.Size(61, 13);
            this.Wind3TurbLabel.TabIndex = 14;
            this.Wind3TurbLabel.Text = "Turbulence";
            // 
            // Wind1Altitude
            // 
            this.Wind1Altitude.Location = new System.Drawing.Point(6, 255);
            this.Wind1Altitude.Maximum = 50000;
            this.Wind1Altitude.Minimum = 1;
            this.Wind1Altitude.Name = "Wind1Altitude";
            this.Wind1Altitude.Size = new System.Drawing.Size(250, 45);
            this.Wind1Altitude.TabIndex = 29;
            this.Wind1Altitude.Value = 16000;
            this.Wind1Altitude.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind3GustsLabel
            // 
            this.Wind3GustsLabel.AutoSize = true;
            this.Wind3GustsLabel.Location = new System.Drawing.Point(222, 96);
            this.Wind3GustsLabel.Name = "Wind3GustsLabel";
            this.Wind3GustsLabel.Size = new System.Drawing.Size(34, 13);
            this.Wind3GustsLabel.TabIndex = 13;
            this.Wind3GustsLabel.Text = "Gusts";
            // 
            // Wind2DirLabel
            // 
            this.Wind2DirLabel.AutoSize = true;
            this.Wind2DirLabel.Location = new System.Drawing.Point(379, 163);
            this.Wind2DirLabel.Name = "Wind2DirLabel";
            this.Wind2DirLabel.Size = new System.Drawing.Size(90, 13);
            this.Wind2DirLabel.TabIndex = 21;
            this.Wind2DirLabel.Text = "Strength direction";
            // 
            // Wind3Gust
            // 
            this.Wind3Gust.Location = new System.Drawing.Point(181, 64);
            this.Wind3Gust.Maximum = 100;
            this.Wind3Gust.Name = "Wind3Gust";
            this.Wind3Gust.Size = new System.Drawing.Size(167, 45);
            this.Wind3Gust.TabIndex = 5;
            this.Wind3Gust.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind3StrengthLabel
            // 
            this.Wind3StrengthLabel.AutoSize = true;
            this.Wind3StrengthLabel.Location = new System.Drawing.Point(68, 96);
            this.Wind3StrengthLabel.Name = "Wind3StrengthLabel";
            this.Wind3StrengthLabel.Size = new System.Drawing.Size(47, 13);
            this.Wind3StrengthLabel.TabIndex = 12;
            this.Wind3StrengthLabel.Text = "Strength";
            // 
            // Wind3Strength
            // 
            this.Wind3Strength.Location = new System.Drawing.Point(6, 64);
            this.Wind3Strength.Maximum = 100;
            this.Wind3Strength.Name = "Wind3Strength";
            this.Wind3Strength.Size = new System.Drawing.Size(169, 45);
            this.Wind3Strength.TabIndex = 4;
            this.Wind3Strength.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind1Turb
            // 
            this.Wind1Turb.Location = new System.Drawing.Point(354, 306);
            this.Wind1Turb.Maximum = 100;
            this.Wind1Turb.Name = "Wind1Turb";
            this.Wind1Turb.Size = new System.Drawing.Size(160, 45);
            this.Wind1Turb.TabIndex = 28;
            this.Wind1Turb.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind3DirLabel
            // 
            this.Wind3DirLabel.AutoSize = true;
            this.Wind3DirLabel.Location = new System.Drawing.Point(337, 45);
            this.Wind3DirLabel.Name = "Wind3DirLabel";
            this.Wind3DirLabel.Size = new System.Drawing.Size(90, 13);
            this.Wind3DirLabel.TabIndex = 11;
            this.Wind3DirLabel.Text = "Strength direction";
            // 
            // Wind1Direction
            // 
            this.Wind1Direction.Location = new System.Drawing.Point(262, 252);
            this.Wind1Direction.Maximum = 360;
            this.Wind1Direction.Name = "Wind1Direction";
            this.Wind1Direction.Size = new System.Drawing.Size(252, 45);
            this.Wind1Direction.TabIndex = 25;
            this.Wind1Direction.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.Wind1Direction.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind2StrengthLabel
            // 
            this.Wind2StrengthLabel.AutoSize = true;
            this.Wind2StrengthLabel.Location = new System.Drawing.Point(68, 214);
            this.Wind2StrengthLabel.Name = "Wind2StrengthLabel";
            this.Wind2StrengthLabel.Size = new System.Drawing.Size(47, 13);
            this.Wind2StrengthLabel.TabIndex = 22;
            this.Wind2StrengthLabel.Text = "Strength";
            // 
            // Wind2TurbLabel
            // 
            this.Wind2TurbLabel.AutoSize = true;
            this.Wind2TurbLabel.Location = new System.Drawing.Point(366, 214);
            this.Wind2TurbLabel.Name = "Wind2TurbLabel";
            this.Wind2TurbLabel.Size = new System.Drawing.Size(61, 13);
            this.Wind2TurbLabel.TabIndex = 24;
            this.Wind2TurbLabel.Text = "Turbulence";
            // 
            // Wind3AltitudeLabel
            // 
            this.Wind3AltitudeLabel.AutoSize = true;
            this.Wind3AltitudeLabel.Location = new System.Drawing.Point(63, 45);
            this.Wind3AltitudeLabel.Name = "Wind3AltitudeLabel";
            this.Wind3AltitudeLabel.Size = new System.Drawing.Size(84, 13);
            this.Wind3AltitudeLabel.TabIndex = 10;
            this.Wind3AltitudeLabel.Text = "Strength altitude";
            // 
            // Wind1Strength
            // 
            this.Wind1Strength.Location = new System.Drawing.Point(6, 306);
            this.Wind1Strength.Maximum = 100;
            this.Wind1Strength.Name = "Wind1Strength";
            this.Wind1Strength.Size = new System.Drawing.Size(169, 45);
            this.Wind1Strength.TabIndex = 26;
            this.Wind1Strength.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind1Gust
            // 
            this.Wind1Gust.Location = new System.Drawing.Point(181, 306);
            this.Wind1Gust.Maximum = 100;
            this.Wind1Gust.Name = "Wind1Gust";
            this.Wind1Gust.Size = new System.Drawing.Size(167, 45);
            this.Wind1Gust.TabIndex = 27;
            this.Wind1Gust.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind2GustsLabel
            // 
            this.Wind2GustsLabel.AutoSize = true;
            this.Wind2GustsLabel.Location = new System.Drawing.Point(222, 214);
            this.Wind2GustsLabel.Name = "Wind2GustsLabel";
            this.Wind2GustsLabel.Size = new System.Drawing.Size(34, 13);
            this.Wind2GustsLabel.TabIndex = 23;
            this.Wind2GustsLabel.Text = "Gusts";
            // 
            // Wind3Turb
            // 
            this.Wind3Turb.Location = new System.Drawing.Point(354, 64);
            this.Wind3Turb.Maximum = 100;
            this.Wind3Turb.Name = "Wind3Turb";
            this.Wind3Turb.Size = new System.Drawing.Size(155, 45);
            this.Wind3Turb.TabIndex = 6;
            this.Wind3Turb.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind3Altitude
            // 
            this.Wind3Altitude.Location = new System.Drawing.Point(6, 13);
            this.Wind3Altitude.Maximum = 50000;
            this.Wind3Altitude.Minimum = 1;
            this.Wind3Altitude.Name = "Wind3Altitude";
            this.Wind3Altitude.Size = new System.Drawing.Size(250, 45);
            this.Wind3Altitude.TabIndex = 8;
            this.Wind3Altitude.Value = 4000;
            this.Wind3Altitude.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind2Turb
            // 
            this.Wind2Turb.Location = new System.Drawing.Point(354, 182);
            this.Wind2Turb.Maximum = 100;
            this.Wind2Turb.Name = "Wind2Turb";
            this.Wind2Turb.Size = new System.Drawing.Size(160, 45);
            this.Wind2Turb.TabIndex = 18;
            this.Wind2Turb.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind2Gust
            // 
            this.Wind2Gust.Location = new System.Drawing.Point(181, 182);
            this.Wind2Gust.Maximum = 100;
            this.Wind2Gust.Name = "Wind2Gust";
            this.Wind2Gust.Size = new System.Drawing.Size(167, 45);
            this.Wind2Gust.TabIndex = 17;
            this.Wind2Gust.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind2Strength
            // 
            this.Wind2Strength.Location = new System.Drawing.Point(6, 182);
            this.Wind2Strength.Maximum = 100;
            this.Wind2Strength.Name = "Wind2Strength";
            this.Wind2Strength.Size = new System.Drawing.Size(169, 45);
            this.Wind2Strength.TabIndex = 16;
            this.Wind2Strength.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind2Direction
            // 
            this.Wind2Direction.Location = new System.Drawing.Point(262, 131);
            this.Wind2Direction.Maximum = 360;
            this.Wind2Direction.Name = "Wind2Direction";
            this.Wind2Direction.Size = new System.Drawing.Size(252, 45);
            this.Wind2Direction.TabIndex = 15;
            this.Wind2Direction.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.Wind2Direction.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Wind3Direction
            // 
            this.Wind3Direction.Location = new System.Drawing.Point(262, 13);
            this.Wind3Direction.Maximum = 360;
            this.Wind3Direction.Name = "Wind3Direction";
            this.Wind3Direction.Size = new System.Drawing.Size(252, 45);
            this.Wind3Direction.TabIndex = 3;
            this.Wind3Direction.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.Wind3Direction.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // CloudsPage
            // 
            this.CloudsPage.Controls.Add(this.Cloud3Type);
            this.CloudsPage.Controls.Add(this.Cloud2Type);
            this.CloudsPage.Controls.Add(this.Cloud1Type);
            this.CloudsPage.Controls.Add(this.Cloud1TopLabel);
            this.CloudsPage.Controls.Add(this.Cloud1BaseLabel);
            this.CloudsPage.Controls.Add(this.Cloud3TopLabel);
            this.CloudsPage.Controls.Add(this.Cloud3BaseLabel);
            this.CloudsPage.Controls.Add(this.Cloud1Top);
            this.CloudsPage.Controls.Add(this.Cloud2BaseLabel);
            this.CloudsPage.Controls.Add(this.Cloud1Base);
            this.CloudsPage.Controls.Add(this.Cloud2TopLabel);
            this.CloudsPage.Controls.Add(this.Cloud3Base);
            this.CloudsPage.Controls.Add(this.Cloud3Top);
            this.CloudsPage.Controls.Add(this.Cloud2Base);
            this.CloudsPage.Controls.Add(this.Cloud2Top);
            this.CloudsPage.Location = new System.Drawing.Point(4, 22);
            this.CloudsPage.Name = "CloudsPage";
            this.CloudsPage.Size = new System.Drawing.Size(517, 494);
            this.CloudsPage.TabIndex = 3;
            this.CloudsPage.Text = "Clouds";
            this.CloudsPage.UseVisualStyleBackColor = true;
            // 
            // Cloud1TopLabel
            // 
            this.Cloud1TopLabel.AutoSize = true;
            this.Cloud1TopLabel.Location = new System.Drawing.Point(350, 337);
            this.Cloud1TopLabel.Name = "Cloud1TopLabel";
            this.Cloud1TopLabel.Size = new System.Drawing.Size(86, 13);
            this.Cloud1TopLabel.TabIndex = 49;
            this.Cloud1TopLabel.Text = "Cloud up altitude";
            // 
            // Cloud1BaseLabel
            // 
            this.Cloud1BaseLabel.AutoSize = true;
            this.Cloud1BaseLabel.Location = new System.Drawing.Point(53, 337);
            this.Cloud1BaseLabel.Name = "Cloud1BaseLabel";
            this.Cloud1BaseLabel.Size = new System.Drawing.Size(100, 13);
            this.Cloud1BaseLabel.TabIndex = 48;
            this.Cloud1BaseLabel.Text = "Cloud down altitude";
            // 
            // Cloud3TopLabel
            // 
            this.Cloud3TopLabel.AutoSize = true;
            this.Cloud3TopLabel.Location = new System.Drawing.Point(350, 45);
            this.Cloud3TopLabel.Name = "Cloud3TopLabel";
            this.Cloud3TopLabel.Size = new System.Drawing.Size(86, 13);
            this.Cloud3TopLabel.TabIndex = 39;
            this.Cloud3TopLabel.Text = "Cloud up altitude";
            // 
            // Cloud3BaseLabel
            // 
            this.Cloud3BaseLabel.AutoSize = true;
            this.Cloud3BaseLabel.Location = new System.Drawing.Point(53, 45);
            this.Cloud3BaseLabel.Name = "Cloud3BaseLabel";
            this.Cloud3BaseLabel.Size = new System.Drawing.Size(100, 13);
            this.Cloud3BaseLabel.TabIndex = 38;
            this.Cloud3BaseLabel.Text = "Cloud down altitude";
            // 
            // Cloud1Top
            // 
            this.Cloud1Top.Location = new System.Drawing.Point(249, 305);
            this.Cloud1Top.Maximum = 50000;
            this.Cloud1Top.Name = "Cloud1Top";
            this.Cloud1Top.Size = new System.Drawing.Size(265, 45);
            this.Cloud1Top.TabIndex = 46;
            this.Cloud1Top.Value = 7000;
            this.Cloud1Top.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Cloud2BaseLabel
            // 
            this.Cloud2BaseLabel.AutoSize = true;
            this.Cloud2BaseLabel.Location = new System.Drawing.Point(53, 197);
            this.Cloud2BaseLabel.Name = "Cloud2BaseLabel";
            this.Cloud2BaseLabel.Size = new System.Drawing.Size(100, 13);
            this.Cloud2BaseLabel.TabIndex = 43;
            this.Cloud2BaseLabel.Text = "Cloud down altitude";
            // 
            // Cloud1Base
            // 
            this.Cloud1Base.Location = new System.Drawing.Point(9, 305);
            this.Cloud1Base.Maximum = 50000;
            this.Cloud1Base.Minimum = 1;
            this.Cloud1Base.Name = "Cloud1Base";
            this.Cloud1Base.Size = new System.Drawing.Size(234, 45);
            this.Cloud1Base.TabIndex = 45;
            this.Cloud1Base.Value = 5000;
            this.Cloud1Base.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Cloud2TopLabel
            // 
            this.Cloud2TopLabel.AutoSize = true;
            this.Cloud2TopLabel.Location = new System.Drawing.Point(350, 197);
            this.Cloud2TopLabel.Name = "Cloud2TopLabel";
            this.Cloud2TopLabel.Size = new System.Drawing.Size(86, 13);
            this.Cloud2TopLabel.TabIndex = 44;
            this.Cloud2TopLabel.Text = "Cloud up altitude";
            // 
            // Cloud3Base
            // 
            this.Cloud3Base.Location = new System.Drawing.Point(9, 13);
            this.Cloud3Base.Maximum = 50000;
            this.Cloud3Base.Minimum = 1;
            this.Cloud3Base.Name = "Cloud3Base";
            this.Cloud3Base.Size = new System.Drawing.Size(234, 45);
            this.Cloud3Base.TabIndex = 35;
            this.Cloud3Base.Value = 25000;
            this.Cloud3Base.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Cloud3Top
            // 
            this.Cloud3Top.Location = new System.Drawing.Point(249, 13);
            this.Cloud3Top.Maximum = 50000;
            this.Cloud3Top.Name = "Cloud3Top";
            this.Cloud3Top.Size = new System.Drawing.Size(265, 45);
            this.Cloud3Top.TabIndex = 36;
            this.Cloud3Top.Value = 27000;
            this.Cloud3Top.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Cloud2Base
            // 
            this.Cloud2Base.Location = new System.Drawing.Point(9, 165);
            this.Cloud2Base.Maximum = 50000;
            this.Cloud2Base.Minimum = 1;
            this.Cloud2Base.Name = "Cloud2Base";
            this.Cloud2Base.Size = new System.Drawing.Size(234, 45);
            this.Cloud2Base.TabIndex = 40;
            this.Cloud2Base.Value = 15000;
            this.Cloud2Base.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // Cloud2Top
            // 
            this.Cloud2Top.Location = new System.Drawing.Point(249, 165);
            this.Cloud2Top.Maximum = 50000;
            this.Cloud2Top.Name = "Cloud2Top";
            this.Cloud2Top.Size = new System.Drawing.Size(265, 45);
            this.Cloud2Top.TabIndex = 41;
            this.Cloud2Top.Value = 17000;
            this.Cloud2Top.ValueChanged += new System.EventHandler(this.WeatherChanged);
            // 
            // airportsTextBox
            // 
            this.airportsTextBox.Location = new System.Drawing.Point(3, 6);
            this.airportsTextBox.Multiline = true;
            this.airportsTextBox.Name = "airportsTextBox";
            this.airportsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.airportsTextBox.Size = new System.Drawing.Size(431, 124);
            this.airportsTextBox.TabIndex = 0;
            // 
            // CmdButtonsTabs
            // 
            this.CmdButtonsTabs.Location = new System.Drawing.Point(6, 6);
            this.CmdButtonsTabs.Name = "CmdButtonsTabs";
            this.CmdButtonsTabs.SelectedIndex = 0;
            this.CmdButtonsTabs.Size = new System.Drawing.Size(396, 546);
            this.CmdButtonsTabs.TabIndex = 0;
            // 
            // WgtLabel
            // 
            this.WgtLabel.AutoSize = true;
            this.WgtLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.WgtLabel.Location = new System.Drawing.Point(6, 66);
            this.WgtLabel.Name = "WgtLabel";
            this.WgtLabel.Size = new System.Drawing.Size(41, 13);
            this.WgtLabel.TabIndex = 16;
            this.WgtLabel.Text = "Weight";
            // 
            // DimBox
            // 
            this.DimBox.FormattingEnabled = true;
            this.DimBox.HorizontalScrollbar = true;
            this.DimBox.Location = new System.Drawing.Point(6, 483);
            this.DimBox.Name = "DimBox";
            this.DimBox.Size = new System.Drawing.Size(395, 69);
            this.DimBox.TabIndex = 15;
            // 
            // DimLabel
            // 
            this.DimLabel.AutoSize = true;
            this.DimLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DimLabel.Location = new System.Drawing.Point(6, 49);
            this.DimLabel.Name = "DimLabel";
            this.DimLabel.Size = new System.Drawing.Size(76, 13);
            this.DimLabel.TabIndex = 14;
            this.DimLabel.Text = "Current aircraft";
            // 
            // RadLabel
            // 
            this.RadLabel.AutoSize = true;
            this.RadLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RadLabel.Location = new System.Drawing.Point(11, 177);
            this.RadLabel.Name = "RadLabel";
            this.RadLabel.Size = new System.Drawing.Size(36, 13);
            this.RadLabel.TabIndex = 13;
            this.RadLabel.Text = "Radar";
            // 
            // FailsListBox
            // 
            this.FailsListBox.FormattingEnabled = true;
            this.FailsListBox.Location = new System.Drawing.Point(6, 333);
            this.FailsListBox.Name = "FailsListBox";
            this.FailsListBox.Size = new System.Drawing.Size(395, 69);
            this.FailsListBox.Sorted = true;
            this.FailsListBox.TabIndex = 11;
            // 
            // AcfsListBox
            // 
            this.AcfsListBox.FormattingEnabled = true;
            this.AcfsListBox.Location = new System.Drawing.Point(6, 408);
            this.AcfsListBox.Name = "AcfsListBox";
            this.AcfsListBox.Size = new System.Drawing.Size(395, 69);
            this.AcfsListBox.Sorted = true;
            this.AcfsListBox.TabIndex = 10;
            // 
            // Message_TextBox
            // 
            this.Message_TextBox.Location = new System.Drawing.Point(3, 39);
            this.Message_TextBox.Multiline = true;
            this.Message_TextBox.Name = "Message_TextBox";
            this.Message_TextBox.Size = new System.Drawing.Size(399, 50);
            this.Message_TextBox.TabIndex = 1;
            // 
            // UDP_TextBox
            // 
            this.UDP_TextBox.Location = new System.Drawing.Point(3, 95);
            this.UDP_TextBox.Multiline = true;
            this.UDP_TextBox.Name = "UDP_TextBox";
            this.UDP_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.UDP_TextBox.Size = new System.Drawing.Size(399, 50);
            this.UDP_TextBox.TabIndex = 2;
            // 
            // SendUDP
            // 
            this.SendUDP.Enabled = false;
            this.SendUDP.Location = new System.Drawing.Point(3, 10);
            this.SendUDP.Name = "SendUDP";
            this.SendUDP.Size = new System.Drawing.Size(146, 23);
            this.SendUDP.TabIndex = 5;
            this.SendUDP.Text = "Send UDP";
            this.SendUDP.UseVisualStyleBackColor = true;
            this.SendUDP.Click += new System.EventHandler(this.SendUDP_Click);
            // 
            // SendTCP
            // 
            this.SendTCP.Enabled = false;
            this.SendTCP.Location = new System.Drawing.Point(245, 10);
            this.SendTCP.Name = "SendTCP";
            this.SendTCP.Size = new System.Drawing.Size(157, 23);
            this.SendTCP.TabIndex = 3;
            this.SendTCP.Text = "Send TCP";
            this.SendTCP.UseVisualStyleBackColor = true;
            this.SendTCP.Click += new System.EventHandler(this.button1_Click);
            // 
            // CentralControl
            // 
            this.CentralControl.Controls.Add(this.WeatherPage);
            this.CentralControl.Controls.Add(this.MapPage);
            this.CentralControl.Location = new System.Drawing.Point(465, 12);
            this.CentralControl.Name = "CentralControl";
            this.CentralControl.SelectedIndex = 0;
            this.CentralControl.Size = new System.Drawing.Size(542, 584);
            this.CentralControl.TabIndex = 6;
            // 
            // MapPage
            // 
            this.MapPage.Controls.Add(this.Map);
            this.MapPage.Controls.Add(this.TextBoxSpd);
            this.MapPage.Controls.Add(this.TextBoxHdg);
            this.MapPage.Controls.Add(this.TextBoxAgl);
            this.MapPage.Controls.Add(this.TextBoxAsl);
            this.MapPage.Controls.Add(this.TextBoxLat);
            this.MapPage.Controls.Add(this.TextBoxLng);
            this.MapPage.Controls.Add(this.splitter1);
            this.MapPage.Controls.Add(this.menuStrip1);
            this.MapPage.Location = new System.Drawing.Point(4, 22);
            this.MapPage.Name = "MapPage";
            this.MapPage.Padding = new System.Windows.Forms.Padding(3);
            this.MapPage.Size = new System.Drawing.Size(534, 558);
            this.MapPage.TabIndex = 3;
            this.MapPage.Text = "Map";
            this.MapPage.UseVisualStyleBackColor = true;
            // 
            // Map
            // 
            this.Map.Bearing = 0F;
            this.Map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Map.CanDragMap = true;
            this.Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Map.EmptyTileColor = System.Drawing.Color.GhostWhite;
            this.Map.GrayScaleMode = false;
            this.Map.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.Map.LevelsKeepInMemory = 5;
            this.Map.Location = new System.Drawing.Point(3, 27);
            this.Map.MarkersEnabled = false;
            this.Map.MaxZoom = 2;
            this.Map.MinZoom = 24;
            this.Map.MouseWheelZoomEnabled = true;
            this.Map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.Map.Name = "Map";
            this.Map.NegativeMode = false;
            this.Map.PolygonsEnabled = false;
            this.Map.RetryLoadTile = 0;
            this.Map.RoutesEnabled = true;
            this.Map.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.Map.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.Map.ShowTileGridLines = false;
            this.Map.Size = new System.Drawing.Size(528, 388);
            this.Map.TabIndex = 8;
            this.Map.Zoom = 2D;
            // 
            // TextBoxSpd
            // 
            this.TextBoxSpd.Location = new System.Drawing.Point(342, 452);
            this.TextBoxSpd.Name = "TextBoxSpd";
            this.TextBoxSpd.Size = new System.Drawing.Size(186, 20);
            this.TextBoxSpd.TabIndex = 6;
            // 
            // TextBoxHdg
            // 
            this.TextBoxHdg.Location = new System.Drawing.Point(342, 426);
            this.TextBoxHdg.Name = "TextBoxHdg";
            this.TextBoxHdg.Size = new System.Drawing.Size(186, 20);
            this.TextBoxHdg.TabIndex = 5;
            // 
            // TextBoxAgl
            // 
            this.TextBoxAgl.Location = new System.Drawing.Point(6, 504);
            this.TextBoxAgl.Name = "TextBoxAgl";
            this.TextBoxAgl.Size = new System.Drawing.Size(232, 20);
            this.TextBoxAgl.TabIndex = 4;
            // 
            // TextBoxAsl
            // 
            this.TextBoxAsl.Location = new System.Drawing.Point(6, 478);
            this.TextBoxAsl.Name = "TextBoxAsl";
            this.TextBoxAsl.Size = new System.Drawing.Size(232, 20);
            this.TextBoxAsl.TabIndex = 3;
            // 
            // TextBoxLat
            // 
            this.TextBoxLat.Location = new System.Drawing.Point(6, 452);
            this.TextBoxLat.Name = "TextBoxLat";
            this.TextBoxLat.Size = new System.Drawing.Size(232, 20);
            this.TextBoxLat.TabIndex = 2;
            // 
            // TextBoxLng
            // 
            this.TextBoxLng.Location = new System.Drawing.Point(6, 426);
            this.TextBoxLng.Name = "TextBoxLng";
            this.TextBoxLng.Size = new System.Drawing.Size(232, 20);
            this.TextBoxLng.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(3, 415);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(528, 140);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapProviderToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(528, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mapProviderToolStripMenuItem
            // 
            this.mapProviderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oSMToolStripMenuItem,
            this.googleMapsToolStripMenuItem,
            this.googleMapsSatteliteToolStripMenuItem,
            this.cloudMapToolStripMenuItem,
            this.arcGISTerrainBaseToolStripMenuItem});
            this.mapProviderToolStripMenuItem.Name = "mapProviderToolStripMenuItem";
            this.mapProviderToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.mapProviderToolStripMenuItem.Text = "Map provider";
            // 
            // oSMToolStripMenuItem
            // 
            this.oSMToolStripMenuItem.Name = "oSMToolStripMenuItem";
            this.oSMToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.oSMToolStripMenuItem.Text = "OSM";
            this.oSMToolStripMenuItem.Click += new System.EventHandler(this.oSMToolStripMenuItem_Click);
            // 
            // googleMapsToolStripMenuItem
            // 
            this.googleMapsToolStripMenuItem.Name = "googleMapsToolStripMenuItem";
            this.googleMapsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.googleMapsToolStripMenuItem.Text = "Google maps";
            this.googleMapsToolStripMenuItem.Click += new System.EventHandler(this.googleMapsToolStripMenuItem_Click);
            // 
            // googleMapsSatteliteToolStripMenuItem
            // 
            this.googleMapsSatteliteToolStripMenuItem.Name = "googleMapsSatteliteToolStripMenuItem";
            this.googleMapsSatteliteToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.googleMapsSatteliteToolStripMenuItem.Text = "Google maps sattelite";
            this.googleMapsSatteliteToolStripMenuItem.Click += new System.EventHandler(this.googleMapsSatteliteToolStripMenuItem_Click);
            // 
            // cloudMapToolStripMenuItem
            // 
            this.cloudMapToolStripMenuItem.Name = "cloudMapToolStripMenuItem";
            this.cloudMapToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.cloudMapToolStripMenuItem.Text = "Google terrain map";
            this.cloudMapToolStripMenuItem.Click += new System.EventHandler(this.cloudMapToolStripMenuItem_Click);
            // 
            // arcGISTerrainBaseToolStripMenuItem
            // 
            this.arcGISTerrainBaseToolStripMenuItem.Name = "arcGISTerrainBaseToolStripMenuItem";
            this.arcGISTerrainBaseToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.arcGISTerrainBaseToolStripMenuItem.Text = "ArcGIS Terrain Base";
            this.arcGISTerrainBaseToolStripMenuItem.Click += new System.EventHandler(this.arcGISTerrainBaseToolStripMenuItem_Click);
            // 
            // FailTabs
            // 
            this.FailTabs.Location = new System.Drawing.Point(3, 3);
            this.FailTabs.Name = "FailTabs";
            this.FailTabs.SelectedIndex = 0;
            this.FailTabs.Size = new System.Drawing.Size(399, 552);
            this.FailTabs.TabIndex = 0;
            // 
            // CommandsTabControl
            // 
            this.CommandsTabControl.Controls.Add(this.ConnectPage);
            this.CommandsTabControl.Controls.Add(this.FailsPage);
            this.CommandsTabControl.Controls.Add(this.AircraftsPage);
            this.CommandsTabControl.Controls.Add(this.CmdButtons);
            this.CommandsTabControl.Controls.Add(this.MiscCommands);
            this.CommandsTabControl.Location = new System.Drawing.Point(1009, 12);
            this.CommandsTabControl.Name = "CommandsTabControl";
            this.CommandsTabControl.SelectedIndex = 0;
            this.CommandsTabControl.Size = new System.Drawing.Size(418, 584);
            this.CommandsTabControl.TabIndex = 8;
            this.CommandsTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.CommandsTabControl_Selecting);
            // 
            // ConnectPage
            // 
            this.ConnectPage.Controls.Add(this.DimBox);
            this.ConnectPage.Controls.Add(this.ConnectNoMasterBtn);
            this.ConnectPage.Controls.Add(this.FailsListBox);
            this.ConnectPage.Controls.Add(this.WgtLabel);
            this.ConnectPage.Controls.Add(this.AcfsListBox);
            this.ConnectPage.Controls.Add(this.MasterIpTxtBx);
            this.ConnectPage.Controls.Add(this.RadLabel);
            this.ConnectPage.Controls.Add(this.TcpDataFound);
            this.ConnectPage.Controls.Add(this.DimLabel);
            this.ConnectPage.Location = new System.Drawing.Point(4, 22);
            this.ConnectPage.Name = "ConnectPage";
            this.ConnectPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConnectPage.Size = new System.Drawing.Size(410, 558);
            this.ConnectPage.TabIndex = 0;
            this.ConnectPage.Text = "Connect";
            this.ConnectPage.UseVisualStyleBackColor = true;
            // 
            // ConnectNoMasterBtn
            // 
            this.ConnectNoMasterBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.ConnectNoMasterBtn.Location = new System.Drawing.Point(3, 23);
            this.ConnectNoMasterBtn.Name = "ConnectNoMasterBtn";
            this.ConnectNoMasterBtn.Size = new System.Drawing.Size(404, 23);
            this.ConnectNoMasterBtn.TabIndex = 1;
            this.ConnectNoMasterBtn.Text = "Connect";
            this.ConnectNoMasterBtn.UseVisualStyleBackColor = true;
            this.ConnectNoMasterBtn.Click += new System.EventHandler(this.ConnectNoMasterBtn_Click_1);
            // 
            // MasterIpTxtBx
            // 
            this.MasterIpTxtBx.Dock = System.Windows.Forms.DockStyle.Top;
            this.MasterIpTxtBx.Location = new System.Drawing.Point(3, 3);
            this.MasterIpTxtBx.Name = "MasterIpTxtBx";
            this.MasterIpTxtBx.Size = new System.Drawing.Size(404, 20);
            this.MasterIpTxtBx.TabIndex = 0;
            this.MasterIpTxtBx.Text = "192.168.5.17";
            // 
            // FailsPage
            // 
            this.FailsPage.Controls.Add(this.FailTabs);
            this.FailsPage.Location = new System.Drawing.Point(4, 22);
            this.FailsPage.Name = "FailsPage";
            this.FailsPage.Size = new System.Drawing.Size(410, 558);
            this.FailsPage.TabIndex = 3;
            this.FailsPage.Text = "Fails";
            this.FailsPage.UseVisualStyleBackColor = true;
            // 
            // AircraftsPage
            // 
            this.AircraftsPage.AutoScroll = true;
            this.AircraftsPage.Location = new System.Drawing.Point(4, 22);
            this.AircraftsPage.Name = "AircraftsPage";
            this.AircraftsPage.Size = new System.Drawing.Size(410, 558);
            this.AircraftsPage.TabIndex = 4;
            this.AircraftsPage.Text = "Aircrafts";
            this.AircraftsPage.UseVisualStyleBackColor = true;
            // 
            // CmdButtons
            // 
            this.CmdButtons.Controls.Add(this.CmdButtonsTabs);
            this.CmdButtons.Location = new System.Drawing.Point(4, 22);
            this.CmdButtons.Name = "CmdButtons";
            this.CmdButtons.Padding = new System.Windows.Forms.Padding(3);
            this.CmdButtons.Size = new System.Drawing.Size(410, 558);
            this.CmdButtons.TabIndex = 1;
            this.CmdButtons.Text = "Cmd Buttons";
            this.CmdButtons.UseVisualStyleBackColor = true;
            // 
            // MiscCommands
            // 
            this.MiscCommands.Controls.Add(this.WriteLogs);
            this.MiscCommands.Controls.Add(this.LogsBox);
            this.MiscCommands.Controls.Add(this.Message_TextBox);
            this.MiscCommands.Controls.Add(this.SendTCP);
            this.MiscCommands.Controls.Add(this.SendUDP);
            this.MiscCommands.Controls.Add(this.UDP_TextBox);
            this.MiscCommands.Location = new System.Drawing.Point(4, 22);
            this.MiscCommands.Name = "MiscCommands";
            this.MiscCommands.Size = new System.Drawing.Size(410, 558);
            this.MiscCommands.TabIndex = 2;
            this.MiscCommands.Text = "Misc";
            this.MiscCommands.UseVisualStyleBackColor = true;
            // 
            // WriteLogs
            // 
            this.WriteLogs.Location = new System.Drawing.Point(3, 532);
            this.WriteLogs.Name = "WriteLogs";
            this.WriteLogs.Size = new System.Drawing.Size(399, 23);
            this.WriteLogs.TabIndex = 7;
            this.WriteLogs.Text = "Write logs";
            this.WriteLogs.UseVisualStyleBackColor = true;
            this.WriteLogs.Click += new System.EventHandler(this.WriteLogs_Click);
            // 
            // LogsBox
            // 
            this.LogsBox.Location = new System.Drawing.Point(3, 151);
            this.LogsBox.Multiline = true;
            this.LogsBox.Name = "LogsBox";
            this.LogsBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogsBox.Size = new System.Drawing.Size(399, 375);
            this.LogsBox.TabIndex = 6;
            // 
            // MainControl
            // 
            this.MainControl.Controls.Add(this.AirportsPage);
            this.MainControl.Controls.Add(this.ConnectionStatusPage);
            this.MainControl.Controls.Add(this.ReleaseNotesPage);
            this.MainControl.Location = new System.Drawing.Point(12, 12);
            this.MainControl.Name = "MainControl";
            this.MainControl.SelectedIndex = 0;
            this.MainControl.Size = new System.Drawing.Size(451, 584);
            this.MainControl.TabIndex = 9;
            this.MainControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.MainControl_Selecting);
            // 
            // AirportsPage
            // 
            this.AirportsPage.Controls.Add(this.AirportControls);
            this.AirportsPage.Controls.Add(this.AirportChoose);
            this.AirportsPage.Controls.Add(this.airportsTextBox);
            this.AirportsPage.Location = new System.Drawing.Point(4, 22);
            this.AirportsPage.Name = "AirportsPage";
            this.AirportsPage.Padding = new System.Windows.Forms.Padding(3);
            this.AirportsPage.Size = new System.Drawing.Size(443, 558);
            this.AirportsPage.TabIndex = 1;
            this.AirportsPage.Text = "Airports";
            this.AirportsPage.UseVisualStyleBackColor = true;
            // 
            // AirportControls
            // 
            this.AirportControls.Location = new System.Drawing.Point(6, 162);
            this.AirportControls.Name = "AirportControls";
            this.AirportControls.Size = new System.Drawing.Size(428, 390);
            this.AirportControls.TabIndex = 2;
            // 
            // AirportChoose
            // 
            this.AirportChoose.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.AirportChoose.Location = new System.Drawing.Point(3, 136);
            this.AirportChoose.Name = "AirportChoose";
            this.AirportChoose.Size = new System.Drawing.Size(431, 20);
            this.AirportChoose.TabIndex = 1;
            this.AirportChoose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AirportChoosed);
            // 
            // ConnectionStatusPage
            // 
            this.ConnectionStatusPage.Location = new System.Drawing.Point(4, 22);
            this.ConnectionStatusPage.Name = "ConnectionStatusPage";
            this.ConnectionStatusPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConnectionStatusPage.Size = new System.Drawing.Size(443, 558);
            this.ConnectionStatusPage.TabIndex = 0;
            this.ConnectionStatusPage.Text = "Connection status";
            this.ConnectionStatusPage.UseVisualStyleBackColor = true;
            // 
            // ReleaseNotesPage
            // 
            this.ReleaseNotesPage.Controls.Add(this.ToDoTextBox);
            this.ReleaseNotesPage.Controls.Add(this.RealesNotesTextBox);
            this.ReleaseNotesPage.Location = new System.Drawing.Point(4, 22);
            this.ReleaseNotesPage.Name = "ReleaseNotesPage";
            this.ReleaseNotesPage.Size = new System.Drawing.Size(443, 558);
            this.ReleaseNotesPage.TabIndex = 2;
            this.ReleaseNotesPage.Text = "Release Notes";
            this.ReleaseNotesPage.UseVisualStyleBackColor = true;
            // 
            // ToDoTextBox
            // 
            this.ToDoTextBox.Location = new System.Drawing.Point(3, 415);
            this.ToDoTextBox.Multiline = true;
            this.ToDoTextBox.Name = "ToDoTextBox";
            this.ToDoTextBox.ReadOnly = true;
            this.ToDoTextBox.Size = new System.Drawing.Size(437, 143);
            this.ToDoTextBox.TabIndex = 1;
            // 
            // RealesNotesTextBox
            // 
            this.RealesNotesTextBox.Location = new System.Drawing.Point(3, 6);
            this.RealesNotesTextBox.Multiline = true;
            this.RealesNotesTextBox.Name = "RealesNotesTextBox";
            this.RealesNotesTextBox.ReadOnly = true;
            this.RealesNotesTextBox.Size = new System.Drawing.Size(437, 403);
            this.RealesNotesTextBox.TabIndex = 0;
            // 
            // Cloud1Type
            // 
            this.Cloud1Type.Location = new System.Drawing.Point(9, 356);
            this.Cloud1Type.Maximum = 6;
            this.Cloud1Type.Name = "Cloud1Type";
            this.Cloud1Type.Size = new System.Drawing.Size(505, 45);
            this.Cloud1Type.TabIndex = 50;
            // 
            // Cloud2Type
            // 
            this.Cloud2Type.Location = new System.Drawing.Point(9, 216);
            this.Cloud2Type.Maximum = 6;
            this.Cloud2Type.Name = "Cloud2Type";
            this.Cloud2Type.Size = new System.Drawing.Size(505, 45);
            this.Cloud2Type.TabIndex = 51;
            // 
            // Cloud3Type
            // 
            this.Cloud3Type.Location = new System.Drawing.Point(9, 64);
            this.Cloud3Type.Maximum = 6;
            this.Cloud3Type.Name = "Cloud3Type";
            this.Cloud3Type.Size = new System.Drawing.Size(505, 45);
            this.Cloud3Type.TabIndex = 52;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1439, 621);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.CentralControl);
            this.Controls.Add(this.MainControl);
            this.Controls.Add(this.CommandsTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Control App";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.VisibleChanged += new System.EventHandler(this.Form1_VisibleChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.WeatherPage.ResumeLayout(false);
            this.WeatherControlTabs.ResumeLayout(false);
            this.WeatherParamsPage.ResumeLayout(false);
            this.WeatherParamsPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dayBar)).EndInit();
            this.ArmospherePage.ResumeLayout(false);
            this.ArmospherePage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThermalStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThermalCover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Baro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Temperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StormBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RainBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VisibilityBar)).EndInit();
            this.WindPage.ResumeLayout(false);
            this.WindPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Altitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Altitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Gust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Strength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Turb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Direction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Strength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind1Gust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Turb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Altitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Turb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Gust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Strength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind2Direction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wind3Direction)).EndInit();
            this.CloudsPage.ResumeLayout(false);
            this.CloudsPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud1Top)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud1Base)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud3Base)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud3Top)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud2Base)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud2Top)).EndInit();
            this.CentralControl.ResumeLayout(false);
            this.MapPage.ResumeLayout(false);
            this.MapPage.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.CommandsTabControl.ResumeLayout(false);
            this.ConnectPage.ResumeLayout(false);
            this.ConnectPage.PerformLayout();
            this.FailsPage.ResumeLayout(false);
            this.CmdButtons.ResumeLayout(false);
            this.MiscCommands.ResumeLayout(false);
            this.MiscCommands.PerformLayout();
            this.MainControl.ResumeLayout(false);
            this.AirportsPage.ResumeLayout(false);
            this.AirportsPage.PerformLayout();
            this.ReleaseNotesPage.ResumeLayout(false);
            this.ReleaseNotesPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud1Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud2Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cloud3Type)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private StatusStrip statusStrip1;
        private ToolStripProgressBar ProgressBar;
        private OpenFileDialog openFileDialog1;
        private ToolStripStatusLabel StatusLabel;
        private Label TcpDataFound;
        private TabPage WeatherPage;
        private Button SendWxr;
        private TabControl WeatherControlTabs;
        private TabPage ArmospherePage;
        private Label ThermalStrLabel;
        private Label ThermCovLabel;
        private Label BaroLabel;
        private Label TempLabel;
        private Label StormLabel;
        private Label RainLabel;
        private Label VisibilityLabel;
        private TrackBar ThermalStrength;
        private TrackBar ThermalCover;
        private TrackBar Baro;
        private TrackBar Temperature;
        private CheckBox PatchyCheckBox;
        private RadioButton WetRadio;
        private RadioButton DampRadio;
        private RadioButton DryRadio;
        private TrackBar StormBar;
        private TrackBar RainBar;
        private TrackBar VisibilityBar;
        private TabPage WeatherParamsPage;
        private Label Cloud1TopLabel;
        private Label Cloud1BaseLabel;
        private TrackBar Cloud1Top;
        private TrackBar Cloud1Base;
        private Label Cloud2TopLabel;
        private Label Cloud2BaseLabel;
        private Label TimeLabel;
        private TrackBar Cloud2Top;
        private Label DayLabel;
        private TrackBar Cloud2Base;
        private Label Cloud3TopLabel;
        private Label Cloud3BaseLabel;
        private TrackBar Cloud3Top;
        private TrackBar Cloud3Base;
        private TrackBar timeBar;
        private TrackBar dayBar;
        private TextBox airportsTextBox;
        private TabControl CmdButtonsTabs;
        private Label WgtLabel;
        private ListBox DimBox;
        private Label DimLabel;
        private Label RadLabel;
        private ListBox FailsListBox;
        private ListBox AcfsListBox;
        private TextBox Message_TextBox;
        private TextBox UDP_TextBox;
        private Button SendUDP;
        private Button SendTCP;
        private TabControl CentralControl;
        private TabControl FailTabs;
        private TabControl CommandsTabControl;
        private TabPage ConnectPage;
        private TabPage CmdButtons;
        private TabPage MiscCommands;
        private TabPage FailsPage;
        private TabPage AircraftsPage;
        private Button WriteLogs;
        private TextBox LogsBox;
        private TabPage WindPage;
        private Label Wind1StrengthLabel;
        private Label Wind1DirLabel;
        private Label Wind1GustsLabel;
        private Label Wind1AltitudeLabel;
        private Label Wind1TurbLabel;
        private Label Wind2AltitudeLabel;
        private TrackBar Wind2Altitude;
        private Label Wind3TurbLabel;
        private TrackBar Wind1Altitude;
        private Label Wind3GustsLabel;
        private Label Wind2DirLabel;
        private TrackBar Wind3Gust;
        private Label Wind3StrengthLabel;
        private TrackBar Wind3Strength;
        private TrackBar Wind1Turb;
        private Label Wind3DirLabel;
        private TrackBar Wind1Direction;
        private Label Wind2StrengthLabel;
        private Label Wind2TurbLabel;
        private Label Wind3AltitudeLabel;
        private TrackBar Wind1Strength;
        private TrackBar Wind1Gust;
        private Label Wind2GustsLabel;
        private TrackBar Wind3Turb;
        private TrackBar Wind3Altitude;
        private TrackBar Wind2Turb;
        private TrackBar Wind2Gust;
        private TrackBar Wind2Strength;
        private TrackBar Wind2Direction;
        private TrackBar Wind3Direction;
        private TabPage CloudsPage;
        private TabControl MainControl;
        private TabPage AirportsPage;
        private TabPage ConnectionStatusPage;
        private TabPage ReleaseNotesPage;
        private TextBox RealesNotesTextBox;
        private TextBox ToDoTextBox;
        private TextBox AirportChoose;
        private Panel AirportControls;
        private TabPage MapPage;
        private TextBox TextBoxSpd;
        private TextBox TextBoxHdg;
        private TextBox TextBoxAgl;
        private TextBox TextBoxAsl;
        private TextBox TextBoxLat;
        private TextBox TextBoxLng;
        private Splitter splitter1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mapProviderToolStripMenuItem;
        private ToolStripMenuItem oSMToolStripMenuItem;
        private ToolStripMenuItem googleMapsToolStripMenuItem;
        private ToolStripMenuItem googleMapsSatteliteToolStripMenuItem;
        private GMap.NET.WindowsForms.GMapControl Map;
        private ToolStripMenuItem cloudMapToolStripMenuItem;
        private ToolStripMenuItem arcGISTerrainBaseToolStripMenuItem;
        private Button ConnectNoMasterBtn;
        private TextBox MasterIpTxtBx;
        private TrackBar Cloud3Type;
        private TrackBar Cloud2Type;
        private TrackBar Cloud1Type;
    }
}

