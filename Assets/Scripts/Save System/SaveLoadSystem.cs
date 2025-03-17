using UnityEngine;
using System.IO;

public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem Instance { get; private set; }
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData()
    {
        GameData data = DataManager.Instance.gameData;
        string json = JsonUtility.ToJson(data);

        // Create a backup of the current save file
        if (File.Exists(saveFilePath))
        {
            File.Copy(saveFilePath, saveFilePath + ".backup", true);
        }

        File.WriteAllText(saveFilePath, json);
    }

    public GameData LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            //gameData object is constructed by default constructor, with all values set to 0 or null. When loading, we overwrite the values with
            //the ones from the save file. WARNING. we may need a constructor with parameters if we want to set default values and to make it more 
            //readable
            DataManager.Instance.gameData = JsonUtility.FromJson<GameData>(json);
            return DataManager.Instance.gameData;
        }
        else
        {
            Debug.LogError("Save file not found in " + saveFilePath);
            return null;
        }
    }
}