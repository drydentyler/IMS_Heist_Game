using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace HackingWalmart
{
    public class Item
    {
        public string UniqueID { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public int QuantityMovedToCart { get; private set; } = 0;
        public decimal NewPrice { get; private set; }

        public Item(string uniqueID, string name, decimal price, int stockQuantity)
        {
            UniqueID = uniqueID;
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
            NewPrice = price;
        }
        public Item(string uniqueId, string name, decimal price, int stockQuantity, int cartQuantity, decimal newPrice)
        {
            UniqueID = uniqueId;
            Name = name;
            Price = price;
            NewPrice = newPrice;
            StockQuantity = stockQuantity;
            QuantityMovedToCart = cartQuantity;
        }

        public void changePrice(decimal newPrice)
        {
            if (newPrice > 0)
            {
                NewPrice = newPrice;
                Price = newPrice;
            }
        }
        public void ChangeStockQuantity(int newAmount)
        {
            if (newAmount > 0)
            {
                StockQuantity = newAmount;
            }
        }
        public int AddQuantityToCart(int amountMoved)
        {
            QuantityMovedToCart += amountMoved;
            return QuantityMovedToCart;
        }
        public void DecrimentCartAmount(int amountToRemove)
        {
            QuantityMovedToCart -= amountToRemove;
        }


    }
}
