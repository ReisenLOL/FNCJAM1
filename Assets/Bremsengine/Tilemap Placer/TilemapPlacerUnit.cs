using Core.Extensions;
using Core.Input;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bremsengine
{
    public class TilemapPlacerUnit : MonoBehaviour
    {
        private static TilemapPlacerUnit testInstance;
        [SerializeField] Transform unitCenter;
        [SerializeField] int horizontalTilePlacementRange = 3;
        [SerializeField] int verticalTilePlacementRange = 2;
        [SerializeField] TileBase testPlaceTile;
        [SerializeField] float tilemapUpdateRepeatDelay = 0.125f;
        float nextTilemapUpdateTime;
        bool MouseHeld;
        public static Vector2 TestPosition => testInstance.unitCenter.position;
        private void Awake()
        {
            testInstance = this;
        }
        private void Start()
        {
            PlayerInputController.OnMouseClick += OnMouseClick;
            PlayerInputController.OnMouseRelease += OnMouseRelease;
        }
        private void OnDestroy()
        {
            PlayerInputController.OnMouseClick -= OnMouseClick;
            PlayerInputController.OnMouseRelease -= OnMouseRelease;
        }
        private void Update()
        {
            if (MouseHeld && TryPlace(testPlaceTile, Helper.MousePosition))
            {
                nextTilemapUpdateTime = Time.time + tilemapUpdateRepeatDelay;
            }
        }
        private void OnMouseClick(Vector2 position)
        {
            MouseHeld = true;
            if (TryPlace(testPlaceTile, position))
            {
                nextTilemapUpdateTime = Time.time + tilemapUpdateRepeatDelay;
            }
        }
        private void OnMouseRelease(Vector2 position)
        {
            MouseHeld = false;
        }
        public bool TryPlace(TileBase tile, Vector2 position)
        {
            if (tile == null)
            {
                return false;
            }
            if (Time.time < nextTilemapUpdateTime)
            {
                return false;
            }
            if (TilemapPlacerManager.CanEditMap)
            {
                if (AllowedDistance(unitCenter, position))
                {
                    Vector3Int tilePosition = TilemapPlacerManager.WorldToCellToInt(position);
                    if (TilemapPlacerManager.IsEmpty(tilePosition) && TilemapPlacerManager.HasNeighbours(tilePosition.x, tilePosition.y))
                    {
                        TilemapPlacerManager.EditTilemap(tilePosition.x, tilePosition.y, tile);
                        return true;
                    }
                }
            }
            return false;
        }
        private bool AllowedDistance(Transform t, Vector2 position)
        {
            Vector2Int positionInt = new(position.x.ToInt(), position.y.ToInt());
            Vector2Int tPosition = new(t.position.x.ToInt(), t.position.y.ToInt());
            if (tPosition.x == positionInt.x)
            {
                if (tPosition.y == positionInt.y || tPosition.y - 1 == positionInt.y)
                {
                    return false;
                }
            }
            int xDistance = (positionInt.x - tPosition.x).Abs();
            int yDistance = ((positionInt.y - tPosition.y).Abs());
            yDistance = yDistance.Max(0);
            return xDistance <= horizontalTilePlacementRange && yDistance <= verticalTilePlacementRange;
        }
    }
}
