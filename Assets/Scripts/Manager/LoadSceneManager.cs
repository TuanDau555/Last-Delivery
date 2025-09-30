using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public Transform playerPrefab;
    public Vector3 position;
    [ContextMenu("Load Test Scene")]
    public void LoadScene()
    {
        SceneManager.LoadScene(2);
    }
}
