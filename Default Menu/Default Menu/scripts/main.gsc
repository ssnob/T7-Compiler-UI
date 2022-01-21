/* menu by serious */

/* Credits to the following people for stuff */
// extinct
// itsfebiven
// _dev
// youtube chat

/* If your game crashes, make sure you have the correct mode selected in config.il */

#include scripts\codescripts\struct;
#include scripts\shared\callbacks_shared;
#include scripts\shared\system_shared;
#include scripts\shared\rank_shared;
#include scripts\shared\math_shared;
#include scripts\codescripts\struct;
#include scripts\shared\callbacks_shared;
#include scripts\shared\clientfield_shared;
#include scripts\shared\system_shared;
#include scripts\shared\util_shared;
#include scripts\shared\hud_util_shared;
#include scripts\shared\hud_message_shared;
#include scripts\shared\hud_shared;
#include scripts\shared\array_shared;
#include scripts\shared\flag_shared;

#define mBG_COLOR = color(0);
#define mBG_SHADER = "white";
#define mBG_ALPHA = .55;

#define mTEXT_COLOR = color(0xFFFFFF);
#define mTEXT_ALPHA = 1;
#define mTEXT_MAXOPTIONS = 10;

#define mSLIDE_SHADER = "white";
#define mSLIDE_COLOR = color(0x205dd6);
#define mSLIDE_ALPHA = .8;
#define mINVERSE_COLOR = color(0xd10606);

#define mSCROLL_DELAY = .15;

#define mANIM_NONE = 0;
#define mANIM_ZOOMIN = 1;
#define mANIM_ZOOMOUT = 2;

#define mHUD_X_OFF = -30;
#define mHUD_Y_OFF = 60;

#namespace serious;
autoexec __init__system__() { system::register("serious", ::__init__, undefined, undefined); }

__init__()
{
    callback::on_start_gametype(::init);
    callback::on_spawned(::on_player_spawned);
    callback::on_connect(::on_player_connect);

    level._camos_ = [];
    for(i = 0; i < 290; i++)
    {
        row = TableLookupRow("gamedata/weapons/common/weaponoptions.csv", i);
        
        if(!isdefined(row) || !isdefined(row.size) || row.size < 3)
            continue;
            
        if(row[1] != "camo")
            continue;
            
        level._camos_[santize_camoname(row[2])] = int(row[0]);
    }
    
    for(i = 0; i < 99; i++)
    {
        level._cmusic[tableLookup("gamedata/tables/common/music_player.csv", 0, i, 1)] = tableLookup("gamedata/tables/common/music_player.csv", 0, i, 2);
    }
}

init()
{
    level.status_strings = ["Unverified", "Verified", "Admin", "Co-Host"];
    thread SeriousUtil();
    #ifdef DEBUG
        thread DevExit();
    #endif
    #ifdef MP
        level.oob_timelimit_ms        = 0x7FFFFFFF;
        level.oob_damage_per_interval = 0;
        foreach(trig in GetEntArray("trigger_out_of_bounds", "classname"))
            trig delete();
    #endif
}

#ifdef DEBUG
DevExit()
{
    level util::waittill_any("game_ended", "end_game");
    ExitLevel(0);
}
#endif

on_player_connect()
{
    if(self ishost())
    {
        //precache zones
        level._teleto = [];
        foreach(zone in GetArrayKeys(level.zones))
        {
            spawns = GetAllSpawnsFromZone(zone);
            
            if(!isdefined(spawns) || spawns.size < 1)
                continue;
            
            level._teleto[zone] = spawns[0];
        }
        
        #ifdef ZM
        level.old_lost_perk  = level.perk_lost_func;
        level.perk_lost_func = serious::LostPerkOverride;
        
        if(!isdefined(level._zombie_custom_spawn_logic))
            level._zombie_custom_spawn_logic = [];
        else if(!isarray(level._zombie_custom_spawn_logic))
            level._zombie_custom_spawn_logic[0] = level._zombie_custom_spawn_logic;
        level._zombie_custom_spawn_logic[level._zombie_custom_spawn_logic.size] = serious::ZmExploder;
        #endif
    
        self thread CatalystAntiEnd();
        self thread CatalystAntiDown();
    }
}

