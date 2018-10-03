using System;
using System.Collections.Generic;
using System.Text;

namespace CovertFuhrerServer
{
    public class PlayerObject
    {
        public string name { get; set; }

        public Role role { get; set; }

        public PlayerObject(string playerName)
        {
            name = playerName;
        }

        public PlayerObject(string playerName, Role playerRole)
        {
            name = playerName;
            role = playerRole;
        }
    }
}