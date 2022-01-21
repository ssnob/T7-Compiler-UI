SetMaxRank(player)
{
    self notify("SetMaxRank");
    self endon("SetMaxRank");

    self EnableStats();
    
    wasAlreadyPM = true;
    
    if(int(player GetDStat("playerstatslist", "rank", "statValue")) < 54)
        player AddRankXPValue("win", 52542000);
    
    if(int(player GetDStat("playerstatslist", "plevel", "statValue")) < 11)
    {
        player SetDStat("playerstatslist", "plevel", "statValue", 10);
        uploadstats(player);
        wasAlreadyPM = false;
    }
    
    player AddRankXPValue("win", 52542000);
    
    self iPrintLnBold("Player rank ^2adjusted");
    
    return wasAlreadyPM;
}

ResetPendingKeys(player)
{
    self notify("ResetPendingKeys");
    self endon("ResetPendingKeys");

    self EnableStats();
    player SetDStat("mp_loot_xp_due", 0);
    
    self iPrintLnBold("Pending keys ^2Reset^7!");
}

EnableStats()
{
    if(level.inPrematchPeriod)
        self iPrintLnBold("Waiting for online to start...");
    
    while(level.inPrematchPeriod)
    {
        wait 3;
    }
    
    level.is_online = true;
    //EnableOnlineMatch();
}

UnlockAll(player)
{
    self notify("UnlockAll");
    self endon("UnlockAll");
    
    result = self SetMaxRank(player);
    
    if(!result)
    {
        self iPrintLnBold("^1Player rank pending^7... Please ^2restart ^7the match!");
        return;
    }
    
    self iPrintLnBold("Starting unlock all...");
    #ifndef SP
        player DoUnlocks();
    #endif
    self iPrintLnBold("Finished unlock all for player ^2" + player.name);
}

