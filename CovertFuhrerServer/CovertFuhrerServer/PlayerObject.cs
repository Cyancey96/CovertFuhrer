using System;
using System.Collections.Generic;
using System.Text;

namespace CovertFuhrerServer
{
    public class PlayerObject
    {
        public string name { get; set; }

        public Role role { get; set; }

        public bool hasVoted { get; set; }

        public bool isAlive { get; set; }

        public PlayerObject(string playerName)
        {
            name = playerName;
            role = Role.Liberal;
            hasVoted = false;
            isAlive = true;
        }

        public PlayerObject(string playerName, Role playerRole)
        {
            name = playerName;
            role = playerRole;
            hasVoted = false;
            isAlive = true;
        }
    }
}