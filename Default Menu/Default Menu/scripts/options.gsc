precacheoptions()
{
    AddMenu("none", "main", "Default Menu");
    
    if(!isdefined(self.submenu) || self.submenu == "none")
        return;
        
    menuId = self GetMenu().i;
    switch(menuId)
    {
        case "main":
            AddSubmenu(menuId, "personal", "Personal Options");
            AddSubmenu(menuId, "weapons", "Weaponry");
            AddSubmenu(menuId, "fun", "Fun Options", undefined, 2);
            #ifndef MP AddSubmenu(menuId, "teleport", "Teleport", undefined, 2); #endif
            #ifdef ZM AddSubmenu(menuId, "zombies", "Zombie Options", undefined, 2); #endif
            AddSubmenu(menuId, "misc", "Misc Options", undefined, 2);
            AddSubmenu(menuId, "lobby", "Lobby Options", undefined, 4);
            AddSubmenu(menuId, "clients", "Client Options", 3);
            break;
        case "personal":
            AddOption(menuId, "Godmode", serious::SimpleToggle, "Godmode", sys::EnableInvulnerability, sys::DisableInvulnerability);
            AddOption(menuId, "Infinite Ammo", serious::SimpleToggle, "Infinite Ammo", serious::InfiniteAmmo);
            AddOption(menuId, "No Clip", serious::SimpleToggle, "No Clip", serious::ANoclipBind);
            AddOption(menuId, "Infinite Specialist", serious::SimpleToggle, "Infinite Specialist", serious::InfiniteHeroPower);
            #ifdef MP AddOption(menuId, "Infinite Scorestreaks", serious::SimpleToggle, "Infinite Scorestreaks", serious::ScoreStreaksForever);#endif
            AddOption(menuId, "Unfair Aimbot", serious::SimpleToggle, "Unfair Aimbot", serious::UnfairAimbotToggle);
            AddOption(menuId, "Grenade Aimbot", serious::SimpleToggle, "Grenade Aimbot", serious::GrenadeAimbot);
            AddOption(menuId, "Projectile Aimbot", serious::SimpleToggle, "Projectile Aimbot", serious::ProjectileAimbot);
            AddOption(menuId, "Super Speed", serious::SimpleToggle, "Super Speed", serious::SpeedToggle, serious::SpeedToggle);
            AddOption(menuId, "Third Person", serious::SimpleToggle, "Third Person", serious::ThirdPersonToggle, serious::ThirdPersonToggle);
            AddOption(menuId, "Invisibility", serious::SimpleToggle, "Invisibility", serious::InvisibilityToggle, serious::InvisibilityToggle);
            AddOption(menuId, "Magic Perks", serious::SimpleToggle, "Magic Perks", serious::AllMagicPerksToggle, serious::AllMagicPerksToggle);
            #ifdef ZM AddOption(menuId, "Zombie Perks", serious::SimpleToggle, "Zombie Perks", serious::ZombiePerksToggle, serious::ZombiePerksToggle); #endif
            #ifdef ZM AddSubmenu(menuId, "give_points", "Adjust Points"); #endif
            #ifdef ZM AddOption(menuId, "Perks While Downed", serious::SimpleToggle, "Perks While Downed", serious::PerksDownedToggle, serious::PerksDownedToggle); #endif
            break;
            
         case "give_points":
            #ifdef ZM
                AddOption(menuId, "Give 100 Points", serious::AdjustPoints, 100);
                AddOption(menuId, "Give 1000 Points", serious::AdjustPoints, 1000);
                AddOption(menuId, "Give 10000 Points", serious::AdjustPoints, 10000);
                AddOption(menuId, "Give 100000 Points", serious::AdjustPoints, 100000);
                AddOption(menuId, "Give 1000000 Points", serious::AdjustPoints, 1000000);
                AddOption(menuId, "Give Max Points", serious::AdjustPoints, 10000000);
                AddOption(menuId, "Take Max Points", serious::AdjustPoints, -10000000);
                AddOption(menuId, "Take 1000000 Points", serious::AdjustPoints, -1000000);
                AddOption(menuId, "Take 100000 Points", serious::AdjustPoints, -100000);
                AddOption(menuId, "Take 10000 Points", serious::AdjustPoints, -10000);
                AddOption(menuId, "Take 1000 Points", serious::AdjustPoints, -1000);
                AddOption(menuId, "Take 100 Points", serious::AdjustPoints, -100);
            #endif
            break;
         case "weapons":
            AddSubmenu(menuId, "give_weapons", "Give Weapon");
            #ifdef ZM AddSubmenu(menuId, "give_weapons_upgraded", "Give Upgraded Weapon"); #endif
            AddSubmenu(menuId, "give_camo", "Give Camo");
            #ifdef ZM AddSubmenu(menuId, "give_gums", "Give Gobblegum"); #endif
            #ifdef ZM AddSubmenu(menuId, "give_aat", "Give AAT"); #endif
            AddOption(menuId, "Shotgun Gun", serious::SimpleToggle, "Shotgun Gun", serious::ShotgunGunToggle, serious::ShotgunGunToggle);
            AddOption(menuId, "Rocket Bullets", serious::SimpleToggle, "Rocket Bullets", serious::RocketBulletsToggle, serious::RocketBulletsToggle);
            AddOption(menuId, "Cluster Grenades", serious::SimpleToggle, "Cluster Grenades", serious::ClusterGrenades);
            AddOption(menuId, "Drop Current Weapon", serious::DropWeaponWrapper);
            AddOption(menuId, "Drop All Your Weapons", serious::DropAllWeps);
            AddOption(menuId, "Drop All The Weapons", serious::DropAllTheWeps);
         case "give_weapons":
            foreach(weapon in EnumerateWeapons("weapon"))
            {
                if(issubstr(weapon.name, "upgraded"))
                    continue;
                
                name = isdefined(weapon.displayname) ? MakeLocalizedString(weapon.displayname) : "";
                if(name == "") name = weapon.name;
                
                AddOption(menuId, name, serious::award_weapon, weapon);
            }
            break;
        case "give_weapons_upgraded":
            foreach(weapon in EnumerateWeapons("weapon"))
            {
                if(!issubstr(weapon.name, "upgraded"))
                    continue;
                    
                name = isdefined(weapon.displayname) ? MakeLocalizedString(weapon.displayname) : "";
                if(name == "") name = weapon.name;
                AddOption(menuId, name, serious::award_weapon, weapon);
            }
            break;
        case "give_camo":
            foreach(k, v in level._camos_)
            {
                AddOption(menuId, k, serious::GiveCamo, v);
            }
            break;
        case "give_aat":
            #ifdef ZM
                foreach(aat in getArrayKeys(level.AAT))
                    AddOption(menuId, "Give " + MakeLocalizedString(aat), serious::AwardAAT, aat);
            #endif
        break;
        case "fun":
            #ifdef ZM 
            AddOption(menuId, "Infinite Powerup Duration", serious::SimpleToggle, "Infinite Powerup Duration", serious::InfinitePowerupToggle, serious::InfinitePowerupToggle);
            AddOption(menuId, "Shoot Powerups", serious::SimpleToggle, "Shoot Powerups", serious::PowerupBullets);  
            AddOption(menuId, "Summon Perk Machines", serious::SummonPerks);
            AddOption(menuId, "Rain Powerups", serious::SimpleToggle, "Rain Powerups", serious::RainPowerups);  
            #endif
            AddOption(menuId, "Forge Tool", serious::SimpleToggle, "Forge Tool", serious::ForgeTool);
            AddOption(menuId, "Teleport Gun", serious::SimpleToggle, "Teleport Gun", serious::TeleportGun);
            AddOption(menuId, "Save Location", serious::SaveLoad, false);
            AddOption(menuId, "Load Location", serious::SaveLoad, true);
            break;
        case "give_gums":
            foreach(bgb in getArrayKeys(level.bgb))
            {
                #ifdef ZM AddOption(menuId, MakeLocalizedString(bgb), serious::award_bgb, bgb); #endif
            }
            break;
        case "clients":
            for(i = 0; i < 18; i++)
            {
                player = level.players[i];
                if(isdefined(player))
                    AddSubmenu(menuId, mToClient("client", i), player.name, player);
                else
                    ClearOption(menuId, i);
            }
            break;
        case "lobby":
            AddOption(menuId, "Add Bot", sys::AddTestClient);
            AddSubmenu(menuId, "play_music", "Play Music");
            #ifdef ZM 
                if(!bool(level._all_parts))
                    AddOption(menuId, "Collect All Parts", serious::CollectParts);
                if(!bool(level._doors_done))
                    AddOption(menuId, "Open All Doors", serious::OpenTheDoors);
                if(!bool(level._power_done))
                    AddOption(menuId, "Turn On Power", serious::AllPower);
                AddOption(menuId, "No Zombie Spawns", serious::SimpleToggle, "No Zombie Spawns", serious::ToggleZombSpawns, serious::ToggleZombSpawns);
                AddOption(menuId, "Auto-Revive Players", serious::SimpleToggle, "Auto-Revive Players", serious::ToggleAutoRes);
            #endif
            AddOption(menuId, "Freeze AI", serious::SimpleToggle, "Freeze AI", serious::AIToggle, serious::AIToggle);
            AddOption(menuId, "Anti-Quit", serious::SimpleToggle, "Anti-Quit", serious::ToggleAntiquit, serious::ToggleAntiquit);
            AddOption(menuId, "Trampoline Mode", serious::SimpleToggle, "Trampoline Mode", serious::ToggleBHop);
            //AddOption(menuId, "Rapid Fire", serious::SimpleToggle, "Rapid Fire", serious::ToggleRapidFire, serious::ToggleRapidFire);
            AddOption(menuId, "Low Gravity", serious::SimpleToggle, "Low Gravity", serious::ToggleLowGrav, serious::ToggleLowGrav);
            #ifdef ZM AddSubmenu(menuId, "set_rounds", "Adjust Round Number"); #endif
            #ifndef SP AddOption(menuId, "End Game", serious::EndTheGame); #endif
            #ifdef ZM AddOption(menuId, "Force Host", serious::SimpleToggle, "Force Host", serious::ForceHostToggle, serious::ForceHostToggle); #endif
            AddOption(menuId, "Fast Restart", sys::map_restart, 0);
            AddOption(menuId, "Exit Level", sys::ExitLevel, 0);
            break;
            
        case "misc":
        #ifdef ZM
            AddOption(menuId, "Show All Boxes", serious::AllBoxStates, true);
            AddOption(menuId, "Hide All Boxes", serious::AllBoxStates, false);
        #endif
            AddOption(menuId, "Clone Yourself", serious::Cloner, false);
            AddOption(menuId, "Clone, but Dead", serious::Cloner, true);
            break;
        #ifdef ZM
        case "set_rounds":
            AddOption(menuId, "Current Round + 1", serious::AdjustRounds, 1);
            AddOption(menuId, "Current Round + 5", serious::AdjustRounds, 5);
            AddOption(menuId, "Current Round + 10", serious::AdjustRounds, 10);
            AddOption(menuId, "Current Round + 100", serious::AdjustRounds, 100);
            AddOption(menuId, "Current Round + 1000", serious::AdjustRounds, 1000);
            AddOption(menuId, "Max Round Number", serious::AdjustRounds, 0x7FFFFFFF - level.round_number);
            AddOption(menuId, "Min Round Number", serious::AdjustRounds, 0xFFFFFFFF - level.round_number);
            AddOption(menuId, "Current Round - 1000", serious::AdjustRounds, -1000);
            AddOption(menuId, "Current Round - 100", serious::AdjustRounds, -100);
            AddOption(menuId, "Current Round - 10", serious::AdjustRounds, -10);
            AddOption(menuId, "Current Round - 5", serious::AdjustRounds, -5);
            AddOption(menuId, "Current Round - 1", serious::AdjustRounds, -1);
            break;
        #endif
        case "play_music":
            foreach(k, v in level._cmusic)
            {
                AddOption(menuId, MakeLocalizedString(v), serious::PlayLobbyMusic, k);
            }
            break;
        case "teleport":
            foreach(k, v in level._teleto)
            {
                AddOption(menuId, k, sys::SetOrigin, v.origin);
            }
            break;
            
        #ifdef ZM
        case "zombies":
            AddOption(menuId, "Spawn a Zombie", serious::SpawnZombieArray, self, 1);
            AddOption(menuId, "Spawn 20 Zombies", serious::SpawnZombieArray, self, 20);
            AddOption(menuId, "Kill All Zombies", serious::KillAllZombies);
            AddOption(menuId, "All to Crosshair", serious::AllZMToCrosshair);
            AddOption(menuId, "All to Me", serious::AllZMToMe);
            AddOption(menuId, "Stack Zombies", serious::ZStack);
            AddOption(menuId, "Control a Zombie", serious::SimpleToggle, "Control a Zombie", serious::ZControl);
            AddOption(menuId, "Zombie Super Melee", serious::ToggleZombieSuperMelee);
            AddOption(menuId, "Zombie Terrorists", serious::ToggleZMExploders);
            AddOption(menuId, "Spooky Zombies", serious::ToggleZMSpooky);
        break;
        #endif
        default:
            if(IsSubStr(menuId, ";"))
                AddClientMenu(menuId);
            break;
    }
}
    
