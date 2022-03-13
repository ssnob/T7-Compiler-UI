using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInjector.Actions
{
    internal class Games
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
