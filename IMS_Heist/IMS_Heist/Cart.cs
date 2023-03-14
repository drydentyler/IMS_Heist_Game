using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HackingWalmart
{
    public class Cart
    {
        public Dictionary<string, Item> cartOfItems = new Dictionary<string, Item>();

        public void AddItemToCart(Item itemBeingAddedToCart)
        {
            cartOfItems[itemBeingAddedToCart.UniqueID] = itemBeingAddedToCart;
        }
        public decimal CartTotalValue()
        {
            decimal sum = 0.0m;
            foreach (KeyValuePair<string, Item> item in cartOfItems)
            {
                sum += (item.Value.Price - item.Value.NewPrice) * item.Value.QuantityMovedToCart;
            }
            return sum;
        }
        public string EditItemsInCart()
        {
            bool validItemInCart = false;
            do
            {
                Console.WriteLine("Please enter the unique ID of the item to change below:");
                string inputUniqueID = Console.ReadLine();
                try
                {
                    Console.WriteLine($"\nUnique ID: {cartOfItems[inputUniqueID].UniqueID}");
                    Console.WriteLine($"Item Name: {cartOfItems[inputUniqueID].Name}");
                    Console.WriteLine($"Price: {cartOfItems[inputUniqueID].NewPrice:C2}");
                    Console.WriteLine($"Stock: {cartOfItems[inputUniqueID].QuantityMovedToCart}");
                    validItemInCart = true;
                    Console.WriteLine();
                    return inputUniqueID;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong, please retry typing the unique ID.");
                }
                return "";
            }
            while (!validItemInCart);
        }
        public void RemoveCartItemsOptions(string uniqueID)
        {
            bool completeRemoving = false;
            do
            {
                try
                {
                    Console.WriteLine("Please select a below option:");
                    Console.WriteLine("[1]Remove Items from Cart \n[2]Return to Checkout");
                    int input = int.Parse(Console.ReadLine());
                    if (input == 1)
                    {
                        ActuallyRemoveItems(uniqueID);
                    }
                    else if (input == 2)
                    {
                        completeRemoving = true;
                    }
                    else
                    {

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong, please try again.");
                }
            }
            while (!completeRemoving);
        }
        public void ActuallyRemoveItems(string uniqueID)
        {
            bool ableToRemoveItems = false;
            do
            {
                try
                {
                    Console.WriteLine("Please select a valid number of items to remove: ");
                    int numberToRemove = int.Parse(Console.ReadLine());
                    if (numberToRemove <= cartOfItems[uniqueID].QuantityMovedToCart)
                    {
                        cartOfItems[uniqueID].DecrimentCartAmount(numberToRemove);
                        ableToRemoveItems = true;
                    }
                    else
                    {
                        Console.WriteLine("Cart amount cannot be decreased below 0.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid amount to remove from this item in your cart.");
                }
            }
            while (!ableToRemoveItems);
        }
    }
}
