using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(DateTime.Today.AddDays(0 - Convert.ToInt16(DateTime.Now.DayOfWeek) - 7));
            Console.WriteLine(DateTime.Today.AddDays(6 - Convert.ToInt16(DateTime.Now.DayOfWeek) - 7));
            Console.WriteLine(DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(-1));
            Console.WriteLine(DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddDays(-1));
            Console.WriteLine(DateTime.Today.AddDays(0 - Convert.ToInt16(DateTime.Now.DayOfWeek)));
            Console.WriteLine(DateTime.Today.AddDays(6 - Convert.ToInt16(DateTime.Now.DayOfWeek)));
            Console.ReadKey();
        }
    }
}
