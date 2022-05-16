using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSOC.Models.ViewModels
{
    public class OrdersCreateModel
    {
        public Int32 UnitId { get; set; }
        public Int32 Number { get; set; }
        public Int32 Index { get; set; }
        public String SerialNumber { get; set; }
        public Int32 Amount { get; set; } = 1;
        public String Status { get; set; }
        public String Measure { get; set; }
    }
}
