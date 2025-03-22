using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [SerializeField] private float typeSpeed = 0.05f; // Time between each letter appearing
    [SerializeField] private Button myButton;
    public string fullText;

    public void StartCredits(string newText) {
        textMesh = GetComponent<TextMeshProUGUI>();
        fullText = newText;
        textMesh.text = ""; // Start with an empty text
        myButton.gameObject.SetActive(false);
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText() {
        for (int i = 0; i < fullText.Length; i++) {
            textMesh.text += fullText[i]; // Add one letter at a time
            yield return new WaitForSeconds(typeSpeed);
        }

        myButton.gameObject.SetActive(true);
    }
}
