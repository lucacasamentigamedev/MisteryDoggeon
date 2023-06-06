using OpenTK;
using System.Collections.Generic;

namespace Aiv.Fast2D.Component {
    public class SheetAnimator : Component, IUpdatable, IStartable{

        private SpriteRenderer spriteRenderer;
        private List<SheetClip> myClip;
        private SheetClip currentClip;

        public SheetClip CurrentClip { get { return currentClip; } }

        private float sliceTime;
        private float currentSliceTime;
        private int currentFrameIndex;


        public SheetAnimator (GameObject owner, SpriteRenderer spriteRenderer) : base (owner) {
            this.spriteRenderer = spriteRenderer;
            myClip = new List<SheetClip>();
        }

        public override Component Clone(GameObject owner) {
            SheetAnimator clone =  new SheetAnimator(owner, 
                owner.GetComponent<SpriteRenderer>());
            for (int i = 0; i < myClip.Count; i++) {
                clone.AddClip(myClip[i]);
            }
            return clone;
        }

        public void AddClip (SheetClip clip) {
            myClip.Add(clip);
        }

        public void Start () {
            ChangeClip(myClip[0].AnimationName);
        }


        public void ChangeClip (string name) {
            for (int i = 0; i < myClip.Count; i++) {
                if (myClip[i].AnimationName != name) continue;
                currentClip = myClip[i];
                sliceTime = 1f / currentClip.FPS;
                currentSliceTime = 0;
                currentFrameIndex = 0;
                spriteRenderer.Texture = currentClip.Texture;
                SetNewFrame(currentClip.Frames[currentFrameIndex]);
            }
        }

        private void SetNewFrame (int index) {
            int rowIndex = index / currentClip.NumberOfColumn;
            int columnIndex = index % currentClip.NumberOfColumn;
            spriteRenderer.TextureOffset = new Vector2(columnIndex * currentClip.FrameWidth,
                rowIndex * currentClip.FrameHeight);
        }

        public void LateUpdate () {
            currentSliceTime += Game.DeltaTime;
            if (currentSliceTime < sliceTime) return;
            currentSliceTime = 0;
            currentFrameIndex++;
            if (currentFrameIndex < currentClip.Frames.Length) {
                SetNewFrame(currentClip.Frames[currentFrameIndex]);
            } else {
                if (currentClip.Loop) {
                    currentFrameIndex = currentFrameIndex % currentClip.Frames.Length;
                    SetNewFrame(currentClip.Frames[currentFrameIndex]);
                } else {
                    if (string.IsNullOrEmpty(currentClip.NextAnimation)) return;
                    ChangeClip(currentClip.NextAnimation);
                }
            }
        }

        public void Update() {
        }

        public void Awake() {
        }

        public void OnEnable() {
        }

        public void OnDisable() {
        }

        public void OnDestroy() {
        }
    }
}
