using Aiv.Fast2D.Component;
using MisteryDungeon.MysteryDungeon.Utility;
using System;
using System.IO;

namespace MisteryDungeon.MysteryDungeon.Mgr {
    public class MemoryCardMgr : UserComponent {

        private string stats;
        private string movGrid;
        private string roomObj;
        private string weapon;

        public MemoryCardMgr(GameObject owner) : base(owner) {
            stats = Path.Combine("Assets/Saves", GameConfigMgr.gameStatsFileName);
            movGrid = Path.Combine("Assets/Saves", GameConfigMgr.movementGridFileName);
            roomObj = Path.Combine("Assets/Saves", GameConfigMgr.roomObjectsFileName);
            weapon = Path.Combine("Assets/Saves", GameConfigMgr.weaponFileName);
        }

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
            File.WriteAllText(stats, string.Empty);
            File.WriteAllText(movGrid, string.Empty);
            File.WriteAllText(roomObj, string.Empty);
            File.WriteAllText(weapon, string.Empty);
        }

        public void OnSaveGame(EventArgs message) {
            EventManager.CastEvent(EventList.LOG_MemoryCard, EventArgsFactory.LOG_Factory("Save game"));
            JsonFileUtils.PrettyWrite(GameStatsMgr.GetGameStats(), stats);
            JsonFileUtils.SaveJaggeredArray(MovementGridMgr.Grids, movGrid);
            JsonFileUtils.PrettyWrite(RoomObjectsMgr.RoomObjects, roomObj);
            if(GameStatsMgr.ActiveWeapon != null) JsonFileUtils.PrettyWrite(GameStatsMgr.ActiveWeapon.GetSerializedWeapon(), weapon);
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
