using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChestWeapon : MonoBehaviour, I_InteractableObject
{
    [SerializeField] protected WeaponsSO weaponSO;
    private new Animation animation;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Clue clue;


    private bool opened = false;
    private bool interactable = true;


    public void Start() {
        animation = GetComponent<Animation>();
    }
    public void Interact(Player player) {
        if (!Player.Instance.GetKeyChestWeapon()) {
            player.Talk("Closed, I need a KEY");
        }
        if (!opened && !player.IsWalking() && Player.Instance.GetKeyChestWeapon()) {
            opened = true;
            animation.Play();
            player.PlayGatherAnimation();
            Weapons.SpawnWeapon(weaponSO, player, this);
            gameObject.layer = LayerMask.NameToLayer("Default");
            interactable = false;
            player.IncreaseHealth();
            //Clue
            clue.Activate();
            DisableCanvas();
        }
    }

    public Transform GetSpawnPosition() {
        return spawnPosition;
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
