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

                /*Console.WriteLine("Received '{1}' from {0}", Id, value);

                using (var p = new NetPacket())
                {
                    p.Write(string.Format("OK: '{0}'", value));
                    Server.SendToAll(p);
                }*/
            }
        }
    }
}