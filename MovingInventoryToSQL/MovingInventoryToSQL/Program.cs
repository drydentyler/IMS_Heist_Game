using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace MovingInventoryToSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=.\SQLEXPRESS;Database=IMSHeist;Trusted_Connection=True;";

            string directory = Environment.CurrentDirectory;
            string fileName = "InventorySource.txt";
            string fullPath = Path.Combine(directory, fileName);

            List<string[]> listInventory = new List<string[]>();

            using(StreamReader sr = new StreamReader(fullPath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] lineArray = line.Split("|");
                    listInventory.Add(lineArray);
                }
            }
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (string[] item in listInventory)
                {
                    if (item[0] == "UniqID")
                    {
                        continue;
                    }
                    else
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand(@"insert into items(unique_id,name,price,stock) values(@unique_id,@name,@price,@stock)", connection);
                            cmd.Parameters.AddWithValue("@unique_id", item[0]);
                            cmd.Parameters.AddWithValue("@name", item[1]);
                            cmd.Parameters.AddWithValue("@price", item[2]);
                            cmd.Parameters.AddWithValue("@stock", item[3]);

                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        
                    }
                }
            }

         }
    }
}
