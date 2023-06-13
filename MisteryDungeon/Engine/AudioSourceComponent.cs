using Aiv.Audio;

enum AudioSourceStatus {
    play,
    pause,
    stop
}

namespace Aiv.Fast2D.Component {

    public class AudioSourceComponent : Component {

        private AudioSourceStatus myStatus;

        private AudioSource internalAudioSource;
        private AudioClip myClip;

        private int myType;
        public int MyType {
            get { return myType; }
            set {
                myType = value;
                internalAudioSource.Volume = myVolume * AudioMgr.GetVolume(myType);
            }
        }
        private int myVolume;
        public int MyVolume {
            get { return myVolume; }
            set {
                myVolume = value < 0 ? 0 : value > 2 ? 2 : value;
                internalAudioSource.Volume = myVolume * AudioMgr.GetVolume(MyType);
            }
        }

        public bool Loop {
            get;
            set;
        }

        public AudioSourceComponent (GameObject owner) : base (owner) {
            myStatus = AudioSourceStatus.stop;
            internalAudioSource = new AudioSource();
            MyVolume = 1;
        }

        public void SetClip (AudioClip clip) {
            myClip = clip; 
        }

        public void Play () {
            if (myClip == null) return;
            if (myStatus == AudioSourceStatus.play) return;
            switch (myStatus) {
                case AudioSourceStatus.pause:
                    internalAudioSource.Resume();
                    break;
                case AudioSourceStatus.stop:
                    internalAudioSource.Play(myClip, Loop);
                    break;
            }
            myStatus = AudioSourceStatus.play;
        }

        public void Pause () {
            if (myStatus != AudioSourceStatus.play) return;
            internalAudioSource.Pause();
            myStatus = AudioSourceStatus.pause;
        }

        public void Stop () {
            if (myStatus == AudioSourceStatus.stop) return;
            internalAudioSource.Stop();
            myStatus = AudioSourceStatus.stop;
        }

        public void PlayOneShot (AudioClip clipToPlay) {
            AudioMgr.PlayOneShot(clipToPlay, MyVolume * AudioMgr.GetVolume(MyType));
        }

    }
}
