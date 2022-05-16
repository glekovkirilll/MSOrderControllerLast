using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSOC.Models
{
    public class Order
    {
        public Int32 Id { get; set; }

        public Int32 UnitId { get; set; }
        public Unit Unit { get; set; }

        public Int32 HistoryId { get; set; }
        public History History { get; set; }

        public Int32 Number { get; set; }
        public Int32 Index { get; set; }
        public String SerialNumber { get; set; }
        public Int32 Amount { get; set; } = 1;
        public String Measure { get; set; }
        public String Status { get; set; }
    }
}
