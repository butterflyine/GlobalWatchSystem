using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class DataConverter
    {
        public static byte GetBCDOfInt(int value)
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
        public static byte[] GetTimeBytes(DateTime time)
        {
            List<byte> lstime = new List<byte>();

            lstime.Add(GetBCDOfInt(time.Year - 2000));
            lstime.Add(GetBCDOfInt(time.Month));
            lstime.Add(GetBCDOfInt(time.Day));

            lstime.Add(GetBCDOfInt(time.Hour));
            lstime.Add(GetBCDOfInt(time.Minute));
            lstime.Add(GetBCDOfInt(time.Second));


            return lstime.ToArray();
        }
    }
}
