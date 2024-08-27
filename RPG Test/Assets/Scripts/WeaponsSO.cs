using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WeaponsSO : ScriptableObject {
    public Transform prefab;
    public string objectName;
    public int damage;
    public float attackSpeed;
    public bool isTwoHanded;
}