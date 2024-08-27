using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private GameObject journalActivation;
    
    private void Start() {
        GameInput.Instance.OnJournalAction += GameInput_OnJournalAction;
    }

    private void GameInput_OnJournalAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePaused()) {
            if (!journalActivation.gameObject.activeSelf && !GameManager.Instance.GetMenuOpened()) {
                Show();
                Player.Instance.SetIsDoingAction(true);
                GameManager.Instance.SetMenuOpened(true);
            } else {
                if (journalActivation.gameObject.activeSelf) {
                    Hide();
                    Player.Instance.SetIsDoingAction(false);
                    GameManager.Instance.SetMenuOpened(false);
                }
            }
        }
    }

    private void Hide() {
        if (journalActivation != null) {
            journalActivation.SetActive(false);
        }
    }

    private void Show() {
        if (journalActivation != null) {
            journalActivation.SetActive(true);
        }
    }
}
