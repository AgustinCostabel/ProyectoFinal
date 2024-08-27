using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour, I_InteractableObject {

    [SerializeField] private Canvas canvas;
    private bool interactable = true;

    public bool GetInteractable() {
        return interactable;
    }

    public void DisableInteractable() {
        interactable = false;
    }

    public void Interact(Player player) {
        if (interactable) {
            interactable = false;
            player.EquipArmor();
            Destroy(gameObject);
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
