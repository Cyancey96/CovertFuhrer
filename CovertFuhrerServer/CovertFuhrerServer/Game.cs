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

        private int totalVotes { get; set; }

        private int yesVotes { get; set; }

        private int noVotes { get; set; }

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
            top3Policies = new List<Policy>();
            populatePolicies();
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
            Client.SendMessageToAllClients(Message.becomePresident(clients[index].player));
            state = TurnState.nominateChancellor;
            Client.SendMessageToAllClients(Message.presidentNominate(clients[currentPresident].player));
            clients[currentPresident].SendMessage(Message.presidentInstructionsNominate());
            return index;
        }

        //the presidency moves to the next player in line
        public void rotatePresident()
        {
            if (currentPresident != clients.Count - 1)
            {
                currentPresident += 1;
            }
            else
            {
                currentPresident = 0;
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
                if (totalVotes == clients.Count)
                {
                    if (yesVotes > noVotes)
                    {
                        Client.SendMessageToAllClients(Message.votePassed(clients[currentChancellor].player));
                        Client.SendMessageToAllClients(Message.presidentDiscard(clients[currentPresident].player));
                        clients[currentPresident].SendMessage(Message.presidentInstructionsDiscard(top3Policies[0], top3Policies[1], top3Policies[2]));
                        state = TurnState.presidentDiscard;
                    }
                    else
                    {
                        Client.SendMessageToAllClients(Message.voteFailed(clients[currentChancellor].player));
                        state = TurnState.rotatePresident;
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
            if (state.Equals(TurnState.presidentDiscard))
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
    }
}

