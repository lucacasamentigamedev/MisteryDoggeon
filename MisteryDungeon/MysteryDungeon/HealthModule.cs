using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class HealthModule : UserComponent {

        private const float UIScale = 0.3f;
        private Vector2 UIOffset;
        private float health;
        private Transform energyBackgroundUI;
        private GameObject energyBackgroundGameObject;
        private Transform energyUI;
        private GameObject energyGameObject;

        private float currentHealth;
        public float Health {
            get { return currentHealth; }
        }
        public float HealthPercentage {
            get { return currentHealth / health; }
        }

        public HealthModule(GameObject owner, float health, Vector2 UIOffset) : base(owner) {
            this.health = health;
            currentHealth = health;
            this.UIOffset = UIOffset;
            CreateUI();
        }

        private void CreateUI() {
            GameObject tempObj = new GameObject("HealthBar_" + gameObject.Name,
                Vector2.Zero);
            energyBackgroundGameObject = tempObj;
            tempObj.AddComponent(SpriteRenderer.Factory(tempObj, "healthBarBackground",
                Vector2.UnitY * 0.5f, DrawLayer.GUI));
            energyBackgroundUI = tempObj.transform;
            energyBackgroundUI.Scale = Vector2.One * UIScale;
            GameObject tempObj2 = new GameObject("HealthBar_" + gameObject.Name, Vector2.Zero);
            energyGameObject = tempObj2;
            tempObj2.AddComponent(SpriteRenderer.Factory(tempObj2, "healthBarForeground",
                Vector2.UnitY * 0.5f, DrawLayer.GUI));
            energyUI = tempObj2.transform;
            energyUI.Scale = Vector2.One * UIScale;
        }

        public override void Update() {
            energyBackgroundUI.Position = transform.Position + UIOffset;
            energyUI.Position = transform.Position + new Vector2(UIOffset.X + 0.05f, UIOffset.Y);
        }

        public bool TakeDamage(float damage) {
            currentHealth -= damage;
            if (currentHealth > health) currentHealth = health;
            energyUI.Scale = new Vector2(UIScale * HealthPercentage, energyUI.Scale.Y);
            return currentHealth <= 0;
        }

        public override void OnEnable() {
            energyBackgroundGameObject.IsActive = true;
            energyGameObject.IsActive = true;
        }

        public override void OnDisable() {
            energyBackgroundGameObject.IsActive = false;
            energyGameObject.IsActive = false;
        }

        public void ResetHealth() {
            currentHealth = health;
            energyUI.Scale = new Vector2(UIScale * HealthPercentage, energyUI.Scale.Y);
        }
    }
}
