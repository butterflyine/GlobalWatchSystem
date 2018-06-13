using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DataServer;
using System.Net.Sockets;
using UtilityLibrary.DBOperator;
using ParseDataPluginInterface;
using System.Configuration;

namespace GPRSServer
{
    public partial class MainForm : Form
    {
        private int onlineDTU = 0;
        private int onlineDCC = 0;

        private FormWindowState preFormState = FormWindowState.Maximized;
        SQLHelper accessHelper = null;
        public MainForm()
        {
            InitializeComponent();
            accessHelper = new SQLHelper(ConfigurationManager.ConnectionStrings["GlobalWatchSystem"].ConnectionString.ToString());
            
            DataServer.DataTranceiver.Instance.DataReceivedHandle += new DataServer.NewDataComeHandler(Instance_DataReceivedHandle);
            DataServer.DataTranceiver.Instance.GPSComeHandler += Instance_GPSComeHandler;
            DataServer.DataTranceiver.Instance.RegisteChangedHandle += new NewRegisteChanged(Instance_RegisteChangedHandle);
            DataServer.DataTranceiver.Instance.DtuOnlineChangeHandle += new DtuOnlineChanged(Instance_DtuOnlineChangeHandle);
            DataServer.DataTranceiver.Instance.DccOnlineChangeHandle += new NewDCCConnected(Instance_DccOnlineChangeHandle);
            listView1.Items.Clear();

            preFormState = this.WindowState;
            StartServer();
        }

        void Instance_GPSComeHandler(object sender, GPSInfo e)
        {
            string sql = string.Format("insert into DtuGps(dev_number,recv_time,longitude,latitude) values('{0}','{1}',{2},{3})",
                  e.imei,
                  e.recvTime,
                  e.longitude,
                  e.latitude);
            accessHelper.ExcuteSql(sql);
        }

        void Instance_DccOnlineChangeHandle(object sender, DCCInfoArgs e)
        {
            //throw new NotImplementedException();
            UpdateDccInfoWindow(e.DccAddress, e.Online);
        }


        void Instance_DtuOnlineChangeHandle(object sender, RegisterInfoArgs e)
        {
            UpdateListBox(e.DtuInfo);
        }

        void Instance_RegisteChangedHandle(object sender, RegisterInfoArgs e)
        {
            UpdateListView(e.DtuInfo);
        }
        private void btnCloseDTUPanel_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = true;
        }

