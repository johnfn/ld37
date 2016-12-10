using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
public class Manager: MonoBehaviour {
  [Inject]
  public IUtil Util { get; set; }

  [Inject]
  public ITimeManager TimeManager { get; set; }

  void Start() {

  }

  void Update() {
    TimeManager.Update();
  }
}