using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] private List<Transform> positions;
    [SerializeField, Min(0.1f)] private float transitionLength; 
    
    private int _targetTransformIndex = 0;

    private float _lastTime = 0;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    
    private void Awake()
    {
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
        _lastTime = Time.time;
    }

    public void SetTarget(int index)
    {
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
        _lastTime = Time.time;
        _targetTransformIndex = index;
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            SetTarget((_targetTransformIndex + 1) % positions.Count);
        }
        
        if (_targetTransformIndex >= positions.Count)
            return;
        
        var t = (Time.time - _lastTime) / transitionLength;
        
        transform.position = Vector3.Slerp(_lastPosition, positions[_targetTransformIndex].position, t);
        transform.rotation = Quaternion.Slerp(_lastRotation, positions[_targetTransformIndex].rotation, t);
    }
}
