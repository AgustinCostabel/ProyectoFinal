using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellsUI : MonoBehaviour
{
    public static SpellsUI Instance { get; private set; }

    [SerializeField] private Image spell1;
    [SerializeField] private Image spell2;
    [SerializeField] private Image spell3;
    [SerializeField] private Image spell4;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE SPELL UI");
        }
        Instance = this;
    }

    public void ActiveSpellSprite(int index, Sprite newSprite) {
        if (index == 0) {
            spell1.sprite = newSprite;
            spell1.gameObject.SetActive(true);
        }
        if (index == 1) {
            spell2.sprite = newSprite;
            spell2.gameObject.SetActive(true);
        }
        if (index == 2) {
            spell3.sprite = newSprite;
            spell3.gameObject.SetActive(true);
        }
        if (index == 3) {
            spell4.sprite = newSprite;
            spell4.gameObject.SetActive(true);
        }
    }

    public void SelectSpellSprite(int index) {
        if (index == 0) {
            spell1.color = new Color(255f, 255f, 255f, 1f);
        } else {
            spell1.color = new Color(255f, 255f, 255f, 0.5f);
        }
        if (index == 1) {
            spell2.color = new Color(255f, 255f, 255f, 1f);
        } else {
            spell2.color = new Color(255f, 255f, 255f, 0.5f);
        }
        if (index == 2) {
            spell3.color = new Color(255f, 255f, 255f, 1f);
        } else {
            spell3.color = new Color(255f, 255f, 255f, 0.5f);
        }
        if (index == 3) {
            spell4.color = new Color(255f, 255f, 255f, 1f);
        } else {
            spell4.color = new Color(255f, 255f, 255f, 0.5f);
        }
    }

    
}
