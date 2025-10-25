using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string saveDirPath = "";
    private string saveFileName = "";
    private string fullSavePath = "";

    #region Constructor
    public FileDataHandler(string saveDirPath, string saveFileName)
    {
        this.saveDirPath = saveDirPath;
        this.saveFileName = saveFileName;
    }
    #endregion

    #region Path
    // Set the save path on difference OS
    public string SavePath()
    {
        fullSavePath = Path.Combine(saveDirPath, saveFileName);
        return fullSavePath;
    }
    #endregion

    #region Save/Load
    public SaveData LoadFile()
    {
        SaveData loadData = null;

        if (File.Exists(SavePath()))
        {
            try
            {
                string dataToLoad = "";

                // Load serialized data from file
                using (FileStream stream = new FileStream(SavePath(), FileMode.Open))
                {
                    // Open stream
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        // read stream
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserialize the data from Json back into Objects
                loadData = JsonUtility.FromJson<SaveData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load file: {e.Message}");
            }
        }
        else
        {
            Debug.LogError($"Save file does not exist at {SavePath()}");
        }

        return loadData;
    }

    public void SaveFile(SaveData data)
    {
        try
        {
            // Create the direct if it doesn't exits
            if (!Directory.Exists(saveDirPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath()));
            }

            // Serialized data to Json
            string json = JsonUtility.ToJson(data, true);

            // Write data to file
            using(FileStream stream = new FileStream(SavePath(), FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save file: {e.Message}");
        }
    }
    #endregion
}