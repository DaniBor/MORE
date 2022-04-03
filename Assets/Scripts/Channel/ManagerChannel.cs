using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManagerChannel")]
public class ManagerChannel : ScriptableObject
{
    public delegate void TierUpgradeCallback(int newTier);
    public TierUpgradeCallback onTierUpgrade;

    public delegate void ManagerCallback();
    public ManagerCallback onGameEnd;

    public void NotifyTierUpgrade(int newTier)
    {
        onTierUpgrade?.Invoke(newTier);
    }


    public void EndGame()
    {
        onGameEnd?.Invoke();
    }
}
