using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingObjects : MonoBehaviour, I_InteractableObject {
    [SerializeField] private Canvas canvas;
    [SerializeField] private ParticleSystem shine;
    [SerializeField] private string text;

    bool interactable = true;

    public void Interact(Player player) {
        //GameManager.Instance.RestartGame();
        text = GetComponent<TextMeshProUGUI>().text;
        CreditsUI.Instance.Activate(text);
        Player.Instance.SetIsDoingAction(true);
    }

    public void EnableCanvas() {
        canvas.gameObject.SetActive(true);
    }

    public void DisableCanvas() {
        canvas.gameObject.SetActive(false);
    }

    public bool IsInteractable() {
        return interactable;
    }

}