on_player_spawned()
{
    self endon("disconnect");
    level endon("game_ended");
    level endon("end_game");
    
    self freezeControls(false);
    
    if(self IsHost())
    {
        self SetAccess(5);
    }
    
    wait .1;
    self notify("stop_player_out_of_playable_area_monitor");
    
    #ifdef ZM
    level.player_intersection_tracker_override = ::true_one_arg;
    level.player_out_of_playable_area_monitor_callback = ::nullsub;
    #endif

    #ifndef serious exitlevel(0); #endif
}

SetAccess(access, player = self)
{
    if(player ishost() && isdefined(player.access))
    {
        if(player != self)
            return self iPrintLnBold("^1Nice Try");
        if(access < 5)
            return self iPrintLnBold("^3Cant Do That");
    }
        
    if(isdefined(player.access) && self.access <= access && access != 5)
        return self iPrintLnBold("^1Nice Try");
    
    player notify("access");
    player.access  = access;
    player.submenu = "none";
    player.cursors = [];
    player.menus   = [];
    
    player SetMenuOpen(false);
    if(bool(player.access))
    {
        player thread MenuBase();
    }
    
    self iPrintLnBold("Access ^2" + level.status_strings[access] + " ^7updated");
}

MenuBase()
{
    self endon("access");
    self endon("disconnect");
    
    self precacheoptions();
    while(bool(self.access))
    {
        wait .025;
        if(self.submenu == "none") //If the menu is closed
        {
            if(!(self IsButtonPressed(SL_BUTTONS_MELEE) && self IsButtonPressed(SL_BUTTONS_ADS)))
                continue;
            
            self SetMenuOpen(true);
            
            while(self IsButtonPressed(SL_BUTTONS_MELEE) || self IsButtonPressed(SL_BUTTONS_ADS))
                wait .025;
            
            continue;
        }
        if(self IsButtonPressed(SL_BUTTONS_MELEE))
        {
            self ExecMenu();
            
            while(self IsButtonPressed(SL_BUTTONS_MELEE))
                wait .025;
            
            continue;
        }
        if(self IsButtonPressed(SL_BUTTONS_ATTACK) || self IsButtonPressed(SL_BUTTONS_ADS))
        {
            self Scroll(self IsButtonPressed(SL_BUTTONS_ATTACK));
            
            wait mSCROLL_DELAY;
            continue;
        }
        if(self IsButtonPressed(SL_BUTTONS_USE))
        {
            self Select();
            
            while(self IsButtonPressed(SL_BUTTONS_USE))
                wait .025;
            
            continue;
        }
    }
}

Select(cursor, menu = self.submenu)
{
    if(!isdefined(cursor))
        cursor = self GetCursor(menu);
    
    option = self GetOption(cursor, menu);
    
    if(!(isdefined(option) && isdefined(option.f)))
        return; //cant execute an undefined option 
     
    a = option.a;
    
    if(!isdefined(a))
        a = [];
    
    self thread ExecOption(option.f, a);
    
    if(option.f == ::execmenu)
    {
        wait mSCROLL_DELAY;
        return;
    }
    
    slider = self.mSlider;
    
    slider SetShader(mSLIDE_SHADER, 160, 16);
    slider.color = inversecolor(mSLIDE_COLOR);
    
    slider FadeOverTime(mSCROLL_DELAY);
    slider ScaleOverTime(mSCROLL_DELAY, 196, 20);
    slider.color = mSLIDE_COLOR;
}

Scroll(up)
{
    curs = self GetCursor();
    
    self SetCursor(self.submenu, curs - up + !up, curs);
}

