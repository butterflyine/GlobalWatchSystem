using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.ComponentModel;

namespace GlobalWatchSystem.Models
{
    public class Device
    {
        [Index]
        public int Id { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DeviceName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DeviceIMEI")]
        public string IMEI { get; set; }

        public float Battery { get; set; }
        public PowerMode PowerMode { get; set; }
        public String Data { get; set; }
        public DateTime? DataDttm { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DeviceNumber")]
        public string SimNumber { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "AreaBelonged")]
        public int AreaId { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "MenuPlanManagement")]
        [ForeignKey("TransportPlan")]
        public Int32? TransportPlanId { get; set; }

        public virtual TransportPlan TransportPlan { get; set; }

        [ForeignKey("DeviceMeta")]
        public Int32? DeviceMetaId { get; set; }

        public virtual DeviceMeta DeviceMeta { get; set; }

        public List<DataEntry> GetData()
        {
            if (String.IsNullOrEmpty(Data)) return new List<DataEntry>();
            return JsonConvert.DeserializeObject<List<DataEntry>>(Data);
        }
    }

    public enum PowerMode
    {
        Battary = 0,
        Power = 1
    }

    public class DataEntry
    {
        public int Channel { get; set; }
        public int Type { get; set; }
        public float Value { get; set; }
    }
}