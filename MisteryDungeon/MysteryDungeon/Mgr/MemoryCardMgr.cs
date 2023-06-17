using Aiv.Fast2D.Component;
using MisteryDungeon.MysteryDungeon.Utility;
using System;
using System.IO;

namespace MisteryDungeon.MysteryDungeon.Mgr {
    public class MemoryCardMgr : UserComponent {

        public MemoryCardMgr(GameObject owner) : base(owner) {}

        public override void Start() {
            EventManager.AddListener(EventList.NewGame, OnNewGame);
            EventManager.AddListener(EventList.LoadGame, OnLoadGame);
            EventManager.AddListener(EventList.SaveGame, OnSaveGame);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.NewGame, OnNewGame);
            EventManager.RemoveListener(EventList.LoadGame, OnLoadGame);
            EventManager.RemoveListener(EventList.SaveGame, OnSaveGame);
        }

        public void OnNewGame(EventArgs message) {
            EventManager.CastEvent(EventList.LOG_MemoryCard, EventArgsFactory.LOG_Factory("New game (reset)"));
            GameStatsMgr.ResetGameStats();
            MovementGridMgr.ResetMovementsGrids();
            RoomObjectsMgr.ResetRoomObjects();
            TiledMapMgr.ResetMaps();
            if (File.Exists(GameConfigMgr.gameStatsFileName)) File.Delete(GameConfigMgr.gameStatsFileName);
            if (File.Exists(GameConfigMgr.movementGridFileName)) File.Delete(GameConfigMgr.movementGridFileName);
            if (File.Exists(GameConfigMgr.roomObjectsFileName)) File.Delete(GameConfigMgr.roomObjectsFileName);
            if (File.Exists(GameConfigMgr.weaponFileName)) File.Delete(GameConfigMgr.weaponFileName);
        }

        public void OnSaveGame(EventArgs message) {
            EventManager.CastEvent(EventList.LOG_MemoryCard, EventArgsFactory.LOG_Factory("Save game"));
            JsonFileUtils.PrettyWrite(GameStatsMgr.GetGameStats(), GameConfigMgr.gameStatsFileName);
            JsonFileUtils.SaveJaggeredArray(MovementGridMgr.Grids, GameConfigMgr.movementGridFileName);
            JsonFileUtils.PrettyWrite(RoomObjectsMgr.RoomObjects, GameConfigMgr.roomObjectsFileName);
            if(GameStatsMgr.ActiveWeapon != null) JsonFileUtils.PrettyWrite(GameStatsMgr.ActiveWeapon.GetSerializedWeapon(), GameConfigMgr.weaponFileName);
            EventManager.CastEvent(EventList.EndLoading, EventArgsFactory.EndLoadingFactory());
        }

        public void OnLoadGame(EventArgs message) {
            EventManager.CastEvent(EventList.LOG_MemoryCard, EventArgsFactory.LOG_Factory("Load game"));
            bool fileExists = false;
            if(fileExists) {
                //TODO: load di tutto 
                //GameStatsMgr.LoadGameStats();
                MovementGridMgr.LoadMovementsGrids();
                RoomObjectsMgr.LoadRoomObjects();
                TiledMapMgr.LoadMaps();
            }
        }
    }
}
