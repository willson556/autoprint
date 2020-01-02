using System;

namespace autoPrint
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Printing... ");
            
            var engine = new PrintingEngine();
            engine.Print();

            Console.WriteLine("Done.");
        }
    }
}
