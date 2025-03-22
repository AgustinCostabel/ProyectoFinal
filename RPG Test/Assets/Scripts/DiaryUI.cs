using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiaryUI : MonoBehaviour
{
    public static DiaryUI Instance { get; private set; }

    // Start is called before the first frame update
    [SerializeField] private DiaryEntrie entrie1;
    [SerializeField] private DiaryEntrie entrie2;
    [SerializeField] private DiaryEntrie entrie3;
    [SerializeField] private DiaryEntrie entrie4;
    [SerializeField] private DiaryEntrie entrie5;

    [SerializeField] private GameObject diaryActivation;
    [SerializeField] private GameObject pageActivation;
    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private TextMeshProUGUI entrieText;
    [SerializeField] private TextMeshProUGUI entrieRiddle;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE DIARYUI");
        }
        Instance = this;
    }

    public void EntrieActivated(DiaryEntrie entrie, string text, string riddle, bool interaction) {
        if(entrie == entrie1 && !interaction) {
            GameStateManager.Instance.FirstEntrieActivated();
            entrie2.gameObject.SetActive(true);
        }
        if (entrie == entrie2 && !interaction) {
            GameStateManager.Instance.SecondEntrieActivated();
            entrie3.gameObject.SetActive(true);
        }
        if (entrie == entrie3 && !interaction) {
            entrie4.gameObject.SetActive(true);
        }
        if (entrie == entrie4 && !interaction) {
            entrie5.gameObject.SetActive(true);
        }
        if (entrie == entrie5 && !interaction) {
            GameStateManager.Instance.FifthEntrieActivated();
        }
        entrieText.text = text;
        entrieRiddle.text = riddle;
        diaryActivation.SetActive(true);
        Player.Instance.SetIsDoingAction(true);
    }

    public void PageActivated(string t) {
        pageText.text = t;
        pageActivation.SetActive(true);
        Player.Instance.SetIsDoingAction(true);
    }

    public void Deactivate() {
        diaryActivation.SetActive(false);
        Player.Instance.SetIsDoingAction(false);
    }

    public void DeactivatePage() {
        pageActivation.SetActive(false);
        Player.Instance.SetIsDoingAction(false);
    }

}
