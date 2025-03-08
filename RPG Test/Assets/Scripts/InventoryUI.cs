using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform contentPanel; // Reference to Scroll View's "Content" object

    public void AddItem(GameObject item) {
        GameObject newItem = Instantiate(item, contentPanel);
    }
}
