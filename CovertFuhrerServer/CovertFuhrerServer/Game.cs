using System;
using System.Collections.Generic;
using System.Text;
namespace CovertFuhrerServer
{
    class Game
    {
        private List<Client> clients;
        private int index;
        static Random random = new Random();
        public Game(List<Client> clients)
        {
            this.clients = clients;
        }
        public void start()
        {
            Client.SendMessageToAllClients(Message.gameStart());
            Client firstPresident = pickRandomPresident(clients);
            Client nextPresident = rotatePresident(index);
            chancellorOptions(firstPresident);
            //firstPresident.SendPresidentialPacket();
        }
        // first president is randomly selected
        public Client pickRandomPresident(List<Client> clients)
        {
            index = random.Next(clients.Count);
            Message.becomePresident(clients[index].player);
            return clients[index];
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
        public void chancellorOptions(Client president)
        {
            Console.WriteLine(president.player.name + ", kindly nominate a Chancellor from among the following: ");
            foreach (var player in clients)
            {
                if (clients.IndexOf(player) != index)
                {
                    Console.WriteLine(player.player.name);
                }
            }
        }
    }
}
