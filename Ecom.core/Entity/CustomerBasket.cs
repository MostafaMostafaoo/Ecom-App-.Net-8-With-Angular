﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Entity
{
    public class CustomerBasket
    {

        public CustomerBasket()
        {
            
        }
        public CustomerBasket(string id)
        {
            Id = id;
            
        }
        public string Id { get; set; }

        public List<BasketItem> basketItems { get; set; } = new List<BasketItem>();
    }
}