GetCursor(menu = self.submenu)
{
    if(!isdefined(self.cursors[menu]))
        self SetCursor(menu);
        
    return self.cursors[menu];
}

SetCursor(menu = "none", value = 0, oldValue = value)
{
    max = self GetMaxCursor();
    
    if(value < 0)
        value = max;
    
    if(value > max)
        value = 0;

    self.cursors[menu] = value;
    
    delta = value - oldvalue;
    
    // if the submenu is equal to the menu we are updating, and there is a delta
    if(self.submenu == menu && delta)
    {
        // if the value changed by a non unary delta, redraw
        if(abs(delta) > 1)
        {
            self thread DrawMenu(mANIM_NONE);
            return;
        }
        
        slider         = self.mSlider;
        slider moveovertime(mSCROLL_DELAY);
        slider.xoffset       = 0;
        slider.yoffset       = 1;
        slider.point         = "center";
        slider.relativepoint = "center";
        
        maxOpt = mTEXT_MAXOPTIONS - 1;
        
        index = ArrayIndexOf(self.mText, slider.parent);

        // Failsafe that prevents undefined behaviour of the scroller and text options
        if(index == -1)
        {
            value = 0;
            index = 0;
            delta = 0;
        }
        
        up = !(delta + 1);
        
        isTop    = !index;
        isBottom = index == maxOpt;

        windowSlide = up ? isTop : isBottom;

        if(!windowSlide)
        {
            index += delta;
            slider hud::SetParent(self.mText[index]);
            return;
        }
        
        del_index = up * maxOpt;
        add_index = maxOpt - del_index;
        str_index = value - (isBottom * del_index);
        
        del_elem   = self.mText[del_index];
        self.mText = ArrayRemove(self.mText, del_elem);
        del_elem destroy();

        new_elem      = Text("", -110 + mHUD_X_OFF, 60 + (add_index * 25) + mHUD_Y_OFF, "default", 1.5, mTEXT_COLOR, 0, 2, "CENTER", "TOP_RIGHT");
        new_elem.text = GetOption(str_index).o;
        new_elem SetText(new_elem.text);
        
        new_elem FadeOverTime(mSCROLL_DELAY * 1.5);
        new_elem.alpha = mTEXT_ALPHA;
        
        for(i = 0; i < self.mText.size; i++)
        {
            elem = self.mText[i];
            elem MoveOverTime(mSCROLL_DELAY * .95);
            elem.y = 60 + ((i + up) * 25) + mHUD_Y_OFF;
        }
 
        self.mText = ArrayInsertValue(self.mText, add_index, new_elem);
        slider hud::SetParent(self.mText[index]);
        
        new_elem thread DelayedBindText(mSCROLL_DELAY * .95);
    }
}

//This method makes sure that we call the overflow fix function after our anims are done so that we can catch overflows
DelayedBindText(duration)
{
    wait duration;
    self SetText(self.text);
}

GetMaxCursor()
{
    return GetMenu().options.size - 1;
}

GetMaxTextIndex()
{
    return int(min(GetMaxCursor() + 1, mTEXT_MAXOPTIONS));
}

SetMenuOpen(isOpen)
{
    if(bool(isOpen))
    {
        self setClientUIVisibilityFlag("hud_visible", 0);
        self.submenu = "main";
        self DrawMenu(mANIM_ZOOMIN);
    }
    else
    {
        self.submenu = "none";
        self EraseMenu();
        self setClientUIVisibilityFlag("hud_visible", 1);
    }
}

