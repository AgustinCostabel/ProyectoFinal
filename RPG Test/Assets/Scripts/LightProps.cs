using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightProps : MonoBehaviour
{
    [SerializeField] private GameObject lightProp;
    private Light newLight;
    void Start()
    {
        newLight = lightProp.GetComponent<Light>();
        GameManager.Instance.OnNight += GameManager_OnNight;
        GameManager.Instance.OnSunrise += GameManager_OnSunrise;
    }

    private void GameManager_OnSunrise(object sender, System.EventArgs e) {
        newLight.enabled = false;
    }

    private void GameManager_OnNight(object sender, System.EventArgs e) {
        newLight.enabled = true;
    }
}
