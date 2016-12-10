using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhysicsController2D))]
public class ControlledByKeyboard : Entity {
  private PhysicsController2D _controller;

  void Awake() {
    _controller = GetComponentSafe<PhysicsController2D>();
  }

  void Update() {
    if (Input.GetKey(KeyCode.D)) {
      _controller.AddHorizontalForce(0.5f);
    }

    if (Input.GetKey(KeyCode.A)) {
      _controller.AddHorizontalForce(-0.5f);
    }

    if (Input.GetKey(KeyCode.W)) {
      _controller.AddVerticalForce(0.5f);
    }

    if (Input.GetKey(KeyCode.S)) {
      _controller.AddVerticalForce(-0.5f);
    }
  }
}