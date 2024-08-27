using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChestMap : MonoBehaviour, I_InteractableObject
{
    private new Animation animation;
    [SerializeField] private Canvas canvas;

    private bool opened = false;
    private bool interactable = true;
    private bool canOpen = false;
    private bool isNight = false;

    public void Start() {
        animation = GetComponent<Animation>();
        GameManager.Instance.OnSunrise += GameManager_OnSunrise;
        GameManager.Instance.OnNight += GameManager_OnNight;
    }

    private void GameManager_OnNight(object sender, EventArgs e) {
        isNight = true;
    }

    private void GameManager_OnSunrise(object sender, EventArgs e) {
        isNight = false;
    }

    public void Interact(Player player) {
        canOpen = Player.Instance.HasKeyMap() && isNight;
        if (!canOpen) {
            //DialoguesUI.Instance.DialogueStart(dialogue);
            player.Talk("closed");
        }
        if (!opened && !player.IsWalking() &&canOpen) {
            opened = true;
            animation.Play();
            player.PlayGatherAnimation();
            gameObject.layer = LayerMask.NameToLayer("Default");
            interactable = false;
            DisableCanvas();
        }
    }

    public void OnDestroy() {
        Destroy(animation);
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
