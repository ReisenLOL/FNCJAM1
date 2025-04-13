using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public float health;
    public GameObject powerObject;
    private GameObject collectableFolder;
    private void Start()
    {
        collectableFolder = GameObject.Find("CollectableFolder");
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        GameObject droppedPower = Instantiate(powerObject, collectableFolder.transform);
        droppedPower.transform.position = gameObject.transform.position;
    }
}
