﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CovertFuhrerServer
{
    class Game
    {
        private List<Client> clients;

        private int index;

        public Game(List<Client> clients)
        {
            this.clients = clients;
        }

        public void start()
        {
            Console.WriteLine("Game has started with the following players:");
            foreach (var client in clients)
            {
                Console.WriteLine(client.player.name);
            }
        }

        private PlayerObject pickRandomPresident(List<Client> clients)
        {
            return PlayerObject 
        }
    }
}
