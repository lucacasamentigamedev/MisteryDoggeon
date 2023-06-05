﻿using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;

namespace MisteryDungeon {
    internal class Room_2 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("crate", "Assets/crate.png");
            GfxMgr.AddTexture("door", "Assets/crate.png");
            GfxMgr.AddTexture("player", "Assets/Spritesheets/player.png");
            GfxMgr.AddTexture("loading", "Assets/loading.png");
            GfxMgr.AddTexture("arrow", "Assets/arrow.png");
            GfxMgr.AddTexture("gate", "Assets/lamp_gate.png");
            GfxMgr.AddTexture("blob", "Assets/blob.gif");
            GfxMgr.AddTexture("spawnPoint", "Assets/spawn_point.png");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateLogMgr();
            CreateMap();
            CheckHordeConditions();
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,  //print pathfinding logs
                false,  //print puzzle logs
                true    //print object creations logs
            );
        }

        public void CreateMap() {
            TiledMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
        }

        public void CheckHordeConditions() {
            if(!GameStats.HordeDefeated && GameStats.ActiveWeapon != null) {
                //attivo gate
                GameObject.Find("Object_2_39").IsActive = true;
                RoomObjectsMgr.SetRoomObjectActiveness(2, 39, true, true, MovementGrid.EGridTile.Wall);
                //attivo spawn point
                GameObject.Find("Object_2_40").IsActive = true;
                GameObject.Find("Object_2_41").IsActive = true;
                GameObject.Find("Object_2_42").IsActive = true;
                GameObject.Find("Object_2_43").IsActive = true;
            }
        }
    }
}
