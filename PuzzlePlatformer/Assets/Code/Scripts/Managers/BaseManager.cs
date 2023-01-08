using UnityEngine;

namespace Code.Scripts.Managers
{
    public abstract class BaseManager<TManager> : MonoBehaviour where TManager : MonoBehaviour
    {
        private static TManager _instance;

        public static TManager Instance
        {
            get
            {
                _instance = FindObjectOfType<TManager>();
                return _instance;
            }
        }
    }
}