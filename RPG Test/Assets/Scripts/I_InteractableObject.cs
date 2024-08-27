using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_InteractableObject
{
    public void Interact(Player player);

    public void EnableCanvas();

    public void DisableCanvas();

    public bool IsInteractable();
}
