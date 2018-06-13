using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Models;
using SharpRepository.Repository.Queries;

namespace GlobalWatchSystem.Controllers
{
    public class CurveController : ApiController
    {
        private readonly IDtuDataRepository dataRepository;

        public CurveController()
        {
            dataRepository = new DtuDataRepository();
        }

        // GET api/<controller>
        public IEnumerable<DeviceData> Get(string id, string date)
        {
            DateTime queryStart = DateTime.Now;
            DateTime queryEnd = DateTime.Now;
            DateTime.TryParse(date, out queryStart);

            return GetDeviceData(id, queryStart, queryEnd);
        }

        private IEnumerable<DeviceData> GetDeviceData(string id, DateTime queryStart, DateTime queryEnd)
        {
            var sortingOptions = new SortingOptions<DtuData, DateTime>(x => x.RecvTime);
            sortingOptions.ThenSortBy(x => x.Channel);
            List<DtuData> datas =
                dataRepository.FindAll(d => d.IMEI == id && d.RecvTime > queryStart && d.RecvTime < queryEnd, sortingOptions).ToList();

            var devDataList = new List<DeviceData>();
            int index = 0;
            while (index < datas.Count)
            {
                if (datas[index].Channel != 1)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }
            if (index + 4 <= datas.Count)
            {
                for (int i = index; i < datas.Count; i += 4)
                {
                    var data = new DeviceData();
                    devDataList.Add(data);

                    data.recvTime = datas[i].RecvTime.ToString("yyyy-MM-dd HH:mm:ss");

                    data.tempValueCH0 = datas[i].Value.ToString("F2");
                    data.humValueCH0 = datas[i + 1].Value.ToString("F2");
                    data.tempValueCH1 = datas[i + 2].Value.ToString("F2");
                    data.humValueCH1 = datas[i + 3].Value.ToString("F2");
                }
            }
            return devDataList.ToArray();
        }

        public IEnumerable<DeviceData> Get(string id, string startDate, string endDate)
        {
            DateTime queryStart = DateTime.Now;
            DateTime queryEnd = DateTime.Now;
            DateTime.TryParse(startDate, out queryStart);
            DateTime.TryParse(endDate, out queryEnd);

            return GetDeviceData(id, queryStart, queryEnd);
        }

        public class DeviceData
        {
            public string recvTime;
            public string tempValueCH0;
            public string humValueCH0;
            public string tempValueCH1;
            public string humValueCH1;
        }
    }
}