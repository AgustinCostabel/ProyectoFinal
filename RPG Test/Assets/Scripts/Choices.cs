using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Choices : MonoBehaviour
{
    public TextAsset dialogueText;
    public int index;
    public Choices(TextAsset dialogueText) {
        this.dialogueText = dialogueText;
    }
}
