using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueChoices : MonoBehaviour
{
    [SerializeField] private Choices[] choices;
    [SerializeField] private GameObject[] choicesObject;

    public void ClearChoices() {
        foreach(GameObject choice in choicesObject) {
            choice.gameObject.SetActive(false);
        }
    }
     public void ActiveChoice(int index) {
        choicesObject[index].SetActive(true);
    }

    public void ChangeChoice(int index, TextAsset newDialogues, string dialogueQuestions) {
        choices[index].dialogueText = newDialogues;
        choices[index].GetComponentInChildren<TextMeshProUGUI>().text = dialogueQuestions;
    }

    public void SetActiveTrue() { 
        gameObject.SetActive(true);
    }

    public void SetActiveFalse() {
        gameObject.SetActive(false);
    }
}
