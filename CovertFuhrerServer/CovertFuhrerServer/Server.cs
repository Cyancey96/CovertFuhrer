
using Ether.Network.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovertFuhrerServer
{
    internal sealed class Server : NetServer<Client>
    {
        private List<Client> clients;
        private Game game;
        /// <summary>
        /// Creates a new <see cref="Server"/> with a default configuration.
        /// </summary>
        public Server()
        {
            Configuration.Backlog = 100;
            Configuration.Port = 4444;
            Configuration.MaximumNumberOfConnections = 5;
            Configuration.Host = "127.0.0.1";
            Configuration.BufferSize = 8;
            Configuration.Blocking = true;
            clients = new List<Client>();
        }

        /// <summary>
        /// Initialize the server resources if needed...
        /// </summary>
        protected async override void Initialize()
        {
            Console.WriteLine("Server is ready.\nAwaiting Players...");
        }

        /// <summary>
        /// On client connected.
        /// </summary>
        /// <param name="connection"></param>
        protected override void OnClientConnected(Client connection)
        {
            Console.WriteLine("New client connected: " + connection.Id);
            clients.Add(connection);
        }

        /// <summary>
        /// On client disconnected.
        /// </summary>
        /// <param name="connection"></param>
        protected override void OnClientDisconnected(Client connection)
        {
            Console.WriteLine("Client disconnected: " + connection.Id);
            clients.Remove(connection);
        }

        /// <summary>
        /// On server error.
        /// </summary>
        /// <param name="exception"></param>
        protected override void OnError(Exception exception)
        {
            // TBA
        }

        protected Task checkNames()
        {

        }
    }
}