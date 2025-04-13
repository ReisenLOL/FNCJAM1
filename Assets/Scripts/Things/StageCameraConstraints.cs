using Unity.Cinemachine;
using UnityEngine;

public class StageCameraConstraints : MonoBehaviour
{
    [SerializeField] BoxCollider2D cameraConstraints;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerUnit>() is PlayerUnit p)
        {
            CameraRig.SetStageCameraBox(cameraConstraints);
        }
    }
}
