using System;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour, I_HasProgress {
    public static Player Instance { get; private set; }

    public event EventHandler OnRollPlayer;
    public event EventHandler OnInteractPlayer;
    public event EventHandler OnAttackPlayer;
    public event EventHandler OnDeathPlayer;
    public event EventHandler OnCastSpellBuffPlayer;
    public event EventHandler OnCastSpellMeleePlayer;
    public event EventHandler OnCastSpellPlayer;
    public event EventHandler OnRevivePlayer;
    public event EventHandler<I_HasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    CharacterController controller;

    [SerializeField] private int healthMax;
    [SerializeField] private float moveSpeedOriginal = 3f;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask InteractbleObjectlayerMask;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform rightHandRest;
    [SerializeField] private Transform leftHand;
    [SerializeField] private GameObject helmet;
    [SerializeField] private GameObject cape;
    [SerializeField] private Transform chest;
    [SerializeField] private Transform gloves;
    [SerializeField] private Transform boots;
    [SerializeField] private ParticleSystem healParticles;
    [SerializeField] private Material chestGlow;
    [SerializeField] private Fireball fireball;
    [SerializeField] private Transform fireballInitialPosition;
    //SPELLS TEST
    [SerializeField] private SpellsSO spellSO1;
    [SerializeField] private SpellsSO spellSO2;
    [SerializeField] private SpellsSO spellSO3;
    [SerializeField] private SpellsSO spellSO4;

    [SerializeField] private float groundCheckValue = 0.05f;
    [SerializeField] private LayerMask ground;

    [SerializeField] private Sprite spritePlayer;
    [SerializeField] private String titlePlayer;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private TextMeshProUGUI playerUIText;

    private int health;
    private bool isWalking = false;
    private bool isRunning = false;
    private Weapons weaponEquiped = null;
    private Weapons weaponSaved = null;
    private float playerRadius = .6f;
    private float playerHeight = 1.85f;
    private int baseDamage = 2;
    private int damage;
    private int armor = 0;
    private bool isDoingAction = false;
    private bool isRolling = false;
    private bool isDeath = false;
    private float gravity = -9.8f;
    private float moveSpeed;
    private Vector3 moveDirection = Vector3.zero;
    private bool grounded;
    private Quest activeQuest = null;
    private SpellsSO activeSpell = null;
    private float buffDuration;
    private bool buffActive = false;
    private int buffDamage = 0;
    private Lantern lantern = null;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    [SerializeField] private bool keyChestWeapon = false;
    [SerializeField] private bool keyChestNecklace = false;
    [SerializeField] private bool hasMap = false;
    [SerializeField] private bool hasJournal = false;
    [SerializeField] private bool hasLamp = false;
    [SerializeField] private bool hasFishingRope = false;
    [SerializeField] private GameObject lamp;
    [SerializeField] private GameObject compass;
    [SerializeField] private bool inTown = false;
    [SerializeField] private bool inForest = false;
    [SerializeField] private bool inLabyrinth = false;
    [SerializeField] private bool inGrass = false;

    private I_InteractableObject selectedObject = null;


    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE PLAYER");
        }
        Instance = this;

        isDoingAction = true;

    }

    private void Start() {
        controller = GetComponent<CharacterController>();
        gameInput.OnRollAction += GameInput_OnRollAction;
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
        gameInput.OnEquipWeaponAction += GameInput_OnEquipWeaponAction;
        gameInput.OnCastSpellAction += GameInput_OnCastSpellAction;
        gameInput.OnSelectedSpellAction += GameInput_OnSelectedSpellAction;

        //Initial Health
        health = healthMax;
        OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs {
            progressNormalized = (float)health / healthMax
        });

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        //SPELLS TEST
        /*Spells.Instance.AddSpell(spellSO1);
        Spells.Instance.AddSpell(spellSO2);
        Spells.Instance.AddSpell(spellSO3);
        Spells.Instance.AddSpell(spellSO4);*/

        if (hasLamp) {
            if (lamp != null) {
                lamp.gameObject.SetActive(true);
            } else {
                lamp.gameObject.SetActive(false);
            }
        }

    }

    private void Update() {
        GroundCheck();
        if (!isDeath) {
            CheckDeath();
            ApplyGravity();
            HandleMovement();
            HandleInteractions();
        }
        if (buffActive) {
            buffDuration -= Time.deltaTime;
            if(buffDuration < 0) {
                buffDamage = 0;
                chestGlow.SetFloat("_Float", 1);
                buffActive = false;
            }
        }
    }

    private void GameInput_OnSelectedSpellAction(object sender, EventArgs e) {
        if (!isDeath && !GameManager.Instance.IsGamePaused() && !isDoingAction) {
            if (Input.GetKey("1")) {
                Spells.Instance.SelectSpell(0);
            }
            if (Input.GetKey("2")) {
                Spells.Instance.SelectSpell(1);
            }
            if (Input.GetKey("3")) {
                Spells.Instance.SelectSpell(2);
            }
            if (Input.GetKey("4")) {
                Spells.Instance.SelectSpell(3);
            }

            activeSpell = Spells.Instance.GetSpellActive();
        }
    }

    private void GameInput_OnCastSpellAction(object sender, EventArgs e) {
        if (!isDeath && !GameManager.Instance.IsGamePaused() && !isDoingAction) {
            if (activeSpell != null && !isDoingAction && !isRolling) {
                Spells.Instance.CastSpell(this);
            }
        }
    }
    private void GameInput_OnEquipWeaponAction(object sender, EventArgs e) {
        if (!isDeath && !GameManager.Instance.IsGamePaused() && !isDoingAction) {
            EquipWeapon();
        }
    }

    private void GameInput_OnAttackAction(object sender, EventArgs e) {
        if (!isDeath && !GameManager.Instance.IsGamePaused() && !isDoingAction) {
            Attack();
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (!isDeath && !GameManager.Instance.IsGamePaused() && !isDoingAction) {
            if(selectedObject != null && selectedObject.IsInteractable()) {
                selectedObject.Interact(this);
            }
        }
    }

    private void GameInput_OnRollAction(object sender, EventArgs e) {
        if (!isDeath && !GameManager.Instance.IsGamePaused()) {
            isRolling = true;
            OnRollPlayer?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("WeaponCrusader")) {
            TakeDamage(15);
        }
        if (other.CompareTag("WeaponSkeleton")) {
            TakeDamage(5);
        }
        if (other.CompareTag("WeaponMinotaur")) {
            TakeDamage(10);
        }
        if (other.CompareTag("WeaponBear")) {
            TakeDamage(10);
        }

        if (other.CompareTag("Village-Forest")) {
            if (inTown) {
                //Music
                WeatherManager.Instance.PlayJungle();
                MusicManager.Instance.StopSong();
                //Lights
                if (GameManager.Instance.IsNight()) {
                    foreach (GameObject lightsTown in LightsSwitch.Instance.GetLightsTown()) {
                        if (lightsTown != null) {
                            lightsTown.GetComponent<Light>().enabled = false;
                        }
                    }
                    foreach (GameObject lightsVillage in LightsSwitch.Instance.GetLightsVillage()) {
                        if (lightsVillage != null) {
                            lightsVillage.GetComponent<Light>().enabled = true;
                        }
                    }
                }
            } else {
                //Music
                WeatherManager.Instance.StopJungle();
                MusicManager.Instance.DayNightSong();
                //Lights
                if (GameManager.Instance.IsNight()) {
                    foreach (GameObject lightsTown in LightsSwitch.Instance.GetLightsTown()) {
                        if (lightsTown != null) {
                            lightsTown.GetComponent<Light>().enabled = true;
                        }
                    }
                    foreach (GameObject lightsVillage in LightsSwitch.Instance.GetLightsVillage()) {
                        if (lightsVillage != null) {
                            lightsVillage.GetComponent<Light>().enabled = false;
                        }
                    }
                }
            }
            //Floor
            inGrass = !inGrass;
            inTown = !inTown;
            inForest = !inForest;
        }
        if (other.CompareTag("Forest-Labyrinth")) {
            if (inForest) {
                WeatherManager.Instance.StopJungle();
                MusicManager.Instance.LabyrinthSong();
                //Lights
                if (GameManager.Instance.IsNight()) {
                    foreach (GameObject lightsVillage in LightsSwitch.Instance.GetLightsVillage()) {
                        if (lightsVillage != null) {
                            lightsVillage.GetComponent<Light>().enabled = false;
                        }
                    }
                }
            } else {
                WeatherManager.Instance.PlayJungle();
                MusicManager.Instance.DayNightSong();
                //Lights
                if (GameManager.Instance.IsNight()) {
                    foreach (GameObject lightsVillage in LightsSwitch.Instance.GetLightsVillage()) {
                        if (lightsVillage != null) {
                            lightsVillage.GetComponent<Light>().enabled = true;
                        }
                    }
                }
            }
            inGrass = !inGrass;
            inLabyrinth = !inLabyrinth;
            inForest = !inForest;
        }
        if (other.CompareTag("Forest-Lake")) {
            if (inForest) {
                WeatherManager.Instance.StopJungle();
                MusicManager.Instance.LakeSong();
            } else {
                WeatherManager.Instance.PlayJungle();
                MusicManager.Instance.DayNightSong();
            }
            inForest = !inForest;
        }
        if (other.CompareTag("BearLimit")) {
            Talk("Careful there is a bear near, I need a weapon");
            GameStateManager.Instance.BearEncounter();
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("LabyrinthLimit")) {
            if (!HasLantern()) {
                Talk("A huge structure. Is dark inside, I will need a LIGHT");
            }
            GameStateManager.Instance.LabyrinthEncounter();
            other.gameObject.SetActive(false);
        }
    }

    private void HandleMovement() {
        moveSpeed = 0;
        isWalking = false;
        isRunning = false;

        if (!isDoingAction) {

            Vector2 inputVector = gameInput.GetMovementVectorNormalized();

            if (inputVector != Vector2.zero) {
                moveSpeed = moveSpeedOriginal;
            }

            if (gameInput.GetRun() && moveSpeed != 0) {
                moveSpeed = moveSpeedOriginal * runSpeed;
            }
            //Move
            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
            moveDirection = Camera.main.transform.right * inputVector.x + forward * inputVector.y;
            moveDirection *= moveSpeed;
            controller.Move(moveDirection * Time.deltaTime);
            //Rotation
            if (moveDirection != new Vector3(0, 0, 0)) {
                float rotateSpeed = 10f;
                transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
            }
            isWalking = inputVector != Vector2.zero;
            isRunning = gameInput.GetRun() && moveSpeed != 0;
        }

        if (isWalking && !isRolling) {
            if (!inGrass) {
                SoundManager.Instance.WalkSoundRock();
            } else {
                SoundManager.Instance.WalkSoundGrass();
            }
        }
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        Vector3 startPosition = transform.position + Vector3.up * (playerHeight / 10f);  // Mid height of player
        Vector3 endPosition = startPosition + Vector3.up * playerHeight;  // End position for the capsule's height

        float interactDistance = 0.75f;


        //CapsuleCast
        if (Physics.CapsuleCast(startPosition, endPosition, playerRadius, transform.forward, out RaycastHit raycastHit, interactDistance, InteractbleObjectlayerMask, QueryTriggerInteraction.Ignore)) {
            //Debug raycast
            // Draw the capsule's vertical line (from start to end)
            Debug.DrawLine(startPosition, endPosition, UnityEngine.Color.yellow);  // The capsule's path (vertical line)

            // Draw the capsule cast direction in green (if it hits)
            Debug.DrawRay(startPosition, transform.forward * interactDistance, UnityEngine.Color.green);  // Direction of the cast

            // Draw a line from the hit point and show the normal at that point (red)
            Debug.DrawRay(raycastHit.point, raycastHit.normal * 2f, UnityEngine.Color.red);  // Normal at the hit point

            if (raycastHit.transform.TryGetComponent(out I_InteractableObject interactableObject)) {
                SetSelectedObject(interactableObject);
            } else {
                SetSelectedObject<I_InteractableObject>(null);
            }
        } else {
            SetSelectedObject<I_InteractableObject>(null);
            //Debug raycast
            // If no hit, just visualize the capsule's vertical line and direction (no hit, so in red)
            Debug.DrawLine(startPosition, endPosition, UnityEngine.Color.yellow); // Capsule path
            Debug.DrawRay(startPosition, transform.forward * interactDistance, UnityEngine.Color.red); // Ray direction
        }

        //Normal RayCast
        if (Physics.Raycast(startPosition, transform.forward, out RaycastHit raycastHit2, interactDistance)) {
            Debug.DrawRay(transform.position, transform.forward * interactDistance, UnityEngine.Color.green);

            if (raycastHit2.transform.TryGetComponent(out I_InteractableObject interactableObject)) {
                SetSelectedObject(interactableObject);
            } else {
                SetSelectedObject<I_InteractableObject>(null);
            }
        }
    }

    private void SetSelectedObject<T>(T interactableObject) where T : I_InteractableObject {
        if (selectedObject != null && selectedObject.IsInteractable()) {
            selectedObject.DisableCanvas();
        }
        selectedObject = interactableObject;
        if (selectedObject != null && selectedObject.IsInteractable()) {
            interactableObject.EnableCanvas();
        }
    }

    public void IncreaseHealth() {
        healthMax = 100;
        health = healthMax;
    }

    public void Heal(int heal) {
        health += heal;
        healParticles.Play();
        OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs {
            progressNormalized = (float)health / healthMax
        });
    }

    public void Buff(int buffDamage, int buffTime) {
        if (!buffActive) {
            OnCastSpellBuffPlayer?.Invoke(this, EventArgs.Empty);
            isDoingAction = true;
            chestGlow.SetFloat("_Float", 0);
            this.buffDamage = buffDamage;
            buffActive = true;
            buffDuration = buffTime;
        }
    }

    public void MeleeSpell(int damageSpell) {
        bool canAttack = !isDoingAction && !isRolling && weaponEquiped != null && weaponEquiped.GetWeaponSO().isTwoHanded;
        if (canAttack) {
            weaponEquiped.GetComponent<MeshCollider>().enabled = true;
            damage = damageSpell;
            OnCastSpellMeleePlayer?.Invoke(this, EventArgs.Empty);
            isDoingAction = true;
        }
    }

    public void CastSpell(int damage) {
        isDoingAction = true;
        OnCastSpellPlayer?.Invoke(this, EventArgs.Empty);
    }

    public void EndCasting() {
        var fireballShoot = Instantiate(fireball, fireballInitialPosition.position, transform.rotation);
        fireballShoot.GetComponent<Rigidbody>().velocity = fireballInitialPosition.forward * fireball.GetSpeed() * Time.deltaTime;
    }

    public void ActiveQuest(Quest quest) {
        if (activeQuest == null) {
            activeQuest = quest;
        }
    }

    private void TakeDamage(int damageTaken) {
        health -= (damageTaken - armor);
        OnProgressChanged?.Invoke(this, new I_HasProgress.OnProgressChangedEventArgs {
            progressNormalized = (float)health/healthMax
        });
    }

    private void Death() {
        OnDeathPlayer?.Invoke(this, EventArgs.Empty);
        isDeath = true;
        Invoke("PlayerRestart", 6);
    }

    public void PlayerRestart() { 
        gameObject.SetActive(false);
        transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        transform.rotation = initialRotation;
        gameObject.SetActive(true);
        inTown = true;
        inForest = false;
        inGrass = false;
        inLabyrinth = false;
        OnRevivePlayer?.Invoke(this, EventArgs.Empty);
        health = healthMax;
        isDeath = false;
    }

    private void CheckDeath() {
        if (health <= 0) {
            Death();
        }
    }

    private void GroundCheck() {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckValue, ground);
        if(grounded) {
            gravity = -1f;
        } else {
            gravity = -9.8f;
        }
    }

    private void ApplyGravity() {
        controller.Move(new Vector3(0, gravity, 0) * Time.deltaTime);
    }

    public void PlayGatherAnimation() {
        OnInteractPlayer?.Invoke(this, EventArgs.Empty);
        isDoingAction = true;
    }

    public void ObtainWeapon(Weapons weapon) {
        if (weaponEquiped == null) {
            weaponEquiped = weapon;
            weaponEquiped.GetComponent<MeshCollider>().enabled = false;
            weaponEquiped.DisableInteractable();
            //MenuUI.Instance.ActiveWeaponSlot(true);
        }
    }

    public void DestroyWeapon() {
        if (weaponEquiped != null) {
            Destroy(weaponEquiped.gameObject);
            weaponEquiped = null;
        }
    }
    private void EquipWeapon() {
        if (weaponSaved != null) {
            if (weaponEquiped != null) {
                weaponEquiped.transform.SetParent(rightHandRest.transform, false);
                weaponSaved.transform.SetParent(rightHand.transform, false);
                Weapons weaponAux = weaponEquiped;
                weaponEquiped = weaponSaved;
                weaponSaved = weaponAux;
            } else {
                weaponEquiped = weaponSaved;
                weaponEquiped.transform.SetParent(rightHand.transform, false);
                weaponSaved = null;
            }
        } else {
            if (weaponEquiped != null) {
                weaponSaved = weaponEquiped;
                weaponSaved.transform.SetParent(rightHandRest.transform, false);
                weaponEquiped = null;
            }
        }

        if(weaponSaved != null) {
            weaponSaved.GetComponent<MeshCollider>().enabled = false;
        }
    }

    public void EquipArmor() {
        helmet.SetActive(true);
        //MenuUI.Instance.ActiveHelmetSlot(true);
        armor += 2;
    }

    private void Attack() {
        bool canAttack = !isDoingAction && !isRolling;
        if (canAttack) {
            if (HasWeaponEquiped()) {
                Invoke("EnableMeshWeapon", 0.5f);
                damage = weaponEquiped.GetWeaponSO().damage;
            } else {
                damage = baseDamage;
            }
            damage += buffDamage;
            OnAttackPlayer?.Invoke(this, EventArgs.Empty);
            isDoingAction = true;
        }
    }

    private void EnableMeshWeapon() {
        weaponEquiped.GetComponent<MeshCollider>().enabled = true;
    }

    public void KillCount(Enemy enemy) {
        if(activeQuest != null) {
            if(activeQuest.GetQuestSO().questMissionSO.enemy.CompareTag(enemy.tag)) {
                activeQuest.GetQuestSO().questMissionSO.amount -= 1;
                if (activeQuest.GetQuestSO().questMissionSO.Finished()) {
                    activeQuest = null;
                }
            }
        }
    }
    public void EndAction() {
        isDoingAction = false;
        if(weaponEquiped != null) {
            weaponEquiped.GetComponent<MeshCollider>().enabled = false;
        }
    }

    public void EndRolling() {
        isRolling = false;
    }

    public void SetIsDoingAction(bool isDoing) {
        isDoingAction = isDoing;
    }

    public bool GetIsDoingAction() {
        return isDoingAction;
    }

    public bool GetIsRolling() {
        return isRolling;
    }

    public int GetDamage() {
        return damage;
    }

    public bool GetIsDeath() {
        return isDeath;
    }

    public bool HasActiveQuest() {
        return activeQuest;
    }

    public Transform GetRightHand() {
        return rightHand;
    }

    public Transform GetLeftHand() { 
        return leftHand;
    }

    public bool IsWalking() {
        return isWalking;
    }

    public bool IsRunning() {
        return isRunning;
    }


    public bool HasWeaponEquiped() {
        return weaponEquiped != null;
    }

    public bool HasWeaponTwoHands() {
        if (weaponEquiped != null) {
            return weaponEquiped.GetWeaponSO().isTwoHanded;
        }
        return false;
    }

    public Weapons GetWeapon() {
        return this.weaponEquiped;
    }

    public void EquipLantern(Lantern lantern) {
        this.lantern = lantern;
    }

    public bool HasLantern() {
        return this.lantern != null;
    }

    public void ObtainKeyChestWeapon() {
        keyChestWeapon = true;
    }

    public bool GetKeyChestWeapon() {
        return keyChestWeapon;
    }

    public void ObtainKeyChestNecklace() {
        keyChestNecklace = true;
    }

    public bool GetKeyChestNecklace() {
        return keyChestNecklace;
    }

    public Sprite GetSpritePlayer() {
        return spritePlayer;
    }

    public string GetTitlePlayer() {
        return titlePlayer;
    }

    public bool HasMap() {
        return hasMap;
    }

    public void ObtainMap() {
        hasMap = true;
    }

    public void ObtainJournal() {
        hasJournal = true;
    }

    public bool HasJournal() {
        return hasJournal;
    }

    public void ObtainFishingRope() {
        hasFishingRope = true;
    }

    public bool HasFishingRope() {
        return hasFishingRope;
    }

    public void ObtainCompass() {
        compass.gameObject.SetActive(true);
    }

    public void Talk(string text) {
        playerUIText.text = text;
        playerUI.gameObject.SetActive(true);
        Invoke("StopTalk", 3);
    }

    public void StopTalk() {
        playerUI.gameObject.SetActive(false);
    }
}
