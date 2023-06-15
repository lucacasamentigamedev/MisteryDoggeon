using Aiv.Audio;
using System;

namespace Aiv.Fast2D.Component {

    public enum SFXList {
        ObjectBroke,
        ObjectPicked,
        SequenceRight,
        SequenceWrong,
        SequenceCompleted,
        ArrowShot,
        PathUnreachable,
        PuzzleReady,
        ClockTick,
        RoomLeft,
        PlayerTakesDamage,
        PlayerDead,
        EnemyTakesDamage,
        EnemyDead,
        BossDefeated,
        HordeDefeated,
        last
    }

    public class SFXMgr : UserComponent {


        private AudioClip[] mySFX;
        private AudioSourceComponent audioSource;

        public SFXMgr(GameObject owner) : base(owner) {
            mySFX = new AudioClip[(int)SFXList.last];
            mySFX[0] = AudioMgr.GetClip("objectBroke");
            mySFX[1] = AudioMgr.GetClip("objectPicked");
            mySFX[2] = AudioMgr.GetClip("sequenceRight");
            mySFX[3] = AudioMgr.GetClip("sequenceWrong");
            mySFX[4] = AudioMgr.GetClip("sequenceCompleted");
            mySFX[5] = AudioMgr.GetClip("arrowShot");
            mySFX[6] = AudioMgr.GetClip("pathUnreachable");
            mySFX[7] = AudioMgr.GetClip("puzzleReady");
            mySFX[8] = AudioMgr.GetClip("clockTick");
            mySFX[9] = AudioMgr.GetClip("roomLeft");
            mySFX[10] = AudioMgr.GetClip("playerTakesDamage");
            mySFX[11] = AudioMgr.GetClip("playerDead");
            mySFX[12] = AudioMgr.GetClip("enemyTakesDamage");
            mySFX[13] = AudioMgr.GetClip("enemyDead");
            mySFX[14] = AudioMgr.GetClip("bossDefeated");
            mySFX[15] = AudioMgr.GetClip("hordeDefeated");
        }

        public override void Awake() {
            audioSource = GetComponent<AudioSourceComponent>();
        }

        public override void Start() {
            EventManager.AddListener(EventList.ObjectBroke, OnObjectBroke);
            EventManager.AddListener(EventList.ObjectPicked, OnObjectPicked);
            EventManager.AddListener(EventList.SequenceRight, OnSequenceRight);
            EventManager.AddListener(EventList.SequenceCompleted, OnSequenceCompleted);
            EventManager.AddListener(EventList.SequenceWrong, OnSequenceWrong);
            EventManager.AddListener(EventList.ArrowShot, OnArrowShot);
            EventManager.AddListener(EventList.PathUnreachable, OnPathUnreachable);
            EventManager.AddListener(EventList.PuzzleReady, OnPuzzleReady);
            EventManager.AddListener(EventList.ClockTick, OnClockTick);
            EventManager.AddListener(EventList.RoomLeft, OnRoomLeft);
            EventManager.AddListener(EventList.PlayerTakesDamage, OnPlayerTakesDamage);
            EventManager.AddListener(EventList.PlayerDead, OnPlayerDead);
            EventManager.AddListener(EventList.EnemyTakesDamage, OnEnemyTakesDamage);
            EventManager.AddListener(EventList.EnemyDead, OnEnemyDead);
            EventManager.AddListener(EventList.BossDefeated, OnBossDead);
            EventManager.AddListener(EventList.HordeDefeated, OnHordeDefeated);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.ObjectBroke, OnObjectBroke);
            EventManager.RemoveListener(EventList.ObjectPicked, OnObjectPicked);
            EventManager.RemoveListener(EventList.SequenceRight, OnSequenceRight);
            EventManager.RemoveListener(EventList.SequenceCompleted, OnSequenceCompleted);
            EventManager.RemoveListener(EventList.SequenceWrong, OnSequenceWrong);
            EventManager.RemoveListener(EventList.ArrowShot, OnArrowShot);
            EventManager.RemoveListener(EventList.PathUnreachable, OnPathUnreachable);
            EventManager.RemoveListener(EventList.PuzzleReady, OnPuzzleReady);
            EventManager.RemoveListener(EventList.ClockTick, OnClockTick);
            EventManager.RemoveListener(EventList.RoomLeft, OnRoomLeft);
            EventManager.RemoveListener(EventList.PlayerTakesDamage, OnPlayerTakesDamage);
            EventManager.RemoveListener(EventList.PlayerDead, OnPlayerDead);
            EventManager.RemoveListener(EventList.EnemyTakesDamage, OnEnemyTakesDamage);
            EventManager.RemoveListener(EventList.EnemyDead, OnEnemyDead);
            EventManager.RemoveListener(EventList.BossDefeated, OnBossDead);
            EventManager.RemoveListener(EventList.HordeDefeated, OnHordeDefeated);
        }

        public void OnObjectBroke(EventArgs message) {
            PlaySFX(SFXList.ObjectBroke);
        }

        public void OnObjectPicked(EventArgs message) {
            PlaySFX(SFXList.ObjectPicked);
        }

        public void OnSequenceRight(EventArgs message) {
            PlaySFX(SFXList.SequenceRight);
        }
        public void OnSequenceCompleted(EventArgs message) {
            PlaySFX(SFXList.SequenceCompleted);
        }
        public void OnSequenceWrong(EventArgs message) {
            PlaySFX(SFXList.SequenceWrong);
        }
        public void OnArrowShot(EventArgs message) {
            PlaySFX(SFXList.ArrowShot);
        }
        public void OnPathUnreachable(EventArgs message) {
            PlaySFX(SFXList.PathUnreachable);
        }
        
        public void OnPuzzleReady(EventArgs message) {
            PlaySFX(SFXList.PuzzleReady);
        }
        
        public void OnClockTick(EventArgs message) {
            PlaySFX(SFXList.ClockTick);
        }
        
        public void OnRoomLeft(EventArgs message) {
            PlaySFX(SFXList.RoomLeft);
        }
        
        public void OnPlayerTakesDamage(EventArgs message) {
            PlaySFX(SFXList.PlayerTakesDamage);
        }
        
        public void OnPlayerDead(EventArgs message) {
            PlaySFX(SFXList.PlayerDead);
        }
        
        public void OnEnemyTakesDamage(EventArgs message) {
            PlaySFX(SFXList.EnemyTakesDamage);
        }

        public void OnEnemyDead(EventArgs message) {
            PlaySFX(SFXList.EnemyDead);
        }

        public void OnBossDead(EventArgs message) {
            PlaySFX(SFXList.BossDefeated);
        }
        
        public void OnHordeDefeated(EventArgs message) {
            PlaySFX(SFXList.HordeDefeated);
        }

        private void PlaySFX(SFXList sfx) {
            audioSource.PlayOneShot(mySFX[(int)sfx]);
        }
    }
}
