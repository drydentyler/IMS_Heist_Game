using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;

namespace HackingWalmart
{
    public class Security
    {
        public int SuspiciousActivityCounter { get; private set; } = 0;
        public string CurrentUserName { get; private set; }
        public string CurrentPassword { get; private set; }

        public Dictionary<string, string> PreviousUsernamesAndPasswords = new Dictionary<string, string>();

        public int SuspiciousAcitivtyIdentified(int amountIncreasedBy)
        {
            SuspiciousActivityCounter += amountIncreasedBy;
            return SuspiciousActivityCounter;
        }
        public int ReceiveInvalidInput()
        {
            SuspiciousActivityCounter++;
            return SuspiciousActivityCounter;
        }
        public string AlertTheAuthorities(int invalidInputCounter)
        {
            if (invalidInputCounter < 7 && invalidInputCounter > 0)
            {
                return $"Suspicious activity has been suspected in {invalidInputCounter} instance(s).";
            }
            else if (invalidInputCounter >= 7)
            {
                return "Local and State authorities have been notified and dispatched to your location. \nPlease stay online until authorities arrive.";
            }
            else
            {
                return "";
            }
        }
        public void FillDictionaryWithPreviousUsers()
        {
            string fullOutputPath = Path.Combine(Environment.CurrentDirectory, "previous_users.txt");
            using (StreamReader sr = new StreamReader(fullOutputPath))
            {
                while (!sr.EndOfStream)
                {
                    try
                    {
                        string line = sr.ReadLine();
                        string[] lineArray = line.Split("|");
                        int atSymbol = lineArray[0].IndexOf("@");
                        string username = lineArray[0].Substring(0, atSymbol);
                        PreviousUsernamesAndPasswords[username] = lineArray[1];
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
        public bool IsValidPassword(string password)
        {
            for (int i = 0; i < password.Length - 1; i++)
            {
                string character = password.Substring(i, 1);
                switch (character)
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        return true;
                    default:
                        continue;
                }
            }
            return false;
        }
        public bool UsernameAndPasswordSecurityCheck()
        {
            bool validUsernamePasswordCombo = false;
            string username = "";
            string password = "";
            do
            {
                Console.Write("Please re-enter your username: ");
                username = Console.ReadLine();
                Console.Write("Please re-enter your password: ");
                password = Console.ReadLine();
                if (username == CurrentUserName && password == CurrentPassword)
                {
                    validUsernamePasswordCombo = true;
                }
                else
                {
                    Console.WriteLine("Either your username or password were incorrect.");
                }
            }
            while (!validUsernamePasswordCombo);
            return true;
        }
        public void CreateUsernameAndPassword()
        {
            bool newUsername = false;
            do
            {
                Console.WriteLine("Please create a new, unique Username and Password.");
                Console.WriteLine("The Password must be atleast 8 characters in length and include at least 1 number.");
                Console.Write("\nUsername: ");
                string username = Console.ReadLine();
                Console.Write("\nPassword: ");
                string password = Console.ReadLine();
                if (password.Length < 8)
                {
                    Console.WriteLine("The password given is too short.\n");
                }
                if (!IsValidPassword(password))
                {
                    Console.WriteLine("The password given does not contain a number.\n");
                }
                if (PreviousUsernamesAndPasswords.ContainsKey(username))
                {
                    Console.WriteLine("This username is already taken.\n");
                }
                else
                {
                    Console.WriteLine($"Your new username is: {username}@IMS_Heist.game");
                    username = username + "@IMS_Heist.game";
                    LogScoreBoard.AddNewUsernameAndPassword(username, password);
                    CurrentUserName = username;
                    CurrentPassword = password;
                    newUsername = true;
                    Console.WriteLine();
                }
            }
            while (!newUsername);
        }
        public void VariableSuspiciousActivityIncrementor(decimal percentageChangeBy)
        {
            string filePath = Environment.CurrentDirectory;
            string outputFile = "GameOver.txt";
            string gameOverFilePath = Path.Combine(filePath, outputFile);

            if (percentageChangeBy >= 0.1m)
            {
                if (percentageChangeBy >= 0.75m)
                {
                    SuspiciousAcitivtyIdentified(6);
                }
                else if (percentageChangeBy >= 0.6m && percentageChangeBy < 0.75m)
                {
                    SuspiciousAcitivtyIdentified(4);
                }
                else if (percentageChangeBy >= 0.5m && percentageChangeBy < 0.6m)
                {
                    SuspiciousAcitivtyIdentified(3);
                }
                else if (percentageChangeBy >= 0.25m && percentageChangeBy < 0.5m)
                {
                    SuspiciousAcitivtyIdentified(2);
                }
                else if (percentageChangeBy < 0.25m && percentageChangeBy >= 0.1m)
                {
                    SuspiciousAcitivtyIdentified(1);
                }
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(AlertTheAuthorities(SuspiciousActivityCounter));
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (SuspiciousActivityCounter >= 7)
            {
                Console.Clear();
                using (StreamReader sr = new StreamReader(gameOverFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(line);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
                LogScoreBoard.GameOver(CurrentUserName);
                Environment.Exit(1);
            }
        }
        public bool AssertCreditCardInformationMatch(CreditCard userCreditCard, CreditCard givenCreditCard)
        {
            int countTrue = 0;

            if (givenCreditCard.CreditCardNumber == userCreditCard.CreditCardNumber)
            {
                countTrue++;
            }
            else
            {
                SuspiciousAcitivtyIdentified(1);
            }
            if (givenCreditCard.ExpirationDate == userCreditCard.ExpirationDate)
            {
                countTrue++;
            }
            else
            {
                SuspiciousAcitivtyIdentified(1);
            }
            if (givenCreditCard.Pin == userCreditCard.Pin)
            {
                countTrue++;
            }
            else
            {
                SuspiciousAcitivtyIdentified(1);
            }
            return countTrue == 3 ? true : false;
        }
    }
}
