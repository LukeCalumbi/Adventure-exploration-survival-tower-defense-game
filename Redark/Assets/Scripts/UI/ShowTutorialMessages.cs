using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorialMessages : MonoBehaviour
{
    [SerializeField] private TargetingSystem target;
    [SerializeField] private GameObject message;

    void Update()
    {
        message.SetActive(target.GetTarget() != null);
    }
}