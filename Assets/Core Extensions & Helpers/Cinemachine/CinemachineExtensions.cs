using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

namespace Core.Extensions
{
    public static class CinemachineExtensions
    {
        public static void SnapTo(this CinemachineBrain cam, Vector2 position)
        {
            cam.transform.position = position;
        }
    }
}
