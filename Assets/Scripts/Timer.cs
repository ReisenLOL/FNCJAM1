using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public float gameStartTime;

    void Awake()
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

    public void ResetTimer()
    {
        gameStartTime = Time.time;
    }
}
