﻿using Aiv.Fast2D.Component;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    public struct GameStatsSerialized {
        public bool PuzzleResolved { get; set; }
        public bool PlayerCanShoot { get; set; }
        public List<int> CollectedKeys { get; set; }
        public List<int> CollectedWeapons { get; set; }
        public int PreviousRoom { get; set; }
        public int ActualRoom { get; set; }
        public int HordesDefeated { get; set; }
        public bool BossDefeated { get; set; }
        public float PlayerHealth { get; set; }
        public bool FirstDoorPassed { get; set; }
        public GameStatsSerialized(bool PuzzleResolved, bool PlayerCanShoot,
            List<int> CollectedKeys, List<int> CollectedWeapons, int PreviousRoom, int ActualRoom, int HordesDefeated,
            bool BossDefeated, float PlayerHealth, bool FirstDoorPassed) {
            this.PuzzleResolved = PuzzleResolved;
            this.PlayerCanShoot = PlayerCanShoot;
            this.CollectedKeys = CollectedKeys;
            this.CollectedWeapons = CollectedWeapons;
            this.PreviousRoom = PreviousRoom;
            this.ActualRoom = ActualRoom;
            this.BossDefeated = BossDefeated;
            this.PlayerHealth = PlayerHealth;
            this.FirstDoorPassed = FirstDoorPassed;
            this.HordesDefeated = HordesDefeated;
        }
    }

    static class GameStatsMgr {

        public static bool PuzzleResolved { get; set; }
        public static bool PlayerCanShoot { get; set; }
        public static Weapon ActiveWeapon { get; set; }
        public static List<int> collectedWeapons { get; set; }
        public static List<int> CollectedWeapons {
            get { return collectedWeapons; }
            set { collectedWeapons = value; }
        }
        private static List<int> collectedKeys;
        public static List<int> CollectedKeys {
            get { return collectedKeys; }
            set { collectedKeys = value; }
        }
        public static int PreviousRoom { get; set; }
        public static int ActualRoom { get; set; }
        public static int HordesDefeated { get; set; }
        public static bool BossDefeated { get; set; }
        public static float maxPlayerHealth = 20;
        private static float playerHealth = 20;
        public static float PlayerHealth {
            get { return playerHealth; }
            set { playerHealth = value; }
        }
        public static bool FirstDoorPassed { get; set; }

        static GameStatsMgr() {
            CollectedKeys = new List<int>();
            CollectedWeapons = new List<int>();
        }

        public static void ResetGameStats() {
            PuzzleResolved = false;
            PlayerCanShoot = false;
            ActiveWeapon = null;
            CollectedKeys.Clear();
            CollectedWeapons.Clear();
            PreviousRoom = 0;
            ActualRoom = 0;
            BossDefeated = false;
            PlayerHealth = maxPlayerHealth;
            FirstDoorPassed = false;
            HordesDefeated = 0;
        }

        public static GameStatsSerialized GetGameStats() {
            return new GameStatsSerialized(PuzzleResolved, PlayerCanShoot,
            CollectedKeys, CollectedWeapons, PreviousRoom, ActualRoom, HordesDefeated,
            BossDefeated, PlayerHealth, FirstDoorPassed);
        }

        public static void LoadGameStats(GameStatsSerialized gameStats) {
            PuzzleResolved = gameStats.PuzzleResolved;
            PlayerCanShoot = gameStats.PlayerCanShoot;
            CollectedKeys = gameStats.CollectedKeys;
            CollectedWeapons = gameStats.CollectedWeapons;
            PreviousRoom = gameStats.PreviousRoom;
            ActualRoom = gameStats.ActualRoom;  
            BossDefeated = gameStats.BossDefeated;
            PlayerHealth = gameStats.PlayerHealth;
            FirstDoorPassed = gameStats.FirstDoorPassed;
            HordesDefeated = gameStats.HordesDefeated;
            Console.WriteLine();
        }

        public static void LoadActiveWeapon(WeaponSerialized weapon) {
            GameObject owner = GameObject.Find("Object_" + weapon.RoomId + "_" + weapon.Id);
            ActiveWeapon = new Weapon(owner, (WeaponType)weapon.WeaponType, (BulletType)weapon.BulletType, weapon.ReloadTime,
                new Vector2(weapon.OffsetShootX, weapon.OffsetShootY), weapon.RoomId, weapon.Id);
        }
    }
}
