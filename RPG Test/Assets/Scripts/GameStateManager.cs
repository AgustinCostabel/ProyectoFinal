using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using TMPro;
using UnityEngine;
using static Clue;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }

    private const string LUKE = "Luke";
    private const string ROSE = "Rose";
    private const string DAREN = "Daren";
    private const string JUDY = "Judy";
    private const string SOFIA = "Sofia";
    private const string REN = "Ren";

    private const string FOOTPRINTS = "Footprints";
    private const string CORPSE = "Corpse";
    private const string WELL = "Well";
    private const string FISHINGROPE = "FishingRope";
    private const string WEARDROBE = "Weardrobe";
    private const string FIREPLACE = "Fireplace";
    private const string BED = "Bed";
    private const string PAINTING = "Painting";
    private const string KEY = "Key";
    private const string CORPSECAVE = "CorpseCave";
    private const string PILLARSTONE1 = "PillarStone1";
    private const string PILLARSTONE2 = "PillarStone2";
    private const string PILLARSTONE3 = "PillarStone3";
    private const string PILLARSTONE4 = "PillarStone4";
    private const string BOOK = "Book";
    private const string LOVEPOTION = "LovePotion";

    [SerializeField] private GameObject sheila;
    [SerializeField] private Lantern lantern;
    [SerializeField] private NPC sofia;
    [SerializeField] private NPC rose;
    [SerializeField] private NPC luke;
    [SerializeField] private NPC daren;
    [SerializeField] private NPC judy;
    [SerializeField] private NPC ren;
    [SerializeField] private NPC ashley;
    [SerializeField] private Transform lanternSpawn;
    [SerializeField] private GameObject fakeWall;
    [SerializeField] private GameObject secretFence;
    [SerializeField] private GameObject cemeteryDoorLeft;
    [SerializeField] private GameObject cemeteryDoorRight;
    [SerializeField] private GameObject towerDoor;
    [SerializeField] private GameObject secretDoor;
    [SerializeField] private GameObject lakeFenceLeft;
    [SerializeField] private GameObject lakeFenceRight;
    [SerializeField] private GameObject vaseFire1;
    [SerializeField] private GameObject vaseFire2;
    [SerializeField] private GameObject vaseFire3;
    [SerializeField] private GameObject vaseFire4;
    [SerializeField] private GameObject sheilaCorpse;

    [SerializeField] private bool isFirstEvent = false;
    [SerializeField] private bool isSecondEvent = false;
    [SerializeField] private bool isThirdEvent = false;

    private bool talkedWithRose = false;
    private bool talkedWithLuke = false;
    private bool talkedWithDaren = false;
    private bool talkedWithJudy = false;
    private bool talkedWithRen = false;
    private bool talkedWithSofia = false;

    private int activatedStones = 0;
    private bool activateLabyrinthSecretDoor;

    //Missions
    [SerializeField] private GameObject missions;

    [SerializeField] private Clue missionWeapon;
    [SerializeField] private Clue missionBear;
    [SerializeField] private Clue missionLabyrinth;
    [SerializeField] private Clue missionDiary;
    [SerializeField] private Clue missionTruth;

    //Clues
    [SerializeField] private Clue[] clues;
    [SerializeField] private Clue ringS;
    [SerializeField] private Clue ringD;
    [SerializeField] private Clue well;
    [SerializeField] private Clue key;
    [SerializeField] private Clue lakeFence;
    [SerializeField] private Clue cemetery;

    //EndingObjects
    [SerializeField] private EndingObjects fire;
    [SerializeField] private EndingObjects potion;

    private void OnEnable() {
        foreach (Clue clue in clues) {
            clue.OnClueInteracted += HandleClueInteraction;
        }
    }

    private void HandleClueInteraction(object sender, ClueEventArgs e) {
        // Get the clue that was interacted with
        Clue interactedClue = e.InteractedClue;

        // Perform actions based on the interacted clue
        if (interactedClue.name == FOOTPRINTS) {
            Debug.Log("Active Ren choice");
            ren.ActiveThirdChoice();
        }
        if (interactedClue.name == FISHINGROPE) {
            Player.Instance.ObtainFishingRope();
            well.ChangePlayerTalkText("A couple of golden rings");
        }
        if (interactedClue.name == WELL) {
            if (Player.Instance.HasFishingRope()) {
                interactedClue.gameObject.SetActive(false);
                ringS.Activate();
                ringD.Activate();
                daren.ActiveThirdChoice();
            }
        }
        if (interactedClue.name == WEARDROBE || interactedClue.name == FIREPLACE || interactedClue.name == BED) {
            sofia.ActiveThirdChoice();
        }
        if (interactedClue.name == PAINTING) {
            key.gameObject.SetActive(true);
        }
        if (interactedClue.name == KEY) {
            Player.Instance.ObtainKeyChestNecklace();
        }
        if (interactedClue.name == CORPSE) {
            interactedClue.Deactivate();
            UpdateMissionText("Corpse", "The corpse has no visible wounds");
        }
        if (interactedClue.name == PILLARSTONE1) {
            Debug.Log("Stone 1 Activated");
            vaseFire1.gameObject.SetActive(true);
            interactedClue.Deactivate();
            SoundManager.Instance.PlaySoundStonePush();
            activatedStones++;
            if (activatedStones == 4) {
                SoundManager.Instance.PlaySoundSecretDoor();
                secretDoor.gameObject.SetActive(false);
            }
        }
        if (interactedClue.name == PILLARSTONE2) {
            Debug.Log("Stone 2 Activated");
            vaseFire2.gameObject.SetActive(true);
            interactedClue.Deactivate();
            SoundManager.Instance.PlaySoundStonePush();
            activatedStones++;
            if (activatedStones == 4) {
                SoundManager.Instance.PlaySoundSecretDoor();
                secretDoor.gameObject.SetActive(false);
            }
        }
        if (interactedClue.name == PILLARSTONE3) {
            Debug.Log("Stone 3 Activated");
            vaseFire3.gameObject.SetActive(true);
            interactedClue.Deactivate();
            SoundManager.Instance.PlaySoundStonePush();
            activatedStones++;
            if (activatedStones == 4) {
                SoundManager.Instance.PlaySoundSecretDoor();
                secretDoor.gameObject.SetActive(false);
            }
        }
        if (interactedClue.name == PILLARSTONE4) {
            Debug.Log("Stone 4 Activated");
            vaseFire4.gameObject.SetActive(true);
            interactedClue.Deactivate();
            SoundManager.Instance.PlaySoundStonePush();
            activatedStones++;
            if(activatedStones == 4) {
                SoundManager.Instance.PlaySoundSecretDoor();
                secretDoor.gameObject.SetActive(false);
            }
        }
        if (interactedClue.name == BOOK) {
            GameManager.Instance.CallNight();
            SoundManager.Instance.PlaySoundThunder();
            UpdateMissionText("Labyrinth", "I found a BOOK");
            BookUI.Instance.Activate();
        }
        if (interactedClue.name == LOVEPOTION) {
            rose.ActiveThirdChoice();
        }
    }

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE GAME STATE MANAGER");
        }
        Instance = this;

        if (GameManager.Instance == null) {
            Debug.LogError("GameManager instance is null!");
        } else {
            GameManager.Instance.OnSunrise += GameManager_OnSunrise;
            GameManager.Instance.OnSunset += GameManager_OnSunset;
            GameManager.Instance.OnNight += GameManager_OnNight;
            GameManager.Instance.OnTimeLapsed += GameManager_OnTimeLapsed;
        }
    }

    private void GameManager_OnTimeLapsed(object sender, System.EventArgs e) {
        
    }

    private void Start() {
        if (isSecondEvent) {
            SecondEvent();
        }
    }

    private void GameManager_OnSunrise(object sender, System.EventArgs e) {
        if (isThirdEvent) {
            secretFence.gameObject.transform.localEulerAngles = new Vector3(-90, 0, -35);
            rose.gameObject.transform.position = new Vector3(225, 10, 227);
        }
        lantern.gameObject.transform.parent = null;
        lantern.gameObject.transform.position = lanternSpawn.position;
        lantern.gameObject.transform.rotation = lanternSpawn.rotation;
        cemeteryDoorLeft.gameObject.transform.localEulerAngles = new Vector3(0, -90, 0);
        cemeteryDoorRight.gameObject.transform.localEulerAngles = new Vector3(0, 90, 0);
    }

    private void GameManager_OnSunset(object sender, System.EventArgs e) {

        //NPC
        judy.transform.position = new Vector3(215, 10, 165);
        judy.transform.localEulerAngles = new Vector3(0, 0, 0);
        judy.ActiveFourChoice();

        ren.transform.position = new Vector3(70, 10, 195);
        ren.transform.localEulerAngles = new Vector3(0, -90, 0);

        rose.transform.position = new Vector3(90, 10, 245);

        sheilaCorpse.gameObject.SetActive(false);
    }

    private void GameManager_OnNight(object sender, System.EventArgs e) {
        secretFence.gameObject.transform.localEulerAngles = new Vector3(-90, 0, -120);
        cemeteryDoorLeft.gameObject.transform.localEulerAngles = new Vector3(0, -180, 0);
        cemeteryDoorRight.gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        towerDoor.gameObject.SetActive(true);
        cemetery.gameObject.SetActive(false);

        //NPC
        judy.transform.position = new Vector3(208, 10, 253);
        judy.transform.localEulerAngles = new Vector3(0, 180, 0);

        ren.transform.position = new Vector3(172, 10, 235);
        ren.transform.localEulerAngles = new Vector3(0, -90, 0);

        daren.transform.position = new Vector3(158.8f, 10, 318.6f);
        daren.transform.localEulerAngles = new Vector3(0, 61, 0);

        rose.transform.position = new Vector3(252, 10, 233.5f);
        rose.transform.localEulerAngles = new Vector3(0, 125, 0);
        rose.ActiveFourChoice();

        sofia.transform.position = new Vector3(253f, 10, 231.7f);
        sofia.transform.localEulerAngles = new Vector3(0, 90, 0);

        luke.transform.position = new Vector3(132.65f, 10.8f, 163.95f);
        luke.transform.localEulerAngles = new Vector3(0, 0, 0);

        //sheilaCorpse.gameObject.SetActive(true);

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
        fakeWall.gameObject.SetActive(false);
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
        missionWeapon.Activate();
        missionBear.Activate();
    }

    public void LabyrinthEncounter() {
        missionLabyrinth.Activate();
    }

    public void NecklaceObtained() {
        luke.ActiveThirdChoice();
    }

    public void FirstEntrieActivated() {
        missionDiary.Activate();
    }

    public void SecondEntrieActivated() {
        sofia.ActiveFourChoice();
    }

    public void FifthEntrieActivated() {
        lakeFenceLeft.transform.localEulerAngles = new Vector3(0, -90, 0);
        lakeFenceRight.transform.localEulerAngles = new Vector3(0, 90, 0);
        lakeFence.gameObject.SetActive(false);
        UpdateMissionText("Diary", "I found all the DIARY entries");
        missionTruth.Activate();
    }

    public void TalkedWithEndingCharacter() {
        fire.gameObject.SetActive(true);
        potion.gameObject.SetActive(true);
    }

    public void UpdateMissionText(string missionName, string newText) {
        TMP_Text[] textObjects = missions.GetComponentsInChildren<TMP_Text>(); // Creates the array

        foreach (TMP_Text text in textObjects) {
            if (text.name == missionName) {
                text.text = newText;
                text.color = new Color32(255, 215, 0, 255);
                SoundManager.Instance.PlaySoundClue();
                break; // Stop loop after finding the mission
            }
        }
    }
}
