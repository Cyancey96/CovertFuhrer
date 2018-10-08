using System;
using Ether.Network.Packets;
using System.Linq;
using System.Threading;

namespace CovertFuhrerClient
{
    internal class Program
    {
        private static string playerName;
        private static void Main()
        {
            string[] lines = System.IO.File.ReadAllLines("config.txt");
            Console.Title = "Covert Fuhrer";
            Console.WriteLine("Enter your name:");
            playerName = Console.ReadLine();
            var client = new Client(lines[0], Int32.Parse(lines[1]), 512);
            Console.WriteLine("Connecting...");
            client.Connect();

            if (!client.IsConnected)
            {
                Console.WriteLine("Can't connect to server!");
                Console.ReadLine();
                return;
            }
            try
            {
                using (var packet = new NetPacket())
                {
                    packet.Write(playerName);
                    client.Send(packet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            try
            {
                while (true)
                {
                    string input = Console.ReadLine();

                    if (input != null)
                    {
                        using (var packet = new NetPacket())
                        {
                            packet.Write(input);
                            client.Send(packet);
                        }
                    }
                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            client.Disconnect();

            Console.WriteLine("Disconnected. Press any key to continue...");
            Console.ReadLine();
        }
    }
}