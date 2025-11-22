using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class SaveManager : SingletonPersistent<SaveManager>
{
    #region Parameter
    [Tooltip("Name of the file that is saved")]
    [SerializeField] private string savefileName;
    [SerializeField] private string checkpointSaveFile;

    private SaveData saveData;
    private FileDataHandler fileDataHandler, checkpointDataHandler;
    [SerializeField] private List<ISaveable> saveableObject;
    #endregion

    #region Execute
    void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, savefileName);
        checkpointDataHandler = new FileDataHandler(Application.persistentDataPath, checkpointSaveFile);
        saveableObject = FindAllSaveableObjects();
    }
    #endregion

    #region Save/Load
    public void NewGame()
    {
        saveData = new SaveData();
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        saveData = fileDataHandler.LoadFile();

        // Push the data to all objects that need it
        foreach (ISaveable saveable in saveableObject)
        {
            saveable.Load(saveData);
            Debug.Log($"Load data for {saveable.GetType().Name}");
            Debug.Log($"Data: {saveData.dataSaved}");
        }
    }

    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        foreach (ISaveable saveable in saveableObject)
        {
            saveable.Save(saveData);
            Debug.Log($"Saved data for {saveable.GetType().Name}");
            Debug.Log($"Data: {saveData.dataSaved}");
        }

        fileDataHandler.SaveFile(saveData);
    }

    public bool HasSaveFile()
    {
        if(fileDataHandler == null)
        {
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, savefileName);
        }
        Debug.Log(File.Exists(fileDataHandler.SavePath()));
        return File.Exists(fileDataHandler.SavePath());
    }
    #endregion

    #region Save/Load Checkpoint
    public void LoadCheckpoint()
    {
        saveData = checkpointDataHandler.LoadFile();

        if (saveData == null)
        {
            Debug.LogError("No save data found. Please start a new game first.");
            NewGame();
        }

        // Push the data to all objects that need it
        foreach (ISaveable saveable in saveableObject)
        {
            saveable.Load(saveData);
        }
    }

    public void SaveCheckpoint()
    {
        foreach (ISaveable saveable in saveableObject)
        {
            saveable.Save(saveData);
        }

        checkpointDataHandler.SaveFile(saveData);
    }
    #endregion
    
    #region Find Save Object
    public void RefreshSaveables()
    {
        saveableObject = FindAllSaveableObjects();
    }

    private List<ISaveable> FindAllSaveableObjects()
        => FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToList();
    #endregion
}