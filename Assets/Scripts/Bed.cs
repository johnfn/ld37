using UnityEngine;
using UnityEngine.UI;
using Zenject;

[DisallowMultipleComponent]
public class Bed : Entity {
  [Inject]
  public ITimeManager TimeManager { get; set; }
}