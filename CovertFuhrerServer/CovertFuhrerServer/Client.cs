using Ether.Network.Common;
using Ether.Network.Packets;
using System;

namespace CovertFuhrerServer
{
    internal sealed class Client : NetUser
    {
        private string _playerName;
        private bool _isPlayerNamed;
        /// <summary>
        /// Send hello to the incoming clients.
        /// </summary>
        public void SendFirstPacket()
        {
            using (var packet = new NetPacket())
            {
                packet.Write("Welcome " + _playerName + "!");

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
            if (!_isPlayerNamed)
            {
                _playerName = value;
                _isPlayerNamed = true;
                SendFirstPacket();
            }
            else
            {
                Console.WriteLine("Received '{1}' from {0}", Id, value);

                using (var p = new NetPacket())
                {
                    p.Write(string.Format("OK: '{0}'", value));
                    Server.SendToAll(p);
                }
            }
        }
    }
}