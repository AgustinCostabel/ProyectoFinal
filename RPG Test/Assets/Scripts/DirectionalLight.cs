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
    float rotationX = 45f;
    float rotationY = 45f;
    public bool direction = true;

    private float lerpTime;
    [SerializeField] private Color[] skyColors;
    private int nextColor;
    private float t = 0f;

    private void Awake() {
        Instance = this;
        light = GetComponent<Light>();
    }

    private void Start() {

        lerpTime = (1/GameManager.Instance.GetGameTimerMax())*10;

        GameManager.Instance.OnSunrise += GameManager_OnSunrise;
        GameManager.Instance.OnNoon += GameManager_OnNoon;
        GameManager.Instance.OnSunset += GameManager_OnSunset;
        GameManager.Instance.OnNight += GameManager_OnNight;
        GameManager.Instance.OnDarkNight += GameManager_OnDarkNight;
        GameManager.Instance.OnTimeLapsed += GameManager_OnTimeLapsed;

        RenderSettings.skybox.SetColor("_Tint", new Color32(10, 10, 10, 1));

        light.color = skyColors[4];
        nextColor = 4;
    }

    void Update() {
        transform.localEulerAngles = new Vector3(70, RotationY(), 0);
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeedSkybox);

        light.color = Color.Lerp(light.color, skyColors[nextColor], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t,1f,lerpTime * Time.deltaTime);
        if(t > .9f) {
            t = 0f;
            if (nextColor == 4) {
                nextColor = 0;
            } else {
                nextColor++;
            }
        }

    }

    private void GameManager_OnTimeLapsed(object sender, EventArgs e) {
        RenderSettings.skybox.SetColor("_Tint", new Color32(120, 120, 120, 1));
        light.color = new Color32(255, 240, 200, 1);
        nextColor = 1;
        t = 0f;
        rotationX = 70f;
        rotationY = 0f;
    }

    private void GameManager_OnSunrise(object sender, System.EventArgs e) {
        RenderSettings.skybox.SetColor("_Tint", new Color32(120, 120, 120, 1));
    }
    private void GameManager_OnNoon(object sender, System.EventArgs e) {
    }
    private void GameManager_OnSunset(object sender, System.EventArgs e) {
        RenderSettings.skybox.SetColor("_Tint", new Color32(75, 75, 75, 1));
    }

    private void GameManager_OnNight(object sender, System.EventArgs e) {
    }

    private void GameManager_OnDarkNight(object sender, System.EventArgs e) {
        RenderSettings.skybox.SetColor("_Tint", new Color32(0, 0, 0, 1));
    }


    float RotationX() {
        rotationX += rotateSpeed * Time.deltaTime;
        if (rotationX >= 70f)
            rotationX -= 25f;
        return direction ? rotationX : -rotationX;
    }

    float RotationY() {
        rotationY += rotateSpeed * Time.deltaTime;
        if (rotationY >= 55f)
            rotationY -= 100f;
        return direction ? rotationY : -rotationY;
    }

    public Light GetLight() {
        return light;
    }
}

