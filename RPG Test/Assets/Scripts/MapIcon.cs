using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class MapIcon : MonoBehaviour
{
    [SerializeField] private Image barImage;

    private void Start() {
    }

    private void Update() {
        barImage.transform.localEulerAngles = new Vector3(90, -(this.transform.parent.gameObject.transform.localEulerAngles.y), 0);
    }


    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
