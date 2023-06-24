﻿using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using ImGuiNET;
using KamiLib.AutomaticUserInterface;
using Lumina.Excel.GeneratedSheets;
using Mappy.Abstracts;
using Mappy.Models;
using Mappy.Models.Enums;
using Mappy.Utility;

namespace Mappy.System.Modules;

public class FlagConfig : IconModuleConfigBase
{
    [Disabled]
    public new bool ShowIcon = false;

    [Disabled]
    public new bool ShowTooltip = false;
}

public unsafe class Flag : ModuleBase
{
    public override ModuleName ModuleName => ModuleName.FlagMarker;
    public override ModuleConfigBase Configuration { get; protected set; } = new FlagConfig();

    public static TemporaryMapMarker? TempMapMarker { get; private set; }

    protected override bool ShouldDrawMarkers(Map map)
    {
        if (TempMapMarker is null) return false;
        if (AgentMap.Instance()->IsFlagMarkerSet != 1) return false;
        if (TempMapMarker.MapID != map.RowId) return false;
        
        return base.ShouldDrawMarkers(map);
    }

    public override void LoadForMap(MapData mapData)
    {
        // Do Nothing.
    }

    protected override void DrawMarkers(Viewport viewport, Map map)
    {
        if (TempMapMarker is null) return;
        var config = GetConfig<IconModuleConfigBase>();

        var iconId = TempMapMarker.IconID;
        var rawPosition = TempMapMarker.Position;
        var position = Position.GetTextureOffsetPosition(rawPosition, map);
        
        DrawUtilities.DrawIcon(iconId, position, config.IconScale);
        ShowContextMenu();
    }

    private void ShowContextMenu()
    {
        if (!ImGui.IsItemClicked(ImGuiMouseButton.Right)) return;
        MappySystem.ContextMenuController.Show(ContextMenuType.Flag);
    }
    
    public static void SetFlagMarker(TemporaryMapMarker marker) => TempMapMarker = marker;
    public static void RemoveFlagMarker()
    {
        TempMapMarker = null;
        AgentMap.Instance()->IsFlagMarkerSet = 0;
    }
}