using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BaseX;
using CodeX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Data;
using FrooxEngine.LogiX.ProgramFlow;
using FrooxEngine.UIX;
using HarmonyLib;
using NeosModLoader;

namespace ClearContactSearch
{
    public class ClearContactSearch : NeosMod
    {
        public override string Author => "Banane9";
        public override string Link => "https://github.com/Banane9/NeosClearContactSearch";
        public override string Name => "ClearContactSearch";
        public override string Version => "1.0.0";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony($"{Author}.{Name}");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(FriendsDialog), "OnAttach")]
        private static class SearchBarPatch
        {
            [HarmonyPostfix]
            private static void OnAttachPostfix(SyncRef<TextField> ____searchBar)
            {
                var searchBar = ____searchBar.Target.Slot;

                var builder = new UIBuilder(searchBar.Parent);
                builder.CurrentRect.OffsetMin.Value += new float2(2, 0);
                builder.CurrentRect.OffsetMax.Value += new float2(-2, 0);

                builder.VerticalFooter(32, out var footer, out var content);

                searchBar.Parent = content.Slot;
                content.OffsetMax.Value += new float2(-2, 0);

                builder = new UIBuilder(footer);
                builder.Button("∅").LocalPressed += (sender, args) =>
                {
                    ____searchBar.Target.Editor.Target.Text.Target.Text = "";
                    ____searchBar.Target.Editor.Target.ForceEditingChangedEvent();
                };
            }
        }
    }
}