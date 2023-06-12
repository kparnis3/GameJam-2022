
using UnityEngine;

[System.Serializable]
public class Sound 
{
    [HideInInspector]
    public AudioSource source;
    public AudioClip clip;
    public string name;

    public enum AudioTypes { sfx, music }
    public AudioTypes type;

    public bool isLoop;
    public bool playOnAwake;
    [Range(0, 1)] public float volume = 0.75f;
}