mToClient(menu, cl)
{
    return menu + ";" + cl;
}
        
AddClientMenu(menu)
{
    player = self GetMenu(menu).player;
    if(!isdefined(player))
        return;
    
    split  = strtok(menu, ";");
    menuId = split[0];
    client = split[1];
    
    switch(menuId)
    {
        case "client":
            AddOption(menu, "Godmode", serious::SimpleToggle, "Godmode", sys::EnableInvulnerability, sys::DisableInvulnerability, player);
            AddOption(menu, "Infinite Ammo", serious::SimpleToggle, "Infinite Ammo", serious::InfiniteAmmo, undefined, player);
            AddSubmenu(menu, mToClient("troll", client), "Troll Menu", player);
            AddSubmenu(menu, mToClient("stats", client), "Stats Menu", player);
            AddSubmenu(menu, mToClient("access", client), "Menu Access", player);
            #ifdef ZM AddOption(menu, "Down Player", serious::DownPlayer, player); #endif
            #ifdef ZM AddOption(menu, "Revive Player", serious::ZM_RevivePlayer, player); #endif
            AddOption(menu, "Kill Player", serious::KillPlayer, player);
            AddOption(menu, "Spawn Player", serious::RespawnPlayer, player);
            AddOption(menu, "Kick Player", serious::KickWrapper, player GetEntityNumber());
            #ifdef ZM
            AddOption(menu, "Spawn a Zombie", serious::SpawnZombieArray, player, 1);
            AddOption(menu, "Spawn 20 Zombies", serious::SpawnZombieArray, player, 20);
            #endif
            break;
        case "troll":
            AddOption(menu, "Kill Loop", serious::SimpleToggle, "Kill Loop", serious::KillLoop, undefined, player);
            AddOption(menu, "Trip Balls", serious::SimpleToggle, "Trip Balls", serious::TripBalls, undefined, player);
            AddOption(menu, "Freeze Controls", serious::SimpleToggle, "Freeze Controls", serious::SFreezeControls, undefined, player);
            AddOption(menu, "Puppet Mode", serious::SimpleToggle, "Puppet Mode", serious::Puppet, undefined, player, self);
            AddOption(menu, "Lag Switch", serious::SimpleToggle, "Lag Switch", serious::LagSwitch, undefined, player);
            break;
        case "access":
        for(i = 0; i < level.status_strings.size && i < self.access; i++)
                AddOption(menu, "Status: " + level.status_strings[i], serious::SetAccess, i, player);
            break;
        case "stats":
            AddOption(menu, "Max Rank", serious::SetMaxRank, player);
            AddOption(menu, "Unlock All", serious::UnlockAll, player);
            AddOption(menu, "All Achievements", serious::GiveAllAchievements, player);
            if(isdefined(GetModeCurrency()))
            {
                AddOption(menu, "Award 25 " + GetModeCurrency(), serious::AdjustCurrency, player, 25);
                AddOption(menu, "Give " + GetModeCurrency() + " Loop", serious::AdjustCurrency, player, -1);
            }
            if(SessionModeIsMultiplayerGame())
            {
                AddOption(menu, "Reset Pending Keys", serious::ResetPendingKeys, player);
            }
            AddOption(menu, "Red Clan Tag", serious::ClanTagMod, player, "^1");
            AddOption(menu, "Green Clan Tag", serious::ClanTagMod, player, "^2");
            AddOption(menu, "Box Clan Tag", serious::ClanTagMod, player, "^B^");
            AddOption(menu, "Fucked Box Tag", serious::ClanTagMod, player, "^I\x7F\x7F");
            AddOption(menu, "Reset Clan Tag", serious::ClanTagMod, player, "{IL}");
        break;
    }
}