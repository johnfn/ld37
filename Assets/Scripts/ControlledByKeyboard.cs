using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace johnfn {

  interface ICanPressSpace {
    /**
    * returns true if we can't interact with it any more
    */
    bool PressSpace();
  }

  [DisallowMultipleComponent]
  [RequireComponent(typeof(PhysicsController2D))]
  public class ControlledByKeyboard : Entity {
    private PhysicsController2D _controller;

    private Interactor _interactor;

    private List<ICanPressSpace> _things;

    [Inject]
    public IPrefabReferences PrefabReferences { get; set; }

    void Awake() {
      _controller = GetComponentSafe<PhysicsController2D>();
      _interactor = GetComponentSafe<Interactor>();
      _things = new List<ICanPressSpace>();
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

      if (Input.GetKeyDown(KeyCode.Space)) {
        // Try to find something to interact with

        var interacted = false;

        if (_things.Count > 0) {
          var thing = _things[0];
          var result = thing.PressSpace();

          if (result) {
            _things.Remove(thing);
          }

          interacted = true;
        } else {
          // fail

          if (!interacted) {
            Interact();
          }
        }
      }
    }

    private void Interact() {
      var target = _interactor.GetTarget();

      if (!target) {
        var result = PrefabReferences.CreateFollowText(gameObject, "There's nothing there.");

        _things.Add(result);
        return;
      }

      switch (target.InteractType) {
        case InteractableTypes.BED:
          var panel = PrefabReferences.TimeSelectionCanvas.GetComponent<SleepUntilPanel>();

          panel.Show();

          break;
        default:
          Debug.LogError("Unknown interact type!");

          break;
      }
    }
  }
}