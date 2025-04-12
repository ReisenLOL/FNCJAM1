using Core.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bremsengine
{
    public class TilemapPlacerManager : MonoBehaviour
    {
        static TilemapPlacerManager instance;
        [SerializeField] Tilemap editableTilemap;
        [SerializeField] Tile[] emptyTiles;
        [SerializeField] bool editDiagonal;
        public static bool EditDiagonal => instance.editDiagonal;
        public static bool CanEditMap => instance != null && instance.editableTilemap != null;
        static Tilemap tilemap => instance.editableTilemap;
        public void Awake()
        {
            instance = this;
        }
        public static Vector3 WorldToCell(Vector2 position, bool center = false)
        {
            if (center)
            {
                tilemap.GetCellCenterWorld(tilemap.WorldToCell(position));
            }
            return tilemap.WorldToCell(position);
        }
        public static Vector3Int WorldToCellToInt(Vector2 position)
        {
            return tilemap.WorldToCell(position);
        }
        public static void EditTilemap(int x, int y, TileBase newTile)
        {
            if (!CanEditMap)
                return;
            if (HasNeighbours(x, y))
            {
                instance.editableTilemap.SetTile(new(x, y), newTile);
            }
        }
        public static void RemoveTile(int x, int y)
        {
            if (!CanEditMap)
                return;
        }
        public static bool IsEmpty(Vector3Int v) => IsEmpty(v.x, v.y);
        public static bool IsEmpty(int x, int y)
        {
            static bool IsTileEmpty(TileBase t)
            {
                if (t == null)
                    return true;
                foreach (var item in instance.emptyTiles)
                {
                    if (t == item)
                    {
                        return true;
                    }
                }
                return false;
            }

            if (!CanEditMap)
                return false;
            TileBase t = instance.editableTilemap.GetTile(new(x, y));
            if (t == null)
            {
                Debug.DrawLine(TilemapPlacerUnit.TestPosition, new(x, y), Color.yellow, 1f);
                return IsTileEmpty(t);
            }
            else
            {
                return false;
            }
        }
        public static bool HasNeighbours(int x, int y)
        {
            if (!CanEditMap)
                return false;
            for (int iX = -1; iX < 2; iX++)
            {
                for (int iY = -1; iY < 2; iY++)
                {
                    if (!EditDiagonal)
                    {
                        if (iX == -1 && iY == -1) continue;
                        if (iX == 1 && iY == 1) continue;
                        if (iX == -1 && iY == 1) continue;
                        if (iX == 1 && iY == -1) continue;
                    }
                    if (!IsEmpty(x + iX, y + iY))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
