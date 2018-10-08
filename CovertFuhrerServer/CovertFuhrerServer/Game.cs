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

        private int nominatedChancellor { get; set; }

        private int facistPolicyCount { get; set; }

        private int currentPresident { get; set; }

        private int currentChancellor { get; set; }

        private int totalVotes { get; set; }

        private int yesVotes { get; set; }

        private int noVotes { get; set; }

        private bool specialPresident { get; set; }

        private int specialPreviousPresident { get; set; }

        private bool fuhrerElected { get; set; }

        private bool fuhrerKilled { get; set; }

        private int facistPlayer { get; set; }

        private int fuhrerPlayer { get; set; }

        private List<Policy> top3Policies;

        static Random random = new Random();

        public Game(List<Client> clients)
        {
            this.clients = clients;
            state = TurnState.rotatePresident;
            liberalPolicyCount = 0;
            facistPolicyCount = 0;
            currentPresident = -1;
            currentChancellor = -1;
            nominatedChancellor = -1;
            facistPlayer = -1;
            fuhrerPlayer = -1;
            top3Policies = new List<Policy>();
            populatePolicies();
            specialPreviousPresident = -1;
            specialPresident = false;
            fuhrerElected = false;
            fuhrerKilled = false;
        }

        public void start()
        {
            Client.SendMessageToAllClients(Message.gameStart());
            assignRoles();
            foreach (var client in clients)
            {
                client.SendMessage(Message.assignRole(client.player.role));
            }
            clients[facistPlayer].SendMessage(Message.teammate(clients[fuhrerPlayer].player));
            clients[fuhrerPlayer].SendMessage(Message.teammate(clients[facistPlayer].player));
            currentPresident = pickRandomPresident();
        }

        // first president is randomly selected
        public int pickRandomPresident()
        {
            int index = random.Next(clients.Count);
            Client.SendMessageToAllClients(Message.becomePresident(clients[index].player));
            state = TurnState.nominateChancellor;
            Client.SendMessageToAllClients(Message.presidentNominate(clients[index].player));
            clients[index].SendMessage(Message.presidentInstructionsNominate());
            return index;
        }

        //the presidency moves to the next player in line
        public void rotatePresident()
        {
            bool presidentChosen = false;
            while (!presidentChosen)
            {
                if (specialPresident)
                {
                    if (specialPreviousPresident != clients.Count - 1)
                    {
                        currentPresident = specialPreviousPresident + 1;
                    }
                    else
                    {
                        currentPresident = 0;
                    }
                    specialPresident = false;
                    specialPreviousPresident = -1;
                    if (clients[currentPresident].player.isAlive)
                    {
                        presidentChosen = true;
                    }
                }
                else
                {
                    if (currentPresident != clients.Count - 1)
                    {
                        currentPresident += 1;
                    }
                    else
                    {
                        currentPresident = 0;
                    }
                    if (clients[currentPresident].player.isAlive)
                    {
                        presidentChosen = true;
                    }
                }
            }
            Client.SendMessageToAllClients(Message.becomePresident(clients[currentPresident].player));
            state = TurnState.nominateChancellor;
            Client.SendMessageToAllClients(Message.presidentNominate(clients[currentPresident].player));
            clients[currentPresident].SendMessage(Message.presidentInstructionsNominate());
        }

        public void vote(int playerIndex, bool vote)
        {
            if (state.Equals(TurnState.voteChancellor) && !clients[playerIndex].player.hasVoted)
            {
                if (vote)
                {
                    totalVotes++;
                    yesVotes++;
                    clients[playerIndex].player.hasVoted = true;
                    Client.SendMessageToAllClients(Message.voteRecieved(clients[playerIndex].player, true));
                }
                else
                {
                    totalVotes++;
                    noVotes++;
                    clients[playerIndex].player.hasVoted = true;
                    Client.SendMessageToAllClients(Message.voteRecieved(clients[playerIndex].player, false));
                }
                if (totalVotes == getAlivePlayerCount())
                {
                    if (yesVotes > noVotes)
                    {
                        if (clients[nominatedChancellor].player.role.Equals(Role.Fuhrer) && facistPolicyCount >= 3)
                        {
                            fuhrerElected = true;
                            finishGame();
                        }
                        currentChancellor = nominatedChancellor;
                        Client.SendMessageToAllClients(Message.votePassed(clients[currentChancellor].player));
                        Client.SendMessageToAllClients(Message.presidentDiscard(clients[currentPresident].player));
                        clients[currentPresident].SendMessage(Message.presidentInstructionsDiscard(top3Policies[0], top3Policies[1], top3Policies[2]));
                        state = TurnState.presidentDiscard;
                    }
                    else
                    {
                        Client.SendMessageToAllClients(Message.voteFailed(clients[nominatedChancellor].player));
                        state = TurnState.rotatePresident;
                        nominatedChancellor = -1;
                        rotatePresident();
                    }
                    totalVotes = 0;
                    yesVotes = 0;
                    noVotes = 0;
                    foreach (var client in clients)
                    {
                        client.player.hasVoted = false;
                    }
                }
            }
        }

        public void discardPolicy(int playerIndex, int policyIndex)
        {
            if (state.Equals(TurnState.presidentDiscard) && playerIndex == currentPresident)
            {
                policyIndex--;
                top3Policies.RemoveAt(policyIndex);
                Client.SendMessageToAllClients(Message.chancellorSelectPolicy(clients[currentChancellor].player));
                clients[currentChancellor].SendMessage(Message.chancellorInstructions(top3Policies[0], top3Policies[1]));
                state = TurnState.chancellorPick;
            }
        }
        private void populatePolicies()
        {
            top3Policies.Clear();
            for (int i = 0; i < 3; i++)
            {
                int policy = random.Next(2);
                if (policy == 0)
                {
                    top3Policies.Add(Policy.Liberal);
                }
                else
                {
                    top3Policies.Add(Policy.Facist);
                }
            }
        }

        public void nominateChancellor(int currentPresident, int index)
        {
            if (state.Equals(TurnState.nominateChancellor) && currentPresident == this.currentPresident)
            {
                nominatedChancellor = index;
                string message = Message.nominateChancellor(clients[currentPresident].player, clients[index].player);
                Client.SendMessageToAllClients(message);
                state = TurnState.voteChancellor;
            }

        }

        public void pickPolicy(int thisPlayerIndex, string selectedPolicy)
        {
            int prevFacistPolicyCount = facistPolicyCount;
            if (state.Equals(TurnState.chancellorPick) && thisPlayerIndex == currentChancellor)
            {
                int convertedToken = Int32.Parse(selectedPolicy) - 1;
                if (top3Policies[convertedToken] == Policy.Liberal)
                {
                    liberalPolicyCount++;
                }
                else
                {
                    facistPolicyCount++;
                }
                string message = Message.chancellorSelectPolicyAfter(clients[thisPlayerIndex].player, top3Policies[convertedToken], liberalPolicyCount, facistPolicyCount);

                Client.SendMessageToAllClients(message);

                populatePolicies();
                if (liberalPolicyCount == 5 || facistPolicyCount == 6)
                {
                    finishGame();
                }
                if (facistPolicyCount > prevFacistPolicyCount)
                {
                    state = TurnState.facistAction;
                    switch (facistPolicyCount)
                    {
                        case 1:
                        case 2:
                            Client.SendMessageToAllClients(Message.investigatePlayerBefore(clients[currentPresident].player));
                            clients[currentPresident].SendMessage(Message.investigateInstructions());
                            break;
                        case 3:
                            Client.SendMessageToAllClients(Message.specialElectionBefore(clients[currentPresident].player));
                            clients[currentPresident].SendMessage(Message.electInstructions());
                            break;
                        case 4:
                        case 5:
                            Client.SendMessageToAllClients(Message.executePlayerBefore(clients[currentPresident].player));
                            clients[currentPresident].SendMessage(Message.executeInstructions());
                            break;
                    }
                }
                else
                {
                    state = TurnState.rotatePresident;
                    rotatePresident();
                }
            }
        }

        public void kill(int thisPlayerIndex, int playerIndex)
        {
            if (state.Equals(TurnState.facistAction) && (facistPolicyCount == 4 || facistPolicyCount == 5) && thisPlayerIndex == currentPresident)
            {
                clients[playerIndex].player.isAlive = false;
                string message = Message.executePlayerAfter(clients[thisPlayerIndex].player, clients[playerIndex].player);
                Client.SendMessageToAllClients(message);
                if (playerIndex == fuhrerPlayer)
                {
                    fuhrerKilled = true;
                    finishGame();
                }
                else
                {
                    state = TurnState.rotatePresident;
                    rotatePresident();
                }
            }

        }

        public void electPresident(int oldPresident, int newPresident)
        {
            if (state.Equals(TurnState.facistAction) && facistPolicyCount == 3 && oldPresident == currentPresident)
            {
                specialPresident = true;
                specialPreviousPresident = oldPresident;
                currentPresident = newPresident;
                Client.SendMessageToAllClients(Message.specialElectionAfter(clients[oldPresident].player,
                    clients[newPresident].player));
                state = TurnState.nominateChancellor;
                Client.SendMessageToAllClients(Message.presidentNominate(clients[currentPresident].player));
                clients[currentPresident].SendMessage(Message.presidentInstructionsNominate());
            }
        }

        private int getAlivePlayerCount()
        {
            int count = 0;
            foreach (var client in clients)
            {
                if (client.player.isAlive)
                {
                    count++;
                }
            }
            return count;
        }

        public void investigate(int thisPlayerIndex, int playerIndex)
        {
            if (state.Equals(TurnState.facistAction) && (facistPolicyCount == 1 || facistPolicyCount == 2) && thisPlayerIndex == currentPresident)
            {
                string privateMessage = Message.investigatePlayerAfterPrivate(clients[playerIndex].player);
                //private message to the president indicating the investigated's role

                clients[thisPlayerIndex].SendMessage(privateMessage);

                string publicMessage = Message.investigatePlayerAfterPublic(clients[thisPlayerIndex].player, clients[playerIndex].player);
                //public message to all players of the presidents knowledge post investigation

                Client.SendMessageToAllClients(publicMessage);
            }
            state = TurnState.rotatePresident;
            rotatePresident();
        }

        public void finishGame()
        {
            StringBuilder finalMessage = new StringBuilder();
            if (fuhrerKilled)
            {
                finalMessage.Append("The Fuhrer was killed.  Liberals win!  Winning Players:");
                foreach (var client in clients)
                {
                    if (client.player.role.Equals(Role.Liberal))
                    {
                        finalMessage.Append($"\n{client.player.name}");
                    }
                }
            }
            else if (fuhrerElected)
            {
                finalMessage.Append("The Fuhrer was elected chancellor.  Facists win!  Winning Players:");
                foreach (var client in clients)
                {
                    if (client.player.role.Equals(Role.Facist) || client.player.role.Equals(Role.Fuhrer))
                    {
                        finalMessage.Append($"\n{client.player.name}");
                    }
                }
            }
            else if (liberalPolicyCount == 5)
            {
                finalMessage.Append("Liberals win!  Winning Players:");
                foreach (var client in clients)
                {
                    if (client.player.role.Equals(Role.Liberal))
                    {
                        finalMessage.Append($"\n{client.player.name}");
                    }
                }
            }
            else
            {
                finalMessage.Append("Facists win!  Winning Players:");
                foreach (var client in clients)
                {
                    if (client.player.role.Equals(Role.Facist) || client.player.role.Equals(Role.Fuhrer))
                    {
                        finalMessage.Append($"\n{client.player.name}");
                    }
                }
            }
            Client.SendMessageToAllClients(finalMessage.ToString());
            while (true)
            {
                //DO NOTHING, game has ended.
            }
        }

        private void assignRoles()
        {
            int facist = random.Next(clients.Count);
            int fuhrer = random.Next(clients.Count);
            while (fuhrer == facist)
            {
                fuhrer = random.Next(clients.Count);
            }
            facistPlayer = facist;
            fuhrerPlayer = fuhrer;
            clients[facist].player.role = Role.Facist;
            clients[fuhrer].player.role = Role.Fuhrer;
        }
    }
}