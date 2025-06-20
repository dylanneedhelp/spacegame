using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource vfxAudioSource;
    public AudioClip backgroundMusicClip;
    public AudioClip coinClip;
    public AudioClip missileClip;
    public AudioClip explosionClip;
    public AudioClip clickClip;
    public AudioClip DeadClip;
    void Start()
    {
        backgroundMusic.clip = backgroundMusicClip;
        backgroundMusic.Play();
    }
    public void PlaySFX(AudioClip sfxClip)
    {
        vfxAudioSource.clip = sfxClip;
        vfxAudioSource.PlayOneShot(sfxClip);
    }
    
}
