using UnityEngine;

public class SwordSweep : Weapon
{
    private float offset;
    void Start()
    {
        transform.Translate(transform.right * offset);
    }
    void Update()
    {
        
    }
}
