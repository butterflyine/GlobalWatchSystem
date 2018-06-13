using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualDTU
{
    public class Protocol
    {

        public enum PowerMode
        {
            DCPower,
            ACPower
        }
        public byte[] GetLoginMsg(string imei)
        {
            DTUMessage msg = new DTUMessage();
            msg.SetIMEI(imei);
            msg.SetName(imei);
            msg.MsgType = (byte)MsgID.DTURegister;
            msg.MsgBody = new byte[] { 0x00, 0x50 };

            return msg.GetBytes();
        }

        public byte[] GetGPSMsg(string imei, int lac, int ci)
        {
            DTUMessage msg = new DTUMessage();
            msg.SetIMEI(imei);
            msg.SetName(imei);
            msg.MsgType = (byte)MsgID.DTUData;

            CoolBoxStationGPS boxMsg = new CoolBoxStationGPS();
            boxMsg.msgType = 0x33;
            boxMsg.lac = lac;
            boxMsg.ci = ci;

            byte[] bodyBuffer = boxMsg.GetBytes();
            msg.MsgBody = new byte[bodyBuffer.Length];
            Array.Copy(bodyBuffer, msg.MsgBody, bodyBuffer.Length);

            return msg.GetBytes();
        }

        public byte[] GetGPSMsg(string imei,double lng,double lat)
        {
            DTUMessage msg = new DTUMessage();
            msg.SetIMEI(imei);
            msg.SetName(imei);
            msg.MsgType = (byte)MsgID.DTUData;

            CoolBoxGPS boxMsg = new CoolBoxGPS();
            boxMsg.msgType = 0x33;
            boxMsg.longitude = lng;
            boxMsg.latitude = lat;

            byte[] bodyBuffer = boxMsg.GetBytes();
            msg.MsgBody = new byte[bodyBuffer.Length];
            Array.Copy(bodyBuffer, msg.MsgBody, bodyBuffer.Length);

            return msg.GetBytes();
        }
       
        public byte[] GetDataMsg(string imei,double temp, double hum, double tempCH1,double humCH1, double batteryVal, PowerMode powerMode )
        {
            DTUMessage msg = new DTUMessage();
            msg.SetIMEI(imei);
            msg.SetName(imei);
            msg.MsgType = (byte)MsgID.DTUData;

            CoolBoxMessage boxMsg = new CoolBoxMessage();

            if(powerMode == PowerMode.ACPower)
            {
                boxMsg.msgType = 0x31;
            }
            else
            {
                boxMsg.msgType = 0x32;
            }

            boxMsg.battery = Common.DataConverter.GetBCDOfInt((int)(batteryVal*10));
            boxMsg.channelCount = 2;
            boxMsg.time = DateTime.Now;
            boxMsg.span = 10;

            FillChannel(temp, boxMsg, 9, 0x01, 0x05);
            FillChannel(hum, boxMsg, 8, 0x02, 0x06);

            FillChannel(tempCH1, boxMsg, 9, 0x03, 0x05);
            FillChannel(humCH1, boxMsg, 8, 0x04, 0x06);

            byte[] bodyBuffer = boxMsg.GetBytes();
            msg.MsgBody = new byte[bodyBuffer.Length];
            Array.Copy(bodyBuffer,msg.MsgBody, bodyBuffer.Length);
            
            return msg.GetBytes();
        }

        private static void FillChannel(double temp, CoolBoxMessage boxMsg,byte len,byte chno,byte unit)
        {
            ChannelData ch1 = new ChannelData();
            boxMsg.dataList.Add(ch1);

            ch1.len = len;
            ch1.channelNo = chno;
            ch1.unit = unit;
            if (temp > 0)
            {
                ch1.sign = 0;
            }
            else
            {
                ch1.sign = 1;
            }
            if (chno % 2 == 1)
            {
                ch1.dotCount = 2;
                int val = (int)(temp * 100);
                ch1.data = new byte[] { (byte)(((int)(val / 1000)) + 0x30),
                    (byte)(((int)((val % 1000) / 100)) + 0x30),
                    (byte)(((int)((val % 100) / 10)) + 0x30),
                    (byte)(((int)(val % 10)) + 0x30)};
            }
            else
            {
                ch1.dotCount = 1;
                int val = (int)(temp * 10);
                ch1.data = new byte[] { (byte)(((int)(val / 100)) + 0x30),
                    (byte)(((int)((val % 100) / 10)) + 0x30),
                    (byte)(((int)(val % 10)) + 0x30)};
            }
           
        }
    }
}
