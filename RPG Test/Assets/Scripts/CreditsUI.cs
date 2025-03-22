using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    public static CreditsUI Instance { get; private set; }

    [SerializeField] private GameObject creditsActivation;
    [SerializeField] private CreditsText creditsText;

    [TextArea(3, 10)] // Min 3 lines, Max 10 lines
    [SerializeField] private string badEndingText;

    [TextArea(3, 10)] // Min 3 lines, Max 10 lines
    [SerializeField] private string goodEndingText;

    public float scrollSpeed = 30f; // Adjust speed
    private RectTransform rectTransform;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE CREDITSUI");
        }
        Instance = this;
    }


    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update() {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }

    public void Activate(string newText) {
        creditsActivation.gameObject.SetActive(true);
        creditsText.StartCredits(newText);
    }

    public void ActivateBadEnding() {
        creditsActivation.gameObject.SetActive(true);
        creditsText.StartCredits(badEndingText);
    }

    public void ActivateGoodEnding() {
        creditsActivation.gameObject.SetActive(true);
        creditsText.StartCredits(goodEndingText);
    }

}
