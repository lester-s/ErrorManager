using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorManager;

namespace TestErrorManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Loop loop = new Loop();
            loop.Main();
        }
    }
}
