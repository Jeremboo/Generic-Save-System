using GBD.SaveSystem.EncryptionDecryptionUsingSymmetricKey;
using System;
using System.IO;
using UnityEngine;

namespace GBD.SaveSystem
{
    public class SaveSystem
    {
        public static event Action GameDataSaved;
        public static event Action<string> GameDataLoaded;

        public static bool SaveGame<T>(string saveName, T serializableObject)
        {
            string json = JsonUtility.ToJson(serializableObject);
            string encryptedJson = AesOperation.EncryptString("b14ca5898a4e4133bbce2ea2315a1916", json);

            string path = $"{Application.persistentDataPath}/{saveName}.json";
            File.WriteAllText(path, encryptedJson);

            if (File.Exists(path))
            {
                Debug.Log("Saved successfully!");
                GameDataSaved?.Invoke();
                return true;
            }
            else
            {
                Debug.LogError("An error has occurred while trying to save the game.");
                return false;
            }
        }

        public static bool LoadGame<T>(string loadName)
        {
            string path = $"{Application.persistentDataPath}/{loadName}.json";

            if (!File.Exists(path))
            {
                Debug.LogWarning($"File not found at path: {path}, nothing will be loaded.");
                return false;
            }

            string json = File.ReadAllText(path);
            string decryptedJson = AesOperation.DecryptString("b14ca5898a4e4133bbce2ea2315a1916", json);
            Debug.Log(decryptedJson);

            Debug.Log($"Loaded {loadName} successfully!");
            GameDataLoaded?.Invoke(decryptedJson);
            return true;
        }

        public static bool LoadGame<T>(string loadName, out T serializableObject)
        {
            serializableObject = default;
            string path = $"{Application.persistentDataPath}/{loadName}.json";

            if (!File.Exists(path))
            {
                Debug.LogWarning($"File not found at path: {path}, nothing will be loaded.");
                return false;
            }

            string json = File.ReadAllText(path);
            string decryptedJson = AesOperation.DecryptString("b14ca5898a4e4133bbce2ea2315a1916", json);
            Debug.Log(decryptedJson);
            serializableObject = JsonUtility.FromJson<T>(decryptedJson);

            Debug.Log($"Loaded {loadName} successfully!");
            GameDataLoaded?.Invoke(decryptedJson);
            return true;
        }
    }

}