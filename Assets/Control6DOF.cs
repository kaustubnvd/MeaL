// Code from: https://developer.magicleap.com/en-us/learn/guides/unity-overview
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class Control6DOF : MonoBehaviour
{

    private MLInput.Controller _controller;
    // Start is called before the first frame update
    void Start()
    {
        _controller = MLInput.GetController(MLInput.Hand.Left);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _controller.Position;
        transform.rotation = _controller.Orientation;
    }
}
