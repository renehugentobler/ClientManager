using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HSoft.ClientManager.CTCTWrapper;

namespace HSoft.ClientManager.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            CTCTDB ctct = null;
//            ctct = new CTCTDB("GET ALL");
            ctct = new CTCTDB("PUT ALL");
        }
    }
}
