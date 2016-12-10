using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhysicsController2D))]
public class ControlledByKeyboard : Entity {
  private PhysicsController2D _controller;

  private Interactor _interactor;

  void Awake() {
    _controller = GetComponentSafe<PhysicsController2D>();
    _interactor = GetComponentSafe<Interactor>();
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

    if (Input.GetKey(KeyCode.Space)) {
      Interact();
    }
  }

  private void Interact() {
    var target = _interactor.GetTarget();

    if (!target) {
      return;
    }
  }
}