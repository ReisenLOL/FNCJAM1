using UnityEngine;

namespace Bremsengine
{
    public class GeneralManagerPauseAction : MonoBehaviour
    {
        public void SetPause(bool state)
        {
            GeneralManager.SetPause(state);
        }
        public void TogglePause()
        {
            SetPause(!GeneralManager.IsPaused);
        }
    }
}
