using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Interactable))]
public class LoadSceneInteractable : MonoBehaviour
{
    public List<string> authorizedTags;
    public int sceneId = 0;

    Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.AddOnInteractCallback(LoadScene);
    }

    void LoadScene(Selector selector)
    {
        if (authorizedTags.Contains(selector.gameObject.tag))
            SceneManager.LoadScene(sceneId);
    }
}
