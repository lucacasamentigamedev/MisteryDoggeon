﻿using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
using MisteryDungeon.MysteryDungeon.Mgr;
using OpenTK;

namespace MisteryDungeon {
    public class Room_2 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/Textures/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("skull", "Assets/Textures/Objects/skull.png");
            GfxMgr.AddTexture("door", "Assets/Textures/Objects/crate.png");
            GfxMgr.AddTexture("player", "Assets/Textures/Spritesheets/player.png");
            GfxMgr.AddTexture("loading", "Assets/Textures/loading.png");
            GfxMgr.AddTexture("redGlobe", "Assets/Textures/red_globe.png");
            GfxMgr.AddTexture("arrow", "Assets/Textures/arrow.png");
            GfxMgr.AddTexture("gate", "Assets/Textures/Objects/mushroom.png");
            GfxMgr.AddTexture("greenBlob", "Assets/Textures/Spritesheets/green_blob.png");
            GfxMgr.AddTexture("spawnPoint", "Assets/Textures/Objects/spawn_point.png");
            GfxMgr.AddTexture("spines", "Assets/Textures/Objects/spines.png");
            GfxMgr.AddTexture("healthBarBackground", "Assets/Textures/healthbar_background.png");
            GfxMgr.AddTexture("healthBarForeground", "Assets/Textures/healthbar_foreground.png");
            GfxMgr.AddTexture("MapTileset.png", "Assets/Tiled/MapTileset.png");
            GfxMgr.AddTexture("blackScreen", "Assets/Textures/black_screen.png");
            GfxMgr.AddTexture("memoryCard", "Assets/Textures/Objects/memory_card.png");
            //Sounds
            AudioMgr.AddClip("objectBroke", "Assets/Sounds/SFX/object_broke.wav");
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
            AudioMgr.AddClip("pathUnreachable", "Assets/Sounds/SFX/path_unreachable.wav");
            AudioMgr.AddClip("roomLeft", "Assets/Sounds/SFX/room_left.ogg");
            AudioMgr.AddClip("hordeDefeated", "Assets/Sounds/SFX/mission_completed.ogg");
            AudioMgr.AddClip("playerTakesDamage", "Assets/Sounds/SFX/player_takes_damage.wav");
            AudioMgr.AddClip("playerDead", "Assets/Sounds/SFX/player_dead.wav");
            AudioMgr.AddClip("enemyTakesDamage", "Assets/Sounds/SFX/enemy_takes_damage.wav");
            AudioMgr.AddClip("enemyDead", "Assets/Sounds/SFX/enemy_dead.wav");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateLogMgr();
            CreateMemoryCardMgr();
            CreateHordeMgr();
            CreateMap();
            /******************FIXME: cheat da togliere**************/
            //RoomObjectsMgr.SetRoomObjectActiveness(2, 39, false, true);
            //GameObject g = GameObject.Find("Object_2_39");
            //if (g != null) g.IsActive = false;*/
            /********************************/
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,  //print pathfinding logs
                false,  //print puzzle logs
                false,  //print object creations logs
                false,  //print enemy horde logs
                true    //print memory card logs
            );
        }

        private static void CreateMemoryCardMgr() {
            GameObject memoryCardMgr = new GameObject("MemoryCardMgr", Vector2.Zero);
            memoryCardMgr.AddComponent<MemoryCardMgr>();
        }
        public void CreateHordeMgr() {
            GameObject go = new GameObject("HordeMgr", Vector2.Zero);
            go.AddComponent<HordeMgr>(
                new Vector2[] { new Vector2(2, 39) },
                new Vector2[] { new Vector2(2, 38), new Vector2(2, 39) }
            );
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
        }

        public void CreateMap() {
            TiledMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
        }
    }
}