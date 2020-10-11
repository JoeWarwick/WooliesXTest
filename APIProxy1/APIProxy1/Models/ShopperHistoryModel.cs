using System;
using System.Collections.Generic;
using System.Text;

namespace APIProxy1.Models
{
    public class ShopperHistoryModel
    {
        public string customerId { get; set; }
        
        public ProductModel[] products { get; set; }
    }
}
