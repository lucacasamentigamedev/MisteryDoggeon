namespace Aiv.Fast2D.Component {
    interface ICollidable {

        bool Enabled { get; }
        void OnCollide(Collision other);

    }
}
