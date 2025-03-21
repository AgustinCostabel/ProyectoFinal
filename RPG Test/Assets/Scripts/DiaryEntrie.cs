using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryEntrie : MonoBehaviour, I_InteractableObject {

    [SerializeField] private Canvas canvas;
    [SerializeField] private ParticleSystem shine;
    [SerializeField] private string text;
    [SerializeField] private string riddle;
    private bool interactable = true;
    private bool firstInteraction = false;
    public void Interact(Player player) {
        if (shine.isPlaying) {
            shine.Stop();
        }
        DiaryUI.Instance.EntrieActivated(this, text, riddle, firstInteraction);
        if (!firstInteraction) {
            firstInteraction = true;
        }
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
