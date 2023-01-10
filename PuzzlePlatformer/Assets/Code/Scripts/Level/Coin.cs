using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class Coin : MonoBehaviour
    {
        public void Interact()
        {
            transform.DOJump(transform.position + (new Vector3(0, 2, 3) * 5f), 1, 1, 0.3f);
        }
    }
}