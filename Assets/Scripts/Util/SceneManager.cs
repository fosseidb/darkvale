using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{

    public void LoadScene(int sceneIndex)
    {
        Debug.Log("sceneBuildIndex to load: " + sceneIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("sceneBuildIndex to load: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}