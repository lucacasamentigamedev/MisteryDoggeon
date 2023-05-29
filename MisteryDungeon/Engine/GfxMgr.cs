using System.Collections.Generic;

namespace Aiv.Fast2D.Component {
    static class GfxMgr {

        private static Dictionary<string, Texture> textures;

        static GfxMgr() {
            textures = new Dictionary<string, Texture>();
        }

        public static Texture AddTexture(string name, string path) {
            if (textures.ContainsKey(name)) return textures[name];
            textures.Add(name, new Texture(path));
            return textures[name];
        }

        public static Texture GetTexture(string name) {
            if (!textures.ContainsKey(name)) return null;
            return textures[name];
        }

        public static void ClearAll() {
            textures.Clear();
        }

    }
}
