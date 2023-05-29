using System.Collections.Generic;

namespace Aiv.Fast2D.Component {

    public enum RigidbodyType { Player = 1, Bullet = 2, Box = 4 }

    public static class PhysicsMgr {

        private static List<Rigidbody> items;

        static Collision collisionInfo;

        static PhysicsMgr() {
            items = new List<Rigidbody>();
        }

        public static void AddItem(Rigidbody item) {
            if (items.Contains(item)) return;
            items.Add(item);
        }

        public static void RemoveItem(Rigidbody item) {
            if (!items.Contains(item)) return;
            items.Remove(item);
        }

        public static void ClearAll() {
            items.Clear();
        }

        public static void FixedUpdate() {
            for (int i = 0; i < items.Count; i++) {
                if (!items[i].Enabled) continue;
                items[i].FixedUpdate();
            }
        }

        public static void CheckCollisions() {
            for (int i = 0; i < items.Count - 1; i++) {
                if (!items[i].Enabled || !items[i].IsCollisionAffected) continue;
                for (int j = i + 1; j < items.Count; j++) {
                    if (!items[j].Enabled || !items[j].IsCollisionAffected) continue;
                    bool firstCheck = items[i].CanInteract(items[j].Type);
                    bool secondCheck = items[j].CanInteract(items[i].Type);
                    if (!firstCheck && !secondCheck) continue;
                    bool collision = items[i].Collides(items[j], ref collisionInfo);
                    if (!collision) continue;
                    if (firstCheck) {
                        collisionInfo.Collider = items[j].Collider;
                        items[i].gameObject.OnCollide(collisionInfo);
                    }
                    if (secondCheck) {
                        collisionInfo.Collider = items[i].Collider;
                        items[j].gameObject.OnCollide(collisionInfo);
                    }
                }
            }
        }

    }
}
