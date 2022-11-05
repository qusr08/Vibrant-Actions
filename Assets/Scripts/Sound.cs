using UnityEngine;
using UnityEngine.Audio;

// Editor:				Yu Ling Dong
// Date Created:		10/27/22
// Date Last Editted:	10/29/22

[System.Serializable]
public class Sound{
    //Name of each audio clip
    public string name;

    public AudioClip clip;

    //Range for volume and pitch
    [Range(0f,1f)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;
    
    //Whether an audio is played in loops
    public bool loop;

    [HideInInspector]

    public AudioSource source;
}
