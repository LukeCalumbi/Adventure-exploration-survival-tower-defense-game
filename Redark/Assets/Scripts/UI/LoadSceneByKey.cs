using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneByKey : MonoBehaviour
{
    public List<KeyScenePair> keyScenePairs;
    int choosenScene = -1;

    void Update()
    {
        if (ScreenFade.IsFadeInComplete())
        {
            ScreenFade.StartFadeOut();
            SceneManager.LoadScene(choosenScene);
            return;
        }

        if (choosenScene != -1)
            return;

        foreach (KeyScenePair keyScenePair in keyScenePairs)
        {
            if (Input.GetKeyDown(keyScenePair.key))
            {
                choosenScene = keyScenePair.scene;
                ScreenFade.StartFadeIn();
                return;
            }
        }
    }
}

[Serializable]
public class KeyScenePair
{
    public KeyCode key;
    public int scene;
}
