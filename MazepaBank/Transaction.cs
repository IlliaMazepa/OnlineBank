using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazepaBank
{
    public class MTransaction
    {
        public string TelNum { get; set; }
        public string Type { get; set; }
        public string TName { get; set; }
        public string Description { get; set; }
        public double Sum { get; set; }

        public MTransaction()
        {

        }
        public MTransaction(string telum,string type, string name, string description, double sum)
        {
            TelNum = telum;
            Type = type;
            TName = name;
            Description = description;
            Sum = sum;
        }

        public override string ToString()
        {
            return $"{TName}\t\t{Sum}";
        }
    }
}
