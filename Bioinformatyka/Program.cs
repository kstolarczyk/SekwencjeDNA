using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioinformatyka
{

    class Program
    {
        static void Main(string[] args)
        {
            Instancja inst = new Instancja("test.txt", 4);
            Console.WriteLine(inst);
            Console.ReadKey();
        }
    }
}
