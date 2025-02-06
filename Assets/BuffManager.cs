using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public ResourceManager resourceManager;

    public void TriggerBuff()
    {
        resourceManager.activateBuff(2f, 10f);
        Debug.Log("Buff activated! Income multiplied by 2 for 120 seconds.");
    }
}
