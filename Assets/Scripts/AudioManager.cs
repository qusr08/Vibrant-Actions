using UnityEngine;
using UnityEngine.Audio;
using System;
using Random = UnityEngine.Random;

// Editor:				Yu Ling Dong
// Date Created:		10/27/22
// Date Last Editted:	10/29/22

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;

    [Tooltip("Swoosh sound effects.")]
    [SerializeField] private Sound[] swooshSounds;

    [Tooltip("Plastic bottle sound effects.")]
    [SerializeField] private Sound[] plasticBottleSounds;

    [Tooltip("Paper cup sound effects.")]
    [SerializeField] private Sound[] paperCupSounds;

    public static AudioManager instance;

    [Tooltip("The two audio sources attached to this game object, used to " +
            "alternate between swoosh and trash pickup sounds. Index 0 is the" +
            "swoosh audio source, and index 1 is the trash pickup audio source.")]
    [SerializeField] private AudioSource[] audioSources;
    // private AudioSource music, effects;

    void Awake() {

        //Make sure there is only one instance of the Audio Manager
        if(instance == null)
        {
            instance = this;
        }else{
            //Destroy the extra Audio Manager
            Destroy(gameObject);
            return;
        }

        //Will continuously play even when scenes are switched
        DontDestroyOnLoad(gameObject);

        //Add each audio source component to the array
        foreach(Sound s in sounds)
        {        
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //Play the theme at the start of the game
    void Start()
    {
        //Play- name of clip
        Play("Theme");
    }

    public void PlayOneShot(TrashTypes trashType)
    {
        // Get random swoosh and trash sounds.
        Sound swoosh = swooshSounds[Random.Range(0, swooshSounds.Length)];
        Sound trash;

        switch (trashType)
        {
            case TrashTypes.Bottle:
                trash = plasticBottleSounds[Random.Range(0, plasticBottleSounds.Length)];
                break;
            default: // case TrashTypes.Cup (to placate the compiler)
                trash = paperCupSounds[Random.Range(0, paperCupSounds.Length)];
                break;
        }

        Debug.Log(swoosh.clip.name);
        audioSources[0].clip = swoosh.clip;
        audioSources[1].clip = trash.clip;

        // Play audio
        audioSources[0].Play(/*swoosh.clip*/);
        audioSources[1].PlayScheduled(swoosh.clip.length);
    }

    public void Play(string name)
    {
        //Get audio from audio manager array
        Sound s  = Array.Find(sounds, sound => sound.name == name);
        
        //Check that the array is not empty
        if(s == null)
        {
            //Warn user if a sound cannot be found and Return without playing
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }
        
        //Play audio
        s.source.Play();
    }
}
