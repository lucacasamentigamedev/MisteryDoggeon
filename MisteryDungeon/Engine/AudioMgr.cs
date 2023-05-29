using Aiv.Audio;
using System.Collections.Generic;

namespace Aiv.Fast2D.Component {
    public static class AudioMgr {

        private const int maxPlayOneShotSource = 20;
        private static AudioSource[] oneShotPool;

        private static Dictionary<int, float> volumes;
        private static Dictionary<string, AudioClip> clips;

        static AudioMgr () {
            clips = new Dictionary<string, AudioClip>();
            volumes = new Dictionary<int, float>();
            volumes.Add(0, 1);
            oneShotPool = new AudioSource[5];
            for (int i = 0; i < oneShotPool.Length; i++) {
                oneShotPool[i] = new AudioSource();
            }
        }

        public static void AddClip (string name, string path) {
            AudioClip clip = new AudioClip(path);
            clips.Add(name, clip);
        }

        public static AudioClip GetClip (string name) {
            if (!clips.ContainsKey(name)) return null;
            return clips[name];
        }

        public static void ClearAll () {
            clips.Clear();
        }

        public static void AddVolume (int index, float volume = 1) {
            volumes.Add(index, 0);
            SetVolume(index, volume);
        }

        public static void SetVolume (int index, float volume) {
            volumes[index] = volume < 0 ? 0 : volume > 1 ? 1 : volume;
        }

        public static float GetVolume (int index) {
            if (!volumes.ContainsKey(index)) return 0;
            return volumes[index];
        }

        public static void PlayOneShot (AudioClip clip, float volume) {
            for (int i = 0; i< oneShotPool.Length; i++) {
                if (oneShotPool[i].IsPlaying) continue;
                oneShotPool[i].Volume = volume;
                oneShotPool[i].Play(clip);
                return;
            }
            if (oneShotPool.Length >= maxPlayOneShotSource) return;
            int newLength = oneShotPool.Length * 2;
            newLength = newLength > maxPlayOneShotSource ? maxPlayOneShotSource : newLength;
            AudioSource[] tempAudioSources = new AudioSource[newLength];
            int j = 0; 
            for (; j < oneShotPool.Length; j++) {
                tempAudioSources[j] = oneShotPool[j];
            }
            for (; j < tempAudioSources.Length; j++) {
                tempAudioSources[j] = new AudioSource();
            }
            oneShotPool = tempAudioSources;
            PlayOneShot(clip, volume);
        }

    }
}
