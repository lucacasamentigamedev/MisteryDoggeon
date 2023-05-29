namespace Aiv.Fast2D.Component {
    interface IUpdatable {

        bool Enabled { get; }
        void Update();
        void LateUpdate();

    }
}
