using System;

namespace autoPrint
{
    class Program
    {
        const string SomeTextToPrint = @"This is some text that I want to print!";

        static void Main(string[] args)
        {
            Console.Write("Printing... ");
            
            var engine = new PrintingEngine();
            engine.Print();

            Console.WriteLine("Done.");
        }
    }
}
