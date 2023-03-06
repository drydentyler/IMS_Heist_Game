using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HackingWalmart
{
    static class LogScoreBoard
    {
        public static void WriteScoreLog(string username, decimal cartTotal)
        {
            string filePath = Environment.CurrentDirectory;
            string outputFile = "Score.txt";
            string fullPath = Path.Combine(filePath, outputFile);
            using (StreamWriter sw = new StreamWriter(fullPath, true))
            {
                sw.WriteLine($"{DateTime.UtcNow} | {username} | {cartTotal:C2}");
            }
        }
        public static void AddNewUsernameAndPassword(string username, string password)
        {
            string outputFilePath = Path.Combine(Environment.CurrentDirectory, "previous_users.txt");
            using (StreamWriter sw = new StreamWriter(outputFilePath, true))
            {
                sw.WriteLine($"{username} | {password}");
            }
        }
        public static void GameOver(string username)
        {
            string filePath = Environment.CurrentDirectory;
            string outputFile = "Score.txt";
            string fullPath = Path.Combine(filePath, outputFile);
            using (StreamWriter sw = new StreamWriter(fullPath, true))
            {
                sw.WriteLine($"{DateTime.UtcNow} | {username} | GAME OVER");
            }
        }
    }
}
