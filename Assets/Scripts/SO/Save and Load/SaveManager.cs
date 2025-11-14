using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SaveManager : SingletonPersistent<SaveManager>
{
    #region Parameter
    [Tooltip("Name of the file that is saved")]
    [SerializeField] private string savefileName;

    private SaveData saveData;
    private FileDataHandler fileDataHandler;
    [SerializeField] private List<ISaveable> saveableObject;
    #endregion

    #region Execute
    void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, savefileName);
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

        // Just for testing
        if (saveData == null)
        {
            Debug.LogError("No save data found. Please start a new game first.");
            NewGame();
        }

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
    #endregion

    #region Find Save Object
    private List<ISaveable> FindAllSaveableObjects()
        => FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToList();
    #endregion
}