using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputActions inputActions;

    public event EventHandler OnRollAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnAttackAction;
    public event EventHandler OnEquipWeaponAction;
    public event EventHandler OnMenuAction;
    public event EventHandler OnCastSpellAction;
    public event EventHandler OnSelectedSpellAction;
    public event EventHandler OnJournalAction;
    public event EventHandler OnMapAction;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE GAME INPUT");
        }
        Instance = this;

        inputActions = new InputActions();
        inputActions.Player.Enable();
        inputActions.Player.Roll.performed += Roll_performed;
        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.Attack.performed += Attack_performed;
        inputActions.Player.EquipWeapon.performed += EquipWeapon_performed;
        inputActions.Player.Menu.performed += Menu_performed;
        inputActions.Player.CastSpell.performed += CastSpell_performed;
        inputActions.Player.SelectedSpell.performed += SelectedSpell_performed;
        inputActions.Player.Journal.performed += Journal_performed;
        inputActions.Player.Map.performed += Map_performed;
    }

    private void Map_performed(InputAction.CallbackContext obj) {
        OnMapAction?.Invoke(this, EventArgs.Empty);
    }

    private void Journal_performed(InputAction.CallbackContext obj) {
        OnJournalAction?.Invoke(this, EventArgs.Empty);
    }

    private void SelectedSpell_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSelectedSpellAction?.Invoke(this, EventArgs.Empty);
    }

    private void CastSpell_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnCastSpellAction?.Invoke(this, EventArgs.Empty);
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMenuAction?.Invoke(this, EventArgs.Empty);
    }

    private void EquipWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnEquipWeaponAction?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Roll_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRollAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
        inputActions.Dispose();
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public bool GetRun() {
        bool isRunning = inputActions.Player.Run.ReadValue<float>() != 0f;

        return isRunning;
    }

}
