//C# Example
using UnityEditor;
using UnityEngine;

namespace EditorExtension {
    public class LowerTerrain : EditorWindow {
        float amount = 10;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Terrain/Lower Terrain")]
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(LowerTerrain));
        }

        void OnGUI() {
            Terrain terrain = null;
            foreach(GameObject g in Selection.gameObjects) {
                terrain = g.GetComponent<Terrain>();
                if (terrain != null)
                    break;
            }

            EditorGUILayout.LabelField("Terrain: " + (terrain == null ? "No Terrain Selected" : terrain.name));

            EditorGUILayout.DoubleField("Lower by (m)", amount);

            if (terrain != null && GUILayout.Button("Lower Terrain")) {
                TerrainFunctions.LowerTerrain(terrain, amount);
            }
        }

    }
}
