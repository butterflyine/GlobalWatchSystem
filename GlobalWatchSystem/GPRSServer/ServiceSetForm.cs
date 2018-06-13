using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GPRSServer
{
    public partial class ServiceSetForm : Form
    {
        private AppSetting appSetting = null;
        public ServiceSetForm()
        {
            InitializeComponent();

            appSetting = AppSetting.LoadConfigInfo<AppSetting>(typeof(AppSetting));
            if (appSetting == null)
            {
                appSetting = new AppSetting();
            }

            // initialize UI
            if (appSetting.DtuConnMode == "TCP")
            {
                rbtnTCP.Checked = true;
                rbtnUDP.Checked = false;
            }
            else
            {
                rbtnTCP.Checked = false;
                rbtnUDP.Checked = true;
            }
            tbDtuPort.Text = appSetting.DtuConnPort.ToString();
            cbDcAllow.Checked = appSetting.DcConnAllow;

            dcConnMax.Value = appSetting.DcConnMax;
            tbDcConnPort.Text = appSetting.DcConnPort.ToString();
            tbGPSPort.Text = appSetting.DtuGpsPort.ToString();
            cbNeedAthority.Checked = appSetting.DcConnNeedAuthority;
            tbUserName.Text = appSetting.DcConnUserName;
            tbPwd.Text = appSetting.DcConnPassword;
            checkBox3.Checked = appSetting.IsRunOnOSStart;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(tbDtuPort.Text, out value);

            RunWhenOSStart(checkBox3.Checked);

            appSetting.IsRunOnOSStart = checkBox3.Checked;
            appSetting.DtuConnPort = value;

            int.TryParse(tbGPSPort.Text, out value);
            appSetting.DtuGpsPort = value;

            appSetting.DtuConnMode = rbtnTCP.Checked ? "TCP" : "UDP";
            appSetting.DcConnAllow = cbDcAllow.Checked;


            appSetting.DcConnMax = decimal.ToInt32(dcConnMax.Value);

            int.TryParse(tbDcConnPort.Text, out  value);
            appSetting.DcConnPort = value;

            appSetting.DcConnUserName = tbUserName.Text;
            appSetting.DcConnPassword = tbPwd.Text;
            AppSetting.SaveConfigInfo(appSetting);

        }
        private void RunWhenOSStart(bool autoRun)
        {
            string str = Application.ExecutablePath;
            RegistryKey key1 = Registry.LocalMachine;
            RegistryKey key2 = key1.CreateSubKey("SOFTWARE");
            RegistryKey key3 = key2.CreateSubKey("Microsoft");
            RegistryKey key4 = key3.CreateSubKey("Windows");
            RegistryKey key5 = key4.CreateSubKey("CurrentVersion");
            RegistryKey key6 = key5.CreateSubKey("Run");

            if (autoRun)
            {
                key6.SetValue("GPRSServer", str);
            }
            else
            {
                key6.SetValue("GPRSServer", false);
            }

        }
    }
}
