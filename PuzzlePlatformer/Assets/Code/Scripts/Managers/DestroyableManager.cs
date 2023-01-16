using UnityEngine;

namespace Code.Scripts.Managers
{
    public abstract class DestroyableManager<TManager> : MonoBehaviour where TManager : MonoBehaviour
    {
        private static TManager _instance;

        public static TManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TManager>();
                }
                return _instance;
            }
        }
    }
}