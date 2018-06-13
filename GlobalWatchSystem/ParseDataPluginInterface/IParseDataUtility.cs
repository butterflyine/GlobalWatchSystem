using System;
using System.Collections.Generic;
using System.Text;

namespace ParseDataPluginInterface
{
    public interface IParseElement
    { 
    }
    [Serializable]
    public class DataElement : IParseElement
    {
        [NonSerialized]
        private Dictionary<byte, string> dicUnit = new Dictionary<byte, string>();

        public Dictionary<byte, string> DicUnit
        {
            get { return dicUnit; }
            private set { dicUnit = value; }
        }

        public DataElement()
        {
            dicUnit[0x01] = "MPa";
            dicUnit[0x02] = "Bar";
            dicUnit[0x03] = "Kpa";
            dicUnit[0x04] = "m";
            dicUnit[0x05] = "℃";
            dicUnit[0x06] = "%RH";
            dicUnit[0x07] = "m³/h";
            dicUnit[0x08] = "m³/m";
            dicUnit[0x09] = "V";
            dicUnit[0x0a] = "A";
            dicUnit[0x0b] = "kwh";
        }
        public int channelNo;
        public byte unit;
        public double value;
        public DateTime recvTime;

        public override string ToString()
        {
            return string.Format("通道:{0} - {1}:{2} {3}\n",channelNo,recvTime.ToString(),value,dicUnit[unit]);
        }

        public string GetUnitDesc()
        {
            if (dicUnit.ContainsKey(unit))
                return dicUnit[unit];
            else
                return "unknown";
        }
    }
    public class DeviceElement : IParseElement
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("电量:{0}\n通道总数:{1}\n", battery, totalChannelCount));
            foreach (DataElement ele in dataElements)
            {
                sb.Append(ele.ToString());
            }
            foreach (GPSElement ele in gpsElement)
            {
                sb.Append(ele.ToString());
            }

            sb.Append(warningElement.ToString());

            return sb.ToString();
        }
        private List<DataElement> dataElements = new List<DataElement>();

        public List<DataElement> DataElements
        {
            get { return dataElements; }
            set { dataElements = value; }
        }

        private List<GPSElement> gpsElement = new List<GPSElement>();

        public List<GPSElement> GpsElement
        {
            get { return gpsElement; }
            set { gpsElement = value; }
        }

        public double battery;
        public byte totalChannelCount;
        public DateTime dataTime;
        public int timeSpan;

        public WarningElement warningElement = new WarningElement();
    }

    public class WarningElement : IParseElement
    {
        public bool isPowerOK = true;

        public override string ToString()
        {
            return isPowerOK ? "电源正常\n":"掉电告警\n";
        }
    }
    public class GPSElement : IParseElement
    {
        public string dataTime;
        public double latitude = double.NaN;
        public double longtitude = double.NaN;

        public override string ToString()
        {
            return string.Format("定位信息:{0}:{1},{2}\n", dataTime, longtitude, latitude);
        }
    }
    public interface IParseDataUtility
    {
        List<List<string>> ParseData(byte[] data);
    }

    public interface IParseDataMain 
    {
        List<DeviceElement> ParseData(byte[] data);
    }
}
