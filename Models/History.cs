using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSOC.Models
{
    public class History
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }
        public Int32 FirstNumber { get; set; }
        public Int32 SecondNumber { get; set; }
        
        public Int32 CustomerId { get; set; }
        public Customer Customer { get; set; }
        public String Date { get; set; } = (DateTime.Now).ToString("dd-MM-yyyy HH:mm:ss");

        public ICollection<Order> Orders { get; set; }
    }
}
