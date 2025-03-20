using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestNecklace : MonoBehaviour, I_InteractableObject {
    private new Animation animation;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Clue clue;


    private bool opened = false;
    private bool interactable = true;


    public void Start() {
        animation = GetComponent<Animation>();
    }
    public void Interact(Player player) {
        if (!Player.Instance.GetKeyChestNecklace()) {
            player.Talk("Closed, I need a KEY");
        } else {
            if (!opened && !player.IsWalking()) {
                player.Talk("What a beautiful necklace");
                GameStateManager.Instance.NecklaceObtained();
                opened = true;
                animation.Play();
                player.PlayGatherAnimation();
                gameObject.layer = LayerMask.NameToLayer("Default");
                interactable = false;
                //Clue
                clue.Activate();
                DisableCanvas();
            }
        }
    }

    public void OnDestroy() {
        Destroy(animation);
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
