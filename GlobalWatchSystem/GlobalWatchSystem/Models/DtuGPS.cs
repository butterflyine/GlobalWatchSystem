using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalWatchSystem.Models
{
    [Table("DtuGps")]
    public class DtuGPS
    {
        [Column("Id")]
        public int Id { get; set; }

        [Index]
        [Column("dev_number")]
        [StringLength(20)]
        public string IMEI { get; set; }

        [Index]
        [Column("recv_time")]
        public DateTime RecvTime { get; set; }

        [Column("longitude")]
        public float Longitude { get; set; }

        [Column("latitude")]
        public float Latitude { get; set; }
    }
}