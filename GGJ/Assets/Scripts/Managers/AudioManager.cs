using System.Collections;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager Obj => _instance;
        
        [SerializeField] private AudioSource EffectsSource;
        [SerializeField] private AudioSource MusicSource;

        [SerializeField] private float LowPitchRange = 0.95f;
        [SerializeField] private float HighPitchRange = 1.05f;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad (gameObject);
            
            if (EffectsSource == null)
            {
                EffectsSource = gameObject.AddComponent<AudioSource>();
            }

            if (MusicSource == null)
            {
                MusicSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public void Play(AudioClip clip)
        {
            EffectsSource.clip = clip;
            EffectsSource.Play();
        }

        public void PlayOneShot(AudioClip clip, float volumescale = 1f)
        {
            EffectsSource.PlayOneShot(clip, 1);
        }

        public void PlayMusic(AudioClip clip)
        {
            MusicSource.volume = 0.6f;
            MusicSource.clip = clip;
            MusicSource.Play();
        }

        public void FadeOutAndSwapMusic(AudioClip newClip, float duration)
        {
            if (MusicSource.clip == null)
            {
                PlayMusic(newClip);
                StartCoroutine(FadeIn(duration));
                return;
            }

            if (MusicSource.clip == newClip) return;
            
            StopAllCoroutines();
        }
        
        private IEnumerator FadeIn(float duration)
        {
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                MusicSource.volume = Mathf.Lerp(0, 1, time / duration);
                yield return null;
            }

            MusicSource.volume = 1;
        }
        
        public void RandomSoundEffect(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

            EffectsSource.pitch = randomPitch;
            EffectsSource.clip = clips[randomIndex];
            EffectsSource.Play();
        }
    }
}
