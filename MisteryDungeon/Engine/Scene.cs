using Aiv.Fast2D.Component.UI;
using System;
using System.Collections.Generic;

namespace Aiv.Fast2D.Component {
    public abstract class Scene {

        protected List<GameObject> sceneObjects = new List<GameObject>();
        protected bool isInitialize;
        public bool IsInitialize {
            get { return isInitialize; }
        }

        public virtual void InitializeScene () {
            LoadAssets();
            isInitialize = true;
        }

        public void Awake () {
            foreach( GameObject obj in sceneObjects) {
                obj.Awake();
            }
        }

        public void Start () {
            foreach (GameObject obj in sceneObjects) {
                if (!obj.IsActive) continue;
                obj.Start();
            }
        }

        public virtual void Update() {
            foreach (GameObject obj in sceneObjects) {
                if (!obj.IsActive) continue;
                obj.Update();
            }
        }

        public void LateUpdate () {
            foreach (GameObject obj in sceneObjects) {
                if (!obj.IsActive) continue;
                obj.LateUpdate();
            }
        }

        public GameObject Find (string name) {
            foreach(GameObject obj in sceneObjects) {
                if (obj.Name != name) continue;
                return obj;
            }
            return null;
        }

        public void RegisterGameObject (GameObject go) {
            sceneObjects.Add(go);
        }

        public virtual void DestroyScene () {
            foreach (GameObject obj in sceneObjects) {
                if (obj.IsActive) obj.OnDisable();
                obj.OnDestroy();
                obj.ClearAll();
            }
            sceneObjects.Clear();
            sceneObjects = null;
            DrawMgr.ClearAll();
            PhysicsMgr.ClearAll();
            GfxMgr.ClearAll();
            FontMgr.ClearAll();
            CameraMgr.ClearAll();
            AudioMgr.ClearAll();
            GC.Collect();
        }

        protected virtual void LoadAssets () {

        }

    }
}
