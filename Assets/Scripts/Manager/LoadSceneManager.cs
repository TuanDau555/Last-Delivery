using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : SingletonPersistent<LoadSceneManager>
{
    [Space(10)]
    [SerializeField] private float _sceneChangeDuration = 0.3f;
    
    private const string LV1 = "Lv1";
    private const string LV2 = "Lv2";

    #region Load Scene
    [ContextMenu("Load Test Scene")]
    public void StartChangeScene()
    {
        StartCoroutine(LoadScene());
    }

    public IEnumerator LoadScene()
    {
        string nextScene = SceneManager.GetActiveScene().name;
        // Player can't go to the next scene while delivering object to prevent null
        if (DeliveryManager.Instance.currentDeliveryState == DeliveryState.DELIVER)
        {
            // TODO: Pop Up UI
            yield break;
        }


        SaveManager.Instance.SaveGame();
        // TODO: Fade in
        yield return new WaitForSeconds(_sceneChangeDuration);

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            nextScene = LV2;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            nextScene = LV1;
        }

        // When done Load Scene Load the data
        SceneManager.sceneLoaded += OnSceneLoad;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        Debug.Log("Scene Load Complete");
        while (!asyncLoad.isDone)
        {
            // ALTERNATIVE: Loading bar
            Debug.Log("loading: " + asyncLoad.progress + "%");
            yield return null;
        }
        // TODO: Fade out
    }
    #endregion

    #region LoadScene
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedLoadGame());
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private IEnumerator DelayedLoadGame()
    {
        yield return new WaitForSeconds(0.01f);
        SaveManager.Instance.LoadGame();
        Debug.Log("Load Data Complete");
    }
    #endregion
}