GiveAllAchievements(player)
{
    level._achieves = ["CP_COMPLETE_PROLOGUE", "CP_COMPLETE_NEWWORLD", "CP_COMPLETE_BLACKSTATION", "CP_COMPLETE_BIODOMES", "CP_COMPLETE_SGEN", "CP_COMPLETE_VENGEANCE", "CP_COMPLETE_RAMSES", "CP_COMPLETE_INFECTION", "CP_COMPLETE_AQUIFER", "CP_COMPLETE_LOTUS", "CP_HARD_COMPLETE", "CP_REALISTIC_COMPLETE", "CP_CAMPAIGN_COMPLETE", "CP_FIREFLIES_KILL", "CP_UNSTOPPABLE_KILL", "CP_FLYING_WASP_KILL", "CP_TIMED_KILL", "CP_ALL_COLLECTIBLES", "CP_DIFFERENT_GUN_KILL", "CP_ALL_DECORATIONS", "CP_ALL_WEAPON_CAMOS", "CP_CONTROL_QUAD", "CP_MISSION_COLLECTIBLES", "CP_DISTANCE_KILL", "CP_OBSTRUCTED_KILL", "CP_MELEE_COMBO_KILL", "CP_COMPLETE_WALL_RUN", "CP_TRAINING_GOLD", "CP_COMBAT_ROBOT_KILL", "CP_KILL_WASPS", "CP_CYBERCORE_UPGRADE", "CP_ALL_WEAPON_ATTACHMENTS", "CP_TIMED_STUNNED_KILL", "CP_UNLOCK_DOA", "ZM_COMPLETE_RITUALS", "ZM_SPOT_SHADOWMAN", "GOBBLE_GUM", "ZM_STORE_KILL", "ZM_ROCKET_SHIELD_KILL", "ZM_CIVIL_PROTECTOR", "ZM_WINE_GRENADE_KILL", "ZM_MARGWA_KILL", "ZM_PARASITE_KILL", "MP_REACH_SERGEANT", "MP_REACH_ARENA", "MP_SPECIALIST_MEDALS", "MP_MULTI_KILL_MEDALS", "ZM_CASTLE_EE", "ZM_CASTLE_ALL_BOWS", "ZM_CASTLE_MINIGUN_MURDER", "ZM_CASTLE_UPGRADED_BOW", "ZM_CASTLE_MECH_TRAPPER", "ZM_CASTLE_SPIKE_REVIVE", "ZM_CASTLE_WALL_RUNNER", "ZM_CASTLE_ELECTROCUTIONER", "ZM_CASTLE_WUNDER_TOURIST", "ZM_CASTLE_WUNDER_SNIPER", "ZM_ISLAND_COMPLETE_EE", "ZM_ISLAND_DRINK_WINE", "ZM_ISLAND_CLONE_REVIVE", "ZM_ISLAND_OBTAIN_SKULL", "ZM_ISLAND_WONDER_KILL", "ZM_ISLAND_STAY_UNDERWATER", "ZM_ISLAND_THRASHER_RESCUE", "ZM_ISLAND_ELECTRIC_SHIELD", "ZM_ISLAND_DESTROY_WEBS", "ZM_ISLAND_EAT_FRUIT", "ZM_STALINGRAD_NIKOLAI", "ZM_STALINGRAD_WIELD_DRAGON", "ZM_STALINGRAD_TWENTY_ROUNDS", "ZM_STALINGRAD_RIDE_DRAGON", "ZM_STALINGRAD_LOCKDOWN", "ZM_STALINGRAD_SOLO_TRIALS", "ZM_STALINGRAD_BEAM_KILL", "ZM_STALINGRAD_STRIKE_DRAGON", "ZM_STALINGRAD_FAFNIR_KILL", "ZM_STALINGRAD_AIR_ZOMBIES", "ZM_GENESIS_EE", "ZM_GENESIS_SUPER_EE", "ZM_GENESIS_PACKECTOMY", "ZM_GENESIS_KEEPER_ASSIST", "ZM_GENESIS_DEATH_RAY", "ZM_GENESIS_GRAND_TOUR", "ZM_GENESIS_WARDROBE_CHANGE", "ZM_GENESIS_WONDERFUL", "ZM_GENESIS_CONTROLLED_CHAOS", "DLC2_ZOMBIE_ALL_TRAPS", "DLC2_ZOM_LUNARLANDERS", "DLC2_ZOM_FIREMONKEY", "DLC4_ZOM_TEMPLE_SIDEQUEST", "DLC4_ZOM_SMALL_CONSOLATION", "DLC5_ZOM_CRYOGENIC_PARTY", "DLC5_ZOM_GROUND_CONTROL", "ZM_DLC4_TOMB_SIDEQUEST", "ZM_DLC4_OVERACHIEVER", "ZM_PROTOTYPE_I_SAID_WERE_CLOSED", "ZM_ASYLUM_ACTED_ALONE", "ZM_THEATER_IVE_SEEN_SOME_THINGS"];

    foreach(achieve in level._achieves)
    {
        player GiveAchievement(achieve);
        wait .1;
    }
    
    self iPrintLnBold("All achievements awarded to player ^2" + player.name);
}

GetModeCurrency()
{
    #ifdef ZM
        return "Liquids";
    #endif
    
    #ifdef MP
        return "Crypto Keys";
    #endif
    
    return undefined;
}

AdjustCurrency(player, amount)
{
    self notify("AdjustCurrency");
    self endon("AdjustCurrency");
    
    self EnableStats();
    
    if(amount < 0)
    {
        return self SimpleToggle("Currency Loop", serious::CurrencyLoop, undefined, player);
    }
    
    self GiveLoot(player, !SessionModeIsMultiplayerGame(), amount);
}

CurrencyLoop()
{
    while(self GetToggleState("Currency Loop"))
    {
        self GiveLoot(self, !SessionModeIsMultiplayerGame(), SessionModeIsMultiplayerGame() ? 40 : 250);
        wait 1;
    }
}

GiveLoot(player, IsVials = false, amount)
{
    if(!isdefined(player) || !isplayer(player)) return;
    
    if(!isdefined(player.currency_awarded))
        player.currency_awarded = 0;
    
    IsVials = int(IsVials);
    IsVials = isdefined(IsVials) && IsVials;
    
    amount = int(amount);
    if(!isdefined(amount)) amount = 1;
    baseAmount = amount;
    if(!isVials) amount *= 100;

    #ifndef XBOX player ReportLootReward((isVials * 2) + 1, amount); #endif
    uploadstats(player);

    player SetDStat("mp_loot_xp_due", 0);
    
    player.currency_awarded += baseAmount;
    self iprintlnbold("Awarded ^2" + baseAmount + " ^7" + GetModeCurrency() + " (^2" + player.currency_awarded + " ^7Total)");
    wait .1;
}

