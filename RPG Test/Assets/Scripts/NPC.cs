using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class NPC : MonoBehaviour, I_InteractableObject 
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Quest quest;
    [SerializeField] private TextAsset[] dialoguesIntroduction;
    [SerializeField] private TextAsset[] dialogueBeforeQuestion;
    [SerializeField] private TextAsset[] dialogueSpecial;
    [SerializeField] private TextAsset[] dialoguesChoices;
    [SerializeField] private string[] questionsText;
    [SerializeField] private Sprite dialogueSprite;
    [SerializeField] private string titleNPC;
    [SerializeField] private GameObject characterBox;
    [SerializeField] private AudioClip[] voice;
    [SerializeField] private Boolean specialDialogueSunset;
    [SerializeField] private Boolean specialDialogueNight;

    private int dialogueIndex = 0;
    private bool interactable = true;
    [SerializeField] private bool thirdChoice = false;
    [SerializeField] private bool fourChoice = false;

    public void Interact(Player player) {
        //Rotate to Player
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 360);

        //SoundManager.Instance.PlaySound(voice);
        DialoguesUI.Instance.ChangeChoices(dialoguesChoices, questionsText);
        if (thirdChoice) {
            DialoguesUI.Instance.ActivateThirdChoice();
        }
        if (fourChoice) {
            DialoguesUI.Instance.ActivateFourChoice();
        }

        //Dialogues
        if (dialoguesIntroduction != null) {
            if (!GameStateManager.Instance.TalkedWithNPC(titleNPC) || !GameStateManager.Instance.IsSecondEvent()) {
                GameStateManager.Instance.TalkedWith(titleNPC);
                DialoguesUI.Instance.DialogueStart(dialoguesIntroduction[dialogueIndex], dialogueSprite, titleNPC, this);
                //characterBox.SetActive(true);
                if (dialogueIndex < dialoguesIntroduction.Length - 1) {
                    dialogueIndex++;
                }
            } else {
                dialogueIndex = 0;
                if((specialDialogueNight && GameManager.Instance.IsNight()) || (specialDialogueSunset && GameManager.Instance.IsSunset())) {
                    DialoguesUI.Instance.SpecialDialogue();
                    DialoguesUI.Instance.DialogueStart(dialogueSpecial[dialogueIndex], dialogueSprite, titleNPC, this);
                } else {
                    DialoguesUI.Instance.DialogueStart(dialogueBeforeQuestion[dialogueIndex], dialogueSprite, titleNPC, this);
                }
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

    public void ActiveThirdChoice() {
        thirdChoice = true;
    }

    public void ActiveFourChoice() {
        fourChoice = true;
    }

    public Quest GetQuest() {
        return quest;
    }
}
