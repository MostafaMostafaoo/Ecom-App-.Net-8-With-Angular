﻿namespace Ecom.core.Entity
{
    public class BasketItem
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string Image { get; set; }
        public int Quntity { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; } 


    }
}