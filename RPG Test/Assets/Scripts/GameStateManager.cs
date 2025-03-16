using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    private const string LUKE = "Luke";
    private const string ROSE = "Rose";
    private const string DAREN = "Daren";
    private const string JUDY = "Judy";
    private const string SOFIA = "Sofia";
    private const string REN = "Ren";

    [SerializeField] private GameObject iara;
    [SerializeField] private Lantern lantern;
    [SerializeField] private NPC sofia;
    [SerializeField] private NPC rose;
    [SerializeField] private NPC luke;
    [SerializeField] private NPC daren;
    [SerializeField] private NPC judy;
    [SerializeField] private NPC ren;
    [SerializeField] private Transform lanternSpawn;
    [SerializeField] private GameObject secretFence;
    [SerializeField] private GameObject cemeteryDoorLeft;
    [SerializeField] private GameObject cemeteryDoorRight;

    [SerializeField] private bool isFirstEvent = false;
    [SerializeField] private bool isSecondEvent = false;
    [SerializeField] private bool isThirdEvent = false;

    private bool talkedWithRose = false;
    private bool talkedWithLuke = false;
    private bool talkedWithDaren = false;
    private bool talkedWithJudy = false;
    private bool talkedWithRen = false;
    private bool talkedWithSofia = false;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE GAME STATE MANAGER");
        }
        Instance = this;

        if (GameManager.Instance == null) {
            Debug.LogError("GameManager instance is null!");
        } else {
            GameManager.Instance.OnSunrise += GameManager_OnSunrise;
            GameManager.Instance.OnSunrise += GameManager_OnNight;
        }
    }

    private void Start() {
        
    }

    private void GameManager_OnSunrise(object sender, System.EventArgs e) {
        if (isThirdEvent) {
            secretFence.gameObject.transform.localEulerAngles = new Vector3(-90, 0, -35);
            rose.gameObject.transform.position = new Vector3(225, 10, 227);
        }
        lantern.gameObject.transform.parent = null;
        lantern.gameObject.transform.position = lanternSpawn.position;
        lantern.gameObject.transform.rotation = lanternSpawn.rotation;
    }

    private void GameManager_OnNight(object sender, System.EventArgs e) {
        if (isThirdEvent) {
            secretFence.gameObject.transform.localEulerAngles = new Vector3(-90, 0, -110);
            rose.gameObject.transform.position = new Vector3(0, 0, 0);
        }
        cemeteryDoorLeft.gameObject.transform.localEulerAngles = new Vector3(0, -180, 0);
        cemeteryDoorRight.gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    private void Update() {
        
    }

    public void FirstEvent() {

        isFirstEvent = true;
    }

    public void SecondEvent() {
        isFirstEvent = false;
        isSecondEvent = true;

        judy.ActiveThirdChoice();
        rose.gameObject.transform.position = new Vector3(225, 10, 227);
        ren.gameObject.transform.position = new Vector3(175, 10, 235);
    }

    public void ThirdEvent() {
        isSecondEvent = false;
        isFirstEvent = false;
        isThirdEvent = true;
    }

    public void TalkedWith(string nameNPC) {
        if(nameNPC == LUKE) {
            talkedWithLuke = true;
        }
        if(nameNPC == ROSE) {
            talkedWithRose = true;
        }
        if (nameNPC == DAREN) {
            talkedWithDaren = true;
        }
        if (nameNPC == JUDY) {
            talkedWithJudy = true;
        }
        if (nameNPC == REN) {
            talkedWithRen = true;
        }
        if (nameNPC == SOFIA) {
            talkedWithSofia = true;
            Player.Instance.ObtainJournal();
            SecondEvent();
        }
    }

    public bool TalkedWithNPC(string nameNPC) {
        if (nameNPC == LUKE) {
            return talkedWithLuke;
        }
        if (nameNPC == ROSE) {
            return talkedWithRose;
        }
        if (nameNPC == DAREN) {
            return talkedWithDaren;
        }
        if (nameNPC == JUDY) {
            return talkedWithJudy;
        }
        if (nameNPC == REN) {
            return talkedWithRen;
        }
        if (nameNPC == SOFIA) {
            return talkedWithSofia;
        }
        return false;
    }

    public bool IsFirstEvent() { 
        return isFirstEvent; 
    }
    public bool IsSecondEvent() {  
        return isSecondEvent; 
    }

    public bool IsThirdEvent() {
        return isThirdEvent;
    }

    public void BearEncounter() {
        daren.ActiveFourChoice();
    }
}
