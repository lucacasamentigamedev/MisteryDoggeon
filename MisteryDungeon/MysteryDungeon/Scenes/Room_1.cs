﻿using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
using MisteryDungeon.MysteryDungeon.Logic;
using MisteryDungeon.MysteryDungeon.Mgr;
using OpenTK;

namespace MisteryDungeon {
    public class Room_1 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/Textures/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("skull", "Assets/Textures/Objects/skull.png");
            GfxMgr.AddTexture("pot", "Assets/Textures/Objects/pot.png");
            GfxMgr.AddTexture("shell", "Assets/Textures/Objects/shell.png");
            GfxMgr.AddTexture("bones", "Assets/Textures/Objects/bones.png");
            GfxMgr.AddTexture("bananas", "Assets/Textures/Objects/bananas.png");
            GfxMgr.AddTexture("leaf", "Assets/Textures/Objects/leaf.png");
            GfxMgr.AddTexture("feather", "Assets/Textures/Objects/feather.png");
            GfxMgr.AddTexture("door", "Assets/Textures/Objects/crate.png");
            GfxMgr.AddTexture("player", "Assets/Textures/Spritesheets/player.png");
            GfxMgr.AddTexture("loading", "Assets/Textures/loading.png");
            GfxMgr.AddTexture("redGlobe", "Assets/Textures/red_globe.png");
            GfxMgr.AddTexture("arrow", "Assets/Textures/arrow.png");
            GfxMgr.AddTexture("key", "Assets/Textures/Objects/key.png");
            GfxMgr.AddTexture("healthBarBackground", "Assets/Textures/healthbar_background.png");
            GfxMgr.AddTexture("healthBarForeground", "Assets/Textures/healthbar_foreground.png");
            GfxMgr.AddTexture("MapTileset.png", "Assets/Tiled/MapTileset.png");
            GfxMgr.AddTexture("blackScreen", "Assets/Textures/black_screen.png");
            GfxMgr.AddTexture("memoryCard", "Assets/Textures/Objects/memory_card.png");
            GfxMgr.AddTexture("goldArrow", "Assets/Textures/gold_arrow.png");
            //Sounds
            AudioMgr.AddClip("objectDestroyed", "Assets/Sounds/SFX/object_destroyed.wav");
            AudioMgr.AddClip("objectPicked", "Assets/Sounds/SFX/object_picked.wav");
            AudioMgr.AddClip("arrowShot", "Assets/Sounds/SFX/arrow_shot.ogg");
            AudioMgr.AddClip("background1", "Assets/Sounds/Background/background1.ogg");
            AudioMgr.AddClip("background2", "Assets/Sounds/Background/background2.ogg");
            AudioMgr.AddClip("background3", "Assets/Sounds/Background/background3.ogg");
            AudioMgr.AddClip("background4", "Assets/Sounds/Background/background4.ogg");
            AudioMgr.AddClip("background5", "Assets/Sounds/Background/background5.ogg");
            AudioMgr.AddClip("background6", "Assets/Sounds/Background/background6.ogg");
            AudioMgr.AddClip("background7", "Assets/Sounds/Background/background7.ogg");
            AudioMgr.AddClip("background8", "Assets/Sounds/Background/background8.ogg");
            AudioMgr.AddClip("background9", "Assets/Sounds/Background/background9.ogg");
            AudioMgr.AddClip("background10", "Assets/Sounds/Background/background10.ogg");
            AudioMgr.AddClip("background11", "Assets/Sounds/Background/background11.ogg");
            AudioMgr.AddClip("background12", "Assets/Sounds/Background/background12.ogg");
            AudioMgr.AddClip("background13", "Assets/Sounds/Background/background13.ogg");
            AudioMgr.AddClip("background14", "Assets/Sounds/Background/background14.ogg");
            AudioMgr.AddClip("background15", "Assets/Sounds/Background/background15.ogg");
            AudioMgr.AddClip("background16", "Assets/Sounds/Background/background16.ogg");
            AudioMgr.AddClip("actionNotAllowed", "Assets/Sounds/SFX/action_not_allowed.wav");
            AudioMgr.AddClip("roomLeft", "Assets/Sounds/SFX/room_left.ogg");

        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateGameStatsMgr();
            CreateLogMgr();
            CreateMemoryCardMgr();
            CreateMap();
        }

        private static void CreateGameStatsMgr() {
            GameObject gameStatsMgr = new GameObject("GameStatsMgr", Vector2.Zero);
            gameStatsMgr.AddComponent<GameStatsMgr>();
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,   //print pathfinding logs
                false,   //print puzzle logs
                false,   //print object creations logs
                false,  //print enemy horde logs
                false    //print memory card logs
            );
        }

        private static void CreateMemoryCardMgr() {
            GameObject memoryCardMgr = new GameObject("MemoryCardMgr", Vector2.Zero);
            memoryCardMgr.AddComponent<MemoryCardMgr>();
        }

        public void CreateMap() {
            TiledMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
        }
    }
}
