﻿using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
using OpenTK;

namespace MisteryDungeon {
    public class Room_0 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/Textures/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("skull", "Assets/Textures/Objects/skull.png");
            GfxMgr.AddTexture("pot", "Assets/Textures/Objects/pot.png");
            GfxMgr.AddTexture("shell", "Assets/Textures/Objects/shell.png");
            GfxMgr.AddTexture("bones", "Assets/Textures/Objects/bones.png");
            GfxMgr.AddTexture("door", "Assets/Textures/Objects/crate.png");
            GfxMgr.AddTexture("loading", "Assets/Textures/loading.png");
            GfxMgr.AddTexture("gate", "Assets/Textures/Objects/mushroom.png");
            GfxMgr.AddTexture("player", "Assets/Textures/Spritesheets/player.png");
            GfxMgr.AddTexture("bow", "Assets/Textures/Objects/bow.png");
            GfxMgr.AddTexture("redGlobe", "Assets/Textures/red_globe.png");
            GfxMgr.AddTexture("arrow", "Assets/Textures/arrow.png");
            GfxMgr.AddTexture("healthBarBackground", "Assets/Textures/healthbar_background.png");
            GfxMgr.AddTexture("healthBarForeground", "Assets/Textures/healthbar_foreground.png");
            GfxMgr.AddTexture("MapTileset.png", "Assets/Tiled/MapTileset.png");
            GfxMgr.AddTexture("platformButton", "Assets/Textures/Objects/platform_button.png");
            //Sounds
            AudioMgr.AddClip("objectBroke", "Assets/Sounds/SFX/object_broke.wav");
            AudioMgr.AddClip("objectPicked", "Assets/Sounds/SFX/object_picked.wav");
            AudioMgr.AddClip("sequenceRight", "Assets/Sounds/SFX/sequence_right.wav");
            AudioMgr.AddClip("sequenceWrong", "Assets/Sounds/SFX/sequence_wrong.wav");
            AudioMgr.AddClip("sequenceCompleted", "Assets/Sounds/SFX/mission_completed.ogg");
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
            AudioMgr.AddClip("clockTick", "Assets/Sounds/SFX/clock_tick.ogg");
            AudioMgr.AddClip("puzzleReady", "Assets/Sounds/SFX/woop.wav");
            AudioMgr.AddClip("roomLeft", "Assets/Sounds/SFX/room_left.ogg");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateLogMgr();
            CreatePuzzleMgr();
            CreateMap();
            /******************FIXME: cheat per andare dritto in stanza 2 con anche la chiave per il boss**************/
            /*RoomObjectsMgr.SetRoomObjectActiveness(0, 27, false);
            GameObject g = GameObject.Find("Object_0_27");
            if (g != null) g.IsActive = false;
            g = GameObject.Find("Object_0_22");
            if (g != null) g.IsActive = true;
            GameStats.collectedKeys.Add(25);*/
            //GameStats.ActiveWeapon = GameObject.Find("Object_0_22").GetComponent<Weapon>();
            /*****************************************************/
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,  //print pathfinding logs
                false,  //print puzzle logs
                false,  //print object creations logs
                false   //print enemy horde logs
            );
        }

        public void CreateMap() {
            TiledMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
        }

        public void CreatePuzzleMgr() {
            GameObject go = new GameObject("PuzzleMgr", Vector2.Zero);
            go.AddComponent<PuzzleMgr>(
                10, //secondi totali del timer del puzzle
                1,  //secondi da aspettare prima che il puzzle sia pronto
                new Vector2[] { new Vector2(0, 22)}, //arma, da attivare
                new Vector2[] { new Vector2(0, 27) } //gate, da disattivare
            );
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
        }
    }
}
