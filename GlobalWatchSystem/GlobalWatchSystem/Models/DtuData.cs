using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalWatchSystem.Models
{
    [Table("DtuData")]
    public class DtuData
    {
        [Column("Id")]
        public int Id { get; set; }

        [Index]
        [Column("dev_number")]
        [StringLength(20)]
        public string IMEI { get; set; }

        [Column("dev_channel")]
        public int Channel { get; set; }

        [Index]
        [Column("recv_time")]
        public DateTime RecvTime { get; set; }

        [Column("data_type")]
        public int DataType { get; set; }

        [Column("data_value")]
        public float Value { get; set; }

        [NotMapped]
        public bool IsHumidity
        {
            get { return DataType == Models.DataType.Humidity; }
        }

        [NotMapped]
        public bool IsTemperature
        {
            get { return DataType == Models.DataType.Temperature; }
        }
    }
}