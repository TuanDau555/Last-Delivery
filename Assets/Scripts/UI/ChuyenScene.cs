using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChuyenScene : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        // Kiểm tra xem tên scene có rỗng hay không để tránh lỗi
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty or null!");
        }
    }

    // Bạn cũng có thể tạo hàm để quay lại Scene trước đó nếu cần
    // Hoặc một hàm để thoát game cho nút Quit
    public void QuitGame()
    {
        //  Debug.Log("Quitting game..."); // Dòng này chỉ hiện trong Editor
        Application.Quit();
    }
}
