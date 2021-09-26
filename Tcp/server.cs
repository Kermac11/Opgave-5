using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ModelLib;

namespace PlainServer
{
    public class Server
    {
        private static List<FootballPlayer> players = new List<FootballPlayer>()
        {
            new FootballPlayer(1,"Peter",80,1),
            new FootballPlayer(5,"John", 50,5),
            new FootballPlayer(7,"Jens", 77,7),
            new FootballPlayer(21,"Knud",150,21)
        };
        public void Start(TcpClient socket)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 2121);
            listener.Start();
            while (true)
            {
                socket = listener.AcceptTcpClient();
                Task.Run(() => Server.DoClient(socket));

            }
        }

        private static void DoClient(TcpClient socket)
        {

            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                sw.AutoFlush = true;
                string str = sr.ReadLine();
                string[] command = str.Split('/');
                switch (command[0])
                {
                    case "HentAlle":
                        foreach (FootballPlayer player in players)
                        {
                         sw.WriteLine(JsonSerializer.Serialize(player));
                        }
                        break;
                    case "Hent":
                        FootballPlayer place = players.Find(p => p.Id == int.Parse(command[1]));
                        sw.WriteLine(JsonSerializer.Serialize(place));
                        break;
                    case "Gem":
                        players.Add(JsonSerializer.Deserialize<FootballPlayer>(command[1]));
                        break;
                }
                Console.WriteLine("Server har modtaget besked : " + str);
                sw.WriteLine(str);
                sw.Flush();
            }

            socket?.Close();
        }
    }
}
