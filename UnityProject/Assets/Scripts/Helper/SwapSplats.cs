#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine;

namespace EditorExtension {
    public class SwapSplats : EditorWindow {
        private int splatIndex1 = 0;
        private int splatIndex2 = 1;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Terrain/Swap Splats")]
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(SwapSplats));
        }

        void OnGUI() {
            Terrain terrain = null;
            foreach (GameObject g in Selection.gameObjects) {
                terrain = g.GetComponent<Terrain>();
                if (terrain != null)
                    break;
            }

            EditorGUILayout.LabelField("Terrain: " + (terrain == null ? "No Terrain Selected" : terrain.name));

            splatIndex1 = EditorGUILayout.IntField("First Splat Index: ", splatIndex1);
            splatIndex2 = EditorGUILayout.IntField("Second Splat Index: ", splatIndex2);

            if (terrain != null && GUILayout.Button("Swap")) {
                TerrainFunctions.SwapSplats(terrain, splatIndex1, splatIndex2);
            }
        }
    }
}
#endif