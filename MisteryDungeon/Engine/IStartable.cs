namespace Aiv.Fast2D.Component {
    interface IStartable {

        bool Enabled { get; }

        void Awake();
        void OnEnable();
        void OnDisable();
        void Start();
        void OnDestroy();

    }
}
