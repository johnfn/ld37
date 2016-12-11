using System.Collections.Generic;
using UnityEngine;

namespace johnfn {
    [Tiled2Unity.CustomTiledImporter]
    class MyCustomTiledImporter : Tiled2Unity.ICustomTiledImporter {
        public void HandleCustomProperties(GameObject gameObject,
            IDictionary<string, string> props) {

            if (props.ContainsKey("Name")) {
                var name = props["Name"];

                switch (name) {
                    case "Bed":
                    {
                        var interactable = gameObject.AddComponent<Interactable>();

                        interactable.InteractVerb = Interactable.InteractTypes["Bed"].Verb;
                        interactable.InteractType = Interactable.InteractTypes["Bed"].Type;
                    }
                    break;

                    case "NPCRec":
                    {
                        var interactable = gameObject.AddComponent<Interactable>();

                        interactable.InteractVerb = Interactable.InteractTypes["NPCRec"].Verb;
                        interactable.InteractType = Interactable.InteractTypes["NPCRec"].Type;
                    }
                    break;

                    case "NPCDrifter":
                    {
                        var interactable = gameObject.AddComponent<Interactable>();

                        interactable.InteractVerb = Interactable.InteractTypes["NPCDrifter"].Verb;
                        interactable.InteractType = Interactable.InteractTypes["NPCDrifter"].Type;

                        gameObject.AddComponent<NPC>();
                    }
                    break;

                    default:
                        Debug.Log("Unrecognized type " + name);

                        break;
                }
            }

            // get component by string

            /*
            if (props.ContainsKey("AddComp")) {
                Debug.Log("Look up " + props["AddComp"]);

                var stringName = props["AddComp"];
                Assembly asm = typeof(Entity).Assembly;
                Type type = asm.GetType(props["AddComp"]);

                gameObject.AddComponent(type);
            }
            */
        }

        public void CustomizePrefab(GameObject prefab) {
            Debug.Log(prefab.name);

            prefab.name = "Woooo";
        }
    }
}