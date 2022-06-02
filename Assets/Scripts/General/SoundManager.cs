using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Players")]
    [SerializeField] AudioSource _musicPlayer;
    [SerializeField] AudioSource _soundPlayer;
    [SerializeField] AudioSource _otherPlayer;
    [Header("Music")]
    [SerializeField] List<SoundData> _sounds;

    #region Methods
    public void playMusic(SoundType type)
    {
        AudioClip _musicToPlay = getSound(type);
            //dont play the same music
            if (_musicPlayer.clip == _musicToPlay)
                return;
            _musicPlayer.clip = _musicToPlay;
            _musicPlayer.Play();
    }
    /// <summary>
    /// Plays a sound.
    /// </summary>
    /// <param name="type">The type of the saound to play.</param>
    /// <param name="otherPlayer">Indicates if need to use the extra player in case the sound player may be in use.</param>
    public void playSound(SoundType type,bool otherPlayer=false)
    {
        if(!otherPlayer)
        _soundPlayer.PlayOneShot(getSound(type));
        else
        _otherPlayer.PlayOneShot(getSound(type));
    }
    private AudioClip getSound(SoundType type)
    {
        return _sounds.Find(_musicData => _musicData.MusicType == type).Sound;
    }
    #endregion

    #region SoundEffects Methods
    public void playBuuttonHoverSound()
    {
        playSound(SoundType.ButtonHoverSound);
    } 
    public void playBuuttonClickSound()
    {
        playSound(SoundType.ButtonClickSound);
    }
    #endregion

}
