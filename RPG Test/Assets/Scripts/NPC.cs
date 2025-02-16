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
        //Rotate to Player
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 360);

        GameStateManager.Instance.TalkedWith(titleNPC);
        DialoguesUI.Instance.ChangeChoices(dialoguesChoices, questionsText);

        //Dialogues
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
