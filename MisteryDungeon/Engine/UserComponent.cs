namespace Aiv.Fast2D.Component {
    public abstract class UserComponent : Component, IStartable, IUpdatable, ICollidable {

        public override bool Enabled {
            get {
                return base.Enabled;
            }
            set {
                if (gameObject.IsActive) {
                    if (Enabled && !value) {
                        OnDisable();
                    } else if (!Enabled && value) {
                        OnEnable();
                    }
                }
                base.Enabled = value;
            }
        }

        public UserComponent (GameObject owner) : base (owner) {

        }

        public virtual void Awake() {

        }

        public virtual void OnEnable () {

        }

        public virtual void OnDisable () {

        }

        public virtual void Start() {

        }

        public virtual void Update () {

        }

        public virtual void LateUpdate() {

        }

        public virtual void OnCollide (Collision collisionInfo) {

        }

        public virtual void OnDestroy() {

        }

    }
}
