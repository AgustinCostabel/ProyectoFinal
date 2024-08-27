using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField] public Button acceptButton;
    [SerializeField] public Button declineButton;
    [SerializeField] public TextMeshProUGUI descrpitionText;
    [SerializeField] public TextMeshProUGUI titleText;
    [SerializeField] public Quest quest;

    public void Start() {
        acceptButton.onClick.AddListener(quest.AcceptQuest);
        declineButton.onClick.AddListener(quest.DeclineQuest);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
