using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] private QuestSO questSO;
    [SerializeField] private Canvas canvas;
    private QuestUI questUI;
    private bool isActive = false;
    private Player player;

    public QuestSO GetQuestSO() {
        return questSO;
    }

    public void Start() {
        questUI = canvas.GetComponent<QuestUI>();
        questUI.descrpitionText.text = questSO.description;
        questUI.titleText.text = questSO.title;
    }

    public void Update() {

    }

    public void ActiveQuestUI() {
        questUI.Show();
    }

    public void DeclineQuest() {
        if (questSO != null) {
            if (isActive) {
                isActive = false;
            }
        }
        questUI.Hide();
        player = null;
    }

    public void AcceptQuest() {
        if (questSO != null) {
            if (player.HasActiveQuest()) {
                Debug.Log("Ya hay Quest Activada");
            } else {
                if (!isActive) {
                    isActive = true;
                    player.ActiveQuest(this);
                }
            }
        }
        questUI.Hide();
    }

    public bool IsActive() {
        return isActive;
    }

    public void ActivePlayer(Player player) {
        if (player != null) {
            this.player = player;
        }
    }
}