DrawMenu(animStyle = mANIM_NONE)
{
    self EraseMenu();
    self precacheoptions();
    
    self.mbg    = self Icon(mBG_SHADER, -10 + mHUD_X_OFF, 10 + mHUD_Y_OFF, 200, 300, mBG_COLOR, mBG_ALPHA, 0, "TOP_RIGHT", "TOP_RIGHT");
    self.mTitle = self Text(GetMenu().o, -110 + mHUD_X_OFF, 30 + mHUD_Y_OFF, "default", 2.0, mTEXT_COLOR, mTEXT_ALPHA, 1, "CENTER", "TOP_RIGHT");
    self.mTitle AnimateMenuText(animStyle);
    
    #region frame
    self.mframe = [];
    
    top = self Icon("white", -2, 0, 2, 2, inversecolor(mSLIDE_COLOR), mSLIDE_ALPHA, 1, "TOP_RIGHT", "TOP_RIGHT");
    top hud::SetParent(self.mbg);
    top ScaleOverTime(mSCROLL_DELAY, 196, 2);
    top FadeOverTime(mSCROLL_DELAY);
    top.color = mSLIDE_COLOR;
    self.mframe["top"] = top;
    
    bottom = self Icon("white", 2, 0, 2, 2, inversecolor(mSLIDE_COLOR), mSLIDE_ALPHA, 1, "BOTTOM_LEFT", "BOTTOM_LEFT");
    bottom hud::SetParent(self.mbg);
    bottom ScaleOverTime(mSCROLL_DELAY, 196, 2);
    bottom FadeOverTime(mSCROLL_DELAY);
    bottom.color = mSLIDE_COLOR;
    self.mframe["bottom"] = bottom;
    
    left = self Icon("white", 0, 0, 2, 2, inversecolor(mSLIDE_COLOR), mSLIDE_ALPHA, 1, "TOP_LEFT", "TOP_LEFT");
    left hud::SetParent(self.mbg);
    left ScaleOverTime(mSCROLL_DELAY, 2, 300);
    left FadeOverTime(mSCROLL_DELAY);
    left.color = mSLIDE_COLOR;
    self.mframe["left"] = left;
    
    right = self Icon("white", 0, 0, 2, 2, inversecolor(mSLIDE_COLOR), mSLIDE_ALPHA, 1, "BOTTOM_RIGHT", "BOTTOM_RIGHT");
    right hud::SetParent(self.mbg);
    right ScaleOverTime(mSCROLL_DELAY, 2, 300);
    right FadeOverTime(mSCROLL_DELAY);
    right.color = mSLIDE_COLOR;
    self.mframe["right"] = right;
    
    #endregion
    
    self.mText = [];
    cursor     = self GetCursor();
    
    for(i = 0; i < GetMaxTextIndex(); i++) //i = i + 1
    {
        offset = int(max(0, (cursor - mTEXT_MAXOPTIONS) + 1));
        self.mText[i] = self Text(GetOption(offset + i).o, -110 + mHUD_X_OFF, 60 + (i * 25) + mHUD_Y_OFF, "default", 1.5, mTEXT_COLOR, mTEXT_ALPHA, 2, "CENTER", "TOP_RIGHT");
        self.mText[i] AnimateMenuText(animStyle);
    }

    cursOpt       = self.mText[int(min(cursor, mTEXT_MAXOPTIONS - 1))];
    slider        = self Icon(mSLIDE_SHADER, cursOpt.xoffset, cursOpt.yoffset + 1, 0, 20, inversecolor(mSLIDE_COLOR), 0, 1, "CENTER", "TOP_RIGHT");
    slider.parent = cursOpt;
    self.mSlider  = slider;
    
    if(self GetMaxCursor() > -1)
    {
        slider.alpha = mSLIDE_ALPHA;
        slider Fadeovertime(mSCROLL_DELAY);
        slider ScaleOverTime(mSCROLL_DELAY, 196, 20);
        slider.color = mSLIDE_COLOR;
    } 
}

EraseMenu()
{
    self.mbg destroy();
    self.mTitle destroy();
    
    foreach(elem in self.mframe)
        elem destroy();
    
    if(isdefined(self.mText))
        foreach(text in self.mText)
            text destroy();
    
    self.mSlider destroy();
}

