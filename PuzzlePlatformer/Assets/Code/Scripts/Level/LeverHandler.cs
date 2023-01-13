using System.Collections;
using Code.Scripts.Managers;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class LeverHandler : MonoBehaviour
    {
        public void HandleBridgeOpening()
        {
            CameraManager.Instance.Discover("LeverDiscoverCamera");
            
            StartCoroutine(OpeningAnimation());
        }

        public void HandleBridgeClosing()
        {
            StartCoroutine(ClosingAnimation());
        }

        IEnumerator OpeningAnimation()
        {
            foreach (Transform piece in transform)
            {
                piece.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator ClosingAnimation()
        {
            foreach (Transform piece in transform)
            {
                piece.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}