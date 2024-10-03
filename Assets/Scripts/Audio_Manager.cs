using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour {
    //Laser Sound FX
    [SerializeField] private GameObject _laser_audio_source;
    [SerializeField] private AudioClip _laser_audio_clip;

    //Explosion Sound FX
    [SerializeField] private GameObject _explosion_audio_source;
    [SerializeField] private AudioClip _explosion_audio_clip;

    //Powerup Sound FX
    [SerializeField] private GameObject _powerup_audio_source;
    [SerializeField] private AudioClip _powerup_audio_clip;

    void Start() {
        //Laser Sound FX
        if (_laser_audio_source == null) Debug.LogError("The Laser Audio Gameobject wasn't found by the Audio Manager Script.");
        else if (_laser_audio_clip == null) Debug.LogError("The Laser Audio Clip wasn't found by the Audio Manager Script.");
        else _laser_audio_source.GetComponent<AudioSource>().clip = _laser_audio_clip;

        //Explosion Sound FX
        if (_explosion_audio_source == null) Debug.LogError("The explosion Audio Gameobject wasn't found by the Audio Manager Script.");
        else if (_explosion_audio_clip == null) Debug.LogError("The explosion Audio Clip wasn't found by the Audio Manager Script.");
        else _explosion_audio_source.GetComponent<AudioSource>().clip = _explosion_audio_clip;

        //Powerup Sound FX
        if (_powerup_audio_source == null) Debug.LogError("The powerup Audio Gameobject wasn't found by the Audio Manager Script.");
        else if (_powerup_audio_clip == null) Debug.LogError("The powerup Audio Clip wasn't found by the Audio Manager Script.");
        else _powerup_audio_source.GetComponent<AudioSource>().clip = _powerup_audio_clip;
    }

    public void Play_Laser_Sound() {
        _laser_audio_source.GetComponent<AudioSource>().Play();
    }

    public void Play_Explosion_Sound() {
        _explosion_audio_source.GetComponent<AudioSource>().Play();
    }

    public void Play_Powerup_Sound() {
        _powerup_audio_source.GetComponent<AudioSource>().Play();
    }

}