AnimateMenuText(animstyle = mANIM_NONE)
{
    if(animstyle == mANIM_NONE)
        return;
        
    aScale = .25;
    fScale = 1.5;
    
    if(animstyle == mANIM_ZOOMIN)
        fScale = 1 / fScale;
        
    baseAlpha     = self.alpha;
    baseFontScale = self.baseFontScale;
    
    self.alpha     = baseAlpha * aScale;
    self.FontScale = baseFontScale * fScale;
    
    self FadeOverTime(mSCROLL_DELAY);
    self ChangeFontScaleOverTime(mSCROLL_DELAY);
    
    self.FontScale = baseFontScale;
    self.alpha     = baseAlpha;
}

AddMenu(parent = "main", newID = "", newName = newID)
{
    if(isdefined(self.menus[newID]))
        return self.menus[newID]; //dont add the same menu twice
    
    menu = spawnstruct();
    
    menu.options = [];
    menu.m       = parent;
    menu.o       = newName;
    menu.i       = newID;
    
    self.menus[newID] = menu;
    
    #ifdef serious #else killserver(); #endif
    
    return menu;
}

AddOption(menu = "main", o = "Undefined Option", f, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9)
{
    rMenu = self.menus[menu];
    if(!isdefined(rMenu))
        return; //cant add to an undefined menu 
    
    option = self GetOptionByName(menu, o);
    if(!isdefined(option))
        option = spawnstruct();
    
    option.o = o;
    option.f = f;
    option.a = [a0, a1, a2, a3, a4, a5, a6, a7, a8, a9];
    
    if(!isdefined(self GetOptionByName(menu, o)))
        rMenu.options[rMenu.options.size] = option;
    
    return option;
}

ClearOption(menu, index, onlyMatch = false)
{
    rMenu = self.menus[menu];
    
    if(!isdefined(rMenu))
        return;
    
    for(i = rMenu.options.size - 1; i >= index; i--)
        if(!onlyMatch || i == index)
            ArrayRemoveIndex(rMenu.options, i);
}
    
ClearOptionByName(menu, o)
{
    rMenu = self.menus[menu];
    
    if(!isdefined(rMenu))
        return;
    
    foreach(i, v in rMenu.options)
        if(v.o == o)
        {
            opt = i;
            break;
        }
        
    if(!isdefined(opt))
        return;
    
    ClearOption(menu, opt, true);
}

GetOptionByName(menu, o)
{
    rMenu = self.menus[menu];
    
    if(!isdefined(rMenu))
        return undefined;
    
    foreach(opt in rMenu.options)
        if(opt.o == o)
            return opt;
}

AddSubmenu(parent, newID, newName, player, access = 0)
{
    if(self.access < access) return;
    
    menu          = AddMenu(parent, newID, newName);
    menu.player   = player;
    option        = AddOption(menu.m, menu.o, ::ExecMenu, menu.i);
    option.player = player;
    
    return option;
}

ExecMenu(menu)
{
    animMode = mANIM_ZOOMIN;
    
    if(!isdefined(menu))
    {
        menu     = self GetMenu().m;
        animMode = mANIM_ZOOMOUT;
    }
    
    if(!isdefined(menu) || menu == "none")
    {
        self SetMenuOpen(false);
        return;
    }
    
    self.submenu = menu;
    DrawMenu(animMode);
}

GetMenu(menu = self.submenu)
{
    if(!isdefined(self.menus[menu]))
        AddMenu("main", menu);
    
    return self.menus[menu];
}

GetOption(option = 0, menu = self.submenu)
{
    menu   = GetMenu(menu);
    option = menu.options[option];
    
    if(!isdefined(option))
    {
        option   = SpawnStruct();
        option.o = "Invalid Option";
        option.a = [];
        option.m = menu.i;
    }
    
    return option;
}

inversecolor(color)
{
    #ifndef serious killserver(); #endif
    return mINVERSE_COLOR;
}