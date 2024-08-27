using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip musicDay;
    [SerializeField] private AudioClip musicNight;
    [SerializeField] private AudioClip musicFight;
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    private AudioClip songPlaying;

    [SerializeField] private float defaultVolume;
    [SerializeField] private float transitionTime;

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        GameManager.Instance.OnNight += GameManager_OnNight;
        GameManager.Instance.OnSunrise += GameManager_OnSunrise;

        songPlaying = musicNight;
    }

    private void GameManager_OnSunrise(object sender, System.EventArgs e) {
        if (audioSource != null && songPlaying != musicFight) {
            StopAllCoroutines();
            StartCoroutine(ChangeSong(musicDay));
        }
    }

    private void GameManager_OnNight(object sender, System.EventArgs e) {
        if (audioSource != null && songPlaying != musicFight) {
            StopAllCoroutines();
            StartCoroutine(ChangeSong(musicNight));
        }
    }

    public void FightSong() {
        StopAllCoroutines();
        StartCoroutine(ChangeSong(musicFight));
    }

    public void StopSong() {
        audioSource.Stop();
        songPlaying = null;
    }

    public void DayNightSong() {
        StopAllCoroutines();
        if(GameManager.Instance.GetGameState() == GameManager.State.Night || GameManager.Instance.GetGameState() == GameManager.State.DarkNight) {
            StartCoroutine(ChangeSong(musicNight));
        } else {
            StartCoroutine(ChangeSong(musicDay));
        }
    }

    IEnumerator ChangeSong(AudioClip newSong) {
        if (newSong != songPlaying) {
            float percentage = 0f;
            if (songPlaying != null) {
                while (audioSource.volume > 0f) {
                    audioSource.volume = Mathf.Lerp(defaultVolume, 0f, percentage);
                    percentage += Time.deltaTime / transitionTime;
                    yield return null;
                }
            }

            audioSource.Stop();
            audioSource.clip = newSong;
            audioSource.Play();
            percentage = 0f;

            while (audioSource.volume < defaultVolume) {
                audioSource.volume = Mathf.Lerp(0f, defaultVolume, percentage);
                percentage += Time.deltaTime / transitionTime;
                yield return null;
            }

            songPlaying = newSong;
        }
    }
}
