using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour, I_InteractableObject {

    [SerializeField] private Canvas canvas;
    [SerializeField] private ParticleSystem shine;
    [SerializeField] private GameObject clueBox;
    bool interactable = true;
    public void Interact(Player player) {
        if (shine.isPlaying) {
            //shine.Stop();
            clueBox.SetActive(true);
        }
        player.Talk("There is something interesting here");
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
