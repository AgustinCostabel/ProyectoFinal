using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DirectionalLight : MonoBehaviour
{

    public static DirectionalLight Instance { get; private set; }
   
    private new Light light;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float rotationSpeedSkybox;
    //[SerializeField] private float rotationX = 45f;
    //[SerializeField] private float rotationY = 45f;
    public bool direction = true;

    [SerializeField] private float lerpTime;
    [SerializeField] private Color[] skyColors;
    private Color sunsetColor;
    private Color nightColor;
    //private int nextColor = 2;
    private float t = 0;

    private bool isSunset = false;
    private bool isNight = false;

    private void Awake() {
        Instance = this;
        light = GetComponent<Light>();
    }

    private void Start() {

        //lerpTime = (1/GameManager.Instance.GetGameTimerMax())*10;

        GameManager.Instance.OnSunrise += GameManager_OnSunrise;
        GameManager.Instance.OnSunset += GameManager_OnSunset;
        GameManager.Instance.OnNight += GameManager_OnNight;
        GameManager.Instance.OnTimeLapsed += GameManager_OnTimeLapsed;

        RenderSettings.skybox.SetColor("_Tint", new Color32(10, 10, 10, 1));

        light.color = skyColors[4];

        sunsetColor = skyColors[2];
        nightColor = skyColors[4];
    }

    void Update() {
        if (isSunset) {
            light.color = Color.Lerp(light.color, sunsetColor, lerpTime * Time.deltaTime);
            t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

            if (t > .9f) {
                t = 0f;

                // Check if the colors are close enough
                if (Vector4.Distance(light.color, sunsetColor) < 0.01f) {
                    light.color = sunsetColor; // Ensure exact match
                    isSunset = false;
                }
            }
        }

        if (isNight) {
            light.color = Color.Lerp(light.color, nightColor, lerpTime * Time.deltaTime);
            t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

            if (t > .9f) {
                t = 0f;

                // Check if the colors are close enough
                if (Vector4.Distance(light.color, nightColor) < 0.01f) {
                    light.color = nightColor; // Ensure exact match
                    isNight = false;
                }
            }
        }
    }

    private void GameManager_OnTimeLapsed(object sender, EventArgs e) {
        RenderSettings.skybox.SetColor("_Tint", new Color32(120, 120, 120, 1));
        light.color = new Color32(255, 240, 200, 1);
    }

    private void GameManager_OnSunrise(object sender, System.EventArgs e) {
        RenderSettings.skybox.SetColor("_Tint", new Color32(175, 175, 175, 1));
        light.color = skyColors[0];
        light.intensity = 4f;
    }

    private void GameManager_OnSunset(object sender, System.EventArgs e) {
        RenderSettings.skybox.SetColor("_Tint", new Color32(125, 125, 125, 1));
        //light.color = skyColors[2];
        light.intensity = 3f;

        isSunset = true;
    }

    private void GameManager_OnNight(object sender, System.EventArgs e) {
        RenderSettings.skybox.SetColor("_Tint", new Color32(0, 0, 0, 1));
        //light.color = skyColors[4];
        light.intensity = 1f;

        isNight = true;
    }


    /*float RotationX() {
        rotationX += rotateSpeed * Time.deltaTime;
        if (rotationX >= 70f)
            rotationX -= 25f;
        return direction ? rotationX : -rotationX;
    }*/

    /*float RotationY() {
        rotationY += rotateSpeed * Time.deltaTime;
        if (rotationY >= 55f)
            rotationY -= 100f;
        return direction ? rotationY : -rotationY;
    }*/

    public Light GetLight() {
        return light;
    }
}

