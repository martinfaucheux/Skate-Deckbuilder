using UnityEngine;

public class MusicSetter : MonoBehaviour
{
    public string musicName;
    private void Start()
    {
        AudioManager.instance.StopMusic();
        AudioManager.instance.Play(musicName);
    }
}