﻿using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
using OpenTK;

namespace MisteryDungeon {
    public class Room_3 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("crate", "Assets/crate.png");
            GfxMgr.AddTexture("door", "Assets/crate.png");
            GfxMgr.AddTexture("player", "Assets/Spritesheets/player.png");
            GfxMgr.AddTexture("loading", "Assets/loading.png");
            GfxMgr.AddTexture("arrow", "Assets/arrow.png");
            GfxMgr.AddTexture("redGlobe", "Assets/red_globe.png");
            GfxMgr.AddTexture("gate", "Assets/lamp_gate.png");
            GfxMgr.AddTexture("boss", "Assets/Spritesheets/boss.png");
            GfxMgr.AddTexture("spines", "Assets/spines.png");
            GfxMgr.AddTexture("healthBarBackground", "Assets/healthbar_background.png");
            GfxMgr.AddTexture("healthBarForeground", "Assets/healthbar_foreground.png");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateLogMgr();
            CreateMap();
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,  //print pathfinding logs
                false,  //print puzzle logs
                false,  //print object creations logs
                false    //print enemy horde logs
            );
        }

        public void CreateMap() {
            TiledMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
        }
    }
}