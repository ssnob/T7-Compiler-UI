using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Idea.Games.BlackOps3.Constants;
using static System.ExCallThreadType;

namespace Idea.Games
{
    internal static class BlackOps3
    {
        internal static class Constants
        {
            public const ulong OFF_CBUF_ADDTEXT = 0x20EC8B0;
            public const ulong OFF_DVAR_SETFROMSTRINGBYNAME = 0x22C7F60;
            public const ulong OFF_SendUIPopUp = 0x228E940;
            public const ulong OFF_GetLocalClientNum = 0x20F0220;
        }

        private static ProcessEx __game;
        internal static ProcessEx Game
        {
            get
            {
                if (__game == null || __game.BaseProcess.HasExited)
                {
                    __game = "blackops3";
                    if (__game == null || __game.BaseProcess.HasExited)
                    {
                        throw new Exception("Black Ops 3 not found.");
                    }
                    __game.SetDefaultCallType(XCTT_RIPHijack);
                }
                if (!__game.Handle)
                {
                    __game.OpenHandle(ProcessEx.PROCESS_ACCESS, true);
                }
                return __game;
            }
        }

        internal static void Cbuf_AddText(string serverText, int client = 0)
        {
            if (serverText == null) return;
            Game.Call<ulong>(Game[OFF_CBUF_ADDTEXT], client, serverText, 0L);
        }

        internal static void Dvar_SetFromStringByName(string dvarName, string _value, bool CreateIfMissing = true)
        {
            if (dvarName == null || _value == null) return;
            Game.Call<ulong>(Game[OFF_DVAR_SETFROMSTRINGBYNAME], dvarName, _value, CreateIfMissing);
        }
        internal static void Popup(string message)
        {
            int clientnum = Game.Call<int>(Game[OFF_GetLocalClientNum], 0);
            Game.Call<VOID>(Game[OFF_SendUIPopUp], clientnum, 0x8, message); // 0x8 == info
        }
        internal static void ApplyHostDvars()
        {
            Dvar_SetFromStringByName("party_minPlayers", "1");
            Dvar_SetFromStringByName("lobbyDedicatedSearchSkip", "1");
            Dvar_SetFromStringByName("lobbySearchTeamSize", "1");
            Dvar_SetFromStringByName("lobbySearchSkip", "1");
            Dvar_SetFromStringByName("lobbyMergeDedicatedEnabled", "0");
            Dvar_SetFromStringByName("lobbyMergeEnabled", "0");
        }
        internal static void ClearHostDVARS()
        {
            Dvar_SetFromStringByName("party_minPlayers", "4");
            Dvar_SetFromStringByName("lobbyDedicatedSearchSkip", "0");
            Dvar_SetFromStringByName("lobbySearchTeamSize", "2");
            Dvar_SetFromStringByName("lobbySearchSkip", "0");
            Dvar_SetFromStringByName("lobbyMergeDedicatedEnabled", "1");
            Dvar_SetFromStringByName("lobbyMergeEnabled", "1");
        }
        internal enum Gamemodes : int
        {
            MODE_ZOMBIES = 0x0,
            MODE_MULTIPLAYER = 0x1,
            MODE_CAMPAIGN = 0x2,
        };
    }
}

