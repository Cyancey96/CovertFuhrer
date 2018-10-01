
using Ether.Network.Server;
using System;
using System.Collections.Generic;

namespace CovertFuhrerServer
{
    internal sealed class Server : NetServer<Client>
    {
        private Dictionary<Guid, PlayerObject> players;
        /// <summary>
        /// Creates a new <see cref="Server"/> with a default configuration.
        /// </summary>
        public Server()
        {
            Configuration.Backlog = 100;
            Configuration.Port = 4444;
            Configuration.MaximumNumberOfConnections = 100;
            Configuration.Host = "127.0.0.1";
            Configuration.BufferSize = 8;
            Configuration.Blocking = true;
            players = new Dictionary<Guid, PlayerObject>();
        }

        /// <summary>
        /// Initialize the server resources if needed...
        /// </summary>
        protected override void Initialize()
        {
            Console.WriteLine("Server is ready.");
        }

        /// <summary>
        /// On client connected.
        /// </summary>
        /// <param name="connection"></param>
        protected override void OnClientConnected(Client connection)
        {
            Console.WriteLine("New client connected: " + connection.Id);
            players.Add(connection.Id, new PlayerObject());
        }

        /// <summary>
        /// On client disconnected.
        /// </summary>
        /// <param name="connection"></param>
        protected override void OnClientDisconnected(Client connection)
        {
            Console.WriteLine("Client disconnected: " + connection.Id);
            players.Remove(connection.Id);
        }

        /// <summary>
        /// On server error.
        /// </summary>
        /// <param name="exception"></param>
        protected override void OnError(Exception exception)
        {
            // TBA
        }
    }
}