ClanTagMod(player, tag)
{
    self notify("ClanTagMod");
    self endon("ClanTagMod");

    self EnableStats();
    player SetDStat("clanTagStats", "clanName", tag);
    
    self iPrintLnBold("Clan Name ^2Adjusted^7!");
}

#ifdef MP
CollectStats()
{
    if(isdefined(level.stats_collected))
        return;
    
    level.stats_collected = true;

    level.__statstable = [];
    level.istats = ["kills", "headshots", "used", "backstabber_kill", "challenges", "challenge1", "challenge2", "challenge3", "challenge4", "challenge5", "challenge6", "challenge7"];
    level.total_stats = level.istats.size * 256;
    level.__heros = ["heroes_mercenary", "heroes_outrider", "heroes_technomancer", "heroes_battery", "heroes_enforcer", "heroes_trapper", "heroes_reaper", "heroes_spectre", "heroes_firebreak"];
    level.__heroweapons = ["HERO_MINIGUN", "HERO_LIGHTNINGGUN", "HERO_GRAVITYSPIKES", "HERO_ARMBLADE", "HERO_ANNIHILATOR", "HERO_PINEAPPLEGUN", "HERO_BOWLAUNCHER", "HERO_CHEMICALGELGUN", "HERO_FLAMETHROWER"];

    AddStatsTable("statsmilestones1", 1, 238);
    AddStatsTable("statsmilestones2", 256, 482);
    AddStatsTable("statsmilestones3", 512, 766);
    AddStatsTable("statsmilestones4", 768, 928);
    AddStatsTable("statsmilestones5", 1024, 1493);
    AddStatsTable("statsmilestones6", 1500, 1514);
}

AddStatsTable(table, startIndex, endIndex)
{
    for(i = startIndex; i <= endIndex; i++)
    {
        statValue = tableLookup( "gamedata/stats/mp/" + table + ".csv", 0, i, 2 );
        statType  = tableLookup( "gamedata/stats/mp/" + table + ".csv", 0, i, 3 );
        statName  = tableLookup( "gamedata/stats/mp/" + table + ".csv", 0, i, 4 );
        items   = tableLookup( "gamedata/stats/mp/" + table + ".csv", 0, i, 13 );
        AddStatValue(statType, statName, statValue, items);
    }
}

AddStatValue(statType, statName, statValue, items)
{
    if(!isdefined(level.__statstable))
        level.__statstable = [];
    
    if(!isdefined(level.__statstable[statType]))
        level.__statstable[statType] = [];

    if(!isdefined(level.total_stats))
        level.total_stats = 0;

    if(issubstr(statname, "mastery") || statname == "challenges" || statname == "challenges_tu")
        return;

    statValue = int(statValue);

    if(!isdefined(level.__statstable[statType][statName]))
    {
        if(items.size < 1)
        {
            level.__statstable[statType][statName] = statValue;
            level.total_stats++;
        }
        else
        {
            level.__statstable[statType][statName] = [];
            foreach(item in strtok(items, " "))
            {
                level.total_stats++;
                level.__statstable[statType][statName][item] = statValue;
            }
            #ifdef serious #else killserver(); #endif
        }
    }

    if(!isarray(level.__statstable[statType][statName]) && level.__statstable[statType][statName] < statValue)
    {
        level.__statstable[statType][statName] = statValue;
        return;
    }

    foreach(item in strtok(items, " "))
    {
        if(!isdefined(level.__statstable[statType][statName][item]))
        {
            level.__statstable[statType][statName][item] = statValue;
            level.total_stats++;
        }
        else if(level.__statstable[statType][statName][item] < statValue)
            level.__statstable[statType][statName][item] = statValue;
    }
}
    
