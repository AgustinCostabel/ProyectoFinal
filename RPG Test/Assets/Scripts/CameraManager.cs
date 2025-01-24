using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

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
}
