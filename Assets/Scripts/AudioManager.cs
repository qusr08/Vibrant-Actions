using UnityEngine;
using UnityEngine.Audio;
using System;

// Editor:				Yu Ling Dong
// Date Created:		10/27/22
// Date Last Editted:	10/29/22

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;

    public static AudioManager instance;
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
