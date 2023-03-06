using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HackingWalmart
{
    public class Inventory
    {
        public Dictionary<string, Item> FullInventory = new Dictionary<string, Item>();

        public void FillInventory()
        {
            string filePath = Environment.CurrentDirectory;
            string outputFile = "InventorySource.txt";
            string fullPath = Path.Combine(filePath, outputFile);

            using (StreamReader sr = new StreamReader(fullPath))
            {
                while (!sr.EndOfStream)
                {
                    try
                    {
                        string line = sr.ReadLine();
                        string[] lineArray = line.Split("|");
                        string uniqueId = lineArray[0];
                        string itemName = lineArray[1];
                        decimal itemCost = decimal.Parse(lineArray[2]);
                        int itemQuantity = int.Parse(lineArray[3]);

                        Item item = new Item(uniqueId, itemName, itemCost, itemQuantity);
                        FullInventory[lineArray[0]] = item;
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
    }
}
