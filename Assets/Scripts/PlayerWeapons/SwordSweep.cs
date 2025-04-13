using UnityEngine;

public class SwordSweep : Weapon
{
    public float offset;
    void Start()
    {
        transform.parent = firedFrom.transform;
        transform.Translate(transform.right * offset);

    }
    void Update()
    {
        
    }
}
