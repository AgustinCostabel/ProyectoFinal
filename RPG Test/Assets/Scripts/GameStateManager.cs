using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

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

    private bool isFirstEvent = false;
    private bool isSecondEvent = false;
    private bool isThirdEvent = false;

    private bool talkedWithRose = false;
    private bool talkedWithLuke = false;
    private bool talkedWithDaren = false;
    private bool talkedWithJudy = false;
    private bool talkedWithRen = false;
    private bool talkedWithSofia = false;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GameManager.Instance.OnNight += GameManager_OnNight;
        GameManager.Instance.OnSunrise += GameManager_OnSunrise;
    }

    private void GameManager_OnSunrise(object sender, System.EventArgs e) {
        if (isThirdEvent) {
            secretFence.gameObject.transform.localEulerAngles = new Vector3(-90, 0, -35);
            rose.gameObject.transform.position = new Vector3(225, 10, 227);
        }
    }

    private void GameManager_OnNight(object sender, System.EventArgs e) {
        if (isThirdEvent) {
            secretFence.gameObject.transform.localEulerAngles = new Vector3(-90, 0, -110);
            rose.gameObject.transform.position = new Vector3(0, 0, 0);
        }  
    }

    private void Update() {
        if (isFirstEvent && TalkedWithALL()) {
            //SecondEvent();
        } else {
            if (isSecondEvent && talkedWithSofia) {
               ThirdEvent();
            }
        }
    }

    public void FirstEvent() {
        lantern.gameObject.transform.parent = null;
        lantern.gameObject.transform.position = lanternSpawn.position;
        lantern.gameObject.transform.rotation = lanternSpawn.rotation;

        isFirstEvent = true;
    }

    public void SecondEvent() {
        isFirstEvent = false;
        isSecondEvent = true;
    }

    public void ThirdEvent() {
        ren.gameObject.transform.position = new Vector3(175, 10, 235);
        rose.gameObject.transform.position = new Vector3(225, 10, 227);
        isSecondEvent = false;
        isThirdEvent = true;
    }

    public void TalkedWith(string nameNPC) {
        if(nameNPC == "Luke") {
            talkedWithLuke = true;
        }
        if(nameNPC == "Rose") {
            talkedWithRose = true;
        }
        if (nameNPC == "Daren") {
            talkedWithDaren = true;
        }
        if (nameNPC == "Judy") {
            talkedWithJudy = true;
        }
        if (nameNPC == "Ren") {
            talkedWithRen = true;
        }
        if(nameNPC == "Sofia" && isSecondEvent) {
            talkedWithSofia = true;
        }
    }

    public bool TalkedWithALL() {
        return talkedWithRose && talkedWithLuke && talkedWithJudy && talkedWithDaren && talkedWithRen;
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
}
