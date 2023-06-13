﻿using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
using OpenTK;

namespace MisteryDungeon {
    public class Room_1 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("skull", "Assets/skull.png");
            GfxMgr.AddTexture("pot", "Assets/pot.png");
            GfxMgr.AddTexture("shell", "Assets/shell.png");
            GfxMgr.AddTexture("bones", "Assets/bones.png");
            GfxMgr.AddTexture("door", "Assets/crate.png");
            GfxMgr.AddTexture("player", "Assets/Spritesheets/player.png");
            GfxMgr.AddTexture("loading", "Assets/loading.png");
            GfxMgr.AddTexture("arrow", "Assets/arrow.png");
            GfxMgr.AddTexture("redGlobe", "Assets/red_globe.png");
            GfxMgr.AddTexture("key", "Assets/key.png");
            GfxMgr.AddTexture("healthBarBackground", "Assets/healthbar_background.png");
            GfxMgr.AddTexture("healthBarForeground", "Assets/healthbar_foreground.png");
            GfxMgr.AddTexture("MapTileset.png", "Assets/Tiled/MapTileset.png");
            //Sounds
            AudioMgr.AddClip("objectBroke", "Assets/Sounds/SFX/object_broke.wav");
            AudioMgr.AddClip("objectPicked", "Assets/Sounds/SFX/object_picked.wav");
            AudioMgr.AddClip("arrowShooted", "Assets/Sounds/SFX/arrow_shooted.ogg");
            AudioMgr.AddClip("background", "Assets/Sounds/background.wav");
            AudioMgr.AddClip("pathUnreachable", "Assets/Sounds/SFX/path_unreachable.wav");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateLogMgr();
            CreateMap();
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,   //print pathfinding logs
                false,   //print puzzle logs
                false,   //print object creations logs
                false    //print enemy horde logs
            );
        }

        public void CreateMap() {
            TiledMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
        }
    }
}
