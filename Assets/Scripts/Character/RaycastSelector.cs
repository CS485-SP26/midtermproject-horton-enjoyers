using UnityEngine;
using Farming;
using System.Runtime.CompilerServices;

namespace Character
{
    public class RaycastSelector : TileSelector
    {

        [SerializeField] private float rayDistance = 2f;
        void Update()
        {
            
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance))
            {
                if(hitInfo.collider.TryGetComponent<FarmTile>(out var tile))
                {
                    SetActiveTile(tile);
                }
                else
                {
                    SetActiveTile(null);
                }
            }
            
        }

    }
}