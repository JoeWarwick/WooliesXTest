using System;
using System.Collections.Generic;
using System.Text;

namespace APIProxy1.Models
{
    public class TrolleyModel
    {
        public ProductModel[] products { get; set; }
        public SpecialModel[] specials { get; set; }
        public QuantityModel[] quantities { get; set; }
    }
}
