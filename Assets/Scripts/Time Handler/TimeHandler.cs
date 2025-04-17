using Bremsengine;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public static float TimeScale => FetchTimescale();
    public static float HandledDeltaTime => Time.deltaTime;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        instance = null;
        TimeHandler handler = new GameObject("Time Handler").AddComponent<TimeHandler>();
    }
    static TimeHandler instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private static float FetchTimescale()
    {
        if (Dialogue.IsDialogueRunning)
        {
            return 0f;
        }
        if (WeaponSelect.IsSelecting)
        {
            return 0f;
        }
        float timeScale = TimeSlowHandler.SlowdownHandledTimescale;
        return timeScale;
    }
    public static void ModifyTimescale(float multiplier, float duration)
    {
        TimeSlowHandler.AddSlow(multiplier, duration);
    }
    private void Update()
    {
        if (instance != this)
        {
            return;
        }
        Time.timeScale = TimeScale;
    }
}
