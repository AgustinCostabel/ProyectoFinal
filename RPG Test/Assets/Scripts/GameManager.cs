using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum State {
        Sunrise,
        Noon,
        Sunset,
        Night,
        DarkNight
    }

    private bool gamePaused = false;
    [SerializeField] private float gameTimerMax = 60f;
    private float gameTimer;
    private State state;
    [SerializeField] private Animator deathFadeOut;
    private bool menuOpened = false;

    public event EventHandler OnSunrise;
    public event EventHandler OnNoon;
    public event EventHandler OnSunset;
    public event EventHandler OnNight;
    public event EventHandler OnDarkNight;
    public event EventHandler OnTimeLapsed;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE GAME MANAGER");
        }
        Instance = this;
    }

    private void Start() {
        Player.Instance.OnDeathPlayer += Player_OnDeathPlayer;

        gameTimer = gameTimerMax;

        OnNight?.Invoke(this, EventArgs.Empty);   

        MenuUI.Instance.SetTimelineActive(true);

        StartGame();
    }

    private void Player_OnDeathPlayer(object sender, EventArgs e) {
        MusicManager.Instance.StopSong();
        deathFadeOut.SetTrigger("Death");
        Invoke("Restart" , 6);
    }

    void Update()
    {
 
        gameTimer -= Time.deltaTime; 

        switch(state) {
            case State.Sunrise:
                if(gameTimer < gameTimerMax - gameTimerMax / 5) {
                    state = State.Noon;
                    OnNoon?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Noon:
                if (gameTimer < gameTimerMax - gameTimerMax / 5 * 2) {
                    state = State.Sunset;
                    OnSunset?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Sunset:
                if (gameTimer < gameTimerMax - gameTimerMax / 5 * 3) {
                    state = State.Night;
                    OnNight?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Night:
                if (gameTimer < gameTimerMax - gameTimerMax / 5 * 4) {
                    state = State.DarkNight;
                    OnDarkNight?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.DarkNight:
                if (gameTimer < 0) {
                    state = State.Sunrise;
                    OnSunrise?.Invoke(this, EventArgs.Empty);
                    gameTimer = gameTimerMax;
                }
                break;
        }
     
    }

    public void StartGame() {
        gameTimer = gameTimerMax;

        UnpauseGame();

        MusicManager.Instance.StopSong();

        OnTimeLapsed.Invoke(this, EventArgs.Empty);
        OnSunrise.Invoke(this, EventArgs.Empty);

        MenuUI.Instance.SetTimelineActive(false);
        Player.Instance.SetIsDoingAction(false);
        WeatherManager.Instance.RainCutsceneStop();
        GameStateManager.Instance.FirstEvent();
    }

    public void Restart() {
        gameTimer = gameTimerMax;
        OnTimeLapsed.Invoke(this, EventArgs.Empty);
        OnSunrise.Invoke(this, EventArgs.Empty);
    }

    public void PauseGame() {
        gamePaused = true;
        Time.timeScale = 0f;
    }

    public void UnpauseGame() {
        gamePaused = false;
        Time.timeScale = 1f;
    }

    public float GetGameTimerMax() {
        return gameTimerMax;
    }

    public bool IsGamePaused() {
        return gamePaused;
    }

    public void PlayDeathFadeOut() {
        deathFadeOut.SetTrigger("Death");
    }

    public State GetGameState() {
        return state;
    }

    public bool GetMenuOpened() {
        return menuOpened;
    }

    public void SetMenuOpened(bool opened) {
        menuOpened = opened;
    }
}
