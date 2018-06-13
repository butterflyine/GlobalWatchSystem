using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalWatchSystem.Models
{
    public class AuditLog
    {
        [Index]
        public int Id { get; set; }

        public String Action { get; set; }
        public String Type { get; set; }
        public String Target { get; set; }
        public String Actioner { get; set; }

        public String Changes { get; set; }

        [Index]
        public DateTime Dttm { get; set; }
    }
}