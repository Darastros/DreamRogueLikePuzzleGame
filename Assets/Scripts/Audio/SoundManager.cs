using System;
using UnityEngine;
using UnityEngine.Audio;

public abstract class SoundManager : MonoBehaviour
{
    [Header("Audio settings")]
    [SerializeField] protected AudioMixerGroup sfxMixerGroup;
    [SerializeField] protected AudioMixerGroup jingleMixerGroup;
    [SerializeField] protected float minRandomPitch = 0.9f;
    [SerializeField] protected float maxRandomPitch = 1.1f;
    [SerializeField] protected float activationJingleVolume = 0.3f;
    [SerializeField] private AudioClip errorClip;

    protected void OnEnable()
    {
        ListenEvent();
    }

    protected void OnDisable()
    {
        UnListenEvent();
    }

    protected abstract void ListenEvent();

    protected abstract void UnListenEvent();

    public void PlayAudio(AudioClip clip, AudioMixerGroup mixerGroup, float pitch = 1f, float volume = 1f)
    {
        if (!this) return;
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource, clip.length / Math.Abs(audioSource.pitch));
    }

    public void PlayJingle(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        PlayAudio(clip, jingleMixerGroup, pitch, volume);
    }

    public void PlaySfx(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        PlayAudio(clip, sfxMixerGroup, pitch, volume);
    }

    protected void ImpossibleAction()
    {
        PlaySfx(errorClip);
    }
}
