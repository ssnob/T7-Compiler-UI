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
using static System.PEStructures.PE;

namespace Idea.Games
{
    internal static class BlackOps3
    {
        internal static class Constants
        {
            public static ulong OFF_CBUF_ADDTEXT = 0x20EC010;
            public static ulong OFF_DVAR_SETFROMSTRINGBYNAME = 0x22C7500;
            public static ulong OFF_SendUIPopUp = 0x228DEE0;
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
            Game.Call<VOID>(Game[OFF_SendUIPopUp], 0, 0x8, message); // 0x8 == info
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
    }
}

