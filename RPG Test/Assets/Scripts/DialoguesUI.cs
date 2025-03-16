using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Ink.Parsed;
using Ink.Runtime;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class DialoguesUI : MonoBehaviour
{
    public static DialoguesUI Instance { get; private set; }

    private const string LUKE = "Luke";
    private const string ROSE = "Rose";
    private const string DAREN = "Daren";
    private const string JUDY = "Judy";
    private const string SOFIA = "Sofia";
    private const string REN = "Ren";

    [SerializeField] public Button continueButton;
    [SerializeField] public Button endDialogueButton;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private UnityEngine.UI.Image dialogueImage;
    [SerializeField] private TextMeshProUGUI dialogueTitle;
    [SerializeField] private float speedText;
    [SerializeField] private DialogueChoices dialogueChoices;

    [SerializeField] private float wiggleSpeed;
    [SerializeField] private float wiggleAmplitud;

    private string titlePlayer;
    private Sprite spritePlayer;
    private string titleNPC;
    private Sprite spriteNPC;
    private Ink.Runtime.Story currentStory;
    private NPC currentNPC;
    private Boolean isDialogueSpecial = false;

    private int sentencesCount = 0;

    private List<(int startIndex, int length)> wiggleWordRanges = new List<(int startIndex, int length)>();

    private void Awake() {
        Instance = this;
    }

    public void Start() {
        continueButton.onClick.AddListener(DialogueContinue);
        titlePlayer = Player.Instance.GetTitlePlayer();
        spritePlayer = Player.Instance.GetSpritePlayer();
        continueButton.gameObject.SetActive(false);
    }

    public void DialogueStart(TextAsset inkJSON, Sprite newSprite, string title, NPC npc) {
        currentStory = new Ink.Runtime.Story(inkJSON.text);
        titleNPC = title;
        spriteNPC = newSprite;
        currentNPC = npc;
        dialogueBox.SetActive(true);
        dialogueChoices.SetActiveTrue();
        Player.Instance.SetIsDoingAction(true);
        GameManager.Instance.SetMenuOpened(true);
        continueButton.gameObject.SetActive(true);
        DialogueContinue();
    }


    public void DialogueContinue() {
        //continueButton.gameObject.SetActive(false);
        if (currentStory.canContinue) {
            /*if (sentencesCount % 2 != 0) {
                dialogueImage.sprite = spritePlayer;
                dialogueTitle.text = titlePlayer;
                dialogueBox.transform.localPosition = new Vector3(-405, -150, 0);
            } else {
                dialogueImage.sprite = spriteNPC;
                dialogueTitle.text = titleNPC;
                dialogueBox.transform.localPosition = new Vector3(405, -150, 0);
                currentNPC.PlayVoiceSound();
            }*/
            dialogueImage.sprite = spriteNPC;
            dialogueTitle.text = titleNPC;
            //dialogueBox.transform.localPosition = new Vector3(405, -150, 0);
            currentNPC.PlayVoiceSound();
            dialogueChoices.SetActiveFalse();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentStory.Continue()));
            sentencesCount++;
        } else {
            if (GameStateManager.Instance.IsSecondEvent() && !isDialogueSpecial) {
                DialogueChoices();
            } else {
                DialogueEnd();
                isDialogueSpecial = false;
            }
        }
    }

    IEnumerator TypeSentence(string sentence) {
        string processedText = ProcessTags(sentence); // Process tags to identify effects
        dialogueText.text = ""; // Clear the dialogue box

        StartCoroutine(WiggleWords()); // Start wiggling as text is added

        bool isAddingRichTextTag = false;
        foreach (char letter in processedText.ToCharArray()) {
            if (letter == '<' || isAddingRichTextTag) {
                // If we're adding a rich text tag, don't wait
                isAddingRichTextTag = true;
                dialogueText.text += letter;

                if (letter == '>') // End of the rich text tag
                {
                    isAddingRichTextTag = false;
                }
            } else {
                // Add the letter and wait for the typing effect
                dialogueText.text += letter;
                dialogueText.ForceMeshUpdate(); // Update the mesh for the wiggle effect
                yield return new WaitForSeconds(speedText / 100);
            }
        }
    }

    private string ProcessTags(string text) {
        wiggleWordRanges.Clear();

        // Regex to find words wrapped in <wiggle>
        Regex regex = new Regex(@"<wiggle>(.*?)</wiggle>");
        MatchCollection matches = regex.Matches(text);

        int indexOffset = 0;
        foreach (Match match in matches) {
            string word = match.Groups[1].Value;

            // Track the start index and length of the word in the processed string
            int startIndex = match.Index - indexOffset;
            wiggleWordRanges.Add((startIndex, word.Length));

            // Replace the tag with plain text or visible styling
            text = text.Replace(match.Value, $"<b>{word}</b>");
            indexOffset += match.Value.Length - word.Length; // Adjust for removed tags
        }

        return text;
    }

    private IEnumerator WiggleWords() {
        TMP_TextInfo textInfo = dialogueText.textInfo;
        dialogueText.ForceMeshUpdate();

        while (true) {
            // Loop through all characters in the text
            for (int i = 0; i < textInfo.characterCount; i++) {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                // Check if the character falls within a "wiggle" word range
                foreach (var range in wiggleWordRanges) {
                    int startIndex = range.startIndex;
                    int endIndex = startIndex + range.length;

                    if (i >= startIndex && i < endIndex) // If character is within this range
                    {
                        int vertexIndex = charInfo.vertexIndex;
                        Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                        // Apply the wiggle effect
                        float offset = Mathf.Sin(Time.time * wiggleSpeed + i) * wiggleAmplitud; // Adjust speed/amplitude as needed
                        for (int j = 0; j < 4; j++) {
                            vertices[vertexIndex + j].y += offset;
                        }

                        break; // Stop checking other ranges for this character
                    }
                }
            }

            // Update the mesh with the modified vertices
            for (int i = 0; i < textInfo.meshInfo.Length; i++) {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

    public void DialogueEnd() {
        continueButton.gameObject.SetActive(false);
        dialogueBox.SetActive(false);
        dialogueChoices.SetActiveFalse();
        Player.Instance.SetIsDoingAction(false);
        GameManager.Instance.SetMenuOpened(false);
        sentencesCount = 0;
        DeactiveChoices();
    }

    public void DialogueChoices() {
        continueButton.gameObject.SetActive(false);
        dialogueChoices.SetActiveTrue();
        sentencesCount = 0;
    }

    public void ChangeChoices(TextAsset[] newDialogues, string[] dialogueQuestions) {
        // Clear existing choices
        // dialogueChoices.ClearChoices();

        // Add new choices to the list
        for (int i = 0; i < newDialogues.Length; i++) {
            //dialogueChoices.ActiveChoice(i);
            dialogueChoices.ChangeChoice(i, newDialogues[i], dialogueQuestions[i]);
        }
    }

    public void ChoiceClicked(Choices choice) {
        sentencesCount = 0;
        dialogueChoices.SetActiveFalse();
        if(titleNPC == DAREN && choice.index == 3) {
            Player.Instance.ObtainKeyChestWeapon();
        }
        if (titleNPC == JUDY && choice.index == 2) {
            Player.Instance.ObtainMap();
        }
        DialogueStart(choice.dialogueText, spriteNPC, titleNPC, currentNPC);
    }

    public void SpecialDialogue() {
        isDialogueSpecial = true;
    }

    public void ActivateThirdChoice() {
        dialogueChoices.ActiveChoice(2);
    }

    public void ActivateFourChoice() {
        dialogueChoices.ActiveChoice(3);
    }

    public void DeactiveChoices() {
        dialogueChoices.DeactiveChoice(2);
        dialogueChoices.DeactiveChoice(3);
    }
}
