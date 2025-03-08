using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{

    public static CameraManager Instance { get; private set; }

    public CinemachineVirtualCamera cam;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE CAMERA MANAGER");
        }
        Instance = this;
    }

    private void Start() {
        cam = GetComponent<CinemachineVirtualCamera>();
        GameInput.Instance.OnZoomAction += Instance_OnZoomAction;
    }

    private void Instance_OnZoomAction(object sender, EventArgs e) {
        cam.m_Lens.FieldOfView -= (float)sender;
        if(cam.m_Lens.FieldOfView < 8) {
            cam.m_Lens.FieldOfView = 8;
        }
        if(cam.m_Lens.FieldOfView > 25) {
            cam.m_Lens.FieldOfView = 25;
        }
    }

    public Vector3 CameraRotation() {
        return cam.transform.localRotation.eulerAngles;
    }
}
