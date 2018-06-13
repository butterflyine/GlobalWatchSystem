using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Models
{
    public class DeviceUnit
    {
        [Index]
        public int Id { get; set; }

        /// <summary>
        /// unit code of device
        /// </summary>
        public int UnitCode { get; set; }

        [StringLength(32)]
        public string UnitName { get; set; }

        /// <summary>
        /// description of chinese
        /// </summary>
        [StringLength(32)]
        public string UnitDesc_CN { get; set; }

        /// <summary>
        /// description of english
        /// </summary>
        [StringLength(32)]
        public string UnitDesc_EN { get; set; }
    }
}