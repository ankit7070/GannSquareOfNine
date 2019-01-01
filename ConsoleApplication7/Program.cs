using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GannLibrary
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Input Current MarketPrice: ");
            double cmp = 0;
            if (Double.TryParse(Console.ReadLine(), out cmp))
            {

                Gann test = new Gann(cmp, 2);
                Console.WriteLine("Buy At :" + test.BuyAt + "\t Sell At:" + test.SellAt);
                Console.WriteLine("Buy T1:" + test.BuyTargetOne + "\t Sell T1:" + test.SellTargetOne);
                Console.WriteLine("Buy T2:" + test.BuyTargetTwo + "\t Sell T2:" + test.SellTargetTwo);
                Console.WriteLine("Buy T3:" + test.BuyTargetThree + "\t Sell T3:" + test.SellTargetThree);
                Console.WriteLine("Buy T4:" + test.BuyTargetFour + "\t Sell T4:" + test.SellTargetFour);
                Console.WriteLine("Buy T5:" + test.BuyTargetFive + "\t Sell T5:" + test.SellTargetFive);
                Console.Read();
            }
            else
                Console.WriteLine("Bad Input");
        }
    }
}
