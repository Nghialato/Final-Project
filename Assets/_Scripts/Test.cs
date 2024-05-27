using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        UnityEngine.Debug.Log("Awake");
    }

    private void OnEnable()
    {
        UnityEngine.Debug.Log("OnEnable");
    }

    private void Start()
    {
        UnityEngine.Debug.Log("Start");
    }

    private void OnDisable()
    {
        UnityEngine.Debug.Log("OnDisable");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
