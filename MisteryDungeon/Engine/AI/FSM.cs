using System.Collections.Generic;

namespace Aiv.Fast2D.Component.AI {
    public class FSM : UserComponent {

        private Dictionary<int, State> states;
        private State currentState;

        private int startState = int.MinValue;

        public FSM (GameObject owner) : base (owner) {
            states = new Dictionary<int, State>();
        }

        public void SetStartState (int startState) {
            this.startState = startState;
        }

        public void RegisterState (int id, State state) {
            states.Add(id, state);
            state.AssignStateMachine(this);
        }

        public void Switch (int stateID) {
            if (currentState != null) currentState.OnExit();
            currentState = states[stateID];
            currentState.OnEnter();
        }

        public override void Awake() {
            foreach (var state in states) {
                state.Value.Awake();
            }
        }

        public override void Start() {
            foreach (var state in states) {
                state.Value.Start();
            }
        }

        public override void OnEnable() {
            if (startState == int.MinValue) return;
            Switch(startState);
        }

        public override void Update() {
            if (currentState == null) return;
            currentState.Update();
        }

        public override void LateUpdate() {
            if (currentState == null) return;
            currentState.LateUpdate();
        }

        public override void OnCollide(Collision collisionInfo) {
            if (currentState == null) return;
            currentState.OnCollide(collisionInfo);
        }

    }
}
