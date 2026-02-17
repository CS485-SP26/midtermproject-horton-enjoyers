using UnityEngine;
using UnityEngine.InputSystem; // new input system
using Farming;

namespace Character
{
    public class SimpleRaycastTileSelector : TileSelector
    {
        void Update()
        {
            SelectTileUnderMouse();
        }

        private void SelectTileUnderMouse()
        {
            if (Camera.main == null) return;

            Vector2 mousePos = Mouse.current.position.ReadValue(); // new Input System
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Ray hit: " + hit.collider.name);
                FarmTile tile = hit.collider.GetComponent<FarmTile>();
                if (tile != null)
                {
                    Debug.Log("Selected FarmTile: " + tile.name);
                    SetActiveTile(tile);
                }
                else
                {
                    Debug.Log("Hit something, but it's not a FarmTile.");
                    SetActiveTile(null);
                }
            }
            else
            {
                Debug.Log("Ray hit nothing.");
                SetActiveTile(null);
            }
        }
    }
}
