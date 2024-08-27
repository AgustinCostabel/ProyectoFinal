using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameObject;

    private I_HasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<I_HasProgress>();

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        //barImage.fillAmount = 0;

        //Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, I_HasProgress.OnProgressChangedEventArgs e) {

        barImage.fillAmount = e.progressNormalized;

        /*if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        } else {
            Show();
        }*/
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
