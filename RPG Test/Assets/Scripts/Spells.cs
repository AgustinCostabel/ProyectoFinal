using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spells : MonoBehaviour
{

    public static Spells Instance { get; private set; }
    [SerializeField] private SpellsSO[] spellsSO;
    private int spellsCount = 0;
    private int spellActive = 5;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE SPELL CLASS");
        }
        Instance = this;
        spellsSO = new SpellsSO[10];
    }

    public void AddSpell(SpellsSO spellSO) {
        spellsSO[spellsCount] = spellSO;
        SpellsUI.Instance.ActiveSpellSprite(spellsCount, spellSO.spellSprite);
        spellsCount++;
    }

    public void CastSpell(Player player) {
        if (spellsSO[spellActive] != null) {
            if(spellsSO[spellActive].type == SpellsSO.Type.Heal) {
                player.Heal(spellsSO[spellActive].heal);
            }
            if (spellsSO[spellActive].type == SpellsSO.Type.Buff) {
                player.Buff(spellsSO[spellActive].buffDamage, spellsSO[spellActive].buffTime);
            }
            if(spellsSO[spellActive].type == SpellsSO.Type.Melee) {
                player.MeleeSpell(spellsSO[spellActive].damage);
            }
            if(spellsSO[spellActive].type == SpellsSO.Type.Cast) {
                player.CastSpell(spellsSO[spellActive].damage);
            }
        }
    }

    public void SelectSpell(int index) {
        if (spellsSO[index] != null) {
            spellActive = index;
            SpellsUI.Instance.SelectSpellSprite(index);
        }
    }

    public SpellsSO GetSpellActive() {
        return spellsSO[spellActive];
    }

}
