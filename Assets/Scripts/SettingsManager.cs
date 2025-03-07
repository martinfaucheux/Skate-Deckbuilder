using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : CoduckStudio.Utils.Singleton<SettingsManager>
{
    private void Awake()
    {
        // Load settings
        volume = PlayerPrefs.GetFloat("SettingsVolume", defaultVolume);
        volumeSlider.SetValueWithoutNotify(volume);
    }

    public float defaultVolume = 0.5f;
    public float volume = 1f;
    public Slider volumeSlider;
    public void OnVolumeChanged(float volume)
    {
        this.volume = volume;
        PlayerPrefs.SetFloat("SettingsVolume", volume);
        FindObjectsByType<AudioSource>(FindObjectsSortMode.None).ToList().ForEach(audioSource => {
            audioSource.volume = volume;
        });
    }
}
