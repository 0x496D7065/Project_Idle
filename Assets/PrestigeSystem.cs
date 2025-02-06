using UnityEngine;

public class PrestigeSystem : MonoBehaviour
{
    public ResourceManager resourceManager;
    public UIManager uiManager;

    private int level;
    private float prestigeThreshold = 100f;
    private int buff = 0;

    public void showPrestigeMenu()
    {
        uiManager.ShowPrestigeMenu();
    }

    public void applyPrestige(int buffChoice)
    {
        if (resourceManager.gold >= prestigeThreshold)
        {
            switch (buffChoice)
            {
                case 1:
                    buff = 1;
                    break;
                case 2:
                    buff = 2;
                    break;
                case 3:
                    buff = 3;
                    break;
            }
        }
        uiManager.ShowConfirmButton();
    }

    public void ResetGame()
    {
        if (resourceManager.gold < prestigeThreshold)
        {
            Debug.Log("Not enough gold to prestige!");
            uiManager.ShowMainMenu();
            return;
        }
        level++;
        //if (buff != 0)
        //resourceManager.prestigeBuff += buff;
        switch (buff)
        {
            case 1:
                resourceManager.prestigeBuff += 0.5f;
                resourceManager.playerGoldRate = 10f;
                break;
            case 2:
                resourceManager.playerGoldRate += 20f;
                break;
            case 3:
                resourceManager.prestigeBuff += 1f;
                resourceManager.playerGoldRate = 10f;
                break;
        }
        resourceManager.gold = 0f;
        resourceManager.goldPerSecond = 0f;
        resourceManager.playerUpgradeCost = 300f;
        resourceManager.buffDuration = 0;
        resourceManager.buffMultiplier = 1f;

        resourceManager.characters.Clear();
        resourceManager.InitializeDefaultsChars();
        uiManager.PopulateCharacterList(resourceManager.characters);
        uiManager.ShowMainMenu();
        Debug.Log($"Prestige completed! Level: {level}, Buff: {resourceManager.prestigeBuff}");
    }

    public void InitializeFromData(PlayerData data)
    {
        level = data.prestigeLevel;
    }

    public void SaveToData(PlayerData data)
    {
        data.prestigeLevel = level;
    }

    public int getPrestigeLevel()
    {
        return level;
    }
}
