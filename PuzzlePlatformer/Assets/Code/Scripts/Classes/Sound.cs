using UnityEngine;

namespace Code.Scripts.Classes
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0, 1)] public float volume = 1;
        [Range(0.1f, 3f)] public float pitch = 1;
        public bool loop;

        [HideInInspector] public AudioSource source;
    }
}