using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSOC.Models.ViewModels
{
    public class HistoriesCreateModel
    {
        public String Name { get; set; }
        public Int32 FirstNumber { get; set; }
        public Int32 SecondNumber { get; set; }

        public Int32 CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