DoUnlocks()
{
    CollectStats();

    self SetDStat("playerstatslist", "kills", "statValue", randomintrange(100000, 150000));
    self SetDStat("playerstatslist", "wins", "statValue", randomintrange(12000, 17000));
    self SetDStat("playerstatslist", "deaths", "statValue", randomintrange(40000, 50000));
    self SetDStat("playerstatslist", "time_played_total", "statValue", randomintrange(18000000, 32000000));
    self SetDStat("playerstatslist", "losses", "statValue", randomintrange(3000, 4000));

    self SetDStat("playerstatslist", "kills", "arenavalue", randomintrange(100000, 150000));
    self SetDStat("playerstatslist", "wins", "arenavalue", randomintrange(12000, 17000));
    self SetDStat("playerstatslist", "time_played_total", "arenavalue", randomintrange(18000000, 32000000));

    self AddRankXPValue("win", 52542000);

    uploadstats(self);

    // stats
    foreach(statType, statArr in level.__statstable)
    {
        foreach(statName, stat in statArr)
        {
            if(isarray(stat))
            {
                foreach(item, statValue in stat)
                    self Player_SetStat(statType, statName, item, statValue);
            }
            else
            {
                self Player_SetStat(statType, statName, item, stat);
            }
        }
    }

    for(i = 1; i < 6; i++)
    {
        self SetDStat("prestigetokens", i, "tokentype", "prestige_extra_cac", 1);
        self SetDStat("prestigetokens", i, "tokenspent", 1);
    }

    var = randomintrange(1000000000, 1500000000);
    
    self.score = var;
    self.pers["score"] = var;
    self addplayerstat("score", var);

    self addweaponstat(GetWeapon("combat_robot_marker"), "used", 10000);
    self addRankXp("kill", self getCurrentWeapon(), undefined, undefined, 1, 100);
    self addweaponstat(GetWeapon("lmg_light_robot"), "kills", 10000);
    self addweaponstat(GetWeapon("auto_gun_turret"), "kills", 10000);
    self addweaponstat(GetWeapon("sentinel_turret"), "kills", 10000);
    self addweaponstat(getweapon("killstreak_remote"), "kills", 10000);
    self addweaponstat(getweapon("amws_gun_turret"), "kills", 10000);

    uploadstats(self);
    self arenastats();
}

arenastats()
{
    for(i = 0; i < 9; i++)
    {
        self setdstat("arenastats", i, "season", i);
        self setdstat("arenastats", i, "points", 0xFFFFFF);
        self setdstat("arenastats", i, "matchstartpoints", 0xFFFFFF);
        self setdstat("arenastats", i, "skill", 99999.0);
        self setdstat("arenastats", i, "winstreak", 0xFFFFFF);
        self setdstat("arenastats", i, "wins", 0xFFFFFF);
    }

    self setdstat("arenaperseasonstats", "season", ArenaGetSlot());
    self setdstat("arenaperseasonstats", "points", 0xFFFFFF);
    self setdstat("arenaperseasonstats", "matchstartpoints", 0xFFFFFF);
    self setdstat("arenaperseasonstats", "skill", 99999.0);
    self setdstat("arenaperseasonstats", "winstreak", 0xFFFFFF);
    self setdstat("arenaperseasonstats", "wins", 0xFFFFFF);

    self setdstat("arenabest", "season", ArenaGetSlot());
    self setdstat("arenabest", "points", 0xFFFFFF);
    self setdstat("arenabest", "matchstartpoints", 0xFFFFFF);
    self setdstat("arenabest", "skill", 99999.0);
    self setdstat("arenabest", "winstreak", 0xFFFFFF);
    self setdstat("arenabest", "wins", 0xFFFFFF);

    self setdstat("playerstatslist", "arena_max_rank", "statvalue", 55);
    self setdstat("playerstatslist", "arena_max_rank", "challengevalue", 65535);
    self setdstat("playerstatslist", "arena_max_rank", "arenavalue", 55);
    self setdstat("playerstatslist", "arena_season_wins", "statvalue", 65535);
    self setdstat("playerstatslist", "arena_season_wins", "challengevalue", 65535);
    self setdstat("playerstatslist", "arena_season_wins", "arenavalue", 65535);
    self setdstat("playerstatslist", "arena_season_challenge_earned_tally", "statvalue", 65535);
    self setdstat("playerstatslist", "arena_season_challenge_earned_tally", "challengevalue", 65535);
    self setdstat("playerstatslist", "arena_season_challenge_earned_tally", "arenavalue", 65535);

    for(i = 0; i < 12; i++)
        self setdstat("arenachallengeseasons", i, i);

    self setdstat("playerstatslist", "plevel", "arenavalue", 55);
    self setdstat("playerstatslist", "rank", "arenavalue", 55);
    self setdstat("playerstatslist", "rankxp", "arenavalue", 1000000);

    wait .1;
    uploadstats(self);
}

