using Ether.Network.Common;
using Ether.Network.Packets;
using System;
using System.Collections.Generic;

namespace CovertFuhrerServer
{
    internal sealed class Client : NetUser
    {
        public static Game game { get; set; }
        public static List<Client> clients { get; set; }
        public bool isPlayerNamed { get; set; }
        public PlayerObject player { get; set; }
        /// <summary>
        /// Send hello to the incoming clients.
        /// </summary>
        public void SendFirstPacket()
        {
            using (var packet = new NetPacket())
            {
                packet.Write("Welcome " + player.name + "!");

                Send(packet);
            }
        }

        public void SendMessage(string message)
        {
            using (var packet = new NetPacket())
            {
                packet.Write(message);

                Send(packet);
            }
        }

        public static void SendMessageToAllClients(string message)
        {
            using (var packet = new NetPacket())
            {
                packet.Write(message);
                foreach (var client in clients)
                {
                    client.Send(packet);
                }
            }
        }

        /// <summary>
        /// Receive messages from the client.
        /// </summary>
        /// <param name="packet"></param>
        public override void HandleMessage(INetPacketStream packet)
        {
            var value = packet.Read<string>();
            //First time this client connects only.  Assigns names to players.
            if (!isPlayerNamed)
            {
                player = new PlayerObject(value);
                isPlayerNamed = true;
                SendFirstPacket();
                //If we have enough players to start the game, we do.
                if (clients.Count == 5)
                {
                    game = new Game(clients);
                    game.start();
                }
            }
            //Handle commands sent from player
            else
            {
                //List players in game
                if (value.ToLower().Equals("players"))
                {
                    string message = "Players:";
                    foreach (var client in clients)
                    {
                        message += "\n" + client.player.name;
                    }
                    SendMessage(message);
                }
                //Others
                else if (value.Contains(" "))
                {
                    String[] tokens = value.Split(" ");
                    int playerIndex = handleSecondToken(tokens[1]);
                    int thisPlayerIndex = getThisPlayerIndex();
                    if (playerIndex == -1 || thisPlayerIndex == -1)
                    {
                        //do nothing
                        Console.WriteLine("DO NOTHING");
                    }
                    else if (playerIndex == -2)
                    {
                        game.vote(thisPlayerIndex, true);
                        Console.WriteLine("VOTE YES");
                    }
                    else if (playerIndex == -3)
                    {
                        game.vote(thisPlayerIndex, false);
                        Console.WriteLine("VOTE NO");
                    }
                    else if (playerIndex == -4)
                    {
                        if (tokens[0].ToLower().Equals("discard"))
                        {
                            game.discardPolicy(thisPlayerIndex, Int32.Parse(tokens[1]));
                            Console.WriteLine($"DISCARD {tokens[1]}");
                        }
                        else if (tokens[0].ToLower().Equals("pick"))
                        {
                            if (Int32.Parse(tokens[1]) < 3)
                            {
                                game.pickPolicy(thisPlayerIndex, tokens[1]);
                                Console.WriteLine($"PICK {tokens[1]}");
                            }
                            else
                            {
                                //DO NOTHING
                                Console.WriteLine("DO NOTHING");
                            }
                        }
                    }
                    else if (tokens[0].ToLower().Equals("kill"))
                    {
                        game.kill(thisPlayerIndex, playerIndex);
                        Console.WriteLine($"KILL {playerIndex}");
                    }
                    else if (tokens[0].ToLower().Equals("nominate"))
                    {
                        game.nominateChancellor(thisPlayerIndex, playerIndex);
                        Console.WriteLine($"NOMINATE {playerIndex}");
                    }
                    else if (tokens[0].ToLower().Equals("investigate"))
                    {
                        //investigate(thisPlayerIndex, playerIndex);
                        Console.WriteLine($"INVESTIGATE {playerIndex}");
                    }
                    else if (tokens[0].ToLower().Equals("elect"))
                    {
                        //electPresident(thisPlayerIndex, playerIndex);
                        Console.WriteLine($"ELECT {playerIndex}");
                    }
                }
            }
        }

        private int handleSecondToken(string name)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].player.name.ToLower().Equals(name.ToLower()))
                {
                    return i;
                }
                else if (name.ToLower().Equals("yes"))
                {
                    return -2;
                }
                else if (name.ToLower().Equals("no"))
                {
                    return -3;
                }
                else if (Int32.TryParse(name, out int cardNumber))
                {
                    if (cardNumber <= 3 && cardNumber >= 1)
                    {
                        return -4;
                    }
                }
            }
            return -1;
        }

        public int getThisPlayerIndex()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].player.name.ToLower().Equals(player.name.ToLower()))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}