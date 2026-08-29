using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CameraBillboarder : MonoBehaviour
{
    private Camera _camera;
    
    private void OnEnable()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera) {
            transform.LookAt(_camera.transform);
            var rot = transform.rotation.eulerAngles;
            rot.x = 0;
            rot.z = 0;
            transform.rotation = Quaternion.Euler(rot);
        }
    }
}
