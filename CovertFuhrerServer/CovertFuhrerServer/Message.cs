﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CovertFuhrerServer
{
    class Message
    {
        public string waitForPlayers()
        {
            return "Waiting for Players...";
        }

        public string gameStart()
        {
            return "Game has started!";
        }

        public string assignRole(Role role)
        {
            string message = "You are a " + role + ".\n";
            switch (role)
            {
                case (Role.Liberal):
                    message += "To win: Enact 5 liberal policies OR assassinate the Fuhrer.";
                    break;
                case (Role.Facist):
                case (Role.Fuhrer):
                    message += "To win: Enact 6 facist policies OR enact 3 facist policies & have the Fuhrer become the Chancellor";
                    break;
            }

            return message;
        }

        public string becomePresident(PlayerObject player)
        {
            return "" + player.name + " has become President.";
        }

        public string nominateChancellor(PlayerObject president, PlayerObject chancellor)
        {
            return "" + president.name + "has nominated " + chancellor.name + " to be chancellor.\nVote now! (Y/N)";
        }

        public string voteRecieved()
        {
            return "Vote recieved!";
        }

        public string votePassed(PlayerObject chancellor)
        {
            return "" + chancellor.name + "has become the chancellor!";
        }

        public string voteFailed(PlayerObject chancellor)
        {
            return "" + chancellor.name + "has failed to become the chancellor.";
        }

        public string presidentDiscard(PlayerObject president)
        {
            return "" + president.name + " is discarding a policy.";
        }

        public string chancellorSelectPolicy(PlayerObject chancellor)
        {
            return "" + chancellor.name + " is picking a policy.";
        }

        public string chancellorSelectPolicyAfter(PlayerObject chancellor, Policy policy, int liberalPolicyCount, int facistPolicyCount)
        {
            return "" + chancellor.name + " has enacted a " + policy + " policy!\nPolicies passed:\nLiberal: " + liberalPolicyCount + "/5\nFacist: " + facistPolicyCount + "/6";
        }

        public string investigatePlayerBefore(PlayerObject president)
        {
            return "" + president.name + " is selecting a player to investigate.";
        }

        public string investigatePlayerAfterPrivate(PlayerObject player)
        {
            return "" + "You know that " + player.name + " is a " + player.role + "!";
        }

        public string investigatePlayerAfterPublic(PlayerObject president, PlayerObject player)
        {
            return "" + president.name + " investigated " + player.name + " and now knows their role!";
        }

        public string specialElectionBefore(PlayerObject president)
        {
            return "" + president.name + " is choosing the next president";
        }

        public string specialElectionAfter(PlayerObject oldPresident, PlayerObject newPresident)
        {
            return "" + oldPresident.name + " has elected " + newPresident.name + " to be the next president!";
        }

        public string policyPeekPublic(PlayerObject president)
        {
            return "" + president.name + " peeked at the top 3 policies.";
        }

        public string policyPeekPrivate(Policy p1, Policy p2, Policy p3)
        {
            return "You peek at the top 3 policies: " + p1 + ", " + p2 + ", " + p3;
        }

        public string executePlayerBefore(PlayerObject president)
        {
            return "" + president.name + " is picking a player to execute.";
        }

        public string executePlayerAfter(PlayerObject president, PlayerObject victim)
        {
            return "" + president.name + " has executed " + victim.name + ".";
        }
    }
}
