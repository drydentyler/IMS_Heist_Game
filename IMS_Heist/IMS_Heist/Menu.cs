using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;

namespace HackingWalmart
{
    public class Menu
    {
        public Security securitySystem = new Security();
        public Inventory inventory = new Inventory();
        CreditCard userCreditCard = new CreditCard();
        Cart shoppingCart = new Cart();

        public string InventorySelection { get; set; }

        public void MainMenu()
        {
            string filePath = Environment.CurrentDirectory;
            string outputFile = "Logo.txt";
            string logoPath = Path.Combine(filePath, outputFile);

            bool validInput = false;
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                using (StreamReader sr = new StreamReader(logoPath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        Console.WriteLine(line);
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nWelcome to the Inventory Management System Heist.");

                Console.WriteLine("\nPlease select from the below options:\n");
                Console.WriteLine("[1] Search Inventory \n[2] View Cart \n[3] Exit Terminal \n");
                Console.Write("Selection:");
                try
                {
                    int mainMenuSelection = int.Parse(Console.ReadLine());
                    switch (mainMenuSelection)
                    {
                        case 1:
                            SearchInventory();
                            break;
                        case 2:
                            ViewItemsInCart();
                            CartMenu();
                            break;
                        case 3:
                            Console.WriteLine("Exiting terminal.");
                            validInput = true;
                            break;
                        default:
                            validInput = false;
                            break;
                    }
                }
                catch (Exception e)
                {
                    securitySystem.ReceiveInvalidInput();
                    Console.WriteLine(securitySystem.AlertTheAuthorities(securitySystem.SuspiciousActivityCounter));
                    if (securitySystem.SuspiciousActivityCounter >= 5)
                    {
                        Environment.Exit(1);
                    }

                }
            }
            while (!validInput);
        }

        //move to file reading class
        public void SearchInventory()
        {
            string filePath = Environment.CurrentDirectory;
            string outputFile = "InventorySource.txt";
            string inventoryFilePath = Path.Combine(filePath, outputFile);

            Console.Write("Please enter a valid keyword to search: ");
            string keyword = Console.ReadLine();
            Console.Clear();
            using (StreamReader sr = new StreamReader(inventoryFilePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Contains(keyword))
                    {
                        Console.WriteLine(line);
                    }
                }
                MakeInventoryPropertyChanges();
            }
        }
        public void DisplayUpdatedItemInfo(string input)
        {
            Console.WriteLine($"Unique ID: {inventory.FullInventory[input].UniqueID}");
            Console.WriteLine($"Item Name: {inventory.FullInventory[input].Name}");
            Console.WriteLine($"Price: {inventory.FullInventory[input].Price:C2}");
            Console.WriteLine($"Stock: {inventory.FullInventory[input].StockQuantity}");
        }
        public void MakeInventoryPropertyChanges()
        {
            bool validItemSelection = false;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Please enter an item ID to adjust or [1] to return to Main Menu: ");
                string input = Console.ReadLine();
                try
                {
                    DisplayUpdatedItemInfo(input);
                    validItemSelection = true;
                    InventorySelection = input;
                    Console.WriteLine();
                    ChangeItemProperty();
                }
                catch (Exception e)
                {
                    if (input == "1")
                    {
                        bool mainMenuReturn = ReturnToMainMenu();
                        validItemSelection = mainMenuReturn;
                    }
                    else
                    {
                        securitySystem.ReceiveInvalidInput();
                        Console.WriteLine(securitySystem.AlertTheAuthorities(securitySystem.SuspiciousActivityCounter));
                        if (securitySystem.SuspiciousActivityCounter >= 5)
                        {
                            Environment.Exit(1);
                        }
                    }
                }
            }
            while (!validItemSelection);
        }
        public void ChangeItemProperty()
        {
            bool validItemPropertySelction = false;
            try
            {
                do
                {
                    Console.WriteLine("\n[1]Change item price \n[2]Change item quantity \n[3]Exit");
                    Console.Write("Please select a property to change: ");
                    int itemPropertySelection = int.Parse(Console.ReadLine());
                    switch (itemPropertySelection)
                    {
                        case 1:
                            ChangeItemPrice();
                            break;
                        case 2:
                            ChangeItemQuantity();
                            break;
                        case 3:
                            validItemPropertySelction = true;
                            break;
                    }
                }
                while (!validItemPropertySelction);
            }
            catch (Exception e)
            {
                securitySystem.ReceiveInvalidInput();
                Console.WriteLine(securitySystem.AlertTheAuthorities(securitySystem.SuspiciousActivityCounter));
                if (securitySystem.SuspiciousActivityCounter >= 5)
                {
                    Environment.Exit(1);
                }
            }
        }
        public void ChangeItemPrice()
        {
            bool validNewPrice = false;
            do
            {
                try
                {
                    DisplayUpdatedItemInfo(InventorySelection);
                    Console.Write("\nPlease enter a valid new price for this item: $");
                    decimal newPrice = decimal.Parse(Console.ReadLine());
                    if (newPrice > 0)
                    {
                        Console.WriteLine($"This item's price has been updated to {newPrice:C2}\n");
                        decimal amountChanged = 1 - (newPrice / inventory.FullInventory[InventorySelection].NewPrice);
                        inventory.FullInventory[InventorySelection].changePrice(newPrice);
                        validNewPrice = true;
                        securitySystem.VariableSuspiciousActivityIncrementor(amountChanged);
                    }
                    else
                    {
                        Console.WriteLine("The price of an item cannot be set to $0.00.");
                        securitySystem.SuspiciousAcitivtyIdentified(2);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid new price.");
                    securitySystem.ReceiveInvalidInput();
                    Console.WriteLine(securitySystem.AlertTheAuthorities(securitySystem.SuspiciousActivityCounter));
                    if (securitySystem.SuspiciousActivityCounter >= 5)
                    {
                        Environment.Exit(1);
                    }
                }
            }
            while (!validNewPrice);
        }
        public void ChangeItemQuantity()
        {
            bool validQuantity = false;
            do
            {
                try
                {
                    DisplayUpdatedItemInfo(InventorySelection);
                    Console.Write("\nPlease enter a value to add or remove from this inventory: ");
                    int inventoryQuantityChange = int.Parse(Console.ReadLine());

                    string reason = ReasonForItemQuantityChange();
                    decimal amountChanged = ((decimal)inventoryQuantityChange / (decimal)inventory.FullInventory[InventorySelection].StockQuantity);
                    int remainingAmount = inventory.FullInventory[InventorySelection].StockQuantity - inventoryQuantityChange;

                    if (reason.Contains("cart"))
                    {
                        inventory.FullInventory[InventorySelection].AddQuantityToCart(inventoryQuantityChange);
                        shoppingCart.AddItemToCart(inventory.FullInventory[InventorySelection]);
                    }

                    Console.WriteLine($"This item's quantity has been changed by {inventoryQuantityChange} due to {reason}");
                    inventory.FullInventory[InventorySelection].ChangeStockQuantity(remainingAmount);
                    validQuantity = true;
                    securitySystem.VariableSuspiciousActivityIncrementor(amountChanged);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid new quantity.");
                }
            }
            while (!validQuantity);
        }
        public string ReasonForItemQuantityChange()
        {
            bool validReason = false;

            do
            {
                Console.WriteLine("What is the reason for the quantity change?");
                Console.WriteLine("\n[1]Damaged product - removed from inventory \n[2]Received new shipment - added to inventory " +
                    "\n[3]Moved to cart for shipping - removed from inventory");
                Console.WriteLine();
                Console.Write("Selection: ");
                try
                {
                    int reasonSelection = int.Parse(Console.ReadLine());
                    switch (reasonSelection)
                    {
                        case 1:
                            validReason = true;
                            return "damaged product - removed from inventory";
                        case 2:
                            validReason = true;
                            return "received new shipment - added to inventory";
                        case 3:
                            validReason = true;
                            return "items moved to cart for shippping - removed from inventory";
                        default:
                            Console.WriteLine("Inventory changes require a valid reason to be recorded.");
                            return "";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Inventory changes require a valid reason to be recorded.");
                    validReason = false;
                    return "";
                }
            }
            while (!validReason);
        }
        public void CartMenu()
        {
            bool validCartOption = false;
            do
            {
                try
                {
                    Console.WriteLine("\n[1]Proceed to Checkout and Shipping \n[2]Edit Items in Cart \n[3]Return to Main Menu");
                    Console.Write("\nSelection: ");
                    int cartInput = int.Parse(Console.ReadLine());
                    switch (cartInput)
                    {
                        case 1:
                            ProceedToCheckOut();
                            validCartOption = true;
                            break;
                        case 2:
                            ViewItemsInCart();
                            shoppingCart.EditItemsInCart();
                            shoppingCart.RemoveCartItemsOptions(InventorySelection);
                            break;
                        case 3:
                            validCartOption = true;
                            break;
                        default:
                            Console.WriteLine("Please enter a valid selection.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid selection.");
                }
            }
            while (!validCartOption);
        }
        public bool ReturnToMainMenu()
        {
            try
            {
                Console.WriteLine("Would you like to return to the Main Menu?");
                Console.WriteLine("\n[1]Yes \n[2]No");
                int mainMenuINput = int.Parse(Console.ReadLine());
                return mainMenuINput == 1 ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Please enter a [1]Yes or [2]No to return to Main Menu");
                return false;
            }

        }
        public void ProceedToCheckOut()
        {
            bool validInput = false;
            do
            {
                try
                {
                    securitySystem.UsernameAndPasswordSecurityCheck();
                    Console.Write("Please enter credit card number(xxxx-xxxx-xxxx-xxxx): ");
                    string creditCardNumber = Console.ReadLine();
                    Console.Write("Expiration Date: ");
                    string expirationDate = Console.ReadLine();
                    Console.Write("Pin: ");
                    string pin = Console.ReadLine();
                    CreditCard givenCreditCard = new CreditCard(creditCardNumber, expirationDate, pin);
                    validInput = securitySystem.AssertCreditCardInformationMatch(userCreditCard, givenCreditCard);
                }
                catch (Exception e)
                {
                    securitySystem.ReceiveInvalidInput();
                    Console.WriteLine("Please enter a valid input.");
                }
            }
            while (!validInput);

            bool validSubmitInput = false;
            do
            {
                try
                {
                    Console.Write("Press [1] to submit order: ");
                    int submit = int.Parse(Console.ReadLine());
                    validSubmitInput = submit == 1 ? true : false;
                }
                catch (Exception e)
                {
                    securitySystem.SuspiciousAcitivtyIdentified(1);
                    Console.WriteLine("Press [1] to submit: ");
                }
            }
            while (!validSubmitInput);
            LogScoreBoard.WriteScoreLog(securitySystem.CurrentUserName, shoppingCart.CartTotalValue());
        }
        public void DisplayGivenCreditInformation()
        {
            userCreditCard.CreateCreditCardNumber();
            userCreditCard.CreateExpirationDate();
            userCreditCard.CreatePin();

            Console.WriteLine("Below is your new credit information: ");
            Console.WriteLine($"Credit Card Number: {userCreditCard.CreditCardNumber}");
            Console.WriteLine($"Expiration Date: {userCreditCard.ExpirationDate}");
            Console.WriteLine($"Pin: {userCreditCard.Pin}");
            Console.ReadLine();
        }
        //move to menu class
        public void ViewItemsInCart()
        {
            Console.Clear();
            foreach (KeyValuePair<string, Item> item in shoppingCart.cartOfItems)
            {
                Console.WriteLine($"{item.Key} | {item.Value.Name} | {item.Value.NewPrice:C2} | {item.Value.QuantityMovedToCart}");
            }
        }
    }
}
