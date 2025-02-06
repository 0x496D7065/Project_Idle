using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ResourceManager resourceManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            resourceManager.gold += resourceManager.playerGoldRate;
            Debug.Log("Gold added! Current gold: " + resourceManager.getGold());
        }
    }
}