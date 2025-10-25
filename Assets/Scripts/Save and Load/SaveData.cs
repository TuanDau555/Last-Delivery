using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<SaveDataEntry> dataSaved = new List<SaveDataEntry>();

    /// <summary>
    /// Set the data to Json (Serialized)
    /// </summary>
    /// <param name="key">Id of the object need to save</param>
    /// <param name="value">Value of this key</param>
    /// <typeparam name="T">Type value want to save</typeparam>
    public void Set<T>(string key, T value)
    {
        string json;

        // if data is primitive type...
        if (value is int || value is float || value is bool || value is string)
        {
            // transfer it to string before save using ToString
            json = value.ToString();
        }
        else
        {
            // other complex Data
            json = JsonUtility.ToJson(value);
        }

        // Find if the key already exists
        var entry = dataSaved.FirstOrDefault(e => e.key == key);
        if (entry != null)
        {
            entry.value = json; // override existing value
        }
        else
        {
            // add new entry
            dataSaved.Add(new SaveDataEntry { key = key, value = json });
        }
    }
    
    /// <summary>
    /// Get the data from Json (Deserialized)
    /// </summary>
    public T Get<T>(string key, T defaultValue = default)
    {
        // Find the entry by key
        var entry = dataSaved.FirstOrDefault(e => e.key == key);
        if (entry == null) return defaultValue; // return default if not found
        
        // We need to handle primitive types separately and manually
        if (typeof(T) == typeof(int))
            return (T)(object)int.Parse(entry.value);
        if (typeof(T) == typeof(float))
            return (T)(object)float.Parse(entry.value);
        if (typeof(T) == typeof(bool))
            return (T)(object)bool.Parse(entry.value);
        if (typeof(T) == typeof(string))
            return (T)(object)entry.value;


        // If the type we Get is none of above
        // deserialized the object
        return JsonUtility.FromJson<T>(entry.value);
    }
}

/// <summary>
/// This class will serializable/deserializable data in Unity   
/// </summary>
[Serializable]
public class SaveDataEntry
{
    public string key;
    public string value;
}