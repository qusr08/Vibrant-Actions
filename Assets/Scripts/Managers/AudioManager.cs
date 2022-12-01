using UnityEngine;
using UnityEngine.Audio;
using System;
using Random = UnityEngine.Random;

// Editor:				Yu Ling Dong, Szun Kidd Choi
// Date Created:		10/27/22
// Date Last Edited:	11/15/22

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;

    [Tooltip("Sound that plays when the correct receptacle is selected.")]
    [SerializeField] private Sound correctSound;

    [Tooltip("Sound that plays when the wrong receptacle is selected.")]
    [SerializeField] private Sound incorrectSound;

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

    void Awake()
    {

        //Make sure there is only one instance of the Audio Manager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //Destroy the extra Audio Manager
            Destroy(gameObject);
            return;
        }

        //Will continuously play even when scenes are switched
        DontDestroyOnLoad(gameObject);

        //Add each audio source component to the array
        foreach (Sound s in sounds)
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

    /// <summary>
    /// Plays the correct or incorrect sound effect.
    /// </summary>
    /// <param name="correct">
    /// Whether the sound denoting a correct choice should be played. If set to 
    /// false, the sound denoting an incorrect choice will be played instead.
    /// </param>
    public void PlayCorrectnessSFX(bool correct)
    {
        // Just pick any of the two audio sources to play the selected clip.
        // It doesn't matter since we are using PlayOneShot() for this.
        // We use PlayOneShot() because the clip should only play once, on top
        // of everything else that is already playing.
        if (correct) audioSources[0].PlayOneShot(correctSound.clip);
        else audioSources[0].PlayOneShot(incorrectSound.clip);
    }

    /// <summary>
    /// Plays the swoosh sound effect, followed by the corresponding trash
    /// type's collection sound effect.
    /// </summary>
    /// <param name="trashType"></param>
    public void PlayCollectSFX(TrashTypes trashType)
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

        // Set the clips for both audio sources accordingly.
        audioSources[0].clip = swoosh.clip;
        audioSources[1].clip = trash.clip;

        // Play audio clips in sequence.
        audioSources[0].Play();
        audioSources[1].PlayScheduled(swoosh.clip.length);
    }

    public void Play(string name)
    {
        //Get audio from audio manager array
        Sound s = Array.Find(sounds, sound => sound.name == name);

        //Check that the array is not empty
        if (s == null)
        {
            //Warn user if a sound cannot be found and Return without playing
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }

        //Play audio
        s.source.Play();
    }
}
