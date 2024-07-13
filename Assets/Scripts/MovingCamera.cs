using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    private Vector3 _viewedPoint;
    private Vector3 _originalViewPoint;
    private Camera _camera;
    private float _maxFOV;
    private float _minFOV;
    private float _maxSize;
    private float _minSize;
    private float _maxPlayerDistance;

    // Start is called before the first frame update
    void Start()
    {
        _viewedPoint = GameUtils.Instance.GetCenterBetweenPlayers();
        _originalViewPoint = _viewedPoint;
        _maxPlayerDistance = GameUtils.Instance.playerMaxDistance;
        _camera = transform.GetComponent<Camera>();
        if (_camera != null)
        {
            _maxFOV = _camera.fieldOfView;
            _minFOV = _maxFOV * 0.75f;

            _minSize = _camera.orthographicSize;
            _maxSize = _minSize * 0.6f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newViewPoint = GameUtils.Instance.GetCenterBetweenPlayers();
        float currentPlayerDistance = GameUtils.Instance.playerMaxDistance;
        if (_maxPlayerDistance >= currentPlayerDistance)
        {
            float ratio = currentPlayerDistance / _maxPlayerDistance;
            _camera.fieldOfView = Mathf.Lerp(_minFOV, _maxFOV, ratio);
            _camera.orthographicSize = Mathf.Lerp(_maxSize, _minSize, ratio);
        }
        else
        {
            newViewPoint = _originalViewPoint;
        }
        newViewPoint.y = 0;
        //Debug.DrawLine(transform.position, transform.forward);
        Vector3 move = newViewPoint - _viewedPoint;
        transform.position += move * GameUtils.Instance.cameraSpeed * Time.deltaTime;
        _viewedPoint += move * GameUtils.Instance.cameraSpeed * Time.deltaTime;
    }
}
