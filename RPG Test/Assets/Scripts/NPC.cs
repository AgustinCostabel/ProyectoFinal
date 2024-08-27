using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour, I_InteractableObject 
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Quest quest;
    [SerializeField] private TextAsset[] dialogues;
    [SerializeField] private TextAsset[] dialoguesChoices;
    [SerializeField] private string[] questionsText;
    [SerializeField] private Sprite dialogueSprite;
    [SerializeField] private string titleNPC;
    [SerializeField] private GameObject characterBox;

    private int dialogueIndex = 0;

    private bool interactable = true;
    public void Interact(Player player) {
            if (dialogues != null) {
                if (!GameStateManager.Instance.IsThirdEvent()) {
                    DialoguesUI.Instance.DialogueStart(dialogues[dialogueIndex], dialogueSprite, "???");
                    characterBox.SetActive(true);
                } else {
                    DialoguesUI.Instance.DialogueStart(dialogues[dialogueIndex], dialogueSprite, titleNPC);
                }
            }
            if (dialogueIndex < dialogues.Length - 1) {
                dialogueIndex++;
            }
            GameStateManager.Instance.TalkedWith(titleNPC);
            DialoguesUI.Instance.ChangeChoices(dialoguesChoices, questionsText);
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

    public Quest GetQuest() {
        return quest;
    }
}
