using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorManager;

namespace TestErrorManager
{
    public class Loop
    {
        public async Task Main()
        {
            int entry = Convert.ToInt32(Console.ReadLine());

            while (entry != 0)
            {
                switch (entry)
                {
                    case 1:
                        FileNotFoundException ex = new FileNotFoundException("synchrone avec exception", new FileLoadException());
                        StaticLogger.Log(ex);
                        StaticLogger.Log("Synchrone avec message");
                        StaticLogger.Log<FormatException>("synchrone avec typage");
                        break;
                    case 2:
                        FileNotFoundException asyncEx = new FileNotFoundException("asynchrone avec exception", new FileLoadException());
                        await StaticLogger.LogAsync(asyncEx);
                        await StaticLogger.LogAsync("aSynchrone avec message");
                        await StaticLogger.LogAsync<FormatException>("asynchrone avec typage");
                        break;
                }

                entry = Convert.ToInt32(Console.ReadLine());
            }
        }
    }
}
