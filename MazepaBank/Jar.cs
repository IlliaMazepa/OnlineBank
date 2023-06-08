using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazepaBank
{
    public class Jar
    {
        public string CreatorNumber { get; set; }
        public string IDName { get; set; }
        public double Balance { get; set; }
        public string Description { get; set; }
        public List<MTransaction> transactions { get; set; }

        public  Jar()
        {


        }

        public Jar(string creatorNumber, string iDName, double balance, string description, List<MTransaction> transactions)
        {
            CreatorNumber = creatorNumber;
            IDName = iDName;
            Balance = balance;
            Description = description;
            this.transactions = transactions;
        }

        


    }
}
