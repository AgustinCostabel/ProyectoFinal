using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestSO : ScriptableObject
{
    public string title;
    public string description;
    public QuestMissionSO questMissionSO;
}
