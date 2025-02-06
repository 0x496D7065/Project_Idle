using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diamondText;
    public ResourceManager resourceManager;
    public PrestigeSystem  prestigeSystem;
    public TextMeshProUGUI goldPerSecondText;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI upgradeCostPlayerText;
    public TextMeshProUGUI buffTimerText;
    public TextMeshProUGUI prestigeLevelText;

    public GameObject mainMenu;
    public GameObject prestigeMenu;
    public GameObject confirmButton;
    public TextMeshProUGUI prestigeInfoText;

    public GameObject characterButtonPrefab;  // Assign the button template in inspector
    public Transform contentPanel;            // The parent container (Content of ScrollView)
    private List<GameObject> characterButtons = new List<GameObject>();

    private GameManager gameManager;

    public void setGameManager(GameManager gm)
    {
        gameManager = gm;
    }

    void Update()
    {
       //if (goldText == null) Debug.LogError("goldText is not assigned.");
       //if (resourceManager == null) Debug.LogError("resourceManager is not assigned.");
       //if (goldPerSecondText == null) Debug.LogError("goldPerSecondText is not assigned.");
       //if (upgradeCostText == null) Debug.LogError("upgradeCostText is not assigned.");
        goldText.text = "" + Mathf.FloorToInt(resourceManager.getGold());
        goldPerSecondText.text = "Gold/sec: " + resourceManager.getGoldPerSecond().ToString("F2");
        diamondText.text = "" + resourceManager.getDiamond();
        prestigeLevelText.text = "" + prestigeSystem.getPrestigeLevel();
        upgradeCostText.text = "Cost: " + Mathf.FloorToInt(resourceManager.getUpgradeCost());
        upgradeCostPlayerText.text = "Cost: " + Mathf.FloorToInt(resourceManager.getPlayerUpgradeCost());
        float remainingTime = Mathf.Max(0, resourceManager.getBuffDuration());
        buffTimerText.text = remainingTime > 0 ? $"Buff Time: {remainingTime:F1}s" : "";
    }

    public void PopulateCharacterList(List<Character> characters)
    {
        // Clear existing buttons to avoid duplicates
        foreach (GameObject button in characterButtons)
        {
            Destroy(button);
        }
        characterButtons.Clear();
        // Create a button for each character
        foreach (Character character in characters)
        {
            GameObject newButton = Instantiate(characterButtonPrefab, contentPanel);
            newButton.SetActive(true);

            //TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            //buttonText.text = $"{character.getName()}\n{character.getIncomePerSecond():F1} Gold/s\nCost: {character.getUpgradeCost()}";

            TextMeshProUGUI nameText = newButton.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            nameText.text = character.getName();

            TextMeshProUGUI goldPerSecText = newButton.transform.Find("CharGoldSecText").GetComponent<TextMeshProUGUI>();
            goldPerSecText.text = $"{character.getIncomePerSecond()} Gold/s";

            TextMeshProUGUI costText = newButton.transform.Find("CostText").GetComponent<TextMeshProUGUI>();
            costText.text = $"Cost: {character.getUpgradeCost()}";

            Button btn = newButton.GetComponent<Button>();
            btn.onClick.AddListener(() => OnCharacterButtonClicked(character));

            characterButtons.Add(newButton);
            //Debug.Log("Char Name:" + character.getName());
            //Debug.Log("Char Income:" + character.getIncomePerSecond());
            //Debug.Log("Char UpgradeCost:" + character.getUpgradeCost());
        }
    }

    private void OnCharacterButtonClicked(Character character)
    {
        gameManager.resourceManager.BuyCharacter(character);
        PopulateCharacterList(gameManager.resourceManager.characters); // Refresh list
    }

    public void ShowMainMenu()
    {
        prestigeMenu.SetActive(false);
    }

    public void ShowPrestigeMenu()
    {
        prestigeMenu.SetActive(true);
        prestigeInfoText.text = "Choose a buff to prestige!";
    }

    public void ShowConfirmButton()
    {
        confirmButton.SetActive(true);
    }

}
