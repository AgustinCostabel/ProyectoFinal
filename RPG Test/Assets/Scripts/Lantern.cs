using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Lantern : MonoBehaviour, I_InteractableObject{

    [SerializeField] private Canvas canvas;
    [SerializeField] private Light lanternLight;
    private bool interactable = true;

    public void Interact(Player player) {
        transform.position = new Vector3(0.05f, 0.05f, 0.01f);
        transform.rotation = Quaternion.Euler(-20f, 90f, 0f);
        Transform lanternTransform = Instantiate(transform, player.GetLeftHand());
        Lantern lantern = lanternTransform.GetComponent<Lantern>();
        player.EquipLantern(lantern);
        lantern.GetComponent<Collider>().enabled = false;
        lantern.lanternLight.transform.localEulerAngles = new Vector3(250, 0, 0);
        lantern.lanternLight.gameObject.SetActive(true);
        lantern.canvas.gameObject.SetActive(false);
        Destroy(gameObject);
        interactable = false;
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

    public Light GetLight() {
        return lanternLight;
    }
}
