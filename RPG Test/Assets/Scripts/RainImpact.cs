using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainImpact : MonoBehaviour
{
    [SerializeField] private TerrainLayer stoneFloor;
    [SerializeField] private Material playerArmor;

    private void Start() {
        stoneFloor.smoothness = 0f;
        playerArmor.SetFloat("_Smoothness", 0f);
        playerArmor.color = new Color32(175, 175, 175, 1);
        WeatherManager.Instance.OnRaining += WeatherManager_OnRaining;
        WeatherManager.Instance.OnStopRaining += WeatherManager_OnStopRaining;
    }

    private void WeatherManager_OnStopRaining(object sender, System.EventArgs e) {
        playerArmor.SetFloat("_Smoothness", 0f);
        playerArmor.color = new Color32(175, 175, 175, 1);
        stoneFloor.smoothness = 0f;
    }

    private void WeatherManager_OnRaining(object sender, System.EventArgs e) {
        playerArmor.SetFloat("_Smoothness", 1f);
        playerArmor.color = new Color32(125, 125, 125, 1);
        stoneFloor.smoothness = 0.6f;
    }
}
