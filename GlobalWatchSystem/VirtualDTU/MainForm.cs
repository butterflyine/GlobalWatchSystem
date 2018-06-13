using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace VirtualDTU
{
    public partial class MainForm : Form
    {
        TcpClient tcpClient = new TcpClient();

        Protocol protocol = new Protocol();

        Random random = null;

        public MainForm()
        {
            InitializeComponent();
            random = new Random(DateTime.Now.Millisecond);
            
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if(tcpClient.Connected)
            {
                tcpClient.Close();
            }
            base.OnFormClosing(e);
        }
        private void timerSendData_Tick(object sender, EventArgs e)
        {
            if (!tcpClient.Connected)
            {
                return;
            }
            btnConn_Click(null, EventArgs.Empty);
            btnLogin_Click(null, EventArgs.Empty);
            if (timerSendData.Tag == "Random")
            {
                VirtualDTU.Protocol.PowerMode powerMode = Protocol.PowerMode.ACPower;
                if(rdBtnACMode.Checked)
                {
                    powerMode = Protocol.PowerMode.ACPower;
                }
                else
                {
                    powerMode = Protocol.PowerMode.DCPower;
                }
                byte[] vals = protocol.GetDataMsg(txtIMEI.Text, random.NextDouble() * 100, random.NextDouble() * 100,
                    random.NextDouble() * 50, random.NextDouble() * 50, Convert.ToDouble(numUDVolt.Value), powerMode);
                tcpClient.Client.Send(vals);

                byte[] recvs = new byte[1024];
                tcpClient.Client.Receive(recvs);
                tbLog.AppendText(System.Text.Encoding.ASCII.GetString(recvs) + "\r\n");
            }
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient != null)
                {
                    btnDisconn_Click(null, EventArgs.Empty);
                }
                tcpClient = new TcpClient();
                tcpClient.Connect(txtIP.Text, Convert.ToInt32(numUDPort.Value));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDisconn_Click(object sender, EventArgs e)
        {
            if (tcpClient.Connected)
            {
                tcpClient.Client.Close();
                tcpClient.Close();
                
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!tcpClient.Connected)
            {
                return;
            }
            byte[] vals = protocol.GetLoginMsg(txtIMEI.Text);
            tcpClient.Client.Send(vals);

            byte[] recvs = new byte[1024];
            tcpClient.Client.Receive(recvs);
            tbLog.AppendText(System.Text.Encoding.ASCII.GetString(recvs) + "\r\n");
            
        }

        private void btnSendSig_Click(object sender, EventArgs e)
        {
            if (!tcpClient.Connected)
            {
                return;
            }
            VirtualDTU.Protocol.PowerMode powerMode = Protocol.PowerMode.ACPower;
            if (rdBtnACMode.Checked)
            {
                powerMode = Protocol.PowerMode.ACPower;
            }
            else
            {
                powerMode = Protocol.PowerMode.DCPower;
            }
            byte[] vals = protocol.GetDataMsg(txtIMEI.Text,
                Convert.ToDouble(numUDTemp.Value),
                Convert.ToDouble(numUDHum.Value),
                Convert.ToDouble(numUDTempCH1.Value),
                Convert.ToDouble(numUDHumCH1.Value),
                Convert.ToDouble(numUDVolt.Value),
                powerMode);

            tcpClient.Client.Send(vals);

            byte[] recvs = new byte[1024];
            tcpClient.Client.Receive(recvs);
            tbLog.AppendText(System.Text.Encoding.ASCII.GetString(recvs) + "\r\n");
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            timerSendData.Tag = "Random";
            timerSendData.Enabled = false;
            timerSendData.Interval = Convert.ToInt32(numUDInterval.Value) * 1000;
            timerSendData.Enabled = true;
        }

        private void btnSinCos_Click(object sender, EventArgs e)
        {
            timerSendData.Tag = "SinCos";
            timerSendData.Enabled = false;
            timerSendData.Interval = Convert.ToInt32(numUDInterval.Value) * 1000;
            timerSendData.Enabled = true;
        }

        private void btnStopTimer_Click(object sender, EventArgs e)
        {
            timerSendData.Stop();
        }

        private void btnSendGPS_Click(object sender, EventArgs e)
        {
            if (!tcpClient.Connected)
            {
                return;
            }
            byte[] vals = protocol.GetGPSMsg(txtIMEI.Text, Convert.ToDouble(numUDLng.Value) * 100, Convert.ToDouble(numUDlat.Value) * 100);
            tcpClient.Client.Send(vals);

            byte[] recvs = new byte[1024];
            tcpClient.Client.Receive(recvs);
            tbLog.AppendText(System.Text.Encoding.ASCII.GetString(recvs) + "\r\n");
        }

        private void btnSendLacCi_Click(object sender, EventArgs e)
        {
            if (!tcpClient.Connected)
            {
                return;
            }
            byte[] vals = protocol.GetGPSMsg(txtIMEI.Text, Convert.ToInt32(nudLac.Value), Convert.ToInt32(nudCi.Value));
            tcpClient.Client.Send(vals);

            byte[] recvs = new byte[1024];
            tcpClient.Client.Receive(recvs);
            tbLog.AppendText(System.Text.Encoding.ASCII.GetString(recvs) + "\r\n");
        }
    }
}
