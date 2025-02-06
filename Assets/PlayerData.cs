using UnityEngine;
using System.Collections.Generic;
[System.Serializable]

public class PlayerData
{
    public float gold;
    public int  diamond;
    public float goldPerSecond;
    public float upgradeCost;
    public float playerGoldRate;
    public float playerUpgradeCost;
    public float buffDuration;
    public float buffMultiplier;
    public int   prestigeLevel;
    public float prestigeBuff;
    public long  lastLoginTime;
    public List<CharacterData> charSave;
}
