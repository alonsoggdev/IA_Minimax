using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Canvas))]
public class CanvasOriented : MonoBehaviour
{
    private Canvas _canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();
        Assert.IsNotNull(_canvas,"Canvas not present");
        _canvas.renderMode = RenderMode.WorldSpace;
        _canvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _canvas.transform.LookAt(transform.position + _canvas.worldCamera.transform.rotation * Vector3.forward, _canvas.worldCamera.transform.rotation * Vector3.up);

    }
}
