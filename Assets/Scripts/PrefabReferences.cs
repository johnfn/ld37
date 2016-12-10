using UnityEngine;

public class PrefabReferences : IPrefabReferences {
  public InteractableIcon InteractableIcon { get; set; }

  public FollowText FollowText { get; set; }

  public GameObject Canvas { get; set; }
}

public interface IPrefabReferences {
  InteractableIcon InteractableIcon { get; set; }

  FollowText FollowText { get; set; }

  GameObject Canvas { get; set; }
}