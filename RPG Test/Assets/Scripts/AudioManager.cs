using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    private const string PLAYER_PREFS_SFX_VOLUME = "SFXVolume";
    private const string PLAYER_PREFS_MASTER_VOLUME = "MasterVolume";

    [SerializeField] AudioMixer audioMixer;

    private float volumeMusic;
    private float volumeSFX;
    private float volumeMaster;
    private float db;

    private void Awake() {

        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE MENU UI");
        }
        Instance = this;

        volumeMusic = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME);

        volumeSFX = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOLUME);

        volumeMaster = PlayerPrefs.GetFloat(PLAYER_PREFS_MASTER_VOLUME);
    }

    private void Start() {
        db = 20 * Mathf.Log10(volumeMusic);
        audioMixer.SetFloat("volumeMusic", db);

        db = 20 * Mathf.Log10(volumeSFX);
        audioMixer.SetFloat("volumeSFX", db);

        db = 20 * Mathf.Log10(volumeMaster);
        audioMixer.SetFloat("volumeMaster", db);

        MenuUI.Instance.SetSlidersSound(volumeMaster, volumeMusic, volumeSFX);
    }

    public void SetVolumeMusic(float volume) {
        if (volume != 0) {
            db = 20 * Mathf.Log10(volume);
            audioMixer.SetFloat("volumeMusic", db);

            PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        }
    }

    public void SetVolumeSFX(float volume) {
        if (volume != 0) {
            db = 20 * Mathf.Log10(volume);
            audioMixer.SetFloat("volumeSFX", db);

            PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME, volume);
        }
    }

    public void SetVolumeMaster(float volume) {
        if (volume != 0) {
            db = 20 * Mathf.Log10(volume);
            audioMixer.SetFloat("volumeMaster", db);

            PlayerPrefs.SetFloat(PLAYER_PREFS_MASTER_VOLUME, volume);
        }
    }

}
