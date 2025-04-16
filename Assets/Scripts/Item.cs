using UnityEngine;

public class Item : MonoBehaviour
{
    public BaseUnit Owner;
    [field: SerializeField] public string ItemName { get; private set; } = "Headhunter, Leather Belt";
    public void SetOwner(BaseUnit owner)
    {
        Owner = owner;
    }
}
