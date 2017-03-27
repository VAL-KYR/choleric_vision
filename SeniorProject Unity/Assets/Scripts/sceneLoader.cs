using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class sceneLoader : MonoBehaviour {
    public Texture2D emptyProgressBar; // Set this in inspector.
    public Texture2D fullProgressBar; // Set this in inspector.
    public string levelName;

    private AsyncOperation async = null; // When assigned, load is in progress.

    // Use this for initialization
    public IEnumerator Start()
    {
        yield return StartCoroutine(LoadALevel());
    }

    private IEnumerator LoadALevel()
    {
        async = SceneManager.LoadSceneAsync(levelName);
        Debug.Log("progress " + async.progress);
        yield return async;
    }
    void OnGUI()
    {
        if (async != null)
        {
            GUI.DrawTexture(new Rect(Screen.width/2 - 400/2, Screen.height/2, 400, 26), emptyProgressBar);
            GUI.DrawTexture(new Rect(Screen.width/2 - 390/2, Screen.height/2 + 3, 390 * async.progress, 20), fullProgressBar);
            GUI.TextArea(new Rect(Screen.width/2 - 400/2, Screen.height/2 + 40, 400, 20), "Loading...");
            
        }
    }
}
