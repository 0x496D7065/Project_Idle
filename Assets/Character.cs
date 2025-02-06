using UnityEngine;

[System.Serializable]
public class Character
{
    private string name;
    private float baseIncome;
    private float incomePerSecond;
    private float upgradeCost;
    private int level;
    public  int unlocked;

    public Character(string name, float baseIncome, float upgradeCost)
    {
        this.name = name;
        this.baseIncome = baseIncome;
        this.incomePerSecond = 0;
        this.upgradeCost = upgradeCost;
        this.level = 0;
        this.unlocked = 0;
    }

    public Character(CharacterData data)
    {
        name = data.name;
        baseIncome = data.baseIncome;
        incomePerSecond = data.incomePerSecond;
        upgradeCost = data.upgradeCost;
        level = data.level;
        unlocked = data.unlocked;
    }

    public void Upgrade()
    {
        level++;
        incomePerSecond = baseIncome * level;
        upgradeCost *= 1.5f;  // Increase cost progressively
    }

    public string getName()
    {
        return name;
    }

    public float getBaseIncome()
    { 
        return baseIncome;
    }

    public float getUpgradeCost()
    {
        return upgradeCost;
    }

    public float getIncomePerSecond()
    {
        return incomePerSecond;
    }

    public int getLevel()
    {
        return level;
    }

    public int getUnlocked()
    {
        return unlocked;
    }
}