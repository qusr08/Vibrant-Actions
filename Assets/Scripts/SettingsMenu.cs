using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        musicSlider.onValueChanged.AddListener(delegate { changeMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { changeSfxVolume(); });
    }

    private void changeMusicVolume()
    {
        PersistentData.Instance.musicVolume = musicSlider.value;
        Debug.Log("music volume = " + PersistentData.Instance.musicVolume);
    }

    private void changeSfxVolume()
    {
        PersistentData.Instance.sfxVolume = sfxSlider.value;
        Debug.Log("sfx volume = " + PersistentData.Instance.sfxVolume);
    }
}
