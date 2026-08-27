using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] private List<Transform> positions;
    [SerializeField, Min(0.1f)] private float transitionLength; 
    
    private int _targetTransformIndex = 0;

    private float _lastTime = 0;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;

    private bool _isFocused = true;
    private Camera _cameraMainCached;
    
    private void Awake()
    {
        if (Camera.main) {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
            
            _cameraMainCached = Camera.main;
        }

        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
        
        _lastTime = Time.time;
    }

    private void OnEnable()
    {
        if (Camera.main) Camera.main.enabled = false;
        _isFocused = true;
    }

    private void OnDisable()
    {
        if(_cameraMainCached) _cameraMainCached.enabled = true;
    }

    public void SetTarget(int index)
    {
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
        _lastTime = Time.time;
        _targetTransformIndex = index;
    }

    public async UniTask MoveBack(CancellationToken token)
    {
        if (!_isFocused)
            return;
        
        _isFocused = false;

        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
        _lastTime = Time.time;
        
        while (!token.IsCancellationRequested && !destroyCancellationToken.IsCancellationRequested) {
            if (!_cameraMainCached)
                return;

            if (LerpCamera(_cameraMainCached.transform)) {
                return;
            }
            
            await UniTask.Yield(cancellationToken: destroyCancellationToken);
        }
    }

    private void Update()
    {
        if (_isFocused) {
            if (Input.GetMouseButtonDown(1)) {
                SetTarget((_targetTransformIndex + 1) % positions.Count);
            }
            
            LerpCamera(positions[_targetTransformIndex]);
        }
    }

    private bool LerpCamera(Transform targetTransform)
    {
        var t = (Time.time - _lastTime) / transitionLength;
        
        transform.position = Vector3.Slerp(_lastPosition, targetTransform.position, t);
        transform.rotation = Quaternion.Slerp(_lastRotation, targetTransform.rotation, t);
        
        return t >= 1;
    }
}
