using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Ink.Runtime;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguesUI : MonoBehaviour
{
    public static DialoguesUI Instance { get; private set; }

    [SerializeField] public Button continueButton;
    [SerializeField] public Button endDialogueButton;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image dialogueImage;
    [SerializeField] private TextMeshProUGUI dialogueTitle;
    [SerializeField] private float speedText;
    [SerializeField] private GameObject dialogueChoices;
    [SerializeField] private Choices[] Choices;

    private string titlePlayer;
    private Sprite spritePlayer;
    private string titleNPC;
    private Sprite spriteNPC;
    private Story currentStory;

    private int sentencesCount = 0;

    private void Awake() {
        Instance = this;
    }

    public void Start() {
        continueButton.onClick.AddListener(DialogueContinue);
        titlePlayer = Player.Instance.GetTitlePlayer();
        spritePlayer = Player.Instance.GetSpritePlayer();
    }

    public void DialogueStart(TextAsset inkJSON, Sprite newSprite, string title) {
        currentStory = new Story(inkJSON.text);
        continueButton.gameObject.SetActive(true);
        titleNPC = title;
        spriteNPC = newSprite;
        dialogueBox.SetActive(true);
        dialogueChoices.SetActive(false);
        Player.Instance.SetIsDoingAction(true);
        GameManager.Instance.SetMenuOpened(true);
        DialogueContinue();
    }


    public void DialogueContinue() {
        if (currentStory.canContinue) {
            if (sentencesCount % 2 != 0) {
                dialogueImage.sprite = spritePlayer;
                dialogueTitle.text = titlePlayer;
                dialogueBox.transform.localPosition = new Vector3(-405, -150, 0);
            } else {
                dialogueImage.sprite = spriteNPC;
                dialogueTitle.text = titleNPC;
                dialogueBox.transform.localPosition = new Vector3(405, -150, 0);
            }
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentStory.Continue()));
            sentencesCount++;
        } else {
            if (GameStateManager.Instance.IsThirdEvent()) {
                DialogueChoices();
            } else {
                DialogueEnd();
            }
        }
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speedText);
        }
    }

    public void DialogueEnd() {
        dialogueBox.SetActive(false);
        dialogueChoices.SetActive(false);
        Player.Instance.SetIsDoingAction(false);
        GameManager.Instance.SetMenuOpened(false);
        sentencesCount = 0;
    }

    public void DialogueChoices() {
        continueButton.gameObject.SetActive(false);
        dialogueChoices.SetActive(true);
        sentencesCount = 0;
    }

    public void ChangeChoices(TextAsset[] newDialogues, string[] dialogueQuesitons) {
        int index = 0;
        foreach (Choices button in Choices) {
            button.dialogueChoice = newDialogues[index];
            button.GetComponentInChildren<TextMeshProUGUI>().text = dialogueQuesitons[index];
            index++;
        }
    }

    public void ChoiceClicked(Choices choice) {
        DialogueStart(choice.dialogueChoice, spriteNPC, titleNPC);
    }
}
