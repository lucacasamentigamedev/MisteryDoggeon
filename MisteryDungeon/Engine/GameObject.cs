using System.Collections.Generic;
using OpenTK;
using System;

namespace Aiv.Fast2D.Component {
    public class GameObject {

        private bool isStarted;

        private bool isActive;
        public bool IsActive {
            get { return isActive; }
            set {
                if (isActive && !value) {
                    OnDisable();
                    isActive = value;
                } else if (!isActive && value) {
                    isActive = value;
                    OnEnable();
                }
                if (!isStarted && value && Game.CurrentScene.IsInitialize) {
                    Start();
                }
            }
        }

        private string name;
        public string Name {
            get { return name; }
            set {
                name = string.IsNullOrEmpty(value) ? "GameObject" : value;
            }
        }

        public int Tag { get; set; }

        public Transform transform {
            get;
            private set;
        }

        private List<Component> components = new List<Component>();

        public GameObject (string name, Vector2 position, bool isActive = true) {
            Name = name;
            transform = new Transform(position, Vector2.One);
            this.isActive = isActive;
            RegisterToScene();
        }

        public GameObject (string name, Vector2 position, Vector2 scale, float rotation = 0, bool isActive = true) {
            Name = name;
            transform = new Transform(position, scale, rotation);
            this.isActive = isActive;
            RegisterToScene();
        }

        private void RegisterToScene () {
            Game.CurrentScene.RegisterGameObject(this);
        }

        public void AddComponent (Component component) {
            components.Add(component);
        }

        public Component AddComponent (Type type, params object[] initialization) {
            object[] parameters = new object[initialization.Length + 1];
            parameters[0] = this;
            for (int i = 0; i < initialization.Length; i++) {
                parameters[i + 1] = initialization[i];
            }
            Component component = Activator.CreateInstance(type, parameters) as Component;
            components.Add(component);
            return component;
        }

        public Component GetComponent (Type type) {
            foreach (Component component in components) {
                if (component.GetType() != type) continue;
                return component;
            }
            Type currentType;
            foreach (Component component in components) {
                currentType = component.GetType().BaseType;
                while (currentType != typeof(object)) {
                    if (currentType == type) return component;
                    currentType = currentType.BaseType;
                }
            }
            return null;
        }

        public T AddComponent<T> (params object[] initialization) where T:Component{
            object[] parameters = new object[initialization.Length + 1];
            parameters[0] = this;
            for (int i = 0; i < initialization.Length; i++) {
                parameters[i + 1] = initialization[i];
            }
            T component = Activator.CreateInstance(typeof(T), parameters) as T;
            components.Add(component);
            return component;
        }

        public T GetComponent<T> () where T : Component{
            foreach (Component c in components) {
                if (c is T) return (T)c;
            }
            return null;
        }

        public void Awake () {
            foreach (Component component in components) {
                IStartable startable = component as IStartable;
                if (startable == null) continue;
                startable.Awake();
                if (!isActive) continue;
                startable.OnEnable();
            }
        }

        public void Start () {
            isStarted = true;
            foreach(Component component in components) {
                if (!component.Enabled) continue;
                IStartable startable = component as IStartable;
                if (startable == null) continue;
                startable.Start();
            }
        }

        public void Update () {
            foreach (Component component in components) {
                if (!component.Enabled) continue;
                IUpdatable updatable = component as IUpdatable;
                if (updatable == null) continue;
                updatable.Update();
            }
        }

        public void LateUpdate() {
            foreach (Component component in components) {
                if (!component.Enabled) continue;
                IUpdatable updatable = component as IUpdatable;
                if (updatable == null) continue;
                updatable.LateUpdate();
            }
        }

        public void OnEnable () {
            foreach (Component component in components) {
                if (!component.Enabled) continue;
                IStartable temp = component as IStartable;
                if (temp == null) continue;
                temp.OnEnable();
            }
        }

        public void OnDisable() {
            foreach (Component component in components) {
                if (!component.Enabled) continue;
                IStartable temp = component as IStartable;
                if (temp == null) continue;
                temp.OnDisable();
            }
        }

        public void OnDestroy() {
            foreach (Component component in components) {
                IStartable temp = component as IStartable;
                if (temp == null) continue;
                temp.OnDestroy();
            }
        }

        public void OnCollide(Collision other) {
            foreach (Component component in components) {
                if (!component.Enabled) continue;
                ICollidable collidable = component as ICollidable;
                if (collidable == null) continue;
                collidable.OnCollide(other);
            }
        }

        public void ClearAll () {
            components.Clear();
        }

        public static GameObject Find (string name) {
            return Game.CurrentScene.Find(name);
        }

        public static GameObject Clone (GameObject gameObjectToClone) {
            GameObject clone = new GameObject(gameObjectToClone.name + "_Clone",
                gameObjectToClone.transform.Position, gameObjectToClone.IsActive);
            foreach(Component component in gameObjectToClone.components) {
                clone.AddComponent(component.Clone (clone));
            }
            return clone;
        }

    }
}
