public class PrefabReferences : IPrefabReferences {
  public InteractableIcon InteractableIcon { get; set; }
}

public interface IPrefabReferences {
  InteractableIcon InteractableIcon { get; set; }
}