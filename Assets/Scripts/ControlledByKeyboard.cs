using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhysicsController2D))]
public class ControlledByKeyboard : Entity {
  private PhysicsController2D _controller;

  private Interactor _interactor;

  [Inject]
  public IPrefabReferences PrefabReferences { get; set; }

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
      var result = Instantiate(PrefabReferences.FollowText, Vector3.zero, Quaternion.identity);

      result.transform.parent = PrefabReferences.Canvas.transform;
      result.GetComponent<FollowText>().Target = gameObject;
      result.GetComponent<FollowText>().ShowText("There's nothing there.");
      result.transform.localScale = new Vector3(1, 1, 1);
    }
  }
}