using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveSystem
{
    private static string saveFilePath = Application.persistentDataPath + "/playerData.json";

    /// <summary>
    /// Save the provided data to a file with hash validation.
    /// </summary>
    /// <param name="data">The data to save.</param>
    public static void SaveProgress(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        string hash = ComputeHash(json); // Generate hash of the JSON data

        // Create a wrapper object that includes both the JSON data and the hash
        SaveFile saveFile = new SaveFile
        {
            jsonData = json,
            hash = hash
        };

        string saveJson = JsonUtility.ToJson(saveFile); // Serialize the wrapper object
        File.WriteAllText(saveFilePath, saveJson); // Save to file
        Debug.Log("Progress saved with hash validation.");
    }

    /// <summary>
    /// Load the player data from the save file. Returns null if invalid or tampered.
    /// </summary>
    /// <returns>The loaded PlayerData object or null if the file is invalid.</returns>
    public static PlayerData LoadProgress()
    {
        if (File.Exists(saveFilePath))
        {
            string saveJson = File.ReadAllText(saveFilePath); // Read the save file
            SaveFile saveFile = JsonUtility.FromJson<SaveFile>(saveJson); // Deserialize the wrapper object

            // Validate the hash
            string recomputedHash = ComputeHash(saveFile.jsonData);
            if (recomputedHash == saveFile.hash)
            {
                Debug.Log("Save file hash is valid. Loading data...");
                return JsonUtility.FromJson<PlayerData>(saveFile.jsonData); // Return the loaded data
            }
            else
            {
                Debug.LogError("Save file hash does not match. Data may have been tampered with.");
                return null;
            }
        }
        else
        {
            Debug.Log("No save file found.");
            return null;
        }
    }

    /// <summary>
    /// Computes a SHA-256 hash for the given input string.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <returns>The Base64-encoded hash.</returns>
    private static string ComputeHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes); // Return Base64 encoded hash
        }
    }
}

// Wrapper class to store JSON data and its hash
[Serializable]
public class SaveFile
{
    public string jsonData;
    public string hash;
}