using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookUI : MonoBehaviour
{
    public static BookUI Instance { get; private set; }

    [SerializeField] private GameObject bookActivation;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN BOOK");
        }
        Instance = this;
    }

    public void Activate() {
        bookActivation.SetActive(true);
        Player.Instance.SetIsDoingAction(true);
    }

    public void Deactivate() {
        bookActivation.SetActive(false);
        Player.Instance.SetIsDoingAction(false);
    }
}
