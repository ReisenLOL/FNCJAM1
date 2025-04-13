using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [field: SerializeField] public BaseUnit Owner { get; private set; }
    public GameObject powerObject;
    private static GameObject collectableFolder => CachedCollactableFolder == null ? FindAndCacheCollectableFolder() : CachedCollactableFolder;
    static GameObject CachedCollactableFolder;
    static GameObject FindAndCacheCollectableFolder()
    {
        GameObject found = GameObject.Find("CollectableFolder");
        CachedCollactableFolder = found;
        return found;
    }
    private void Start()
    {
        Owner.WhenHit += WhenHit;
    }
    private void OnDestroy()
    {
        Owner.WhenHit -= WhenHit;
    }
    public void WhenHit(HitPacket packet, BaseUnit unit)
    {
        if (!unit.IsAlive) // alive check is after health calculations.
        {
            GameObject droppedPower = Instantiate(powerObject, collectableFolder.transform);
            droppedPower.transform.position = unit.CurrentPosition;
        }
    }
}
