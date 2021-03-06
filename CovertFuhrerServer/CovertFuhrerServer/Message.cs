﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CovertFuhrerServer
{
    class Message
    {
        public static string waitForPlayers()
        {
            return "Waiting for Players...";
        }

        public static string gameStart()
        {
            return "Game has started!  Type \"players\" at any time to see the names of other players.";
        }

        public static string assignRole(Role role)
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

        public static string becomePresident(PlayerObject player)
        {
            return "" + player.name + " has become President.";
        }

        public static string presidentInstructionsNominate()
        {
            return "Nominate a chancellor by typing \"nominate <player>\"";
        }

        public static string presidentInstructionsDiscard(Policy p1, Policy p2, Policy p3)
        {
            return $"Discard a policy by typing \"discard <policy number>\".\nAvailable policies:\n1. {p1}\n2. {p2}\n3. {p3}";
        }

        public static string nominateChancellor(PlayerObject president, PlayerObject chancellor)
        {
            return "" + president.name + " has nominated " + chancellor.name + " to be chancellor.\nVote by typing \"vote yes\" or \"vote no\".";
        }

        public static string chancellorInstructions(Policy p1, Policy p2)
        {
            return $"Pick a policy by typing \"pick <policy number>\".\nAvailable policies:\n1. {p1}\n2. {p2}";
        }

        public static string voteRecieved(PlayerObject player, bool vote)
        {
            if (vote)
            {
                return $"{player.name} has voted YES.";
            }
            else
            {
                return $"{player.name} has voted NO.";
            }
        }

        public static string votePassed(PlayerObject chancellor)
        {
            return "" + chancellor.name + " has become the chancellor!";
        }

        public static string voteFailed(PlayerObject chancellor)
        {
            return "" + chancellor.name + " has failed to become the chancellor.";
        }

        public static string presidentDiscard(PlayerObject president)
        {
            return "" + president.name + " is discarding a policy.";
        }

        public static string chancellorSelectPolicy(PlayerObject chancellor)
        {
            return "" + chancellor.name + " is picking a policy.";
        }

        public static string chancellorSelectPolicyAfter(PlayerObject chancellor, Policy policy, int liberalPolicyCount, int facistPolicyCount)
        {
            return "" + chancellor.name + " has enacted a " + policy + " policy!\nPolicies passed:\nLiberal: " + liberalPolicyCount + "/5\nFacist: " + facistPolicyCount + "/6";
        }

        public static string investigatePlayerBefore(PlayerObject president)
        {
            return "" + president.name + " is selecting a player to investigate.";
        }

        public static string investigatePlayerAfterPrivate(PlayerObject player)
        {
            if (player.role.Equals(Role.Fuhrer))
            {
                return "" + "You know that " + player.name + " is a Facist!";
            }
            return "" + "You know that " + player.name + " is a " + player.role + "!";
        }

        public static string investigatePlayerAfterPublic(PlayerObject president, PlayerObject player)
        {
            return "" + president.name + " investigated " + player.name + " and now knows their role!";
        }

        public static string specialElectionBefore(PlayerObject president)
        {
            return "" + president.name + " is choosing the next president";
        }

        public static string specialElectionAfter(PlayerObject oldPresident, PlayerObject newPresident)
        {
            return "" + oldPresident.name + " has elected " + newPresident.name + " to be the next president!";
        }

        public static string policyPeekPublic(PlayerObject president)
        {
            return "" + president.name + " peeked at the top 3 policies.";
        }

        public static string policyPeekPrivate(Policy p1, Policy p2, Policy p3)
        {
            return "You peek at the top 3 policies: " + p1 + ", " + p2 + ", " + p3;
        }

        public static string executePlayerBefore(PlayerObject president)
        {
            return "" + president.name + " is picking a player to execute.";
        }

        public static string executePlayerAfter(PlayerObject president, PlayerObject victim)
        {
            return "" + president.name + " has executed " + victim.name + ".";
        }

        public static string executeInstructions()
        {
            return "Kill a player by typing \"kill <player>\"";
        }

        public static string investigateInstructions()
        {
            return "Investigate a player by typing \"investigate <player>\"";
        }

        public static string electInstructions()
        {
            return "Elect the next president by typing \"elect <player>\"";
        }

        public static string presidentNominate(PlayerObject president)
        {
            return $"{president.name} is nominating a chancellor.";
        }

        public static string teammate(PlayerObject player)
        {
            return $"You are on a team with {player.name}, who is a {player.role}";
        }
    }
}
