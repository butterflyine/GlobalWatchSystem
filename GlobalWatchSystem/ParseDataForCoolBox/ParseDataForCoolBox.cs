using System;
using System.Collections.Generic;
using System.Text;
using ParseDataPluginInterface;
using UtilityLibrary.DBOperator;
using UtilityLibrary;

namespace ParseDataForCoolBox
{
    public class ParseDataForCoolBox : IParseDataMain
    {
        private SQLHelper accessHelper = null;
        
        public ParseDataForCoolBox(SQLHelper help)
        {
            this.accessHelper = help;
        }

        private bool GetGPSByLacCi(string lac, string ci, out double lon, out double lat)
        {
            lon = 0;
            lat = 0;
            var dt = accessHelper.GetDataTableFromDB(string.Format("select lon,lat from cellinfo where lac = '{0}' and ci = '{1}'", Convert.ToInt32(lac,16), Convert.ToInt32(ci,16)));
            if(dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            lon = Convert.ToDouble(dt.Rows[0][0]);
            lat = Convert.ToDouble(dt.Rows[0][1]);

            return true;
        }

        private void ParseGPSData(ref byte[] data, int nStartPos, int nBodyLen, DeviceElement devData)
        {
            //if (nBodyLen - nStartPos < 36)
            //{
            //    return;
            //}
            string gpsData = Encoding.ASCII.GetString(data, nStartPos + 1, nBodyLen - nStartPos - 1);

            if (gpsData.StartsWith("+CIPGSMLOC:"))
            {
                string[] set = gpsData.Substring("+CIPGSMLOC:".Length).Split(new char[]{','},   StringSplitOptions.RemoveEmptyEntries);

                GPSElement ele = new GPSElement();
                ele.longtitude = Convert.ToDouble(set[1]);
                ele.latitude = Convert.ToDouble(set[2]);
                ele.dataTime = string.Format("{0} {1}",set[3],set[4]);

                devData.GpsElement.Add(ele);
            }
            else if (gpsData.StartsWith("+CENG:"))
            {
                string[] set = gpsData.Substring("+CENG:".Length).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                GPSElement ele = new GPSElement();
                double lon = 0, lat = 0;
                if (GetGPSByLacCi(set[3],set[4],out lon, out lat))
                {
                    ele.longtitude = lon;
                    ele.latitude = lat;
                    ele.dataTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    devData.GpsElement.Add(ele);
                }
                
            }
            else
            {
                string[] set = gpsData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                GPSElement ele = new GPSElement();

                ele.longtitude = GpsCorrect.ConvertGPSDFMToData( Convert.ToDouble(set[2]));
                ele.latitude =  GpsCorrect.ConvertGPSDFMToData(Convert.ToDouble(set[0]));

                string gtcTime = "20" + set[4].Substring(4) + "-" + set[4].Substring(2,2) + "-"
                    + set[4].Substring(0,2) + " " + set[5].Substring(0,2) + ":"
                    + set[5].Substring(2,2) + ":" +set[5].Substring(4,2);

                DateTime recvTime = DateTime.Now;
                if(DateTime.TryParse(gtcTime, out recvTime))
                {
                    recvTime = recvTime.AddHours(8);
                }
                ele.dataTime = recvTime.ToString("yyyy-MM-dd HH:mm:ss");
                devData.GpsElement.Add(ele);
            }
           
        }

        private void ParseDeviceData(ref byte[] data, int nStartPos, int nBodyLen, DeviceElement devData)
        {
            int nIndex = data[nStartPos];
            DateTime tm = devData.dataTime;
            int i = 0;
            while (nIndex + nStartPos <= nBodyLen)
            {
                DataElement ele = GetDevDataElement(ref data, nStartPos);
                
                devData.DataElements.Add(ele);
                
                ele.recvTime = tm;


                if (ele.channelNo == devData.totalChannelCount)
                {
                    i++;
                    tm = devData.dataTime.AddSeconds(devData.timeSpan * i);
                }

                if (nIndex + nStartPos  >= nBodyLen)
                {
                    break;
                }
                else
                {
                    nStartPos += nIndex;
                    nIndex = data[nStartPos];
                }
                
            }
        }
        
        private DataElement GetDevDataElement(ref byte[] data,int nStartPos)
        {
            DataElement dataEle = new DataElement();

            int nLen = data[nStartPos];
            dataEle.channelNo = data[nStartPos + 1];
            dataEle.unit = data[nStartPos + 2];
            int factor = data[nStartPos + 3] == 0x00 ? 1 : -1;
            int dotPos = data[nStartPos + 4];

            string val = Encoding.ASCII.GetString(data, nStartPos + 5, nLen - 5);
            dataEle.value = Convert.ToInt32(val) * factor / Math.Pow(10,dotPos);
            return dataEle;

        }
        private void ParseDeviceHeader(ref byte[] data, int nStartPose, int nBodyLen, DeviceElement devData)
        {
            devData.battery = (((data[nStartPose + 1] & 0xf0) >> 4) + (data[nStartPose + 1] & 0x0f) *0.1);
            devData.totalChannelCount = data[nStartPose + 2];
            devData.dataTime = BCDTime2Time(data, nStartPose + 3);
            devData.timeSpan = (data[nStartPose + 9] << 8) + data[nStartPose + 10];

        }

        private DateTime BCDTime2Time(byte[] data, int index)
        {
            int year = data[index] >> 4;
            year *= 10;
            year += (data[index] & 0x0f) + 2000;

            int month = data[index + 1] >> 4;
            month *= 10;
            month += data[index + 1] & 0x0f;

            int day = data[index + 2] >> 4;
            day *= 10;
            day += data[index + 2] & 0x0f;

            int hour = data[index + 3] >> 4;
            hour *= 10;
            hour += data[index + 3] & 0x0f;

            int minute = data[index + 4] >> 4;
            minute *= 10;
            minute += data[index + 4] & 0x0f;

            int sec = data[index + 5] >> 4;
            sec *= 10;
            sec += data[index + 5] & 0x0f;
            DateTime tm = new DateTime(year,
                month, day, hour, minute, sec);

            return tm;
        }
        #region IParseDataMain 成员

        public List<DeviceElement> ParseData(byte[] data)
        {
            int nHeadLength = 11;

            int i = 0;
            List<DeviceElement> valueSet = new List<DeviceElement>();
            try
            {
                DeviceElement devData = new DeviceElement();
                valueSet.Add(devData);
                int nBodyLen = 0;
                while (i < data.Length)
                {
                    if (i + nHeadLength > data.Length)
                    {
                        break;
                    }
                    nBodyLen = data.Length;

                    if (i + nHeadLength  < data.Length)
                    {
                        byte ucMsgType = data[i];
                        
                        if (ucMsgType == 0x31)
                        {
                            ParseDeviceHeader(ref data, i, nBodyLen, devData);
                            ParseDeviceData(ref data, i + nHeadLength, nBodyLen, devData);
                        }
                        else if (ucMsgType == 0x32)
                        {
                            ParseDeviceHeader(ref data, i, nBodyLen, devData);
                            ParseDeviceData(ref data, i + nHeadLength, nBodyLen, devData);
                            devData.warningElement.isPowerOK = false;
                            
                        }
                        else
                        {
                            //ParseDeviceHeader(ref data, i, nBodyLen, devData);

                            ParseGPSData(ref data, i, nBodyLen, devData);
                        }
                    }
                    else                 
                    {
                        break;
                    }
                    i += nBodyLen + nHeadLength;
                }
            }                     
            catch (Exception ex)
            {
            }
            return valueSet;
        }

        #endregion
    }
}
