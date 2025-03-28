
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] bool keepThroughScenes;
    [SerializeField] Sound[] sounds;

    private Dictionary<string, Sound> _soundDict;
    void Awake()
    {
        CheckSingleton();

        // detach parent
        transform.parent = null;

        if (keepThroughScenes)
            DontDestroyOnLoad(gameObject);

        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        _soundDict = new Dictionary<string, Sound>();
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            _soundDict.Add(sound.name, sound);
        }
    }

    public void Play(string soundName)
    {
        if (soundName.Length > 0)
        {
            if (_soundDict.ContainsKey(soundName))
            {
                _soundDict[soundName].Play();
            }
            else
            {
                Debug.LogError("Unknown sound: " + soundName);
            }
        }
    }

    private void CheckSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Stop(string soundName)
    {
        if (soundName.Length > 0)
        {
            if (_soundDict.ContainsKey(soundName))
            {
                _soundDict[soundName].source.Stop();
            }
            else
            {
                Debug.LogError("Unknown sound: " + soundName);
            }
        }
    }

    public void StopMusic()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name.Contains("Music"))
            {
                Stop(sound.name);
            }
        }
    }
}
