using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ResourceManager  resourceManager;
    public UIManager        uiManager;
    public PrestigeSystem   prestigeSystem;

    void Start()
    {
        if (resourceManager == null)
        {
            Debug.LogError("ResourceManager is not assigned in GameManager.");
            return;
        }

        resourceManager.setGameManager(this);
        uiManager.setGameManager(this);

        // Load saved data or set defaults
        PlayerData data = SaveSystem.LoadProgress();
        if (data != null)
        {
            Debug.Log("Save data loaded successfully.");
            resourceManager.InitializeFromData(data);
            prestigeSystem.InitializeFromData(data);
            uiManager.PopulateCharacterList(resourceManager.characters);
            float timeElapsed = GetCurrentUnixTimestamp() - data.lastLoginTime;
            Debug.Log("time elapsed since last launch= " + timeElapsed);
            Debug.Log("lastLoginTime= " + data.lastLoginTime);
            float goldBefore = resourceManager.gold;
            //Debug.Log("Current time= " + GetCurrentUnixTimestamp());
            if (timeElapsed < 0)
            {
                Debug.LogWarning("Negative time elapsed: " + timeElapsed);
                timeElapsed = 0; // Set to 0 to avoid negative gold generation
            }
            if (timeElapsed < resourceManager.buffDuration)
            {
                resourceManager.gold += (resourceManager.goldPerSecond * resourceManager.buffMultiplier) * timeElapsed;
                resourceManager.buffDuration -= timeElapsed;
                Debug.Log("Gold generated while offline: " + (resourceManager.gold - goldBefore));
            }
            else if (timeElapsed > resourceManager.buffDuration && resourceManager.buffDuration > 0)
            {
                float timeElapsedWOBuff = timeElapsed - resourceManager.buffDuration;
                resourceManager.gold += (resourceManager.goldPerSecond * resourceManager.buffMultiplier) * resourceManager.buffDuration;
                Debug.Log("Gold generated while the buff: " + ((resourceManager.goldPerSecond * resourceManager.buffMultiplier) * resourceManager.buffDuration));
                resourceManager.buffDuration = 0;
                resourceManager.buffMultiplier = 1f;
                resourceManager.gold += (resourceManager.goldPerSecond * timeElapsedWOBuff);
                resourceManager.UpdateTotalIncome();
                Debug.Log("Gold generated while offline: " + (resourceManager.gold - goldBefore));
            }
            else
            {
                resourceManager.gold += resourceManager.goldPerSecond * timeElapsed;
                Debug.Log("Gold generated while offline: " + (resourceManager.gold - goldBefore));
            }
        }
        else
        {
            Debug.Log("No save data found. Starting with default values.");
            resourceManager.InitializeWithDefaults();
            uiManager.PopulateCharacterList(resourceManager.characters);
        }
    }

    public void SaveProgress()
    {
        if (resourceManager != null)
        {
            PlayerData data = resourceManager.GetPlayerData();
            prestigeSystem.SaveToData(data);
            SaveSystem.SaveProgress(data);
        }
        else
        {
            Debug.LogError("ResourceManager is not assigned. Cannot save progress.");
        }
    }

    void OnApplicationQuit()
    {
        SaveProgress();
    }

    public long GetCurrentUnixTimestamp()
    {
        return (long)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
    }
}