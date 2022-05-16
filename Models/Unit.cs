using System;
using System.ComponentModel.DataAnnotations;

namespace MSOC.Models
{
    public class Unit
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }
        public String CatalogNumber { get; set; }
        public String Mark { get; set; }
    }
}
