using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    public class BridgeHandler : MonoBehaviour
    {
        [SerializeField] private PressurePad pressurePad;
        private void Start()
        {
            pressurePad.OnButtonPress += HandleBridgeOpening;
            pressurePad.OnButtonRelease += HandleBridgeClosing;
        }

        private void HandleBridgeOpening()
        {
            StartCoroutine(OpeningAnimation());
        }
        
        private void HandleBridgeClosing()
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
