using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsSO audioClipsSO;
    [SerializeField] AudioMixerGroup audioMixerGroup;

    private AudioSource audioSource;

    private float volume = 1f;
    private bool walkSound = false;
    private float walkSoundTimer = 0f;
    private float walkSoundTimerMax = 0.5f;
    [SerializeField] private float runningSoundTimerMax = 0.4f;

    private void Awake() {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = audioMixerGroup;

    }

    private void Start() {
        Player.Instance.OnDeathPlayer += Player_OnDeathPlayer;
    }

    private void Update() {
        walkSoundTimer -= Time.deltaTime;

        if (Player.Instance.IsRunning()) {
            walkSoundTimerMax = runningSoundTimerMax;
        } else {
            walkSoundTimerMax = 0.5f;
        }

        if(walkSoundTimer <= 0) {
            walkSound = false;
        }
    }

    private void Player_OnDeathPlayer(object sender, System.EventArgs e) {
        PlaySound(audioClipsSO.death, Camera.main.transform.position, 0.5f);
    }

    public void WalkSoundRock() {
        if (!walkSound && walkSoundTimer < 0) {
            PlaySound(audioClipsSO.footStepRock, Camera.main.transform.position, 0.7f);
            walkSound = true;
            walkSoundTimer = walkSoundTimerMax;
        }
    }

    public void WalkSoundGrass() {
        if (!walkSound && walkSoundTimer < 0) {
            PlaySound(audioClipsSO.footStepGrass, Camera.main.transform.position, 0.7f);
            walkSound = true;
            walkSoundTimer = walkSoundTimerMax;
        }
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume * volumeMultiplier);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
        audioSource.PlayOneShot(audioClip, volumeMultiplier * volume);
    }

}