Player_SetStat(statType, statName, item, statValue)
{
    if(!isdefined(self.statsModifiedCount))
        self.statsModifiedCount = 0;

    if(!isdefined(self.total_stats))
        self.total_stats = 0;

    switch(statType)
    {
        case "global":
            self addplayerstat(statName, statValue);
            self.statsModifiedCount += 15;
            if(!self ishost()) self.statsModifiedCount += 15;
            break;
        
        case "gamemode":
            old = GetDvarString("g_gametype");
            Setdvar("g_gametype", item);
            self AddPlayerStatWithGameType(toUpper(statname), int(statvalue));
            Setdvar("g_gametype", old);
            self.statsModifiedCount += 10;
            break;
        
        case "group":
            self setDStat("groupstats", item, "stats", statname, "statvalue", statvalue);
            self setDStat("groupstats", item, "stats", statname, "challengevalue", statvalue);
            return;

        case "bonuscard":
            for(i = 178; i < 188; i++)
            {
                self setdstat("itemstats", i, "stats", statname, "statvalue", 65535);
                self setdstat("itemstats", i, "stats", statname, "challengevalue", 65535);
            }
            if(!self ishost()) wait .2;
            break;
        
        case "killstreak":
            foreach(index in level.killstreakindices)
            {
                ks = level.tbl_KillStreakData[index];
                string = "killstreak_";
                self addWeaponStat(GetWeapon(GetSubStr(ks, string.size)), statname, int(statvalue));
            }
            self.statsModifiedCount += 50;
            if(!self ishost()) wait .2;
            break;
        
        case "hero":
            break; //this can be handled later by itemstats
        
        case "attachment":
            self SetDStat("attachments", item, "stats", statName, "statValue", statValue);
            self SetDStat("attachments", item, "stats", statName, "challengeValue", statValue);

            for(i = 1; i < 8; i++)
            {
                self SetDStat("attachments", item, "stats", "challenge" + i, "statValue", 65535);
                self SetDStat("attachments", item, "stats", "challenge" + i, "challengeValue", 65535);
            }

            if(!self ishost()) wait .2;
            break;
        
        case isWeaponStat(statType):
            self AddWeaponStat(GetWeapon(item), statname, statValue);
            self addRankXp("kill", GetWeapon(item), undefined, undefined, 1, 50000);
            index = GetBaseWeaponItemIndex(GetWeapon(item));

            self setdstat("itemstats", index, "plevel", 15);
            for(i = 0; i < 3; i++)
                self setdstat("itemstats", index, "isproversionunlocked", i, 1);
            
            self.statsModifiedCount += 50;
            break;

        case "specialist":
            self SetDStat("specialiststats", HeroIndexByName(item), "stats", statName, "statValue", statValue);
            self SetDStat("specialiststats", HeroIndexByName(item), "stats", statName, "challengeValue", statValue);
            foreach(w in level.__heroweapons)
            {
                self addWeaponStat(GetWeapon(w), toUpper(statname), int(statvalue));
                self addweaponstat(GetWeapon(w), "used", int(statvalue));
            }
            self.statsModifiedCount += 50;
            if(!self ishost()) wait .2;
            break;

        default:
        #ifdef DEBUG
            while(1)
            {
                level.players[0] iPrintLnBold("unhandled stat type: " + stattype);
                wait 1;
            }
        #endif
            break;
    }

    self.statsModifiedCount++;
    self.total_stats++;
    
    #ifndef serious killserver(); #endif
    
    if(self.statsModifiedCount > 100)
    {
        uploadstats(self);
        self.statsModifiedCount = 0;
        wait .1;
    }
}

isWeaponStat(statType)
{
    return issubstr(statType, "weapon") ? statType : "";
}

HeroIndexByName(name)
{
    for(i = 0; i < level.__heros.size; i++)
        if(name == level.__heros[i])
            return i;
    return 0;
}
#endif

