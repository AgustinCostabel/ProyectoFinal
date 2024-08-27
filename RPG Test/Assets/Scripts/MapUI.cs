using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] private GameObject mapActivation;

    private void Start() {
        GameInput.Instance.OnMapAction += GameInput_OnMapAction;
    }

    private void GameInput_OnMapAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePaused()) {
            if (!mapActivation.gameObject.activeSelf && !GameManager.Instance.GetMenuOpened()) {
                Show();
                Player.Instance.SetIsDoingAction(true);
                GameManager.Instance.SetMenuOpened(true);
            } else {
                if (mapActivation.gameObject.activeSelf) {
                    Hide();
                    Player.Instance.SetIsDoingAction(false);
                    GameManager.Instance.SetMenuOpened(false);
                }
            }
        }
    }

    private void Hide() {
        if (mapActivation != null) {
            mapActivation.SetActive(false);
        }
    }

    private void Show() {
        if (mapActivation != null) {
            mapActivation.SetActive(true);
        }
    }
}
