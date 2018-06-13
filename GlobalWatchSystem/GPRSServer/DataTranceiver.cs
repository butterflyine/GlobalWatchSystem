using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using GPRSServer;
using System.Globalization;

namespace DataServer
{
    public enum MsgID
    {
        DTURegister = 0x01,
        DTUHeartTick= 0x00,
        DTUData = 0x02,

        DTUATCmd = 0x05,
        DTUP2P = 0x06
    }
    public enum DCCMsgID
    {
        DCCRegister = 0x01,
        DCC_DTUStatus = 0x02,
        DCC_DTUData = 0x03
    }
    public class DTUInfo
    {

        private bool isRegisted = false;

        /// <summary>
        /// 是否注册
        /// </summary>
        public bool IsRegisted
        {
            get { return isRegisted; }
            set { isRegisted = value; }
        }
        private string iMEIString = "";

        public string IMEIString
        {
            get { return iMEIString; }
            set { iMEIString = value; }
        }
        private string dtuPosition = "未知";

        public string DtuPosition
        {
            get { return dtuPosition; }
            set { dtuPosition = value; }
        }

        private string simNumber = "";

        public string SimNumber
        {
            get { return simNumber; }
            set { simNumber = value; }
        }
        private DateTime loginTime;

        public DateTime LoginTime
        {
            get { return loginTime; }
            set { loginTime = value; }
        }
        private DateTime logoffTime;

        public DateTime LogoffTime
        {
            get { return logoffTime; }
            set { logoffTime = value; }
        }

        private DateTime lastTickTime;

        /// <summary>
        /// 上次心跳时间
        /// </summary>
        public DateTime LastTickTime
        {
            get { return lastTickTime; }
            set { lastTickTime = value; }
        }
        private string remoteIPAddress;

        public string RemoteIPAddress
        {
            get { return remoteIPAddress; }
            set { remoteIPAddress = value; }
        }

        private int tickSpan = 120; //unit s

        /// <summary>
        /// 心跳间隔
        /// </summary>
        public int TickSpan
        {
            get { return tickSpan; }
            set { tickSpan = value; }
        }

        private bool online = false;

        public bool Online
        {
            get { return online; }
            set { online = value; }
        }

        private List<string> listP2PDTU = new List<string>();

        /// <summary>
        /// 本DTU作为接收端，此为发送端DTU IMEI列表
        /// </summary>
        public List<string> ListP2PDTU
        {
            get { return listP2PDTU; }
            set { listP2PDTU = value; }
        }
    }

