using Aiv.Audio;
using System;

namespace Aiv.Fast2D.Component {

    public enum SFXList {
        objectBroke,
        objectPicked,
        sequenceRight,
        sequenceWrong,
        sequenceCompleted,
        arrowShooted,
        pathUnreachable,
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
            mySFX[5] = AudioMgr.GetClip("arrowShooted");
            mySFX[6] = AudioMgr.GetClip("pathUnreachable");
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
            EventManager.AddListener(EventList.ArrowShooted, OnArrowShooted);
            EventManager.AddListener(EventList.PathUnreachable, OnPathUnreachable);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.ObjectBroke, OnObjectBroke);
            EventManager.RemoveListener(EventList.ObjectPicked, OnObjectPicked);
            EventManager.RemoveListener(EventList.SequenceRight, OnSequenceRight);
            EventManager.RemoveListener(EventList.SequenceCompleted, OnSequenceCompleted);
            EventManager.RemoveListener(EventList.SequenceWrong, OnSequenceWrong);
            EventManager.RemoveListener(EventList.ArrowShooted, OnArrowShooted);
            EventManager.RemoveListener(EventList.PathUnreachable, OnPathUnreachable);
        }

        public void OnObjectBroke(EventArgs message) {
            PlaySFX(SFXList.objectBroke);
        }

        public void OnObjectPicked(EventArgs message) {
            PlaySFX(SFXList.objectPicked);
        }

        public void OnSequenceRight(EventArgs message) {
            PlaySFX(SFXList.sequenceRight);
        }
        public void OnSequenceCompleted(EventArgs message) {
            PlaySFX(SFXList.sequenceCompleted);
        }
        public void OnSequenceWrong(EventArgs message) {
            PlaySFX(SFXList.sequenceWrong);
        }
        public void OnArrowShooted(EventArgs message) {
            PlaySFX(SFXList.arrowShooted);
        }
        public void OnPathUnreachable(EventArgs message) {
            PlaySFX(SFXList.pathUnreachable);
        }

        private void PlaySFX(SFXList sfx) {
            audioSource.PlayOneShot(mySFX[(int)sfx]);
        }
    }
}
