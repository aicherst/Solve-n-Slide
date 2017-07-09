using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnmodifiableTerrainExchanger : MonoBehaviour {
    private static readonly string UNMODIFIABLE_TERRAIN_SUFFIX = "Unmodifiable";

    private TerrainWithBaseAlphas[] terrainsWithBaseAlphas;

    // Use this for initialization
    void Start() {
        CreateTerrainsWithBaseAlphas();

        GameStateManager.instance.gamePhase.AddListener(OnGamePhaseChange);
    }

    private void OnGamePhaseChange(ReadOnlyProperty<GamePhase> changedProperty, GamePhase gamePhase, GamePhase oldGamePhase) {
        switch (gamePhase) {
            case GamePhase.Action:
                DoExchange();
                break;
            case GamePhase.Manipulation:
                UndoExchange();
                break;
        }
    }

    private void CreateTerrainsWithBaseAlphas() {
        Terrain[] terrains = FindObjectsOfType<Terrain>();

        terrainsWithBaseAlphas = new TerrainWithBaseAlphas[terrains.Length];

        for (int i = 0; i < terrains.Length; i++) {
            terrainsWithBaseAlphas[i] = CreateTerrainWithBaseAlphas(terrains[i]);
        }
    }


    private void DoExchange() {
        foreach (TerrainWithBaseAlphas terrainWithBaseAlphas in terrainsWithBaseAlphas) {
            terrainWithBaseAlphas.Apply();
        }
    }

    private void UndoExchange() {
        foreach (TerrainWithBaseAlphas terrainWithBaseAlphas in terrainsWithBaseAlphas) {
            terrainWithBaseAlphas.Undo();
        }
    }

    private void OnDestroy() {
        UndoExchange();
    }

    private TerrainWithBaseAlphas CreateTerrainWithBaseAlphas(Terrain terrain) {
        TerrainData terrainData = terrain.terrainData;

        float[,,] baseAlphas = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        float[,,] replaceAlphas = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        Dictionary<string, int> textureNameToIndex = new Dictionary<string, int>();
        for(int i = 0; i < terrainData.splatPrototypes.Length; i++) {
            textureNameToIndex[terrainData.splatPrototypes[i].texture.name] = i;
        }

        foreach (SplatPrototype splatPrototype in terrainData.splatPrototypes) {
            string textureName = splatPrototype.texture.name;

            if (IsTerrainModifiable(textureName))
                continue;

            int oldTexture = textureNameToIndex[textureName];
            int newTexture = textureNameToIndex[GetModifiableTerrainName(textureName)];

            for (int j = 0; j < terrainData.alphamapWidth; j++) {
                for (int k = 0; k < terrainData.alphamapHeight; k++) {
                    replaceAlphas[j, k, newTexture] = Mathf.Max(replaceAlphas[j, k, oldTexture], replaceAlphas[j, k, newTexture]);
                    replaceAlphas[j, k, oldTexture] = 0f;
                }
            }
        }

        return new TerrainWithBaseAlphas(terrainData, baseAlphas, replaceAlphas);
    }

    private bool IsTerrainModifiable(string textureName) {
        return !textureName.EndsWith(UNMODIFIABLE_TERRAIN_SUFFIX);
    }

    private string GetModifiableTerrainName(string textureName) {
        return textureName.Substring(0, textureName.Length - UNMODIFIABLE_TERRAIN_SUFFIX.Length);
    }

    private class TerrainWithBaseAlphas {
        private float[,,] baseAlphas;
        private float[,,] replacedAlphas;

        private TerrainData terrainData;

        public TerrainWithBaseAlphas(TerrainData terrainData, float[,,] baseAlphas, float[,,] replacedAlphas) {
            this.terrainData = terrainData;
            this.baseAlphas = baseAlphas;
            this.replacedAlphas = replacedAlphas;
        }

        public void Apply() {
            terrainData.SetAlphamaps(0, 0, replacedAlphas);
        }

        public void Undo() {
            terrainData.SetAlphamaps(0, 0, baseAlphas);
        }
    }
}
