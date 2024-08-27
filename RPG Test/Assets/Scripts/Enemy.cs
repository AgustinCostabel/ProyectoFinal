using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, I_HasProgress {
    private const string WALK = "Walk";
    private const string RUN = "Run";
    private const string ATTACK = "Attack";
    private const string DEATH = "Death";

    public event EventHandler<I_HasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private float speed;
    [SerializeField] private Player player;
    [SerializeField] private int healthMax;
    [SerializeField] GameObject weapon;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private LayerMask MainCharacterLayerMask;
    [SerializeField] private Transform originalPosition;

    private int action;
    private bool isAttacking = false;
    private Animator animator;
    private Quaternion angle;
    private float degrees;
    private Vector3 lookPosReal;
    private bool isDeath = false;
    private int health;
    private bool canTakeDamage = true;
    private float canTakeDamageTime = 0f;
    private bool fighting;
    //[SerializeField] private LayerMask MainCharacter;

    private void Start() {
        animator = GetComponent<Animator>();
        health = healthMax;

        GameManager.Instance.OnTimeLapsed += GameManager_OnTimeLapsed;
    }

    private void GameManager_OnTimeLapsed(object sender, EventArgs e) {
        Reset();
    }

    private void Update() {
        if (!isDeath) {
            EnemyBehaviour();
        }

        canTakeDamageTime -= Time.deltaTime;
        if (canTakeDamageTime < 0f) {
            canTakeDamage = true;
        }
    }

    private void EnemyBehaviour() {

        if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance || player.GetIsDeath()) {
            if (fighting == true) {
                MusicManager.Instance.StopSong();
                MusicManager.Instance.DayNightSong();
                fighting = false;
                animator.SetBool(RUN, false);
                chaseDistance /= 5;
            }
        } else {
            if (fighting == false) {
                MusicManager.Instance.StopSong();
                MusicManager.Instance.FightSong();
                fighting = true;
                chaseDistance *= 5;
            }
            if (Vector3.Distance(transform.position, player.transform.position) > attackDistance && !isAttacking) {
                Vector3 lookPos = player.transform.position - transform.position;
                lookPos.y = 0f;
                lookPosReal = lookPos;
                Quaternion rotation = Quaternion.LookRotation(lookPosReal);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
                animator.SetBool(RUN, true);
                transform.Translate(Vector3.forward * speed * 4 * Time.deltaTime);
            } else {
                if (!isAttacking) {
                    if (Physics.CapsuleCast(transform.position, transform.position + Vector3.up * 2f,
                        .1f, transform.forward, out RaycastHit raycastHit, attackDistance, MainCharacterLayerMask)) {
                        isAttacking = true;
                        animator.SetBool(RUN, false);
                        animator.SetTrigger(ATTACK);
                    } else {
                        Vector3 lookPos = player.transform.position - transform.position;
                        lookPos.y = 0f;
                        lookPosReal = lookPos;
                        Quaternion rotation = Quaternion.LookRotation(lookPosReal);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
                    }
                }
            }
        }
    }

    private void Death() {
        animator.SetTrigger(DEATH);
        player.KillCount(this);
        //Destroy(gameObject, 10f);
        isDeath = true;
        MusicManager.Instance.StopSong();
        MusicManager.Instance.DayNightSong();
    }

    public void EndAttackAnimation() {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("WeaponPlayer") && player.GetIsDoingAction() && !isDeath) {
            TakeDamage();
        }
        if (other.CompareTag("Fireball") && !isDeath) {
            TakeDamage(30);
        }
    }

    private void TakeDamage(int damage) {
        health -= damage;
        OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs {
            progressNormalized = (float)health / healthMax
        });
        if (health <= 0) {
            Death();
        }
    }

    private void TakeDamage() {
        if (canTakeDamage) {
            health -= player.GetDamage();
            OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float)health / healthMax
            });
            if (health <= 0) {
                Death();
            }
            canTakeDamage = false;
            if (player.HasWeaponEquiped()) {
                canTakeDamageTime = player.GetWeapon().GetWeaponSO().attackSpeed;
            }
        }
    }

    private void Reset() {
        gameObject.transform.position = originalPosition.position;
    }

    public GameObject GetGameObject() {
        return weapon;
    }

}
