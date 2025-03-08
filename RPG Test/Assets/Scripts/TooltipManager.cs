using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{

    public static TooltipManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject tooltipActivation;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN TOOLTIP MANAGER");
        }
        Instance = this;
    }

    void Start()
    {
        Cursor.visible = true;
        tooltipActivation.gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }


    public void SetAndShowTooltip(string message) {
        if (text.text == string.Empty) {
            tooltipActivation.gameObject.SetActive(true);
            text.text = message;
        }
    }

    public void HideTooltip() {
        tooltipActivation.gameObject.SetActive(false);
        text.text = string.Empty;
    }
}