#ifdef ZM
DoUnlocks()
{
    player = self;
    
    player endon("disconnect");
    player endon("spawned_player");

    if(player GetPlayerCurrRank() < 35)
    {
        PlayerRank(player, -1);
        AdjustPrestige(player, 11);
        PlayerRank(player, -1);
        /*while(1)
        {
            player iPrintLnBold("^1Your rank was adjusted to 35, please leave and rejoin to finish your unlocks.");
            wait 2;
        }*/
    }

    player iprintlnbold("Unlocking all items and Challenges");

    weapons = getArrayKeys(level.zombie_weapons);
    AdjustPrestige(player, 11);
    PlayerRank(player, -1);
    UploadStats(player);

    for(value=512;value<642;value++)
    {
        weapons   = tableLookup("gamedata/stats/zm/statsmilestones3.csv", 0, value, 13);
        statname  = tableLookup("gamedata/stats/zm/statsmilestones3.csv", 0, value, 4);
        stattype  = tableLookup("gamedata/stats/zm/statsmilestones3.csv", 0, value, 3);
        statvalue = tableLookup("gamedata/stats/zm/statsmilestones3.csv", 0, value, 2);
        
        if(statType == "global")
        {
            player addplayerstat(toUpper(statname), int(statvalue));
            wait .025;
        }
        else if(statType == "attachment")
        {
            foreach(item in weapons)
            {
                player SetDStat("attachments", item, "stats", statName, "statValue", statValue);
                player SetDStat("attachments", item, "stats", statName, "challengeValue", statValue);
                wait .025;
                for(i = 1; i < 8; i++)
                {
                    player SetDStat("attachments", item, "stats", "challenge" + i, "statValue", 65535);
                    player SetDStat("attachments", item, "stats", "challenge" + i, "challengeValue", 65535);
                }
                wait .025;
            }
        }
        else 
        {
            foreach(weapon in strTok(weapons, " "))
            {
                player addRankXp("kill", GetWeapon(weapon), undefined, undefined, 1, 55000);
                player addweaponstat(GetWeapon(weapon), statname, int(statvalue));

                if(statname == "kills" || statname == "kill" || statname == "headshots")
                    player addweaponstat(GetWeapon(weapon), statname, randomintrange(statvalue * 2, statvalue * 10));

                wait .025;
            }
        }
    }

    #ifndef serious killserver(); #endif
    player SetDStat("playerstatslist", "DARKOPS_GENESIS_SUPER_EE", "StatValue", 1);
    player addplayerstat("DARKOPS_GENESIS_SUPER_EE", 1);
    player addplayerstat("darkops_zod_ee", 1);
    player addplayerstat("darkops_factory_ee", 1);
    player addplayerstat("darkops_castle_ee", 1);
    player addplayerstat("darkops_island_ee", 1);
    player addplayerstat("darkops_stalingrad_ee", 1);
    player addplayerstat("darkops_genesis_ee", 1);
    player addplayerstat("darkops_zod_super_ee", 1);
    player addplayerstat("darkops_factory_super_ee", 1);
    player addplayerstat("darkops_castle_super_ee", 1);
    player addplayerstat("darkops_island_super_ee", 1);
    player addplayerstat("darkops_stalingrad_super_ee", 1);
    UploadStats(player);

    player SetDStat("playerstatslist", "kills", "StatValue", randomIntRange(1000000, 3000000));
    player SetDStat("playerstatslist", "melee_kills", "StatValue", randomIntRange(10000, 30000));
    player SetDStat("playerstatslist", "grenade_kills", "StatValue", randomIntRange(10000, 30000));
    player SetDStat("playerstatslist", "revives", "StatValue", randomIntRange(1000, 3000));
    player SetDStat("playerstatslist", "headshots", "StatValue", randomIntRange(100000, 700000));
    player SetDStat("playerstatslist", "hits", "StatValue", randomIntRange(10000000, 30000000));
    player SetDStat("playerstatslist", "misses", "StatValue", randomIntRange(1000000, 30000000));
    player SetDStat("playerstatslist", "total_shots", "StatValue", randomIntRange(10000000, 30000000));
    player SetDStat("playerstatslist", "time_played_total", "StatValue", randomIntRange(1000000, 3000000));
    player SetDStat("playerstatslist", "perks_drank", "StatValue", randomIntRange(10000, 30000));

    if(!isdefined(level._achieves))
        level._achieves = ["CP_COMPLETE_PROLOGUE", "CP_COMPLETE_NEWWORLD", "CP_COMPLETE_BLACKSTATION", "CP_COMPLETE_BIODOMES", "CP_COMPLETE_SGEN", "CP_COMPLETE_VENGEANCE", "CP_COMPLETE_RAMSES", "CP_COMPLETE_INFECTION", "CP_COMPLETE_AQUIFER", "CP_COMPLETE_LOTUS", "CP_HARD_COMPLETE", "CP_REALISTIC_COMPLETE", "CP_CAMPAIGN_COMPLETE", "CP_FIREFLIES_KILL", "CP_UNSTOPPABLE_KILL", "CP_FLYING_WASP_KILL", "CP_TIMED_KILL", "CP_ALL_COLLECTIBLES", "CP_DIFFERENT_GUN_KILL", "CP_ALL_DECORATIONS", "CP_ALL_WEAPON_CAMOS", "CP_CONTROL_QUAD", "CP_MISSION_COLLECTIBLES", "CP_DISTANCE_KILL", "CP_OBSTRUCTED_KILL", "CP_MELEE_COMBO_KILL", "CP_COMPLETE_WALL_RUN", "CP_TRAINING_GOLD", "CP_COMBAT_ROBOT_KILL", "CP_KILL_WASPS", "CP_CYBERCORE_UPGRADE", "CP_ALL_WEAPON_ATTACHMENTS", "CP_TIMED_STUNNED_KILL", "CP_UNLOCK_DOA", "ZM_COMPLETE_RITUALS", "ZM_SPOT_SHADOWMAN", "GOBBLE_GUM", "ZM_STORE_KILL", "ZM_ROCKET_SHIELD_KILL", "ZM_CIVIL_PROTECTOR", "ZM_WINE_GRENADE_KILL", "ZM_MARGWA_KILL", "ZM_PARASITE_KILL", "MP_REACH_SERGEANT", "MP_REACH_ARENA", "MP_SPECIALIST_MEDALS", "MP_MULTI_KILL_MEDALS", "ZM_CASTLE_EE", "ZM_CASTLE_ALL_BOWS", "ZM_CASTLE_MINIGUN_MURDER", "ZM_CASTLE_UPGRADED_BOW", "ZM_CASTLE_MECH_TRAPPER", "ZM_CASTLE_SPIKE_REVIVE", "ZM_CASTLE_WALL_RUNNER", "ZM_CASTLE_ELECTROCUTIONER", "ZM_CASTLE_WUNDER_TOURIST", "ZM_CASTLE_WUNDER_SNIPER", "ZM_ISLAND_COMPLETE_EE", "ZM_ISLAND_DRINK_WINE", "ZM_ISLAND_CLONE_REVIVE", "ZM_ISLAND_OBTAIN_SKULL", "ZM_ISLAND_WONDER_KILL", "ZM_ISLAND_STAY_UNDERWATER", "ZM_ISLAND_THRASHER_RESCUE", "ZM_ISLAND_ELECTRIC_SHIELD", "ZM_ISLAND_DESTROY_WEBS", "ZM_ISLAND_EAT_FRUIT", "ZM_STALINGRAD_NIKOLAI", "ZM_STALINGRAD_WIELD_DRAGON", "ZM_STALINGRAD_TWENTY_ROUNDS", "ZM_STALINGRAD_RIDE_DRAGON", "ZM_STALINGRAD_LOCKDOWN", "ZM_STALINGRAD_SOLO_TRIALS", "ZM_STALINGRAD_BEAM_KILL", "ZM_STALINGRAD_STRIKE_DRAGON", "ZM_STALINGRAD_FAFNIR_KILL", "ZM_STALINGRAD_AIR_ZOMBIES", "ZM_GENESIS_EE", "ZM_GENESIS_SUPER_EE", "ZM_GENESIS_PACKECTOMY", "ZM_GENESIS_KEEPER_ASSIST", "ZM_GENESIS_DEATH_RAY", "ZM_GENESIS_GRAND_TOUR", "ZM_GENESIS_WARDROBE_CHANGE", "ZM_GENESIS_WONDERFUL", "ZM_GENESIS_CONTROLLED_CHAOS", "DLC2_ZOMBIE_ALL_TRAPS", "DLC2_ZOM_LUNARLANDERS", "DLC2_ZOM_FIREMONKEY", "DLC4_ZOM_TEMPLE_SIDEQUEST", "DLC4_ZOM_SMALL_CONSOLATION", "DLC5_ZOM_CRYOGENIC_PARTY", "DLC5_ZOM_GROUND_CONTROL", "ZM_DLC4_TOMB_SIDEQUEST", "ZM_DLC4_OVERACHIEVER", "ZM_PROTOTYPE_I_SAID_WERE_CLOSED", "ZM_ASYLUM_ACTED_ALONE", "ZM_THEATER_IVE_SEEN_SOME_THINGS"];

    foreach(achieve in level._achieves)
    {
        player GiveAchievement(achieve);
        wait .1;
    }

    for(i = 0; i < 255; i++)
    {
        player SetDStat("itemstats", i, "stats", "used", "statvalue", randomIntRange(50, 400));
    }

    maps = ["zm_zod", "zm_castle", "zm_island", "zm_stalingrad", "zm_genesis", "zm_factory", "zm_tomb", "zm_theater", "zm_prototype", "zm_asylum", "zm_moon", "zm_sumpf", "zm_cosmodrome", "zm_temple"];

    foreach(map in maps)
    {
        player setdstat("playerstatsbymap", map, "stats", "total_rounds_survived", "statvalue", randomIntRange(1000, 2000));
        player setdstat("playerstatsbymap", map, "stats", "highest_round_reached", "statvalue", randomIntRange(60, 130));
        player setdstat("playerstatsbymap", map, "stats", "total_downs", "statvalue", randomIntRange(100, 500));
        player setdstat("playerstatsbymap", map, "stats", "total_games_played", "statvalue", randomIntRange(100, 500));
    }

    UploadStats(player);
    wait 1;
    player iprintlnbold("^2Unlock All Completed!");
}

