using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Interactable))]
public class LoadSceneInteractable : InteractableFunction
{
    public int sceneId = 0;

    public override void Action(Selector selector)
    {
        SceneManager.LoadScene(sceneId);
    }
}
