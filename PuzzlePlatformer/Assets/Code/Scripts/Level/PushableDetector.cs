using System;
using UnityEngine;

namespace Code.Scripts
{
    public class PushableDetector : MonoBehaviour
    {
        public event Action<ControllerColliderHit> OnPushableDetect;

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.transform.CompareTag("Pushable"))
            {
                OnPushableDetect?.Invoke(hit);
            }
        }
    }
}