using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apriori
{
    class Program
    {
        static void Main(string[] args)
        {
            /*public Apriori(
             *              string inFileName, 
             *              string outFileName, 
             *              string separator, 
             *              int minSupport)*/
            Apriori a = new Apriori("input.txt", "output.txt", "\\", 2);
            //Apriori is defined in Apriori.cs
        }
    }
}
