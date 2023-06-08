using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazepaBank
{
    public class Client
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string ThirdName { get; set; }
        public string Password { get; set; }
        public string Number { get; set; }
        public string CardNumber { get; set; }
        public string CVCode { get; set; }
        public string TerminateDate { get; set; }
        public double Balance { get; set; }

        public Client(string name, string surName, string thirdName, string number, string password, string cardNumber, string cVCode, string terminateDate, double balance)
        {
            Name = name;
            SurName = surName;
            ThirdName = thirdName;
            Password = password;
            Number = number;
            CardNumber = cardNumber;
            CVCode = cVCode;
            TerminateDate = terminateDate;
            Balance = balance;
        }

        public Client(string name, string surName, string thirdName, string password, string number, string cardNumber)
        {
            Random rnd = new Random();
            Name = name;
            SurName = surName;
            ThirdName = thirdName;
            Password = password;
            Number = number;
            CardNumber = cardNumber;
            CVCode = rnd.Next(0, 10).ToString() + rnd.Next(0, 10).ToString() + rnd.Next(0, 10).ToString();
            TerminateDate = TermDateGen();
            Balance = 0;
        }


        string TermDateGen()
        {
            string str = "";
            str = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString().Substring(2);
            if (str.Length == 5) return str;
            else return $"0{str}";
        }

        public override string ToString()
        {
            return $"{Name} {CardNumber} {ThirdName}";
        }
    }
}
