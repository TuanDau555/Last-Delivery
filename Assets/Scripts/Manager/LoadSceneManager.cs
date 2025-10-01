using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [ContextMenu("Load Test Scene")]
    public void LoadScene(string sceneName, string spawnID)
    {
        SceneManager.LoadScene(2);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    
    #region OnSceneLoad
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (DeliveryManager.Instance.currentDeliveryObject != null)
        {
            CargoObjectSO cargoObjectSO = DeliveryManager.Instance.currentDeliveryObject;

            CargoObject.SpawnCargoObject(cargoObjectSO, GetComponent<PlayerController>());
        }
    }
    #endregion
}
