using System;
using System.Collections.Generic;
using System.Text;

namespace Idea.Games
{
    class Game
    {
        public enum GAMES : int
        {
            BLACKOPS3 = 0x0,
            BLACKOPS4 = 0x1,
            UNKNOWN = 0x2,
        }
        public enum GAMEMODE : int
        {
            CAMPAIGN = 0x0,
            MULTIPLAYER = 0x1,
            ZOMBIES = 0x2,
            UNKNOWN = 0x3,
        };
    }
}
