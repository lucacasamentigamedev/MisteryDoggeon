using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Fast2D.Component.AI {
    public abstract class State {

        protected FSM machine;

        public virtual void OnEnter () {

        }

        public virtual void OnExit () {

        }

        public virtual void Awake () {

        }

        public virtual void Start () {

        }

        public virtual void Update () {

        }

        public virtual void LateUpdate () {

        }

        public virtual void OnCollide (Collision collisionInfo) {

        }

        public void AssignStateMachine (FSM machine) {
            this.machine = machine;
        }


    }
}
