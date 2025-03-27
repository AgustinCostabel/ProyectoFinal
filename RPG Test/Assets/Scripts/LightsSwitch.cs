using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsSwitch : MonoBehaviour
{
    public static LightsSwitch Instance { get; private set; }

    [SerializeField] private GameObject[] lightsTown;
    [SerializeField] private GameObject[] lightsCemetery;
    [SerializeField] private GameObject[] lightsVillage;
    [SerializeField] private GameObject[] lightsLabyrinth;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE GAME MANAGER");
        }
        Instance = this;
    }

    private void Start() {
        GameManager.Instance.OnNight += Instance_OnNight;
    }

    private void Instance_OnNight(object sender, System.EventArgs e) {
        foreach (GameObject lightsTown in GetLightsTown()) {
            if (lightsTown != null) {
                lightsTown.GetComponent<Light>().enabled = false;
            }
        }
        foreach (GameObject lightsVillage in GetLightsVillage()) {
            if (lightsVillage != null) {
                lightsVillage.GetComponent<Light>().enabled = false;
            }
        }
    }

    public GameObject[] GetLightsTown() {
        return lightsTown;
    }

    public GameObject[] GetLightsVillage() {
        return lightsVillage;
    }

    public GameObject[] GetLightsLabyrinth() {
        return lightsLabyrinth;
    }


}
