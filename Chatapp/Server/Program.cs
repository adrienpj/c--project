using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server serv = new Server(8976);
            serv.start();
        }
    }
}
