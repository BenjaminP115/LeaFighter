using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct Audio
    {
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
    }

    [SerializeField] private Audio[] audioClips;

    [HideInInspector] public AudioSource[] sources;

    void Start()
    {
        sources = new AudioSource[audioClips.Length];

        int i = 0;
        foreach (Audio audioClip in audioClips)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = audioClip.clip;
            source.playOnAwake = false;
            source.volume = audioClip.volume;
            source.spatialBlend = 1;

            sources[i] = source;
            i++;
        }
    }

    public void Play(int index)
    {
        sources[index].Play();
    }
}
