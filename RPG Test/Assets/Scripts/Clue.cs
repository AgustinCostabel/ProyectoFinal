using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clue : MonoBehaviour, I_InteractableObject {

    [SerializeField] private Canvas canvas;
    [SerializeField] private ParticleSystem shine;
    [SerializeField] private GameObject clue;
    [SerializeField] private InventoryUI clueTextBox;
    [SerializeField] private string clueText;
    [SerializeField] private string playerTalk;
    [SerializeField] private Sprite clueImage;
    [SerializeField] private bool isText = false;

    bool interactable = true;
    bool obtainable = true;
    public void Interact(Player player) {
        if (interactable) {
            if (shine.isPlaying) {
                shine.Stop();
            }
            if (obtainable) {
                obtainable = false;
                if (isText) {
                    clue.GetComponent<TextMeshProUGUI>().text = clueText;
                } else {
                    clue.GetComponent<Image>().sprite = clueImage;
                    clue.GetComponent<Tooltip>().ChangeText(clueText);
                }
                clueTextBox.AddItem(clue);
                SoundManager.Instance.PlaySoundClue();
            }
        }
        player.Talk(playerTalk);
    }

    public void Activate() {
        if (shine.isPlaying) {
            shine.Stop();
        }
        if (obtainable) {
            obtainable = false;
            if (isText) {
                clue.GetComponent<TextMeshProUGUI>().text = clueText;
            } else {
                clue.GetComponent<Image>().sprite = clueImage;
                clue.GetComponent<Tooltip>().ChangeText(clueText);
            }
            clueTextBox.AddItem(clue);
            SoundManager.Instance.PlaySoundClue();
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
