using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SpellsSO : ScriptableObject {
    public enum Type {
        Cast,
        Buff,
        Heal,
        Melee
    }

    public Type type;
    public Sprite spellSprite;
    public string objectName;
    public int damage;
    public int heal;
    public int buffDamage;
    public int buffTime;
}
