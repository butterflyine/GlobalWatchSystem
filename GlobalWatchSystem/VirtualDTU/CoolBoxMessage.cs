using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualDTU
{
    public class CoolBoxMessage :IToBytes
    {
        public byte msgType;
        public byte battery;
        public byte channelCount;
        public DateTime time;
        public ushort span;

        public List<ChannelData> dataList = new List<ChannelData>();

        public byte[] GetBytes()
        {
            List<byte> buffers = new List<byte>();
            buffers.Add(msgType);
            buffers.Add(battery);
            buffers.Add(channelCount);
            buffers.AddRange(Common.DataConverter.GetTimeBytes(time));
            buffers.Add((byte)(span >> 8));
            buffers.Add((byte)span);

            foreach (var ele in dataList)
            {
                buffers.AddRange(ele.GetBytes());
            }
            return buffers.ToArray();
        }
        
    }

    public class CoolBoxStationGPS:IToBytes
    {
        public byte msgType;

        public int lac;

        public int ci;

        public byte[] GetBytes()
        {
            List<byte> buffers = new List<byte>();

            buffers.Add(msgType);

            string strGPSInfo = string.Format("+CENG:0,460,00,{0:x},{1:x},55,63\r\n",
                lac,
                ci,
                DateTime.Now.ToString("ddMMyy"), DateTime.Now.ToString("HHmmss"));
            byte[] vals = System.Text.Encoding.ASCII.GetBytes(strGPSInfo);
            buffers.AddRange(vals);

            return buffers.ToArray();
        }
    }

    public class CoolBoxGPS:IToBytes
    {
        public byte msgType;

        public double longitude;

        public double latitude;

        public byte[] GetBytes()
        {
            List<byte> buffers = new List<byte>();

            buffers.Add(msgType);
            string strGPSInfo = string.Format("{0},N,{1},E,{2},{3}.2",
                latitude,
                longitude,
                DateTime.Now.ToString("ddMMyy"), DateTime.Now.ToString("HHmmss"));
            byte[] vals = System.Text.Encoding.ASCII.GetBytes(strGPSInfo);
            buffers.AddRange(vals);

            return buffers.ToArray();
        }
    }

    public class ChannelData : IToBytes
    {
        public byte len;
        public byte channelNo;
        public byte unit;
        public byte sign;
        public byte dotCount;
        public byte[] data;


        #region IToBytes 成员

        public byte[] GetBytes()
        {
            List<byte> buffers = new List<byte>();
            buffers.Add(len);
            buffers.Add(channelNo);
            buffers.Add(unit);
            buffers.Add(sign);
            buffers.Add(dotCount);
            buffers.AddRange(data);

            return buffers.ToArray();
        }

        #endregion
    }
}
