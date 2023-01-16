using Code.Scripts.Classes;
using UnityEngine;

namespace Code.Scripts.Managers
{
    public class SoundManager : BaseManager<SoundManager>
    {
        public Sound[] sounds;
        private void Awake()
        {
            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = 1;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

        public void Play(string soundName)
        {
            Sound s = System.Array.Find(sounds, sound => sound.name == soundName);
            if (s == null)
                return;
            //s.source.Play();
            // For completely play all sounds without cutting some last of sounds
            s.source.PlayOneShot(s.clip);
        }

        public void Stop(string soundName)
        {
            Sound s = System.Array.Find(sounds, sound => sound.name == soundName);
            if (s == null)
                return;
            //s.source.Play();
            // For completely play all sounds without cutting some last of sounds
            s.source.Stop();
        }
        
        public void StopAll()
        {
            foreach (var sound in sounds)
            {
                sound.source.Stop();
            }
        }


        public void ChangeVolume(float volume)
        {
            foreach (var sound in sounds)
            {
                sound.source.volume = volume;
            }
        }
    }
}