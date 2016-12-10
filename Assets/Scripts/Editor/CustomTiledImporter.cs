using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

[Tiled2Unity.CustomTiledImporter]
class MyCustomTiledImporter : Tiled2Unity.ICustomTiledImporter {
    public void HandleCustomProperties(GameObject gameObject,
        IDictionary<string, string> props) {

        if (props.ContainsKey("AddComp")) {
            Debug.Log("Look up " + props["AddComp"]);

            var stringName = props["AddComp"];
            Assembly asm = typeof(Entity).Assembly;
            Type type = asm.GetType(props["AddComp"]);

            gameObject.AddComponent(type);
        }
    }

    public void CustomizePrefab(GameObject prefab) {
        Debug.Log(prefab.name);

        prefab.name = "Woooo";
    }
}