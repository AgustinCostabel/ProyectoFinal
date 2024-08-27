using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapons : MonoBehaviour, I_InteractableObject
{
    
    [SerializeField] private WeaponsSO weaponsSO;
    [SerializeField] private Canvas canvas;
    private bool interactable = true;

    public WeaponsSO GetWeaponSO() {
        return weaponsSO;
    }

    public void DisableInteractable() {
        interactable = false;
    }

    public void Interact(Player player) {
        if (interactable) {
            interactable = false;
            if (!player.HasWeaponEquiped()) {
                Transform weaponSOTransform = Instantiate(weaponsSO.prefab, player.GetRightHand());
                Weapons weapon = weaponSOTransform.GetComponent<Weapons>();
                player.ObtainWeapon(weapon);
                Destroy(gameObject);
            } else {
                //Creo Arma del jugador
                Transform playerWeaponSOTransform = Instantiate(player.GetWeapon().GetWeaponSO().prefab);
                Weapons playerWeapon = playerWeaponSOTransform.GetComponent<Weapons>();
                //La destruyo
                player.DestroyWeapon();
                //Le doy arma nueva
                Transform weaponSOTransform = Instantiate(weaponsSO.prefab, player.GetRightHand());
                Weapons weapon = weaponSOTransform.GetComponent<Weapons>();
                player.ObtainWeapon(weapon);
                //Posiciono arma antigua del jugador en el nuevo lugar
                playerWeapon.transform.position = this.transform.position;
                //Destruyo arma original
                Destroy(gameObject);
            }
        }
    }

    public static void SpawnWeapon(WeaponsSO weaponsSO, Player player, ChestWeapon chest) {
        if (!player.HasWeaponEquiped()) {
            Transform weaponSOTransform = Instantiate(weaponsSO.prefab, player.GetRightHand());
            Weapons weapon = weaponSOTransform.GetComponent<Weapons>();
            player.ObtainWeapon(weapon);
        } else {
            Transform weaponSOTransform = Instantiate(weaponsSO.prefab);
            Weapons weapon = weaponSOTransform.GetComponent<Weapons>();
            weapon.transform.position = chest.GetSpawnPosition().position;
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
}