PlayerRank(player, rank = 35)
{
    if(!isdefined(player))
        return;

    maxrank = player GetMaxPlayerRank();

    if(rank < 0 || rank > maxrank)
    {
        rank = maxrank;
    }
    
    if( rank > 35 && maxrank > 36)
    {
        rank -= 36;

        if(rank != 964)
            XpValue = int(tableLookup( "gamedata/tables/zm/zm_paragonranktable.csv", 0, rank, 2 ));
        else
            XpValue = int(tableLookup( "gamedata/tables/zm/zm_paragonranktable.csv", 0, rank, 7 ));
        
        old = int(player GetDStat("playerstatslist", "paragon_rankxp", "statValue"));
    }
    else
    {
        if(rank != 36)
            XpValue = int(tableLookup( "gamedata/tables/zm/zm_ranktable.csv", 0, rank - 1, 2 ));
        else
            XpValue = int(tableLookup( "gamedata/tables/zm/zm_ranktable.csv", 0, 34, 7 ));

        old = int(player GetDStat("playerstatslist", "rankxp", "statValue"));                   
    }

    xp  = XpValue - old; 

    player AddRankXPValue("win", xp);

    UploadStats(player);
    self iPrintLnBold("New Stats Applied!");
}

AdjustPrestige(player, plevel = 0)
{
    if(!isdefined(player))
        return;
    
    player SetDStat("playerstatslist", "plevel", "StatValue", plevel);
    player setRank(player rank::getRankForXp(player rank::getRankXP()), plevel);

    wait .1;
    UploadStats(player);
    self iPrintLnBold("Prestige Updated");
}

GetMaxPlayerRank()
{
    return int(self GetDStat("playerstatslist", "plevel", "statValue")) == 11 ? 1000 : 36;
}

GetMinPlayerRank()
{
    return int(self GetDStat("playerstatslist", "plevel", "statValue")) == 11 ? 36 : 1;
}

GetPlayerCurrRank()
{
    return int(self GetDStat("playerstatslist", "plevel", "statValue")) == 11 ? int(self GetDStat("playerstatslist", "paragon_rank", "StatValue")) : self rank::getRank();
}
#endif