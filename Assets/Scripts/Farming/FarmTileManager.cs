using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Environment;
using Core;

namespace Farming
{
    public class FarmTileManager:MonoBehaviour
    {
        [SerializeField] private GameObject farmTilePrefab;
        [SerializeField] DayController dayController;
        [SerializeField] private int rows = 4;
        [SerializeField] private int cols = 4;
        [SerializeField] private float tileGap = 0.1f;
        private List<FarmTile> tiles = new List<FarmTile>();
        
        void Awake()
        {
            // Populate tiles from existing scene children at runtime,
            // before Start() runs so RestoreTileStates() has something to work with.
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<FarmTile>(out var tile))
                    tiles.Add(tile);
            }
        }

        void Start()
        {
            Debug.Assert(farmTilePrefab, "FarmTileManager requires a farmTilePrefab");
            Debug.Assert(dayController, "FarmTileManager requires a dayController");
            RestoreTileStates();
        }

        void OnEnable()
        {
            dayController.dayPassedEvent.AddListener(this.OnDayPassed);
        }

        void OnDisable()
        {
            dayController.dayPassedEvent.RemoveListener(this.OnDayPassed);
            SaveTileStates();
        }

        private void SaveTileStates()
        {
            var saves = new List<TileSave>();
            foreach (FarmTile tile in tiles)
                saves.Add(new TileSave { condition = tile.GetCondition, daysSinceLastInteraction = tile.DaysSinceLastInteraction });
            GameManager.Instance.savedTileStates = saves;
            GameManager.Instance.hasSavedTiles = true;
        }

        private void RestoreTileStates()
        {
            if (!GameManager.Instance.hasSavedTiles) return;
            List<TileSave> saves = GameManager.Instance.savedTileStates;
            for (int i = 0; i < tiles.Count && i < saves.Count; i++)
                tiles[i].RestoreState(saves[i].condition, saves[i].daysSinceLastInteraction);
        }

        public void OnDayPassed()
        {
            IncrementDays(1);
        }

        public int CountWateredTiles()
        {
            int count = 0;
            foreach (FarmTile tile in tiles)
            {
                if (tile.GetCondition == FarmTile.Condition.Watered) count++;
            }
            return count;
        }

        public void IncrementDays(int count)
        {
            while (count > 0)
            {
                foreach (FarmTile farmTile in tiles)
                {
                    farmTile.OnDayPassed();
                }
                count--;
            }
        }

        void InstantiateTiles()
        {
            Vector3 spawnPos = transform.position;
            int count = 0;
            GameObject clone = null; 

            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    clone = Instantiate(farmTilePrefab, spawnPos, Quaternion.identity);
                    clone.name = "Farm Tile " + count++.ToString();
                    spawnPos.x += clone.transform.localScale.x + tileGap;
                    clone.transform.parent = transform; // build heirarchy
                    tiles.Add(clone.GetComponent<FarmTile>()); // for resize/delete
                }
                spawnPos.z += clone.transform.localScale.z + tileGap;
                spawnPos.x = transform.position.x;
            }
        }

        // ***************************************************************** //
        // Below this line is code to suppor the Unity Editor (Advanced)
        // Please feel free to disregard everything below this
        // ***************************************************************** //
        void OnValidate()
        {
            #if UNITY_EDITOR
            EditorApplication.delayCall += () => {
                if (this == null) return; // Guard against the object being deleted
                ValidateGrid();
            };
            #endif
        }

        void ValidateGrid() 
        {
            if (!farmTilePrefab) return;
            tiles.Clear();
            foreach (Transform child in transform)
            {
                if (child.gameObject.TryGetComponent<FarmTile>(out var tile))
                {
                    tiles.Add(tile);
                }
            }

            int newCount = rows * cols;

            if (tiles.Count != newCount)
            {
                DestroyTiles();
                InstantiateTiles();
            }
        }

        void DestroyTiles()
        {
            foreach (FarmTile tile in tiles)
            {
                #if UNITY_EDITOR
                DestroyImmediate(tile.gameObject);
                #else
                Destroy(tile.gameObject);
                #endif
            }
            tiles.Clear();
        }
    }
}