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
            Console.Title = "Covert Fuhrer";
            var client = new Client("127.0.0.1", 4444, 512);
            client.Connect();

            if (!client.IsConnected)
            {
                Console.WriteLine("Can't connect to server!");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Enter your name:");
            playerName = Console.ReadLine();
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