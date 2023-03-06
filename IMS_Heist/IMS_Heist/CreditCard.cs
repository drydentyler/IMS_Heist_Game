using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace HackingWalmart
{
    public class CreditCard
    {
        Random random = new Random();
        public string CreditCardNumber { get; private set; }
        public string ExpirationDate { get; private set; }
        public string Pin { get; private set; }

        public CreditCard()
        {

        }
        public CreditCard(string creditcardnumber, string expirationdate, string pin)
        {
            CreditCardNumber = creditcardnumber;
            ExpirationDate = expirationdate;
            Pin = pin;
        }

        public void CreateCreditCardNumber()
        {
            int firstFour = random.Next(1000, 9999);
            int secondFour = random.Next(1000, 9999);
            int thirdFour = random.Next(1000, 9999);
            int fourthFour = random.Next(1000, 9999);
            CreditCardNumber = $"{firstFour}-{secondFour}-{thirdFour}-{fourthFour}";
        }
        public void CreateExpirationDate()
        {
            int month = random.Next(12);
            int year = random.Next(2024, 2030);
            string fullMonth = month < 10 ? $"0{month}" : $"{month}";
            ExpirationDate = $"{fullMonth}/{year}";
        }
        public void CreatePin()
        {
            int first = random.Next(9);
            int second = random.Next(9);
            int third = random.Next(9);
            Pin = $"{first}{second}{third}";
        }

    }
}
