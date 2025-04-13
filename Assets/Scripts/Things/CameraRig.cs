using Unity.Cinemachine;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] Transform trackedObject;
    static CinemachineConfiner2D playerBoxConfiner;
    private void Awake()
    {
        playerBoxConfiner = GetComponentInChildren<CinemachineConfiner2D>();
    }
    public static void SetStageCameraBox(Collider2D box)
    {
        if (playerBoxConfiner == null)
        {
            return;
        }
        playerBoxConfiner.BoundingShape2D = box;
    }
    void Start()
    {
        transform.SetParent(null);
    }
    private void Update()
    {
        if (trackedObject.gameObject.activeInHierarchy)
        {
            transform.position = trackedObject.transform.position;
        }
    }
}
