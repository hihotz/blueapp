using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blueapp.Models
{
    public class Product_ProductionModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime ProductionDate { get; set; }
        public int Quantity { get; set; }
    }
}
