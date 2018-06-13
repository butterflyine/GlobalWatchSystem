using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualDTU
{
    public enum MsgID
    {
        DTURegister = 0x01,
        DTUHeartTick = 0x00,
        DTUData = 0x02
    }
    public class DTUMessage :IToBytes
    {
        public DTUMessage()
        {
            dtuID[0] = 0x7e;
            dtuID[1] = 0x7e;
            dtuID[2] = 0x7e;
            dtuID[3] = 0x7e;

            InitIMEI();
            InitName();

            msgType = (byte)MsgID.DTUData;
            reserved = 0x0;
            msgBody = null;
        }
        private void InitIMEI( )
        {
            for (int i = 0; i < dtuIMEI.Length; i++)
            {
                dtuIMEI[i] = (byte)('\0');
            }
        }
        private void InitName()
        {
            for (int i = 0; i < dtuName.Length; i++)
            {
                dtuName[i] = (byte)('\0');
            }
        }

        public void SetIMEI(string value)
        {
            InitIMEI();
            byte[] vals = System.Text.Encoding.ASCII.GetBytes(value);
            Array.Copy(vals, dtuIMEI, vals.Length);
        }

        public void SetName(string name)
        {
            InitName();
            byte[] vals = System.Text.Encoding.ASCII.GetBytes(name);
            Array.Copy(vals, dtuIMEI, vals.Length);
        }
        private byte[] dtuID = new byte[4];

        private byte[] dtuIMEI = new byte[16];

        private byte[] dtuName = new byte[16];

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
        
        private byte[] msgBody = null;

        public byte[] MsgBody
        {
            get { return msgBody; }
            set { msgBody = value; }
        }

        #region IToBytes 成员

        public byte[] GetBytes()
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(dtuID);
            buffer.AddRange(dtuIMEI);
            buffer.AddRange(dtuName);
            buffer.Add(msgType);
            buffer.Add(reserved);
            buffer.Add((byte)(msgBody.Length >> 8));
            buffer.Add((byte)msgBody.Length);
            buffer.AddRange(msgBody);


            return buffer.ToArray();
        }

        #endregion
    }
}