    public class GPSInfo
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string recvTime { get; set; }
        public string imei { get; set; }
    }
    public class GPRSMsg
    {
        public GPRSMsg()
        {
            dtuID[0] = 0x7e;
            dtuID[1] = 0x7e;
            dtuID[2] = 0x7e;
            dtuID[3] = 0x7e;

            InitIMEI();
            InitName();

            msgType = (byte)MsgID.DTUData;
            reserved = 0x0;
            msgLen = 0x0;
            msgBody = null;
        }
        public void InitIMEI( )
        {
            for (int i = 0; i < dtuIMEI.Length; i++)
            {
                dtuIMEI[i] = (byte)('\0');
            }
        }
        public void InitName()
        {
            for (int i = 0; i < dtuName.Length; i++)
            {
                dtuName[i] = (byte)('\0');
            }
        }
        private byte[] dtuID = new byte[4];

        public byte[] DtuID
        {
            get { return dtuID; }
            set { dtuID = value; }
        }

        private byte[] dtuIMEI = new byte[16];

        public byte[] DtuIMEI
        {
            get { return dtuIMEI; }
            set { dtuIMEI = value; }
        }
        private byte[] dtuName = new byte[16];

        public byte[] DtuName
        {
            get { return dtuName; }
            set { dtuName = value; }
        }

        private byte msgType;

        public byte MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }

        private byte reserved;

        public byte Reserved
        {
            get { return reserved; }
            set { reserved = value; }
        }
        private UInt16 msgLen;

        public UInt16 MsgLen
        {
            get { return msgLen; }
            set { msgLen = value; }
        }
        private byte[] msgBody = null;

        public byte[] MsgBody
        {
            get { return msgBody; }
            set { msgBody = value; }
        }

    }

    public delegate void DtuOnlineChanged(object sender, RegisterInfoArgs e);
    public class RegisterInfoArgs : EventArgs
    {
        public RegisterInfoArgs(DTUInfo info)
        {
            dtuInfo = info;
        }
        private DTUInfo dtuInfo;

        public DTUInfo DtuInfo
        {
            get { return dtuInfo; }
            set { dtuInfo = value; }
        }
    }
    public class DCCInfoArgs : EventArgs
    {
        private string _DccAddress;

        public string DccAddress
        {
            get { return _DccAddress; }
            set { _DccAddress = value; }
        }
        private bool _Online = false;

        public bool Online
        {
            get { return _Online; }
            set { _Online = value; }
        }

        public DCCInfoArgs(string dccAddr, bool online)
        {
            _DccAddress = dccAddr;
            _Online = online;
        }
    }
    public delegate void NewRegisteChanged(object sender, RegisterInfoArgs e);
    public delegate void NewDCCConnected(object sender, DCCInfoArgs e);

    public class DataComeArgs : EventArgs
    {
        public DataComeArgs(GPRSMsg msg)
        {
            gprsMsg = msg;
        }
        private GPRSMsg gprsMsg = null;

        public GPRSMsg GprsMsg
        {
            get { return gprsMsg; }
            set { gprsMsg = value; }
        }
    }

    public class ConnInfo
    {
        public TcpClient client;
        public List<string> deviceID = new List<string>();
        public int nNonAliveTick = 0; // unit :ms

        public ConnInfo()
        {
            client = null;
        }
        public ConnInfo(TcpClient cli)
        {
            client = cli;
        }

    }
    public class DCCConnInfo : ConnInfo
    {
        public DCCConnInfo(TcpClient cli)
            : base(cli)
        { }
    }

    public delegate void NewDataComeHandler(object sender, DataComeArgs e);

    public delegate void NewGPSComeHandler(object sender, GPSInfo e);

    public class DataTranceiver
    {

        private List<byte[]> listBuffer = new List<byte[]>();
        private Object lockObj = new object();

        #region Custom Event Handle

        public event NewDataComeHandler DataReceivedHandle = null;

        public event NewGPSComeHandler GPSComeHandler = null;

        public event NewRegisteChanged RegisteChangedHandle = null;

        public event DtuOnlineChanged DtuOnlineChangeHandle = null;

        public event NewDCCConnected DccOnlineChangeHandle = null;

        #endregion

        private Dictionary<string, List<string>> dicDtuP2PConfig = new Dictionary<string, List<string>>();

        /// <summary>
        /// DTU tcp描述符字典，key -- IMEI号， value - TcpClient
        /// </summary>
        private Dictionary<string, TcpClient> dicDtuConn = new Dictionary<string, TcpClient>();

        public Dictionary<string, TcpClient> DicDtuConn
        {
            get { return dicDtuConn; }
            set { dicDtuConn = value; }
        }

        private List<DCCConnInfo> userClientList = new List<DCCConnInfo>();
        
       
        private bool stopTag = false;


        private DataTranceiver()
        {
           
        }

        public DCCConnInfo[] GetDccClient(string imei)
        {
            List<DCCConnInfo> listConnInfo = new List<DCCConnInfo>();
            for (int i = 0; i < userClientList.Count; i++)
            {
                if (userClientList[i].deviceID.Contains(imei))
                {
                    listConnInfo.Add(userClientList[i]);
                }
            }
            return listConnInfo.ToArray();
        }

        private static DataTranceiver _instance = null;

        public static DataTranceiver Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataTranceiver();
                }
                return _instance;
            }
        }

        public void StartClientServer()
        {
            AppSetting appSetting = AppSetting.LoadConfigInfo<AppSetting>(typeof(AppSetting));

            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPAddr = null;
            foreach (IPAddress ipaddr in IpEntry.AddressList)
            {
                if (ipaddr.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAddr = ipaddr;
                    break;
                }
            }
            if (IPAddr == null)
            {
                throw new Exception("没有找到合适的IPv4地址");

            }
            stopTag = false;

            // listener for dtu connection
            TcpListener listener = new TcpListener(IPAddr, appSetting.DcConnPort);

            listener.Start();

            while (true)
            {
                if (!listener.Pending())
                {
                    if (stopTag)
                        break;
                    Thread.Sleep(100);
                    continue;
                }
                TcpClient client = listener.AcceptTcpClient();

                Thread trd = new Thread(new ParameterizedThreadStart(RunTcpClientDCC));
                trd.Start(client);
                //clientList.Add(client);
            }
            listener.Stop();
            userClientList.Clear();
        }

        /// <summary>
        /// 处理来自DTU的链接监听线程
        /// </summary>
        public void StartDTUServer()
        {
            AppSetting appSetting = AppSetting.LoadConfigInfo<AppSetting>(typeof(AppSetting));

            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPAddr = null;
            foreach (IPAddress ipaddr in IpEntry.AddressList)
            {
                if (ipaddr.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAddr = ipaddr;
                    break;
                }
            }
            if (IPAddr == null)
            {
                throw new Exception("没有找到合适的IPv4地址");
                
            }
            stopTag = false;
            
            // listener for dtu connection
            TcpListener listener = new TcpListener(IPAddr, appSetting.DtuConnPort);

            listener.Start();
            
            while (true)
            {
                if (!listener.Pending())
                {
                    if (stopTag)
                        break;
                    Thread.Sleep(100);
                    continue;
                }
                TcpClient client =  listener.AcceptTcpClient();
               
                Thread trd = new Thread(new ParameterizedThreadStart(RunTcpClient));
                trd.Start(client);
                //clientList.Add(client);
            }
            listener.Stop();
            dicDtuConn.Clear();
        }

        public void StartGPSServer()
        {
            AppSetting appSetting = AppSetting.LoadConfigInfo<AppSetting>(typeof(AppSetting));

            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPAddr = null;
            foreach (IPAddress ipaddr in IpEntry.AddressList)
            {
                if (ipaddr.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAddr = ipaddr;
                    break;
                }
            }
            if (IPAddr == null)
            {
                throw new Exception("没有找到合适的IPv4地址");

            }
            stopTag = false;

            // listener for dtu connection
            TcpListener listener = new TcpListener(IPAddr, appSetting.DtuGpsPort);

            listener.Start();

            while (true)
            {
                if (!listener.Pending())
                {
                    if (stopTag)
                        break;
                    Thread.Sleep(100);
                    continue;
                }
                TcpClient client = listener.AcceptTcpClient();

                Thread trd = new Thread(new ParameterizedThreadStart(RunGPSClient));
                trd.Start(client);
            }

            listener.Stop();
        }

        public void StopServer()
        {
            stopTag = true;
        }

        private byte[] ConstructAckData(GPRSMsg msg)
        {
            byte[] heartTick = new byte[40 + msg.MsgLen];
            int index = 0;


            for (int i = 0; i < msg.DtuID.Length; i++)
            {
                heartTick[index] = msg.DtuID[i];
                index++;
            }
            for (int i = 0; i < msg.DtuIMEI.Length; i++)
            {
                heartTick[index] = msg.DtuIMEI[i];
                index++;
            }
            for (int i = 0; i < msg.DtuName.Length; i++)
            {
                heartTick[index] = msg.DtuName[i];
                index++;
            }
            heartTick[index++] = msg.MsgType;
            heartTick[index++] = 0;
            heartTick[index++] = (byte)(msg.MsgLen >> 8);
            heartTick[index++] = (byte)msg.MsgLen;
            Array.Copy(msg.MsgBody, 0, heartTick, index, msg.MsgLen);

            return heartTick;
 
        }
        public byte[] ConstructAckMsg(GPRSMsg msg, byte reserved, byte msgType)
        {
          
            string strAck = string.Format("+TOPSAIL{0}{1}",DateTime.Now.Year - 2000,DateTime.Now.ToString("MMddHHmmss"));

            byte[] bytesAck = Encoding.ASCII.GetBytes(strAck);

            return bytesAck;

        }
        private byte GetBCDOfInt(int value)
        {
            byte btVal = 0x0;
            string strValue = value.ToString("00");


            Dictionary<char, byte> dic = new Dictionary<char, byte>();
            dic.Add('0', 0x00);
            dic.Add('1', 0x01);
            dic.Add('2', 0x02);
            dic.Add('3', 0x03);
            dic.Add('4', 0x04);
            dic.Add('5', 0x05);
            dic.Add('6', 0x06);
            dic.Add('7', 0x07);
            dic.Add('8', 0x08);
            dic.Add('9', 0x09);

            btVal += (byte)(dic[strValue[0]] << 4);
            btVal += dic[strValue[1]];

            return btVal;
        }
        private byte[] GetTimeBytes()
        {
            List<byte> lstime = new List<byte>();

            lstime.Add(GetBCDOfInt(DateTime.Now.Year - 2000));
            lstime.Add(GetBCDOfInt(DateTime.Now.Month));
            lstime.Add(GetBCDOfInt(DateTime.Now.Day));

            lstime.Add(GetBCDOfInt(DateTime.Now.Hour));
            lstime.Add(GetBCDOfInt(DateTime.Now.Minute));
            lstime.Add(GetBCDOfInt(DateTime.Now.Second));


            return lstime.ToArray();
        }
        public byte[] ConstructAckMsg()
        {
            byte[] ackMsg = new byte[14];
            string ackID = "+TOPSAIL";

            Array.Copy(Encoding.ASCII.GetBytes(ackID), ackMsg, ackID.Length);
            //ackMsg[]
            byte[] time = GetTimeBytes();
            Array.Copy(time, 0, ackMsg, ackID.Length, time.Length);
            return ackMsg;
        }
        // processing connection from  data center client
        private void RunTcpClientDCC(object obj)
        {
            TcpClient client = obj as TcpClient;
            client.NoDelay = true;
            int waitUnit = 100;
            int timeout = 60 * 9 * 1000; // 设置为心跳超时 9min
            //int nTotalCount = 0;
            string hostIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            DCCConnInfo dccInfo = new DCCConnInfo(client);
            dccInfo.nNonAliveTick = 0;

            while (true)
            {
                if (stopTag)
                {
                    break;
                }
                try
                {
                    if (client.Client.Poll(0, SelectMode.SelectRead) && (!client.Client.Poll(0, SelectMode.SelectError))) // client close connection positive
                    {
                        if (client.Available == 0)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
                
                NetworkStream ns = client.GetStream();
                if (!ns.DataAvailable)
                {

                    Thread.Sleep(waitUnit); // sleep 100ms
                    dccInfo.nNonAliveTick += waitUnit;

                    if (dccInfo.nNonAliveTick > timeout) // when timeout eclipse, close client connection
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                dccInfo.nNonAliveTick = 0;
                GPRSMsg msg = new GPRSMsg();
                try
                {

                    byte[] header = new byte[4];
                    byte[] msgLen = new byte[2];

                    ns.Read(header, 0, 4);
                    if (header[0] == msg.DtuID[0]
                        && header[1] == msg.DtuID[1]
                        && header[2] == msg.DtuID[2]
                        && header[3] == msg.DtuID[3])
                    {
                        msg.MsgType = (byte)ns.ReadByte();
                        ns.Read(msgLen, 0, 2);
                        msg.MsgLen = (ushort)((msgLen[0] << 8) + msgLen[1]);
                        msg.MsgBody = new byte[msg.MsgLen];

                        if (msg.MsgType == (byte)DCCMsgID.DCCRegister)
                        {
                            // uppdate DCC Window, by dcc online status
                            if (DccOnlineChangeHandle != null)
                            {
                                DccOnlineChangeHandle(this, new DCCInfoArgs(hostIP, true));
                            }
                            int nDevIDCount = (int)msg.MsgLen / 16;
                            for (int i = 0; i < nDevIDCount; i++)
                            {
                                ns.Read(msg.DtuIMEI, 0, 16);
                                dccInfo.deviceID.Add(System.Text.Encoding.Default.GetString(msg.DtuIMEI,0,15));
                            }
                            lock (this)
                            {
                                userClientList.Add(dccInfo);
                            }
                            // send device online state
                            UInt16 dccOnlineCount = 0;
                            List<byte> listBuf = new List<byte>();
                            listBuf.Add(0x7e);
                            listBuf.Add(0x7e);
                            listBuf.Add(0x7e);
                            listBuf.Add(0x7e);
                            listBuf.Add((byte)(DCCMsgID.DCC_DTUStatus)); // msg id
                            foreach (string devID in dccInfo.deviceID)
                            {
                                if (dicDtuConn.ContainsKey(devID))
                                {
                                    dccOnlineCount++;
                                    listBuf.AddRange(ASCIIEncoding.Default.GetBytes(devID));
                                    listBuf.Add((byte)('\0'));
                                    listBuf.Add(0x1); // 0 -- offline;1 -- online
                                }

                            }
                            listBuf.Insert(5, (byte)((dccOnlineCount * 17) >> 8));
                            listBuf.Insert(6, (byte)(dccOnlineCount * 17));

                            ns.Write(listBuf.ToArray(), 0, listBuf.Count);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
            }
            if (DccOnlineChangeHandle != null)
            {
                DccOnlineChangeHandle(this, new DCCInfoArgs(hostIP, false));
            }
            client.Client.Close();
            client.Close();
            lock (this)
            {
                if (userClientList.Contains(dccInfo))
                {
                    userClientList.Remove(dccInfo);
                }
            }

        }

        // processing connection from DTU
        private void RunTcpClient(object obj)
        {
            TcpClient client = obj as TcpClient;
            //client.Client.Blocking = true;
            client.NoDelay = true;
            int timeout = 60 * 1000 * 6; // 设置为心跳超时
            int waitUnit = 100; // 100 ms
            int ntotalWait = 0;
            const int regWaitTime = 60 * 1000 * 3;
            int regNowWaitTime = 0;
            DTUInfo dtuInfo = new DTUInfo();
            //NetworkStream ns = client.GetStream();
            GPRSMsg msg = new GPRSMsg();
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            while (true)
            {
                if (stopTag)
                {
                    break;
                }
                if (!client.Connected)
                {
                    return;
                }
                try
                {
                    if (client.Client.Poll(0, SelectMode.SelectRead) && (!client.Client.Poll(0, SelectMode.SelectError))) // client close connection positive
                    {
                        if (client.Available == 0)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
                NetworkStream ns = client.GetStream();
          
                if (!ns.DataAvailable)
                {
                    
                    Thread.Sleep(waitUnit); // sleep 100ms
                    ntotalWait += waitUnit;
                    if (!dtuInfo.IsRegisted)
                    {
                        regNowWaitTime += waitUnit;
                        if (regNowWaitTime > regWaitTime)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (ntotalWait > timeout)
                        {
                            break;
                        }
                    }
                    continue;
                }
                stopWatch.Reset();
                stopWatch.Start();
                if (client.Available == 0) // client close connection positive
                {
                    break;
                }
                //ntotalWait = 0;
                try
                {
                    byte[] header = new byte[4];
                    byte[] msgLen = new byte[2];
                   
                    ns.Read(header, 0, 4);
                    if (header[0] == msg.DtuID[0]
                        && header[1] == msg.DtuID[1]
                        && header[2] == msg.DtuID[2]
                        && header[3] == msg.DtuID[3])
                    {
                        ns.Read(msg.DtuIMEI, 0, 16);
                        ns.Read(msg.DtuName, 0, 16);
                        msg.MsgType =(byte)ns.ReadByte();
                        msg.Reserved = (byte)ns.ReadByte();
                        ns.Read(msgLen, 0, 2);
                        msg.MsgLen = (ushort)((msgLen[0] << 8) + msgLen[1]);
                        msg.MsgBody = new byte[msg.MsgLen];
                        ns.Read(msg.MsgBody, 0, msg.MsgLen);

                        #region Register Of DTU
                        if (msg.MsgType == (byte)MsgID.DTURegister)
                        {
                            dtuInfo.IsRegisted = true;
                            timeout = ((msg.MsgBody[0] << 8) + msg.MsgBody[1]) * 1000; // unit ms
                            if (timeout < 120 * 1000)
                            {
                                timeout = 120 * 1000;
                            }

                            dtuInfo.Online = true;
                            dtuInfo.LoginTime = DateTime.Now;
                            dtuInfo.TickSpan = (int)(timeout / 1000);
                            dtuInfo.RemoteIPAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                            dtuInfo.IMEIString = ASCIIEncoding.ASCII.GetString(msg.DtuIMEI, 0, 15);

                            // query database by imei get dtu info
                            dtuInfo.SimNumber = "13500000000";
                            dtuInfo.DtuPosition = "未知";

                            if (RegisteChangedHandle != null)
                            {
                                RegisteChangedHandle(this, new RegisterInfoArgs(dtuInfo));
                            }
                            if (DtuOnlineChangeHandle != null)
                            {
                                DtuOnlineChangeHandle(this, new RegisterInfoArgs(dtuInfo));
                            }
                            lock (this)
                            {
                                if (!dicDtuConn.ContainsKey(dtuInfo.IMEIString))
                                {
                                    dicDtuConn.Add(dtuInfo.IMEIString, client);
                                }
                                else
                                {
                                    dicDtuConn[dtuInfo.IMEIString] = client;
                                }
                            }
                            byte[] tempAck = ConstructAckMsg();
                            ns.Write(tempAck, 0, tempAck.Length);

                        }
                        #endregion

                        #region DTU Data
                        if (msg.MsgType == (byte)MsgID.DTUData)
                        {
                            if (!dtuInfo.IsRegisted)
                            {
                                break;
                            }
                            else
                            {
                                try
                                {
                                    byte[] tempAck = ConstructAckMsg();
                                    ns.Write(tempAck, 0, tempAck.Length);
                                }
                                catch (Exception ex)
                                { 
                                }

                                if (DataReceivedHandle != null)
                                {
                                    DataReceivedHandle(this, new DataComeArgs(msg));

                                    lock (this)
                                    {
                                        // 解析数据，存入数据库 
                                    }
                                }
                            }
                        }
                        #endregion

                        #region DTU Cmd
                        if (msg.MsgType == (byte)MsgID.DTUATCmd)
                        {
                            if (!dtuInfo.IsRegisted)
                            {
                                break;
                            }
                            else
                            {
                            }
                        }
                        #endregion

                        #region DTU HeartTick
                        if (msg.MsgType == (byte)MsgID.DTUHeartTick)
                        {
                            if (!dtuInfo.IsRegisted)
                            {
                                break;
                            }
                            
                        }
                        #endregion

                        #region DTUP2P
                        if (msg.MsgType == (byte)MsgID.DTUP2P)
                        {
                            if (!dtuInfo.IsRegisted)
                            {
                                break;
                            }
                            else
                            {
                                List<string> listDtu = new List<string>();
                                byte reserved = 0;
                                if (msg.MsgLen % 16 != 0)
                                {
                                    reserved = 0xff;
                                }
                                else
                                {
                                    byte[] tempBuf = new byte[16];
                                    for (int i = 0; i < msg.MsgLen; i += 16)
                                    {
                                        Array.Copy(msg.MsgBody, i, tempBuf, 0, 16);
                                        listDtu.Add(ASCIIEncoding.ASCII.GetString(tempBuf, 0, 15));
                                    }
                                    lock (this)
                                    {
                                        if (!dicDtuP2PConfig.ContainsKey(dtuInfo.IMEIString))
                                        {
                                            dicDtuP2PConfig.Add(dtuInfo.IMEIString, listDtu);
                                        }
                                        else
                                        {
                                            dicDtuP2PConfig[dtuInfo.IMEIString] = listDtu;
                                        }
                                    }
                                    dtuInfo.ListP2PDTU.Clear();
                                    dtuInfo.ListP2PDTU.AddRange(listDtu);

                                    // 此处用于更新P2P通信是对端IMEI列表
                                    if (RegisteChangedHandle != null)
                                    {
                                        RegisteChangedHandle(this, new RegisterInfoArgs(dtuInfo));
                                    }
                                }
                                byte[] tempAck = ConstructAckMsg(msg, reserved, (byte)MsgID.DTUP2P);
                                ns.Write(tempAck, 0, tempAck.Length);
                            }
                            
                        }
                        #endregion
                    }
                         
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
                stopWatch.Stop();
                TimeSpan spanRun = stopWatch.Elapsed;
                ntotalWait += (int)spanRun.TotalSeconds;
            }
            stopWatch.Reset();
            dtuInfo.Online = false;
            dtuInfo.LogoffTime = DateTime.Now;

            if (RegisteChangedHandle != null)
            {
                RegisteChangedHandle(this, new RegisterInfoArgs(dtuInfo));
            }
            if (DtuOnlineChangeHandle != null)
            {
                DtuOnlineChangeHandle(this, new RegisterInfoArgs(dtuInfo));
            }

            client.Client.Close();
            client.Close();
            lock (this)
            {
                if (dicDtuConn.ContainsKey(dtuInfo.IMEIString))
                {
                    dicDtuConn.Remove(dtuInfo.IMEIString);
                }
            }
        }

        // processing connection from DTU
        private void RunGPSClient(object obj)
        {
            StringBuilder recvBuffer = new StringBuilder();
            TcpClient client = obj as TcpClient;
            byte[] recvs = new byte[client.ReceiveBufferSize];
            client.NoDelay = true;
            int waitUnit = 100; // 100 ms
            int ntotalWait = 0;
            DTUInfo dtuInfo = new DTUInfo();
            GPRSMsg msg = new GPRSMsg();
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            while (true)
            {
                if (stopTag)
                {
                    break;
                }
                if (!client.Connected)
                {
                    return;
                }
                try
                {
                    if (client.Client.Poll(0, SelectMode.SelectRead) && (!client.Client.Poll(0, SelectMode.SelectError))) // client close connection positive
                    {
                        if (client.Available == 0)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
                NetworkStream ns = client.GetStream();

                if (!ns.DataAvailable)
                {

                    Thread.Sleep(waitUnit); // sleep 100ms
                    continue;
                }
                stopWatch.Reset();
                stopWatch.Start();
                if (client.Available == 0) // client close connection positive
                {
                    break;
                }
                try
                {
                    int count = ns.Read(recvs, 0, client.ReceiveBufferSize);

                    if (count > 0)
                    {
                        string strMsg = Encoding.Default.GetString(recvs, 0, count);
                        recvBuffer.Append(strMsg);

                        while (recvBuffer.Length > "+RESP".Length && recvBuffer.ToString().IndexOf("+RESP") == -1)
                        {
                            recvBuffer.Remove(0, 1);
                        }

                        //find the start pos,end process it
                        string[] dataLines = recvBuffer.ToString().Split(new char[] { '$' }, StringSplitOptions.None);

                        foreach (var ele in dataLines)
                        {
                            string[] dataSegs = ele.Split(new char[] { ',' }, StringSplitOptions.None);
                            GPSInfo gps = new GPSInfo();
                            // GL300W,GV300W
                            if (dataSegs.Length  > 12)
                            {
                                gps.imei = dataSegs[2];
                                gps.longitude = dataSegs[11];
                                gps.latitude = dataSegs[12];

                                DateTime dt = DateTime.Now;
                                if (DateTime.TryParseExact(dataSegs[13], "yyyyMMddHHmmss", null, DateTimeStyles.None, out dt))
                                {
                                    dt = dt.AddHours(8);
                                    gps.recvTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    gps.recvTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                if (GPSComeHandler != null)
                                {
                                    GPSComeHandler(this, gps);
                                }
                            }

                            
                        }
                        int lastIndex = recvBuffer.ToString().LastIndexOf('$');
                        if (lastIndex != -1)
                        {
                            recvBuffer.Remove(0, lastIndex + 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
                stopWatch.Stop();
                TimeSpan spanRun = stopWatch.Elapsed;
                ntotalWait += (int)spanRun.TotalSeconds;
            }
            stopWatch.Reset();

            if (RegisteChangedHandle != null)
            {
                RegisteChangedHandle(this, new RegisterInfoArgs(dtuInfo));
            }
            if (DtuOnlineChangeHandle != null)
            {
                DtuOnlineChangeHandle(this, new RegisterInfoArgs(dtuInfo));
            }

            client.Client.Close();
            client.Close();
        }
    }
}
