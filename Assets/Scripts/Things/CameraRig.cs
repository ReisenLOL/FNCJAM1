using Core.Extensions;
using Unity.Cinemachine;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] Transform trackedObject;
    static CinemachineConfiner2D playerBoxConfiner;
    static CinemachineCamera cinemachineCamera;
    static CameraRig instance;
    private void Awake()
    {
        instance = this;
        if (TryGetComponent(out CinemachineConfiner2D confiner))
        {
            playerBoxConfiner = confiner;
        }
        else
        {
            playerBoxConfiner = GetComponentInChildren<CinemachineConfiner2D>();
        }
        if (TryGetComponent(out CinemachineCamera cam))
        {
            cinemachineCamera = cam;
        }
        else
        {
            cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
        }
        if (cinemachineCamera == null || playerBoxConfiner == null)
        {
            Debug.LogError("Bad");
            return;
        }
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
