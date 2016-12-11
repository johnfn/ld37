using UnityEngine;

namespace johnfn {
    [DisallowMultipleComponent]
    public class ControllableStats : Entity
    {
        public float Gravity;

        public float HorizontalSpeed;

        public float JumpHeight;

        public float Friction;

        public float MaxHorizontalSpeed;
    }
}