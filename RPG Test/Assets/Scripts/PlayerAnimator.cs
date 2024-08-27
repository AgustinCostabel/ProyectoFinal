using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING_FORWARD = "isWalkingForward";
    private const string IS_RUNNING = "isRunning";
    private const string ATTACK = "Attack";
    private const string ROLL = "Roll";
    private const string GATHER = "Gather";
    private const string DEATH = "Death";
    private const string HAS_WEAPON = "HasWeapon";
    private const string HAS_WEAPON_TWO = "HasWeaponTwo";
    private const string BUFF = "Buff";
    private const string MELEE_SPELL = "MeleeSpell";
    private const string CAST_SPELL = "CastSpell";
    private const string REVIVE = "Revive";
    private float rollTimer;
    private float rollTimerMax = 1f;

    [SerializeField] private Player player;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        player.OnRollPlayer += Player_OnRollPlayer;
        player.OnInteractPlayer += Player_OnInteractPlayer;
        player.OnAttackPlayer += Player_OnAttackPlayer;
        player.OnDeathPlayer += Player_OnDeathPlayer;
        player.OnCastSpellBuffPlayer += Player_OnCastSpellBuffPlayer;
        player.OnCastSpellMeleePlayer += Player_OnCastSpellMeleePlayer;
        player.OnCastSpellPlayer += Player_OnCastSpellPlayer;
        player.OnRevivePlayer += Player_OnRevivePlayer;
    }

    private void Player_OnRevivePlayer(object sender, System.EventArgs e) {
        animator.SetTrigger(REVIVE);
    }

    private void Player_OnCastSpellPlayer(object sender, System.EventArgs e) {
        animator.SetTrigger(CAST_SPELL);
    }

    private void Player_OnCastSpellMeleePlayer(object sender, System.EventArgs e) {
        animator.SetTrigger(MELEE_SPELL);
    }

    private void Player_OnCastSpellBuffPlayer(object sender, System.EventArgs e) {
        animator.SetTrigger(BUFF);
    }

    private void Player_OnDeathPlayer(object sender, System.EventArgs e) {
        animator.SetTrigger(DEATH);
    }

    private void Player_OnAttackPlayer(object sender, System.EventArgs e) {
        animator.SetTrigger(ATTACK);
    }

    private void Player_OnInteractPlayer(object sender, System.EventArgs e) {
        animator.SetTrigger(GATHER);
    }

    private void Player_OnRollPlayer(object sender, System.EventArgs e) {
        if (rollTimer < 0 && player.IsWalking()) {
            animator.SetTrigger(ROLL);
            rollTimer = rollTimerMax;
        }
    }

    private void Update() {
        animator.SetBool(IS_WALKING_FORWARD, player.IsWalking());
        animator.SetBool(IS_RUNNING, player.IsRunning());
        animator.SetBool(HAS_WEAPON, player.HasWeaponEquiped());
        animator.SetBool(HAS_WEAPON_TWO, player.HasWeaponTwoHands());

        rollTimer -= Time.deltaTime;
    }

    public void EndActionPlayer() {
        player.EndAction();
    }

    public void EndRollingPlayer() {
        player.EndRolling();
    }

    public void EndCastingPlayer() {
        player.EndCasting();
    }
}
