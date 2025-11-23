using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGameLibrary.Audio;

public class AudioController : IDisposable
{
    #region Member Variables

    private readonly List<SoundEffectInstance> activeSoundEffectInstances;
    private float previousSongVolume;
    private float previousSoundEffectVolume;

    #endregion

    #region Properties

    public bool IsDisposed { get; private set; }
    public bool IsMuted { get; private set; }

    public float CurrentSongVolume
    {
        get
        {
            if (IsMuted == true)
            {
                return 0.0f;
            }

            return MediaPlayer.Volume;
        }
        set
        {
            if (IsMuted == true)
            {
                return;
            }

            MediaPlayer.Volume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    public float CurrentSoundEffectVolume
    {
        get
        {
            if (IsMuted == true)
            {
                return 0.0f;
            }

            return SoundEffect.MasterVolume;
        }
        set
        {
            if (IsMuted == true)
            {
                return;
            }

            SoundEffect.MasterVolume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    #endregion

    #region Constructor

    public AudioController()
    {
        activeSoundEffectInstances = new List<SoundEffectInstance>();
    }

    #endregion

    #region Deconstructor

    ~AudioController() => Dispose(false);

    #endregion

    #region Public Methods

    public void Update()
    {
        for (int i = activeSoundEffectInstances.Count - 1; i >= 0; i--)
        {
            SoundEffectInstance soundEffectInstance = activeSoundEffectInstances[i];
            if (soundEffectInstance.State == SoundState.Stopped)
            {
                if (soundEffectInstance.IsDisposed == false)
                {
                    soundEffectInstance.Dispose();
                }

                activeSoundEffectInstances.RemoveAt(i);
            }
        }
    }

    public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect)
    {
        return PlaySoundEffect(soundEffect, 1.0f, 0.0f, 0.0f, false);
    }

    public SoundEffectInstance PlaySoundEffect(
        SoundEffect soundEffect,
        float volume,
        float pitch,
        float pan,
        bool isLooped
    )
    {
        SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();
        soundEffectInstance.Volume = volume;
        soundEffectInstance.Pitch = pitch;
        soundEffectInstance.Pan = pan;
        soundEffectInstance.IsLooped = isLooped;
        soundEffectInstance.Play();
        activeSoundEffectInstances.Add(soundEffectInstance);
        return soundEffectInstance;
    }

    public void PlaySong(Song song, bool isRepeating = true)
    {
        if (MediaPlayer.State == MediaState.Playing)
        {
            MediaPlayer.Stop();
        }

        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = isRepeating;
    }

    public void PauseAllAudio()
    {
        MediaPlayer.Pause();
        foreach (SoundEffectInstance soundEffectInstance in activeSoundEffectInstances)
        {
            soundEffectInstance.Pause();
        }
    }

    public void ResumeAllAudio()
    {
        MediaPlayer.Resume();
        foreach (SoundEffectInstance soundEffectInstance in activeSoundEffectInstances)
        {
            soundEffectInstance.Resume();
        }
    }

    public void MuteAllAudio()
    {
        previousSongVolume = MediaPlayer.Volume;
        previousSoundEffectVolume = SoundEffect.MasterVolume;
        MediaPlayer.Volume = 0.0f;
        SoundEffect.MasterVolume = 0.0f;
        IsMuted = true;
    }

    public void UnmuteAllAudio()
    {
        MediaPlayer.Volume = previousSongVolume;
        SoundEffect.MasterVolume = previousSoundEffectVolume;
        IsMuted = false;
    }

    public void ToggleMute()
    {
        if (IsMuted)
        {
            UnmuteAllAudio();
        }
        else
        {
            MuteAllAudio();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (IsDisposed == true)
        {
            return;
        }

        if (isDisposing == true)
        {
            foreach (SoundEffectInstance soundEffectInstance in activeSoundEffectInstances)
            {
                soundEffectInstance.Dispose();
            }

            activeSoundEffectInstances.Clear();
        }

        IsDisposed = true;
    }

    #endregion
}
