using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    [SerializeField] private ParticleSystem rainParticles;
    [SerializeField] private ParticleSystem rainCutscene;
    [SerializeField] private AudioSource rainSound;
    [SerializeField] private AudioSource windSound;
    [SerializeField] private AudioSource jungleSound;

    public event EventHandler OnRaining;
    public event EventHandler OnStopRaining;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.OnNight += GameManager_OnNight;
        GameManager.Instance.OnSunrise += GameManager_OnSunrise;

    }

    private void GameManager_OnSunrise(object sender, EventArgs e) {
        RainStop();
    }

    private void GameManager_OnNight(object sender, System.EventArgs e) {
        RainPlay();
    }

    private void RainPlay() {
        rainParticles.Play();
        rainSound.Play();
        OnRaining?.Invoke(this, EventArgs.Empty);
    }

    private void RainStop() {
        rainParticles.Stop();
        rainSound?.Stop();
        OnStopRaining?.Invoke(this, EventArgs.Empty);
    }

    public void RainCutsceneStop() {
        rainCutscene.gameObject.SetActive(false);
    }

    public void PlayJungle() {
        if (jungleSound.isPlaying) {
            jungleSound.Stop();
        } else {
            jungleSound.Play();
        }
    }
}
