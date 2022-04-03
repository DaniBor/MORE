using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatSet")]
public class PlayerStatSets : ScriptableObject
{
    public float speed;
    public bool ability1Unlocked;
    public bool ability2Unlocked;
    public bool ability3Unlocked;
    public bool ability4Unlocked;
    public bool ability5Unlocked;
}
