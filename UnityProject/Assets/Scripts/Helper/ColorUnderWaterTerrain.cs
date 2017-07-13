using UnityEditor;
using UnityEngine;

namespace EditorExtension {
    public class ColorUnderWaterTerrain : EditorWindow {
        private float min = 10;
        private float max = 30;

        private int groundIndex = 0;
        private int baseIndex = 1;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Terrain/Color Under Water Terrain")]
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(ColorUnderWaterTerrain));
        }

        void OnGUI() {
            Terrain terrain = null;
            foreach (GameObject g in Selection.gameObjects) {
                terrain = g.GetComponent<Terrain>();
                if (terrain != null)
                    break;
            }

            EditorGUILayout.LabelField("Terrain: " + (terrain == null ? "No Terrain Selected" : terrain.name));

            groundIndex = EditorGUILayout.IntField("Ground Splat Index: ", groundIndex);
            baseIndex = EditorGUILayout.IntField("Base Splat Index: ", baseIndex);


            min = EditorGUILayout.FloatField("from", min);
            max = EditorGUILayout.FloatField("to", max);

            if (terrain != null && GUILayout.Button("Color Under Water Terrain")) {
                TerrainFunctions.ColorUnderWaterTerrain(terrain, min, max, groundIndex, baseIndex);
            }
        }
    }
}
