using Aiv.Fast2D.Component;
using MisteryDungeon.MysteryDungeon.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using static MisteryDungeon.MysteryDungeon.Utility.JsonFileUtils;

namespace MisteryDungeon.MysteryDungeon.Mgr {
    public class MemoryCardMgr : UserComponent {

        private string stats;
        private string movGrid;
        private string roomObj;
        private string weapon;

        private GameStatsMgr gsm;

        public MemoryCardMgr(GameObject owner) : base(owner) {
            stats = Path.Combine("Saves", GameConfigMgr.gameStatsFileName);
            movGrid = Path.Combine("Saves", GameConfigMgr.movementGridFileName);
            roomObj = Path.Combine("Saves", GameConfigMgr.roomObjectsFileName);
            weapon = Path.Combine("Saves", GameConfigMgr.weaponFileName);
        }

        public override void Awake() {
            gsm = GameObject.Find("GameStatsMgr").GetComponent<GameStatsMgr>();
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
            EventManager.CastEvent(EventList.LOG_MemoryCard, EventArgsFactory.LOG_Factory("New game"));
            gsm.ResetGameStats();
            MovementGridMgr.ResetMovementsGrids();
            RoomObjectsMgr.ResetRoomObjects();
            File.WriteAllText(stats, string.Empty);
            File.WriteAllText(movGrid, string.Empty);
            File.WriteAllText(roomObj, string.Empty);
            File.WriteAllText(weapon, string.Empty);
        }

        public void OnSaveGame(EventArgs message) {
            EventManager.CastEvent(EventList.LOG_MemoryCard, EventArgsFactory.LOG_Factory("Save game"));
            PrettyWrite(gsm.GetGameStats(), stats);
            WriteJaggeredArray(MovementGridMgr.Grids, movGrid);
            PrettyWrite(RoomObjectsMgr.RoomObjects, roomObj);
            if(GameStats.ActiveWeapon != null) PrettyWrite(GameStats.ActiveWeapon.GetSerializedWeapon(), weapon);
            EventManager.CastEvent(EventList.EndLoading, EventArgsFactory.EndLoadingFactory());
        }

        public void OnLoadGame(EventArgs message) {
            var file = new FileInfo(stats);
            if (file.Length == 0) {
                EventManager.CastEvent(EventList.LOG_MemoryCard, EventArgsFactory.LOG_Factory("Non ci sono salvataggi, faccio un nuovo gioco"));
                OnNewGame(message);
            } else {
                EventManager.CastEvent(EventList.LOG_MemoryCard, EventArgsFactory.LOG_Factory("Load game"));
                gsm.LoadGameStats(ReadJson<GameStatsSerialized>(stats));
                MovementGridMgr.LoadMovementsGrids(ReadJaggeredArray<List<MovementGridArrayElemSerialized>>(movGrid));
                RoomObjectsMgr.LoadRoomObjects(ReadJson<Dictionary<int, bool>[]>(roomObj));
                if (new FileInfo(weapon).Length != 0) gsm.LoadActiveWeapon(ReadJson<WeaponSerialized>(weapon));
            }
        }
    }
}
