using System;
using System.Collections.Generic;
using System.Text;

namespace CovertFuhrerServer
{
    class Game
    {
        private List<Client> clients;

        private TurnState state { get; set; }

        private int liberalPolicyCount { get; set; }

        private int facistPolicyCount { get; set; }

        private int currentPresident { get; set; }

        private int currentChancellor { get; set; }

        static Random random = new Random();

        public Game(List<Client> clients)
        {
            this.clients = clients;
            state = TurnState.rotatePresident;
        }

        public void start()
        {
            Client.SendMessageToAllClients(Message.gameStart());
            currentPresident = pickRandomPresident();
            
        }

        // first president is randomly selected
        public int pickRandomPresident()
        {
            int index = random.Next(clients.Count);
            Message.becomePresident(clients[index].player);
            return index;
        }

        //the presidency moves to the next player in line
        public Client rotatePresident(int number)
        {
            if (number != clients.Count - 1)
            {
                number = number + 1;
            }
            else
            {
                number = 0;
            }

            Message.becomePresident(clients[number].player);
            return clients[number];
        }
    }
}
