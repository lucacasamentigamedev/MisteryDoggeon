using System;

namespace Aiv.Fast2D.Component {
    public abstract class Component {

        private bool enabled;
        public virtual bool Enabled {
            get { return enabled && gameObject.IsActive; }
            set { enabled = value; }
        }

        public bool InternalEnabled {
            get { return enabled; }
        }

        public GameObject gameObject {
            get;
            private set;
        }

        public Transform transform {
            get {
                return gameObject.transform;
            }
        }

        public Component (GameObject owner) {
            enabled = true;
            gameObject = owner;
        }

        public Component GetComponent (Type type) {
            return gameObject.GetComponent(type);
        }

        public T GetComponent<T> () where T:Component{
            return gameObject.GetComponent<T>();
        }

        public virtual Component Clone (GameObject owner) {
            throw new NotImplementedException();
        }
    }
}
