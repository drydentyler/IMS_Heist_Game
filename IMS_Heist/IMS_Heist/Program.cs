using System;
using System.IO;

namespace HackingWalmart
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Things I'd like to change or add:
            //change inventory, score, and username file i/os to sql databases
            //add timer / countdown
            //Password hasher and salt

            Menu menu = new Menu();

            menu.securitySystem.FillDictionaryWithPreviousUsers();
            menu.securitySystem.CreateUsernameAndPassword();
            menu.DisplayGivenCreditInformation();
            Console.WriteLine("Press [ENTER] to continue.");
            Console.Clear();

            Console.WriteLine("Loading . . .");
            menu.inventory.FillInventory();
            Console.Clear();

            menu.MainMenu();




        }
    }
}
