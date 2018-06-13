using GlobalWatchSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Models
{
    public class DeviceMapMode
    {
        public DeviceModel Device { get; set; }
        public DtuGPS DtuGPS { get; set; }
    }
}