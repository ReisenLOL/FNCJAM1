using System.Collections.Generic;
using UnityEngine;

public class NoDomainReloadExample : MonoBehaviour
{
    static NoDomainReloadExample instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }
#if UNITY_EDITOR
    /*
     * 
     * Ive put a setting on in Project Settings > Editor > Scene Reload = Reload Scene Only (Domain Reload takes 10+ secs per Playmode start.)
     * 
     * Handling static variables is done manually.
     * They will keep their values between plays (only in editor)
     * so they will be Reset manually.
     * 
     * Not all matters, but know that if you for example set the player death count to 15 it will stay at 15 for the next time you play.
     * lists that contains found units for exmaple would just hold a lot of null units so it would have to be reset.
     * 
     * you can do .Clear() or = new() for IEnumerables (Lists, dictionaries, hashset etc).
     * 
     */
    static int PlayerDeathCount;

    static Dictionary<int, string> exampleUnitNames;
    static List<string> startingUnits = new()
    {
        "Bob",
        "Travis",
        "John",
        "Boris"
    };
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Demo_InitializeUnitNames()
    {
        instance = null;
        exampleUnitNames = new();
        PlayerDeathCount = 0;
        int iteration = 0;
        foreach (var item in startingUnits)
        {
            exampleUnitNames.Add(iteration, item);
            iteration++;
        }
    }
    private static void lmao()
    {
        PlayerDeathCount++;
    }
#endif
}
