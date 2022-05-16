using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSOC.Models
{
    public class Customer
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }
        public String ShortName { get; set; }
        public Int32 Index { get; set; }
        public String Region { get; set; }
        public String City { get; set; }
        public String Adress { get; set; }
    }
}
