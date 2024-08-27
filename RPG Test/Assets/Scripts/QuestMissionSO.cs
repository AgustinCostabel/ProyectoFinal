using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestMissionSO : ScriptableObject
{
    public int amount;
    public GameObject enemy;
    public int goldReward;
    public int experienceReward;

    public bool Finished() {
        return amount <= 0;
    }
}
