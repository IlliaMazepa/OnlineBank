using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MazepaBank
{
    public class Credit
    {
        public string Number { get; set; }
        public double Balance { get; set; }
        public DateTime DateBegin { get; set; }
        public double Percent { get; set; }
        public DateTime LastDate { get; set; }

        public Credit()
        {

        }

        public Credit(string number, double balance, DateTime dateBegin, double percent, DateTime lastDate)
        {
            Number = number;
            Balance = balance;
            DateBegin = dateBegin;
            Percent = percent;
            LastDate = lastDate;
        }

        
    }
}
