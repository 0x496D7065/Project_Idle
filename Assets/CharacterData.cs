[System.Serializable]

public class CharacterData
{
    public string   name;
    public float    baseIncome;
    public float    incomePerSecond;
    public float    upgradeCost;
    public int      level;
    public int      unlocked;

    public CharacterData(Character character)
    {
        name = character.getName();
        baseIncome = character.getBaseIncome();
        incomePerSecond = character.getIncomePerSecond();
        upgradeCost = character.getUpgradeCost();
        level = character.getLevel();
        unlocked = character.getUnlocked();
    }
}
