using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSigleton<AudioManager>
{
    [SerializeField] AudioSource sfxPlayer;

    const float MIN_PITCH = 0.9f;
    const float MAX_PITCH = 1.1f;

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    public void PlaySFX(AudioData audioData)
    {
        sfxPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    /// <summary>
    /// 随机播放音效
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    public void PlayRandomSFX(AudioData audioData)
    {
        sfxPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData);
    }

    /// <summary>
    /// 随机播放音效
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);
    }

}


[System.Serializable]
public class AudioData
{
    public AudioClip audioClip;
    public float volume;
}
