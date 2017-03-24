using UnityEngine;
using UnityEngine.SceneManagement;
using AssetBundles;
using System.Collections;

public class sceneLoadController : MonoBehaviour {

    public bool virtualAdditive = true;
    public string error;
    public string lvl;

    public string scene1AssetBundle;
    public string scene1Name;
    public string scene2AssetBundle;
    public string scene2Name;
    public string scene3AssetBundle;
    public string scene3Name;

    public bool loadStart = false;
    public bool loadGame = false;
    public bool loadCredits = false;
    private bool loadedStart = false;
    private bool loadedGame = false;
    private bool loadedCredits = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        // Switching levels
        if (Input.GetButtonDown("Scene1"))
        {
            LevelChoose("Scene1");
        }
        else if (Input.GetButtonDown("Scene2"))
        {
            LevelChoose("Scene2");
        }
        else if (Input.GetButtonDown("Scene3"))
        {
            LevelChoose("Scene3");
        }
        //

        // Additive loads
        if (loadStart && !loadedStart)
        {
            LoadLevel(scene1AssetBundle, scene1Name);

            loadedStart = true;
        }
        else if (loadGame && !loadedGame)
        {
            LoadLevel(scene2AssetBundle, scene2Name);

            loadedGame = true;
        }
        else if (loadCredits && !loadedCredits)
        {
            LoadLevel(scene3AssetBundle, scene3Name);

            loadedCredits = true;
        }
        //

        // Unloads
        if (!loadStart && loadedStart)
        {
            UnloadLevel(scene1AssetBundle, scene1Name);

            loadedStart = false;
        }
        else if (!loadGame && loadedGame)
        {
            UnloadLevel(scene2AssetBundle, scene2Name);

            loadedGame = false;
        }
        else if (!loadCredits && loadedCredits)
        {
            UnloadLevel(scene3AssetBundle, scene3Name);

            loadedCredits = false;
        }
        //

    }

    public void LoadLevel(string bundle, string scene)
    {
        AssetBundleManager.LoadLevelAsync(bundle, scene, true);
        AssetBundleManager.GetLoadedAssetBundle(bundle, out error);
    }

    public void UnloadLevel(string bundle, string scene)
    {
        AssetBundleManager.UnloadAssetBundle(bundle);
        SceneManager.UnloadScene(scene);
    }

    public void LevelChoose(string lvl)
    {
        // Switching levels
        if (lvl == "Scene1")
        {
            if (virtualAdditive)
            {
                if (loadStart)
                    loadStart = false;
                else
                    loadStart = true;
            }
            else
            {
                loadGame = false;
                loadCredits = false;
                loadStart = true;
            }

        }
        else if (lvl == "Scene2")
        {
            if (virtualAdditive)
            {
                if (loadGame)
                    loadGame = false;
                else
                    loadGame = true;
            }
            else
            {
                loadCredits = false;
                loadStart = false;
                loadGame = true;
            }
        }
        else if (lvl == "Scene3")
        {
            if (virtualAdditive)
            {
                if (loadCredits)
                    loadCredits = false;
                else
                    loadCredits = true;
            }
            else
            {
                loadGame = false;
                loadStart = false;
                loadCredits = true;
            }
        }

        lvl = "ready to load";
        //
    }
}
