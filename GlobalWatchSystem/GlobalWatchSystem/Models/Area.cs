using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace GlobalWatchSystem.Models
{
    public class Area
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "AreaName")]
        public String Name { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "Description")]
        public String Description { get; set; }

        [Index]
        [Display(ResourceType = typeof(Resources.Resources), Name = "AreaParent")]
        public int ParentId { get; set; }

        [InverseProperty("ParentId")]
        public virtual List<int> SubAreas { get; set; }

        [Index]
        public int Id { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0}, Name: {1}, Parent: {2}", Id, Name, ParentId);
        }
    }
}