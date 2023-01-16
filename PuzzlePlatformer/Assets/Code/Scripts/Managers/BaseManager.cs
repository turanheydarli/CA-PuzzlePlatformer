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
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TManager>();
                    DontDestroyOnLoad(_instance);
                }
                
                return _instance;
            }
        }
    }
}