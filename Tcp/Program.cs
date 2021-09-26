using System;
using System.Net.Sockets;
using ModelLib;

namespace PlainServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server test = new Server();
            test.Start(new TcpClient());
        }
    }
}
