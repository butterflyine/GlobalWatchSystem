using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace GPRSServer
{
    public class AppSetting
    {
        private bool isRunOnOSStart = true;

        public bool IsRunOnOSStart
        {
            get { return isRunOnOSStart; }
            set { isRunOnOSStart = value; }
        }
        private string dtuConnMode = "TCP";

        public string DtuConnMode
        {
            get { return dtuConnMode; }
            set { dtuConnMode = value; }
        }

        private int dtuGpsPort = 4200;

        /// <summary>
        /// gps port 
        /// </summary>
        public int DtuGpsPort
        {
            get { return dtuGpsPort; }
            set { dtuGpsPort = value; }
        }

        private int dtuConnPort = 5200;

        public int DtuConnPort
        {
            get { return dtuConnPort; }
            set { dtuConnPort = value; }
        }

        private bool dcConnAllow = true;

        /// <summary>
        /// Data Clients are allowed to connect with this server
        /// </summary>
        public bool DcConnAllow
        {
            get { return dcConnAllow; }
            set { dcConnAllow = value; }
        }

        private int dcConnMax = 100;
        /// <summary>
        /// data clients number of server can hold at while
        /// </summary>
        public int DcConnMax
        {
            get { return dcConnMax; }
            set { dcConnMax = value; }
        }
        private int dcConnPort = 15300;

        /// <summary>
        /// 用于监听DataClient连接的端口号
        /// </summary>
        public int DcConnPort
        {
            get { return dcConnPort; }
            set { dcConnPort = value; }
        }

        private bool dcConnNeedAuthority = false;

        /// <summary>
        /// data client connects to thi server whether need authority or not
        /// </summary>
        public bool DcConnNeedAuthority
        {
            get { return dcConnNeedAuthority; }
            set { dcConnNeedAuthority = value; }
        }

        private string dcConnUserName = "admin";

        public string DcConnUserName
        {
            get { return dcConnUserName; }
            set { dcConnUserName = value; }
        }

        private string dcConnPassword;

        public string DcConnPassword
        {
            get { return dcConnPassword; }
            set { dcConnPassword = value; }
        }
        public static T LoadConfigInfo<T>(Type type) where T : class, new()
        {
            XmlSerializer xs = new XmlSerializer(type);
            string fullPathFileName = GetFullPath("AppSetting.xml");
            T appInfo = null;


            if (File.Exists(fullPathFileName))
            {

                using (TextReader tr = new StreamReader(fullPathFileName))
                {
                    appInfo = xs.Deserialize(tr) as T;
                }
            }
            return appInfo;

        }
        public static void SaveConfigInfo<T>(T obj)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            string fullPathFileName = GetFullPath("AppSetting.xml");

            if (obj == null)
            {
                // need add errlog record here
                return;
            }
            using (TextWriter tr = new StreamWriter(fullPathFileName))
            {
                xs.Serialize(tr, obj);
            }

        }

        public static string GetFullPath(string fileName)
        {
            if (fileName.IndexOf("\\") >= 0)
            {
                return fileName;
            }
            else
            {
                return Application.StartupPath + "\\" + fileName;
            }

        }
    }
}
