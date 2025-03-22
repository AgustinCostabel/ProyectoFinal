using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalChoiceUI : MonoBehaviour
{
    public static FinalChoiceUI Instance { get; private set; }

    [SerializeField] private GameObject finalChoiceActivation;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE FINALCHOICEUI");
        }
        Instance = this;
    }

    public void Activate() {
        finalChoiceActivation.SetActive(true);
        Player.Instance.SetIsDoingAction(true);

    }
    public void Deactivate() {
        finalChoiceActivation.SetActive(false);
        Player.Instance.SetIsDoingAction(false);

    }
}
