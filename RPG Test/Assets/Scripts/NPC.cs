using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour, I_InteractableObject 
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Quest quest;
    [SerializeField] private TextAsset[] dialoguesIntroduction;
    [SerializeField] private TextAsset[] dialogueBeforeQuestion;
    [SerializeField] private TextAsset[] dialoguesChoices;
    [SerializeField] private string[] questionsText;
    [SerializeField] private Sprite dialogueSprite;
    [SerializeField] private string titleNPC;
    [SerializeField] private GameObject characterBox;
    [SerializeField] private AudioClip[] voice;

    private int dialogueIndex = 0;
    private bool interactable = true;

    public void Interact(Player player) {
        //Rotate to Player
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 360);

        //SoundManager.Instance.PlaySound(voice);

        GameStateManager.Instance.TalkedWith(titleNPC);
        DialoguesUI.Instance.ChangeChoices(dialoguesChoices, questionsText);

        //Dialogues
        if (dialoguesIntroduction != null) {
            if (GameStateManager.Instance.IsFirstEvent()) {
                DialoguesUI.Instance.DialogueStart(dialoguesIntroduction[dialogueIndex], dialogueSprite, "???", this);
                characterBox.SetActive(true);
                if (dialogueIndex < dialoguesIntroduction.Length - 1) {
                    dialogueIndex++;
                }
            } else {
                DialoguesUI.Instance.DialogueStart(dialogueBeforeQuestion[dialogueIndex], dialogueSprite, titleNPC, this);
            }
        }
    }

    public void PlayVoiceSound() {
        SoundManager.Instance.PlaySound(voice);
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