        private void btnCloseCltWnd_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel1Collapsed = true;
        }

        private void TSMI_ClsDisp_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel1Collapsed = false;
        }

        private void TSMI_DTUShow_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = false;
        }

        private void TMSI_About_Click(object sender, EventArgs e)
        {
            AboutForm aboutform = new AboutForm();
            aboutform.ShowDialog();
        }

        private void tsBtnStart_Click(object sender, EventArgs e)
        {
            StartServer();

        }


        private void StartServer()
        {
            try
            {
                Thread trd = new Thread(new ThreadStart(DataTranceiver.Instance.StartDTUServer));
                trd.Start();

                Thread trdDCC = new Thread(new ThreadStart(DataTranceiver.Instance.StartClientServer));
                trdDCC.Start();

                Thread trdGPS = new Thread(new ThreadStart(DataTranceiver.Instance.StartGPSServer));
                trdGPS.Start();

                tsBtnStop.Enabled = true;
                TSMI_StopServer.Enabled = true;

                停止服务MToolStripMenuItem.Enabled = true;

                启动服务TToolStripMenuItem.Enabled = false;

                TSMI_StartServer.Enabled = false;
                tsBtnStart.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法启动服务：" + ex.Message);
            }
        }

        private void tsBtnStop_Click(object sender, EventArgs e)
        {
            StopServer();
        }
        private void StopServer()
        {
            DataServer.DataTranceiver.Instance.StopServer();
            tsBtnStop.Enabled = false;
            TSMI_StopServer.Enabled = false;
            停止服务MToolStripMenuItem.Enabled = false;

            启动服务TToolStripMenuItem.Enabled = true;

            TSMI_StartServer.Enabled = true;
            tsBtnStart.Enabled = true;
        }

        void SaveToDB(string imei,byte[] data)
        {

            try
            {
                IParseDataMain box = new ParseDataForCoolBox.ParseDataForCoolBox(accessHelper);
                var vals = box.ParseData(data);

                foreach (var val in vals)
                {
                    foreach (var ele in val.DataElements)
                    {
                        string sql = string.Format("insert into DtuData(dev_number,dev_channel,recv_time,data_type,data_value) values('{0}',{1},'{2}',{3},{4})",
                            imei,
                            ele.channelNo,
                            val.dataTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            ele.unit,
                            ele.value);
                        accessHelper.ExcuteSql(sql);
                    }

                    foreach (var ele in val.GpsElement)
                    {
                        string sql = string.Format("insert into DtuGps(dev_number,recv_time,longitude,latitude) values('{0}','{1}',{2},{3})",
                            imei,
                            ele.dataTime,
                            ele.longtitude,
                            ele.latitude);
                        accessHelper.ExcuteSql(sql);
                    }
                    string updateSql = string.Format("update Devices set Battery = {0},PowerMode = {1} where IMEI = '{2}'",
                        val.battery,
                        val.warningElement.isPowerOK ? 0x01:0x00,
                        imei);
                    accessHelper.ExcuteSql(updateSql);
                }
            }
            catch(Exception ex)
            { 
            }
        }

        // -------------------- 
        void Instance_DataReceivedHandle(object sender, DataServer.DataComeArgs e)
        {
            string data = System.Text.Encoding.Default.GetString(e.GprsMsg.MsgBody);
            string dtuID = System.Text.Encoding.Default.GetString(e.GprsMsg.DtuIMEI, 0,15);

            StringBuilder sb = new StringBuilder();
           
            string msg = string.Format("{0} >> 收到数据【{1}字节】【设备:{2}】: {3}",
                DateTime.Now.ToString(),e.GprsMsg.MsgBody.Length, dtuID, data);

            UpdateRecvWnd(msg, Color.Blue);

            // send dtu data to dcc
            List<byte> sendBuf = new List<byte>();
            sendBuf.AddRange(new byte[] { 0x7e, 0x7e, 0x7e, 0x7e });
            sendBuf.Add(0x03); // dtu data
            int nLen = e.GprsMsg.DtuIMEI.Length + e.GprsMsg.MsgBody.Length;
            sendBuf.Add((byte)(nLen >> 8));
            sendBuf.Add((byte)(nLen));
           
            sendBuf.AddRange(e.GprsMsg.DtuIMEI);
            sendBuf.AddRange(e.GprsMsg.MsgBody);

            SaveToDB(dtuID, e.GprsMsg.MsgBody);

            DCCConnInfo[] clients = DataServer.DataTranceiver.Instance.GetDccClient(dtuID);

            foreach (DCCConnInfo client in clients)
            {
                if (client != null && client.client != null)
                {
                    NetworkStream ns = client.client.GetStream();

                    if (ns != null && ns.CanWrite)
                    {
                        ns.Write(sendBuf.ToArray(), 0, sendBuf.Count);
                    }
                    lock (this)
                    {
                        client.nNonAliveTick = 0;
                    }
                }
            }
        }

        private delegate void UpdateDccListBox(string dccAddress, bool online);
        private void UpdateDccInfoWindow(string dccAddress, bool online)
        {
            if (lbDCCConn.InvokeRequired)
            {
                lbDCCConn.Invoke(new UpdateDccListBox(UpdateDccInfoWindow), new object[] { dccAddress, online });
            }
            else
            {
                lbDCCConn.Items.Insert(0, string.Format("远程数据终端({0}) {1}:{2}", dccAddress,DateTime.Now.ToString() ,online ? "上线" : "下线"));

                if (online)
                {
                    onlineDCC++;
                }
                else
                {
                    onlineDCC--;
                }
                tslDCCCount.Text = string.Format("在线DCC {0}", onlineDCC);

                if (lbDCCConn.Items.Count > 3600)
                {
                    for (int i = 0; i < 1800; i++)
                    {
                        lbDCCConn.Items.RemoveAt(lbDCCConn.Items.Count - 1);
                    }
                }
            }
            
        }

        private delegate void InvokeUpdateListBox(DTUInfo info);
        private void UpdateListBox(DTUInfo info)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new InvokeUpdateListBox(UpdateListBox), new object[] { info });
            }
            else
            {
                if (!info.IsRegisted)
                {
                    return;
                }
                listBox1.Items.Insert(0, string.Format("{0}：DTU-{1}-{2}", info.LoginTime.ToString(), info.IMEIString, info.Online ? "上线" : "离线"));

                if (info.Online)
                {
                    onlineDTU++;
                }
                else
                {
                    onlineDTU--;
                }
                tslDTUCount.Text = string.Format("在线DTU {0}", onlineDTU);

                if (listBox1.Items.Count > 3600)
                {
                    for (int i = 0; i < 1800; i++)
                    {
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                    }
                }

                // send dtu state to dcdc
                List<byte> sendBuf = new List<byte>();
                sendBuf.AddRange(new byte[] { 0x7e, 0x7e, 0x7e, 0x7e});
                sendBuf.Add(0x02); // dtu status
                sendBuf.Add(0x00);
                sendBuf.Add(0x11);
                sendBuf.AddRange(ASCIIEncoding.Default.GetBytes(info.IMEIString));
                sendBuf.Add((byte)('\0'));

                sendBuf.Add((byte)(info.Online ? 0x01 : 0x00));

                DCCConnInfo[] clients = DataServer.DataTranceiver.Instance.GetDccClient(info.IMEIString);
                foreach (DCCConnInfo client in clients)
                {
                    if (client != null && client.client != null)
                    {
                        NetworkStream ns = client.client.GetStream();

                        if (ns != null && ns.CanWrite)
                        {
                            ns.Write(sendBuf.ToArray(), 0, sendBuf.Count);
                        }
                    }
                }
            }
        }
        private delegate void InvokeUpdateListView(DTUInfo msg);
        private void UpdateListView(DTUInfo info)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new InvokeUpdateListView(UpdateListView), new object[] { info });
            }
            else
            {
                if (!info.IsRegisted)
                {
                    return;
                }
                bool bExist = false;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].Text.Equals(info.IMEIString))
                    {
                        bExist = true;
                        listView1.Items[i].Text = info.IMEIString;
                        listView1.Items[i].SubItems[1].Text = info.DtuPosition;
                        listView1.Items[i].SubItems[2].Text = info.SimNumber;
                        listView1.Items[i].SubItems[3].Text = info.LoginTime.ToString("yyyy-MM-dd HH:mm:ss");
                        listView1.Items[i].SubItems[4].Text = info.LogoffTime.ToString("yyyy-MM-dd HH:mm:ss");
                        listView1.Items[i].SubItems[5].Text = info.RemoteIPAddress;
                        listView1.Items[i].SubItems[6].Text = info.Online ? "在线" : "离线";
                        listView1.Items[i].SubItems[7].Text = info.LastTickTime.ToString("yyyy-MM-dd HH:mm:ss");
                        listView1.Items[i].SubItems[8].Text = info.TickSpan.ToString();
                        StringBuilder sb = new StringBuilder();

                        foreach(string p2pImei in info.ListP2PDTU)
                        {
                            sb.Append(p2pImei);
                            sb.Append(";");
                        }
                        if (sb.Length > 0)
                        {
                            sb = sb.Remove(sb.Length - 1, 1);
                        }
                        listView1.Items[i].SubItems[9].Text = sb.ToString();
                    }
                }
                if (bExist == false)
                {
                    ListViewItem item = new ListViewItem(info.IMEIString);
                    
                    item.SubItems.Add(info.DtuPosition);
                    item.SubItems.Add(info.SimNumber);
                    item.SubItems.Add(info.LoginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(info.LogoffTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(info.RemoteIPAddress);
                    item.SubItems.Add(info.Online ? "在线" : "离线");
                    item.SubItems.Add(info.LastTickTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(info.TickSpan.ToString());
                    StringBuilder sb = new StringBuilder();

                    foreach (string p2pImei in info.ListP2PDTU)
                    {
                        sb.Append(p2pImei);
                        sb.Append(";");
                    }
                    if (sb.Length > 0)
                    {
                        sb = sb.Remove(sb.Length - 1, 1);
                    }
                    item.SubItems.Add(sb.ToString());
                    listView1.Items.Add(item);
                }
            }
        }
        private delegate void InvokeUpdateRecvWnd(string msg, Color color);
        private void UpdateRecvWnd(string msg, Color color)
        {
            if (lvRecvWnd.InvokeRequired)
            {
                lvRecvWnd.Invoke(new InvokeUpdateRecvWnd(UpdateRecvWnd), new object[] { msg, color });
            }
            else
            {
                if (lvRecvWnd.Items.Count > 3600)
                {
                    lvRecvWnd.BeginUpdate();
                    for (int i = 0; i < 1800; i++)
                    {
                        lvRecvWnd.Items.RemoveAt(0);
                    }
                    lvRecvWnd.EndUpdate();
                }
                lvRecvWnd.Items.Add(new ListViewItem(msg));
                lvRecvWnd.EnsureVisible(lvRecvWnd.Items.Count - 1);
            }
        }

        private void TMSI_ServiceSet_Click(object sender, EventArgs e)
        {
            ServiceSetForm svrForm = new ServiceSetForm();

            svrForm.ShowDialog();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                notifyIcon1.Visible = true;
            }
            base.OnSizeChanged(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            preFormState = this.WindowState;
            if (e.CloseReason == CloseReason.ApplicationExitCall
                || e.CloseReason == CloseReason.WindowsShutDown)
            {
                if (tsBtnStop.Enabled == true)
                {
                    DialogResult dr = MessageBox.Show(this, "服务正在运行，确定要退出？", "提示", MessageBoxButtons.YesNo);

                    if (dr == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        StopServer();
                    }
                }
            }
            else 
            {
                e.Cancel = true;
                this.Visible = false;
                notifyIcon1.Visible = true;
                return;
            }
            base.OnFormClosing(e);
        }

        private void ShowMainFormClicked(object sender, EventArgs e)
        {
            this.Visible = true;
            notifyIcon1.Visible = false;
            this.WindowState = preFormState;
        }

        private void StarServerItemClicked(object sender, EventArgs e)
        {
            StartServer();
        }

        private void StopServerItemClicked(object sender, EventArgs e)
        {
            StopServer();
        }

        private void ConfigItemClicked(object sender, EventArgs e)
        {
            TMSI_ServiceSet_Click(sender, e);
        }

        private void QuitItemClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            notifyIcon1.Visible = false;
            this.WindowState = preFormState;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           // for(int i = 0; i < DataTranceiver.Instance.)
        }
    }
}
