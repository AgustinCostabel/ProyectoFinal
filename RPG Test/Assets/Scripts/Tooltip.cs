using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private string message;

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData) {
        //Output to console the GameObject's name and the following message
        TooltipManager.Instance.SetAndShowTooltip(message);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData) {
        //Output the following message with the GameObject's name
        TooltipManager.Instance.HideTooltip();
    }

    public void ChangeText(string t) {
        message = t;
    }
}
