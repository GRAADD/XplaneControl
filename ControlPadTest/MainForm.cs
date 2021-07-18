using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ControlPadTest.Properties;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
//using MainLibrary;
using XplaneControl;

namespace ControlPadTest
{
    public partial class MainForm : Form
    {
        public static bool ThreadStopFlag;
        private object locker = new object();
        public static Thread loopThread;
        public static Thread Init;
        private Connection _connection;

        public MainForm()
        {
            InitializeComponent();
            FileInfo f = new FileInfo(Application.ExecutablePath);
            this.Text = $"Control app (release from {f.LastWriteTime.ToString()})";
        }
        private async void MainForm_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            Map.RoutesEnabled = true;
            Map.PolygonsEnabled = false;
            Map.MarkersEnabled = false;
            Map.NegativeMode = false;
            Map.RetryLoadTile = 0;
            Map.ShowTileGridLines = false;
            Map.AllowDrop = true;
            Map.IgnoreMarkerOnMouseWheel = true;
            Map.DragButton = MouseButtons.Left;
            Map.MapProvider = GMapProviders.OpenStreetMap;
            Map.MinZoom = 2;
            Map.MaxZoom = 22;
            Map.Zoom = 9;
            //loopThread = new Thread(PositionDrawer);
            //loopThread.Start();

            getAtmosphere();
            WriteReleaseNotes();

        }

        #region MapsRegion
        
