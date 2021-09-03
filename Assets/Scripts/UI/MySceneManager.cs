using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
    
    public void CallScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
