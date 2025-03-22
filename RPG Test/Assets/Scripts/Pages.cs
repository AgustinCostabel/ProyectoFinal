using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pages : MonoBehaviour, I_InteractableObject {
    [SerializeField] private Canvas canvas;
    [SerializeField] private string text;
    private bool interactable = true;

    public void Interact(Player player) {
        DiaryUI.Instance.PageActivated(text);
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