        private void oSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Map.MapProvider = GMapProviders.OpenStreetMap;
        }

        private void googleMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Map.MapProvider = GMapProviders.GoogleMap;
        }

        private void googleMapsSatteliteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Map.MapProvider = GMapProviders.GoogleSatelliteMap;

        }

        private void cloudMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Map.MapProvider = GMapProviders.GoogleTerrainMap;
        }

        private void arcGISTerrainBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Map.MapProvider = GMapProviders.ArcGIS_World_Terrain_Base_Map;
        }

        private void DrawPosition()
        {
            Position.GetData(_connection.xLoc);
            Map.Invoke((MethodInvoker)delegate
            {
                Map.Position = new PointLatLng(Position.Lat, Position.Lng);
            });

            TextBoxAsl.Invoke((MethodInvoker)delegate
            {
                TextBoxAsl.Text = $"Altitude - {Position.Alt_Asl} meters";
            });

            TextBoxAgl.Invoke((MethodInvoker)delegate
            {
                TextBoxAgl.Text = $"Altitude (AGL) - {Position.Alt_Agl}";
            });

            TextBoxHdg.Invoke((MethodInvoker)delegate
            {
                TextBoxHdg.Text = $"Heading - {Position.Hdg}";
            });

            TextBoxSpd.Invoke((MethodInvoker)delegate
            {
                TextBoxSpd.Text = $"Speed - {Position.Speed} knots";
            });

            TextBoxLat.Invoke((MethodInvoker)delegate
            {
                TextBoxLat.Text = $"Latitude - {Position.Lat}";
            });

            TextBoxLng.Invoke((MethodInvoker)delegate
            {
                TextBoxLng.Text = $"Longitude - {Position.Lng}";
            });
        }
        //======================================================================================================================
        #endregion

        private void WriteReleaseNotes()
        {
            RealesNotesTextBox.Text = Resources.ReleaseNotes.Replace("\\r\\n", "\r\n").Replace("~", "\t");
            ToDoTextBox.Text = Resources.ToDoNotes.Replace("\\r\\n", "\r\n").Replace("~", "\t");
        }

        bool ntd = false;

        private async void FormUpdateLoop()
        {
            //Logger.WriteLog("Form update loop started!", ErrorLevel.Base);

            Thread.CurrentThread.Name = "FormUpdateLoop";
            bool AptsDrawed = false;
            string UDPTemp = "";

            while (Thread.CurrentThread.IsAlive)
            {
                try
                {
                    FillUpDowns();
                    UDP_TextBox.Invoke((MethodInvoker)delegate ()
                    {
                        if (UDPTemp != _connection.responseUDP && _connection.responseUDP != null)
                        {
                            UDPTemp = _connection.responseUDP;
                            UDP_TextBox.AppendText($"\r\n{_connection.responseUDP}");
                        }
                    });

                    TcpDataFound.Invoke((MethodInvoker)delegate ()
                    {
                        //"xLOC", "xFAL", "xDIM", "xACF", "xFIX", "xCMD", "xRAD", "xCON"
                        TcpDataFound.Text = $"XplaneIP = {_connection.MasterIp}, xCMD = {_connection.xCMD.Count}, " +
                                            $"xFIX = {_connection.xFIX.Count}, xNAVH = {_connection.xNAVH.Count},\r\n " +
                                            $"xRAD = {_connection.xRAD.Count}, xAPTP = {_connection.Airports.Count}, " +
                                            $"Fails = {_connection.FailMessages.Count}, Aircrafts = {_connection.AcfMessages.Count}";
                    });

                    string TCPTemp = "";
                    //if (CmdButtons.IsHandleCreated && _connection.CmdStarted)
                    //{
                    //    Thread finishingThread = new Thread(WriteCmdButtons);
                    //    finishingThread.Start();
                    //}

                    //DrawAirports();
                    //Answer_TextBox.Text += $"\n{_connection.responseUDP}";
                    

                    //string pos = $"{Position.DataFloats[0]}\r\n" +
                                 //$"{Position.DataFloats[1]}\r\n" +
                                 //$"{Position.DataFloats[2]}\r\n" +
                                 //$"{Position.DataFloats[3]}\r\n" +
                                 //$"{Position.DataFloats[4]}\r\n" +
                                 //$"{Position.DataFloats[5]}\r\n" +
                                 //$"{Position.DataFloats[6]}\r\n" +
                                 //$"{Position.DataFloats[7]}\r\n" +
                                 //$"{Position.DataFloats[8]}\r\n" +
                                 //$"{Position.DataFloats[9]}\r\n" +
                                 //$"{Position.DataFloats[10]}\r\n" +
                                 //$"{Position.DataFloats[11]}\r\n" +
                                 //$"{Position.DataFloats[12]}\r\n" +
                                 //$"{Position.DataFloats[13]}\r\n" +
                                 //$"{Position.DataFloats[14]}\r\n" +
                                 //$"{Position.DataFloats[15]}\r\n" +
                                 //$"{Position.DataFloats[16]}\r\n" +
                                 //$"{Position.DataFloats[17]}\r\n" +
                                 //$"{Position.DataFloats[18]}\r\n" +
                                 //$"{Position.DataFloats[19]}\r\n" +
                                 //$"{Position.DataFloats[20]}\r\n" +
                                 //$"{Position.DataFloats[21]}\r\n";
                    //PositionLabel.Invoke((MethodInvoker)delegate ()
                    //{
                    //    PositionLabel.Text = $"Position - {pos}";
                    //});
                    RadLabel.Invoke((MethodInvoker)delegate ()
                    {
                        RadLabel.Text = $"Dim - {_connection.xDim}";
                    });
                    DimLabel.Invoke((MethodInvoker)delegate ()
                    {
                        DimLabel.Text = $"Currenct Aircraft - {_connection.xAcf}";
                    });
                    WgtLabel.Invoke((MethodInvoker)delegate ()
                    {
                        WgtLabel.Text = $"Weight - {_connection.xWgt}";
                    });
                    DrawPosition();
                    if (AircraftsPage.IsHandleCreated)
                        await WriteAcfButtons();
                    

                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    //Logger.WriteLog(e, ErrorLevel.Error);
                }
            }
        }

        private Task FillUpDowns()
        {
            try
            {
                //if (UpDownRunning)
                //        return Task.CompletedTask;
                //    UpDownRunning = true;

                int failCount = _connection.FailMessages.Count;
                for (int i = 0; i < failCount; i++)
                {
                    string line = _connection.FailMessages[i].FullString();
                    if (FailsListBox.InvokeRequired)
                        FailsListBox.Invoke((MethodInvoker)delegate ()
                        {
                            if (!FailsListBox.Items.Contains(line))
                                FailsListBox.Items.Add(line);
                        });
                }

                int acfCount = _connection.AcfMessages.Count;
                for (int i = 0; i < acfCount; i++)
                {
                    string line = _connection.AcfMessages[i].FullString();
                    if (AcfsListBox.InvokeRequired)
                        AcfsListBox.Invoke((MethodInvoker)delegate ()
                        {
                            if (!AcfsListBox.Items.Contains(line))
                                AcfsListBox.Items.Add(line);
                        });
                }

                int radCount = _connection.RadMessages.Count;
                for (int i = 0; i < radCount; i++)
                {
                    string line = _connection.RadMessages[i].FullString();
                    if (DimBox.InvokeRequired)
                        DimBox.Invoke((MethodInvoker)delegate ()
                        {
                            if (!DimBox.Items.Contains(line))
                                DimBox.Items.Add(line);
                        });
                }
            }
            catch (Exception e)
            {
                //Logger.WriteLog(e, ErrorLevel.Error);
                //throw;
            }
            //UpDownRunning = false;
            return Task.CompletedTask;
        }
        
        List<string> WritedAircrafts = new List<string>();

        private Task WriteAcfButtons()
        {
            int acfCount = _connection.AcfMessages.Count;
            for (int i = 0; i < acfCount; i++)
            {
                if (_connection.AcfMessages[i].MessageString == "")
                    continue;
                if (!WritedAircrafts.Contains(_connection.AcfMessages[i].MessageString))
                    WritedAircrafts.Add(_connection.AcfMessages[i].MessageString);

                bool created = false;
                foreach (Button btn in AircraftsPage.Controls)
                {
                    if (btn.Text == _connection.AcfMessages[i].MessageString)
                        created = true;
                }

                if (created)
                    continue;

                int left = 30;
                int height = 25;
                Button button = new Button()
                {
                    Text = _connection.AcfMessages[i].MessageString,
                    Left = left,
                    Width = AircraftsPage.Width - left * 2,
                    Height = height,
                    Top = AircraftsPage.Controls.Count * (height + 2) + 10,
                    Name = _connection.AcfMessages[i].MessageString.Replace(' ', '_')
                };
                button.Click += AcfClick;

                AircraftsPage.Invoke((MethodInvoker)delegate ()
                {
                    AircraftsPage.Controls.Add(button);
                });
            }
            return Task.CompletedTask;
        }
        private void AcfClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string name = button.Text;
            //Logger.WriteLog($"Changing aircraft to {name}", ErrorLevel.Info);
            _connection.SendMessage($"ACFN {name}");
        }

        private int WritedFailsCount = 0;
        private Task WriteFails()
        {
            if (WritedFailsCount == Fails.FailsList.Count)
                return Task.CompletedTask;
            WritedFailsCount = Fails.FailsList.Count;
            if (Fails.Pages.Count == 0)
                return Task.CompletedTask;

            foreach (var fail in Fails.FailsList)
            {
                try
                {

                    FailPage failPage = Fails.Get(fail.Page);
                    if (failPage.Num == -1)
                        continue;
                    FailSubPage failSubPage = failPage.Get(fail.SubPage);
                    if (failSubPage.Name == "")
                        continue;

                    this.Invoke((MethodInvoker)delegate ()
                    {
                        ProgressBar.Maximum = Fails.FailsList.Count;
                        if (fail.Num > ProgressBar.Maximum)
                            ProgressBar.Maximum = fail.Num;
                        ProgressBar.Value = fail.Num;
                    });

                    string pageName = failPage.Name;
                    string subPageName = failSubPage.Name;

                    TabPage tabPage = new TabPage()
                    {
                        Text = "-1"
                    };
                    TabControl tabControl = new TabControl();
                    foreach (TabPage tab in FailTabs.TabPages)
                        if (tab.Text == pageName)
                        {
                            tabPage = tab;
                            foreach (TabControl control in tab.Controls)
                                tabControl = control;
                        }
                    if (tabPage.Text == "-1")
                    {
                        tabPage.Text = pageName;
                        FailTabs.Invoke((MethodInvoker)delegate ()
                        {
                            FailTabs.TabPages.Add(tabPage);
                        });
                        tabControl = new TabControl()
                        {
                            Width = tabPage.Width - 8,
                            Left = 2,
                            Top = 2,
                            Height = tabPage.Height,
                            Name = pageName.Replace(' ', '_')
                        };
                        tabPage.Invoke((MethodInvoker)delegate ()
                        {
                            tabPage.Controls.Add(tabControl);
                        });
                    }

                    TabPage tabSubPage = new TabPage()
                    {
                        Text = "-1"
                    };
                    foreach (TabPage tabSub in tabControl.TabPages)
                        if (tabSub.Text == subPageName)
                            tabSubPage = tabSub;
                    if (tabSubPage.Text == "-1")
                    {
                        tabSubPage.Text = subPageName;
                        tabSubPage.AutoScroll = true;
                        tabControl.Invoke((MethodInvoker)delegate ()
                        {
                            tabControl.TabPages.Add(tabSubPage);
                        });
                    }

                    Button button = new Button();
                    button.Text = "-1";
                    foreach (Button btn in tabSubPage.Controls)
                        if (btn.Text == fail.Name)
                            button = btn;
                    if (button.Text == "-1")
                    {
                        button = new Button()
                        {
                            Tag = fail.Num,
                            Text = fail.Name,
                            Name = fail.Name.Replace(' ', '_'),
                            Enabled = fail.Visibility,
                            Height = 30,
                            Width = FailTabs.Width - 60,
                            Left = 2,
                            Top = tabSubPage.Controls.Count * 30 + 4
                        };
                        button.Click += FailClick;
                        tabSubPage.Invoke((MethodInvoker)delegate ()
                        {
                            tabSubPage.Controls.Add(button);
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            this.Invoke((MethodInvoker)delegate ()
            {
                ProgressBar.Maximum = 100;
                ProgressBar.Value = 0;
            });
            return Task.CompletedTask;
        }

        private void FailClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string num = button.Tag.ToString();
            string name = button.Name.ToString();
            //Logger.WriteLog($"Sending fail {name} (number {num})", ErrorLevel.Info);
            _connection.SendMessage($"FAIL {num}");
        }

        /// <summary>
        /// создание кнопок команд CMD
        /// </summary>
        private void WriteCmdButtons()
        {
            lock (locker)
            {
                //Thread.CurrentThread.Name = "Filling data";
                //Logger.WriteLog($"Creating CMD buttons", ErrorLevel.Info);
                int top = 10;
                int left = 30;
                int pageIndex = 0;
                int width = CmdButtonsTabs.Width - left * 2;

                for (int i = 0; i < _connection.xCMD.Count; i++)
                {
                    int createTime = 0;
                    CmdButtonsTabs?.Invoke((MethodInvoker)delegate ()
                    {
                        CmdButtonsTabs.UseWaitCursor = true;
                    });

                    this?.Invoke((MethodInvoker)delegate ()
                    {
                        ProgressBar.Maximum = _connection.xCMD.Count;
                        ProgressBar.Value = i;
                    });

                    if (_connection.xCMD[i].Split('/').Length < 2)
                        continue;
                    string tabname = _connection.xCMD[i].Split('/')[1];
                    TabPage page = new TabPage(tabname);
                    page.Name = tabname;
                    page.AutoScroll = true;

                    bool needNew = true;

                    foreach (TabPage tab in CmdButtonsTabs.TabPages)
                    {
                        if (tab.Text == tabname)
                        {
                            needNew = false;
                            page = tab;
                        }
                    }

                    if (needNew)
                        CmdButtonsTabs?.Invoke((MethodInvoker)delegate ()
                        {
                            CmdButtonsTabs.TabPages.Add(page);
                        });

                    bool buttonCreated = false;
                    foreach (Button but in page.Controls)
                    {
                        if (but.Text == _connection.xCMD[i])
                            buttonCreated = true;
                    }
                    if (buttonCreated)
                        continue;


                    Button button = new Button();
                    button.Left = left;
                    button.Width = CmdButtonsTabs.Width - button.Left * 2;
                    button.Top = top;
                    button.Name = "CmdButton" + i;
                    button.Click += CmdClick;
                    button.Text = _connection.xCMD[i];
                    button.Top = page.Controls.Count * (button.Height + 2) + 10;

                    Thread.Sleep(createTime);
                    while (!page.IsHandleCreated)
                    {
                        if (!this.Visible)
                            return;
                        createTime++;
                        Thread.Sleep(createTime);
                    }
                    page?.Invoke((MethodInvoker)delegate ()
                    {
                        page.Controls.Add(button);
                    });
                }
                CmdButtonsTabs?.Invoke((MethodInvoker)delegate ()
                {
                    CmdButtonsTabs.UseWaitCursor = false;
                });
                this?.Invoke((MethodInvoker)delegate ()
                {
                    ProgressBar.Maximum = 100;
                    ProgressBar.Value = 0;
                });
            }
        }

        private void CmdClick(object sender, EventArgs args)
        {
            //_connection.Stop = true;
            var button = (Button)sender;
            //_connection.MasterIp = button.Tag.ToString();
            button.Invoke((MethodInvoker)delegate ()
            {
                //button.Enabled = false;
                if (button != null)
                    _connection.SendMessage($"CMND {button.Text}");
                //Logger.WriteLog($"Sending {button.Text}", ErrorLevel.Base);
                //button.Enabled = true;
            });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Logger.WriteLog($"Closing form", ErrorLevel.Info);
                //if (_connection != null)
                //    _connection.forceStop = true;

                //_connection = null;
                if (loopThread != null)
                {
                    loopThread.Abort();
                    //_connection = null;
                }
            }
            catch (Exception exception)
            {
                //Logger.WriteLog(exception, ErrorLevel.Error);
                throw;
            }

            //Logger.SaveLogs();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (loopThread != null)
                {
                    loopThread.Abort();
                    _connection.CloseConnection();
                    //_connection = null;
                }
            }
            catch (Exception exception)
            {
                //Logger.WriteLog(exception, ErrorLevel.Error);
                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _connection.messageTCP = Message_TextBox.Text;
            Message_TextBox.Clear();
        }

        private void SendUDP_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Message_TextBox.Text))
                foreach (var line in Message_TextBox.Text.Split(new[] { '\r', '\n' }))
                {
                    if (!String.IsNullOrEmpty(line))
                        _connection.SendMessage(line);
                    //Logger.WriteLog($"Sending manually {line}", ErrorLevel.Info);
                }
            Message_TextBox.Clear();
        }

        private void SendWxr_Click(object sender, EventArgs e)
        {
            _connection.SendMessage(CmdEncoder.MakeWeatherBytes(getAtmosphere()));
        }

        #region WeatherRegion

        
        private void WeatherChanged(object sender, EventArgs e)
        {
            getAtmosphere();
            //Logger.WriteLog($"Changing atmosphere settings", ErrorLevel.Info);
        }

        private bool WxrCreated = false;

        private Atmosphere getAtmosphere()
        {
            Atmosphere stats = new Atmosphere();
            try
            {
                if (!WxrCreated)
                {
                    Atmosphere atmosphereTemp = new Atmosphere();
                    //Cloud1Type.Items.Clear();
                    //Cloud2Type.Items.Clear();
                    //Cloud3Type.Items.Clear();
                    //for (int i = 0; i <= 6; i++)
                    //{
                    //    if (i == 0)
                    //    {
                    //        Cloud1Type.Text = atmosphereTemp.GetCatString(i);
                    //        Cloud2Type.Text = atmosphereTemp.GetCatString(i);
                    //        Cloud3Type.Text = atmosphereTemp.GetCatString(i);
                    //    }

                    //    Cloud1Type.Items.Add(atmosphereTemp.GetCatString(i));
                    //    Cloud2Type.Items.Add(atmosphereTemp.GetCatString(i));
                    //    Cloud3Type.Items.Add(atmosphereTemp.GetCatString(i));
                    //}

                    //Cloud1Type.Select();
                    //Cloud1Type.Select(0, Cloud1Type.Text.Length);
                    //Cloud2Type.Select();
                    //Cloud2Type.Select(1, Cloud1Type.Text.Length);
                    //Cloud3Type.Select();
                    //Cloud3Type.Select(1, Cloud1Type.Text.Length);
                }

                DayLabel.Text = stats.GetStringFromValue(dayBar.Value, Atmosphere.Type.TDate);
                TimeLabel.Text = stats.GetStringFromValue(timeBar.Value, Atmosphere.Type.TTime);

                VisibilityLabel.Text = stats.GetStringFromValue(VisibilityBar.Value, Atmosphere.Type.TVisibility);
                RainLabel.Text = stats.GetStringFromValue(RainBar.Value, Atmosphere.Type.TRain);
                StormLabel.Text = stats.GetStringFromValue(StormBar.Value, Atmosphere.Type.TStorm);

                if (DryRadio.Checked)
                {
                    WetRadio.Checked = false;
                    DampRadio.Checked = false;
                    stats.RunWayStat = Atmosphere.RwStat.dry;
                }
                else if (WetRadio.Checked)
                {
                    DampRadio.Checked = false;
                    stats.RunWayStat = Atmosphere.RwStat.wet;
                }
                else
                    stats.RunWayStat = Atmosphere.RwStat.damp;

                stats.RunWayPatches = PatchyCheckBox.Checked;

                TempLabel.Text = stats.GetStringFromValue(Temperature.Value, Atmosphere.Type.TTemperature);
                BaroLabel.Text = stats.GetStringFromValue(Baro.Value, Atmosphere.Type.TBaro);


                ThermCovLabel.Text = stats.GetStringFromValue(ThermalCover.Value, Atmosphere.Type.TTermCov);
                ThermalStrLabel.Text = stats.GetStringFromValue(ThermalStrength.Value, Atmosphere.Type.TTermStr);

                if (Wind2Altitude.Value < Wind1Altitude.Value)
                    if (Wind1Altitude.Value + 2000 > Wind1Altitude.Maximum)
                    {
                        Wind1Altitude.Value = Wind1Altitude.Maximum - 2000;
                        Wind2Altitude.Value = Wind2Altitude.Maximum - 1000;
                        Wind3Altitude.Value = Wind3Altitude.Maximum;
                    }
                    else
                        Wind2Altitude.Value = Wind1Altitude.Value + 1000;

                if (Wind3Altitude.Value < Wind2Altitude.Value)
                {
                    if (Wind2Altitude.Value + 1000 > Wind2Altitude.Maximum)
                    {
                        Wind2Altitude.Value = Wind2Altitude.Maximum - 1000;
                        Wind3Altitude.Value = Wind3Altitude.Maximum;
                    }
                    else
                        Wind3Altitude.Value = Wind2Altitude.Value + 1000;
                }

                Wind1AltitudeLabel.Text = stats.GetStringFromValue(Wind1Altitude.Value, Atmosphere.Type.TWindLayer1Alt);
                Wind2AltitudeLabel.Text = stats.GetStringFromValue(Wind2Altitude.Value, Atmosphere.Type.TWindLayer2Alt);
                Wind3AltitudeLabel.Text = stats.GetStringFromValue(Wind3Altitude.Value, Atmosphere.Type.TWindLayer3Alt);

                Wind1DirLabel.Text = stats.GetStringFromValue(Wind1Direction.Value, Atmosphere.Type.TWindLayer1Dir);
                Wind2DirLabel.Text = stats.GetStringFromValue(Wind2Direction.Value, Atmosphere.Type.TWindLayer2Dir);
                Wind3DirLabel.Text = stats.GetStringFromValue(Wind3Direction.Value, Atmosphere.Type.TWindLayer3Dir);

                Wind1StrengthLabel.Text = stats.GetStringFromValue(Wind1Strength.Value, Atmosphere.Type.TWindLayer1Str);
                Wind2StrengthLabel.Text = stats.GetStringFromValue(Wind2Strength.Value, Atmosphere.Type.TWindLayer2Str);
                Wind3StrengthLabel.Text = stats.GetStringFromValue(Wind3Strength.Value, Atmosphere.Type.TWindLayer3Str);

                if (Wind1Strength.Value > Wind1Gust.Value)
                    Wind1Gust.Value = Wind1Strength.Value;
                if (Wind2Strength.Value > Wind2Gust.Value)
                    Wind2Gust.Value = Wind2Strength.Value;
                if (Wind3Strength.Value > Wind3Gust.Value)
                    Wind3Gust.Value = Wind3Strength.Value;
                Wind1GustsLabel.Text = stats.GetStringFromValue(Wind1Gust.Value - Wind1Strength.Value, Atmosphere.Type.TWindLayer1Gusts);
                Wind2GustsLabel.Text = stats.GetStringFromValue(Wind2Gust.Value - Wind2Strength.Value, Atmosphere.Type.TWindLayer2Gusts);
                Wind3GustsLabel.Text = stats.GetStringFromValue(Wind3Gust.Value - Wind3Strength.Value, Atmosphere.Type.TWindLayer3Gusts);



                if (Cloud3Base.Value + 2000 > Cloud3Top.Maximum)
                {
                    Cloud3Base.Value = Cloud3Top.Maximum - 2000;
                    Cloud3Top.Value = Cloud3Top.Maximum;
                }
                if (Cloud2Base.Value < Cloud1Base.Minimum + 3000)
                {
                    Cloud2Base.Value = Cloud1Base.Value + 3000;
                }

                if (Cloud1Base.Value + 2000 > Cloud1Top.Value)
                    if (Cloud1Top.Value + 1000 > Cloud2Base.Value)
                        Cloud1Top.Value = Cloud2Base.Value - 1000;
                    else
                        Cloud1Top.Value = Cloud1Base.Value + 2000;
                if (Cloud2Base.Value + 2000 > Cloud2Top.Value)
                    if (Cloud2Top.Value + 1000 > Cloud3Base.Value)
                        Cloud2Top.Value = Cloud3Base.Value - 1000;
                    else
                        Cloud2Top.Value = Cloud2Base.Value + 2000;
                if (Cloud3Base.Value + 2000 > Cloud3Top.Value)
                    if (Cloud3Base.Value + 2000 < Cloud3Base.Maximum)
                        Cloud3Top.Value = Cloud3Base.Value + 2000;
                
                Cloud1BaseLabel.Text = stats.GetStringFromValue(Cloud1Base.Value, Atmosphere.Type.TCloudsLayer1Base);
                Cloud2BaseLabel.Text = stats.GetStringFromValue(Cloud2Base.Value, Atmosphere.Type.TCloudsLayer2Base);
                Cloud3BaseLabel.Text = stats.GetStringFromValue(Cloud3Base.Value, Atmosphere.Type.TCloudsLayer3Base);

                Cloud1TopLabel.Text = stats.GetStringFromValue(Cloud1Top.Value, Atmosphere.Type.TCloudsLayer1Top);
                Cloud2TopLabel.Text = stats.GetStringFromValue(Cloud2Top.Value, Atmosphere.Type.TCloudsLayer2Top);
                Cloud3TopLabel.Text = stats.GetStringFromValue(Cloud3Top.Value, Atmosphere.Type.TCloudsLayer3Top);

                stats.GetStringFromValue(Cloud1Type.Value, Atmosphere.Type.TCloudsLayer1Cat);
                stats.GetStringFromValue(Cloud2Type.Value, Atmosphere.Type.TCloudsLayer2Cat);
                stats.GetStringFromValue(Cloud3Type.Value, Atmosphere.Type.TCloudsLayer1Cat);

                stats.CloudsLayer1.BaseHight = Cloud1Base.Value;
                stats.CloudsLayer2.BaseHight = Cloud2Base.Value;
                stats.CloudsLayer3.BaseHight = Cloud3Base.Value;

                stats.CloudsLayer1.TopHight = Cloud1Top.Value;
                stats.CloudsLayer2.TopHight = Cloud2Top.Value;
                stats.CloudsLayer3.TopHight = Cloud3Top.Value;
                
                stats.CloudsLayer1.CategoryClouds = stats.GetCloudsCat(Cloud1Type.Value);
                stats.CloudsLayer2.CategoryClouds = stats.GetCloudsCat(Cloud2Type.Value);
                stats.CloudsLayer3.CategoryClouds = stats.GetCloudsCat(Cloud3Type.Value);

                if (Cloud3Type.Created)
                    WxrCreated = true;
            }
            catch (StackOverflowException soException)
            {
                //Logger.WriteLog(soException, ErrorLevel.Base);

            }
            catch (Exception e)
            {
                //Logger.WriteLog(e, ErrorLevel.Error);
            }

            return stats;
        }

        
        //======================================================================================================================
        #endregion

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            //Logger.SaveLogs();
        }

        private void WriteLogs_Click(object sender, EventArgs e)
        {
            LogsBox.Text = "";
            //foreach (var line in Logger.LogsList)
            //    LogsBox.Text += $"{line}\r\n";
        }

        #region Aiports
        
        private void DrawAirports()
        {
            //Logger.WriteLog($"Writing airports", ErrorLevel.Info);
            //string apts = "";

            //airportsTextBox?.Invoke((MethodInvoker)delegate ()
            //{
            //    airportsTextBox.Text = "";
            //});
            for (int i = AirportChoose.AutoCompleteCustomSource.Count * 4; i < _connection.Airports.Count; i++)
            {
                //string rws = "";
                //foreach (var rw in _connection.Airports[i].Rws)
                //{
                //    rws += $"{rw.Rw1String} {rw.Rw2String}; ";
                //}

                //apts +=
                //    $"{_connection.Airports[i].CodeString} - {_connection.Airports[i].NameString} ({rws}) {Environment.NewLine}";
                AirportChoose?.Invoke((MethodInvoker)delegate ()
                {
                    AirportChoose.AutoCompleteCustomSource.Add(_connection.Airports[i].NameString.ToLower());
                    AirportChoose.AutoCompleteCustomSource.Add(_connection.Airports[i].NameString.ToUpper());
                    AirportChoose.AutoCompleteCustomSource.Add(_connection.Airports[i].CodeString.ToLower());
                    AirportChoose.AutoCompleteCustomSource.Add(_connection.Airports[i].CodeString.ToUpper());
                });
                this?.Invoke((MethodInvoker)delegate ()
                {
                    ProgressBar.Maximum = _connection.Airports.Count;
                    ProgressBar.Value = i;
                });
            }
            //AirportChoose?.Invoke((MethodInvoker)delegate ()
            //{
            //    AirportChoose.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //});

            //airportsTextBox?.Invoke((MethodInvoker)delegate ()
            //{
            //    airportsTextBox.Text = apts;
            //});

            this.Invoke((MethodInvoker)delegate ()
            {
                ProgressBar.Maximum = 100;
                ProgressBar.Value = 0;
            });
        }

        private void AirportChoosed(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CreateAirportButtons(AirportChoose.Text);
            }
        }

        private Task CreateAirportButtons(string text)
        {

            AirportControls.Controls.Clear();

            XAirport airport = ByteOperations.GetAirport(text);
            if (string.IsNullOrEmpty(airport.CodeString))
                return Task.CompletedTask;

            AirportControls.BorderStyle = BorderStyle.None;
            AirportControls.AutoScroll = true;
            int top = 5;
            int left = 10;
            int height = 20;
            int smlWidth = 50;
            AirportControls.Controls.Add(new Label()
            {
                Text = $"{airport.CodeString} - {airport.NameString}",
                Left = left,
                Top = top,
                Height = height,
                Width = AirportControls.Width - left * 2 - 10
            });

            top = top * 2 + height + 5;
            foreach (var rws in airport.Rws)
            {
                Button button = new Button()
                {
                    Name = $"{rws.Rw1String}_10nm",
                    Tag = airport.CodeString,
                    Text = $"{rws.Rw1String}_10nm",
                    Left = left,
                    Top = top,
                    Height = height,
                    Width = smlWidth
                };
                button.Click += RWButton_click;
                AirportControls.Controls.Add(button);

                button = new Button()
                {
                    Name = $"{rws.Rw1String}_3nm",
                    Tag = airport.CodeString,
                    Text = $"{rws.Rw1String}_3nm",
                    Left = left * 2 + smlWidth,
                    Top = top,
                    Height = height,
                    Width = smlWidth
                };
                button.Click += RWButton_click;
                AirportControls.Controls.Add(button);

                button = new Button()
                {
                    Name = $"{rws.Rw1String}_start",
                    Tag = airport.CodeString,
                    Text = $"{rws.Rw1String}_start",
                    Left = left * 3 + smlWidth * 2,
                    Top = top,
                    Height = height,
                    Width = AirportControls.Width - smlWidth * 2 - left * 4 - 10
                };
                button.Click += RWButton_click;
                AirportControls.Controls.Add(button);

                top += height + 5;

                if (!string.IsNullOrEmpty(rws.Rw2String))
                {
                    button = new Button()
                    {
                        Name = $"{rws.Rw2String}_10nm",
                        Tag = airport.CodeString,
                        Text = $"{rws.Rw2String}_10nm",
                        Left = left,
                        Top = top,
                        Height = height,
                        Width = smlWidth
                    };
                    button.Click += RWButton_click;
                    AirportControls.Controls.Add(button);

                    button = new Button()
                    {
                        Name = $"{rws.Rw2String}_3nm",
                        Tag = airport.CodeString,
                        Text = $"{rws.Rw2String}_3nm",
                        Left = left * 2 + smlWidth,
                        Top = top,
                        Height = height,
                        Width = smlWidth
                    };
                    button.Click += RWButton_click;
                    AirportControls.Controls.Add(button);

                    button = new Button()
                    {
                        Name = $"{rws.Rw2String}_start",
                        Tag = airport.CodeString,
                        Text = $"{rws.Rw2String}_start",
                        Left = left * 3 + smlWidth * 2,
                        Top = top,
                        Height = height,
                        Width = AirportControls.Width - smlWidth * 2 - left * 4 - 10
                    };
                    button.Click += RWButton_click;
                    AirportControls.Controls.Add(button);

                    top += height + 5;
                }
            }

            for (int standNum = 0; standNum < 5; standNum++)
            {
                Button stand1Button = new Button()
                {
                    Name = "SetStand",
                    Tag = airport.CodeString,
                    Text = $"Set Stand {standNum}",
                    Left = left,
                    Top = top + height + 5,
                    Height = height,
                    Width = AirportControls.Width - left * 2 - 10
                };
                stand1Button.Click += (object sender, EventArgs e) =>
                {
                    //CmdEncoder CmdEncoder = new CmdEncoder();
                    Button button = (Button)sender;
                    string code = button.Tag.ToString();
                    string standNumber = $"{button.Text.Split(' ')[2]}_Stand";
                    _connection.SendMessage(CmdEncoder.MoveToAirportBytes(code, standNumber));
                };
                AirportControls.Controls.Add(stand1Button);
                top = top + height + 5; 
            }

            Label label = new Label();
            byte[] htmlCode;
            try
            {
                using (WebClient client = new WebClient())
                {
                    htmlCode = client.DownloadData($"https://metartaf.ru/{airport.CodeString}.json");
                }

                string metar = Encoding.UTF8.GetString(htmlCode);
                metar = metar.Replace("\"", "");
                metar = metar.Replace("{", "");
                metar = metar.Replace("}", "");
                metar = metar.Replace(",", "\r\n");
                metar = metar.Replace("\\n", "\r\n");
                metar = metar.Replace("NOSIG", "\r\nNOSIG");
                metar = metar.Replace("TEMPO", "\r\nTEMPO");
                metar = metar.Replace("BECMG", "\r\nBECMG");
                label = new Label()
                {
                    Text = $"{metar}",
                    Left = left,
                    Top = top + height + 5,
                    Height = height * 8,
                    Width = AirportControls.Width - left * 2 - 10
                };
                AirportControls.Controls.Add(label);
            }
            catch (Exception e)
            {
                //Logger.WriteLog(e, ErrorLevel.Base);
            }


            return Task.CompletedTask;
        }

        private void RWButton_click(object sender, EventArgs e)
        {
            //CmdEncoder CmdEncoder = new CmdEncoder();
            Button button = (Button)sender;
            string code = button.Tag.ToString();
            string place = button.Text;
            _connection.SendMessage(CmdEncoder.MoveToAirportBytes(code, place));
        }

        private void SetParking(object sender, EventArgs e)
        {
            //CmdEncoder CmdEncoder = new CmdEncoder();
            Button button = (Button)sender;
            string code = button.Tag.ToString();
            string standNumber = "1_Stand";
            _connection.SendMessage(CmdEncoder.MoveToAirportBytes(code, standNumber));
            //for (int i = 1; i < 255; i++)
            //{
            //    _connection.SendMessage(CmdEncoder.MoveToAirportBytes(code, $"{i}_Stand"));
            //    Thread.Sleep(3000);
            //}
        }

        #endregion

        private void dayBar_Scroll(object sender, EventArgs e)
        {

        }

        private void ConnectNoMasterBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                _connection = new Connection(MasterIpTxtBx.Text);
                //ByteOperations.connection = _connection;
                SendTCP.Enabled = true;
                SendUDP.Enabled = true;
                loopThread = new Thread(FormUpdateLoop);
                loopThread.Start();
                //ConnectPage.Dispose();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void MainControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabControl control = (TabControl) sender;
            if (control.SelectedTab.Text == "Airports")
            {
                Thread drawAptsThread = new Thread(DrawAirports);
                drawAptsThread.Name = "Airports draw thread";
                drawAptsThread.Start();
            }
        }

        private void CommandsTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            
            TabControl control = (TabControl) sender;
            if (control.SelectedTab.Text == "Cmd Buttons")
            {
                Thread drawCmdThread = new Thread(WriteCmdButtons);
                drawCmdThread.Name = "Cmd draw thread";
                drawCmdThread.Start();
            }
        }
    }
}