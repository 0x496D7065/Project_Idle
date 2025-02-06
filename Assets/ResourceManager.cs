using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public float gold = 0f;
    public int   diamond = 0;
    public float goldPerSecond;
    public float upgradeCost;
    public float upgradeFactor = 1.2f;
    public float playerGoldRate;
    public float playerUpgradeCost;
    public float playerUpgradeFactor = 2f;
    public float buffDuration = 0;
    public float buffMultiplier = 1f;
    public float prestigeBuff = 1f;

    public List<Character> characters;
    public List<CharacterData> charSave;

    private GameManager gameManager;

    public void setGameManager(GameManager gm)
    {
       gameManager = gm;
    }

    public void InitializeWithDefaults()
    {
        gold = 0f;
        diamond = 0;
        goldPerSecond = 0f;
        upgradeCost = 50f;
        playerGoldRate = 10f;
        playerUpgradeCost = 300f;
        InitializeDefaultsChars();
    }

    public void InitializeDefaultsChars()
    {
        characters = new List<Character>
        {
            new Character("Char1", 5f, 50f),
            new Character("Char2", 100f, 300f),
            new Character("Char3", 500f, 2500f),
        };
        Debug.Log("Default char list generated");
    }

    public void InitializeFromData(PlayerData data)
    {
        gold = data.gold;
        diamond = data.diamond;
        goldPerSecond = data.goldPerSecond;
        upgradeCost = data.upgradeCost;
        playerGoldRate = data.playerGoldRate;
        playerUpgradeCost = data.playerUpgradeCost;
        buffDuration = data.buffDuration;
        buffMultiplier = data.buffMultiplier;
        prestigeBuff = data.prestigeBuff;
        characters = new List<Character>();
        foreach (CharacterData characterData in data.charSave)
        {
            Character character = new Character(characterData);
            characters.Add( character );
        }
    }

    public PlayerData GetPlayerData()
    {
        charSave = new List<CharacterData>();
        foreach(Character character in characters)
        {
            charSave.Add(new CharacterData(character));
        }
        return new PlayerData
        {
            gold = gold,
            diamond = diamond,
            goldPerSecond = goldPerSecond,
            upgradeCost = upgradeCost,
            playerGoldRate = playerGoldRate,
            playerUpgradeCost = playerUpgradeCost,
            lastLoginTime = gameManager.GetCurrentUnixTimestamp(),
            buffDuration = buffDuration,
            buffMultiplier = buffMultiplier,
            prestigeBuff = prestigeBuff,
            charSave = charSave,
        };
    }

    void Update()
    {
        if (buffDuration > 0)
        {
            buffDuration -= Time.deltaTime;
            if (buffDuration <= 0)
            {
                buffMultiplier = 1f;
                UpdateTotalIncome();
            }
        }
        gold += (goldPerSecond * buffMultiplier * prestigeBuff) * Time.deltaTime;
    }

    public void activateBuff(float multiplier, float duration)
    {
        buffMultiplier = multiplier;
        buffDuration = duration;
        UpdateTotalIncome();
    }

    //Upgrade passive income
    public void TryUpgrade()
    {
        if (gold >= upgradeCost)
        {
            gold -= upgradeCost;
            goldPerSecond *= upgradeFactor;
            upgradeCost *= upgradeFactor;
            gameManager.SaveProgress();
        }
    }

    //Upgrade active income
    public void TryUpgrade2()
    {
        if (gold >= playerUpgradeCost)
        {
            gold -= playerUpgradeCost;
            playerGoldRate *= playerUpgradeFactor;
            playerUpgradeCost *= playerUpgradeFactor;
            gameManager.SaveProgress();
        }
    }

    public bool BuyCharacter(Character character)
    {
        Debug.Log("Tried to buy");
        if (gold >= character.getUpgradeCost())
        {
            Debug.Log("Buying");
            gold -= character.getUpgradeCost();
            character.Upgrade();
            character.unlocked = 1;
            UpdateTotalIncome();
            return true;
        }
        return false;
    }

    public void UpdateTotalIncome()
    {
        goldPerSecond = 0f;
        foreach (Character character in characters)
        {
            goldPerSecond += character.getIncomePerSecond();
        }
        goldPerSecond *= prestigeBuff * buffMultiplier;
    }

    public float getGold()
    {
        return gold;
    }

    public float getGoldPerSecond()
    {
        return goldPerSecond;
    }

    public int getDiamond()
    {
        return diamond;
    }

    public float getUpgradeCost()
    {
        return upgradeCost; 
    }

    public float getPlayerUpgradeCost()
    {
        return playerUpgradeCost;
    }

    public float getBuffDuration()
    {
        return buffDuration;
    }
}