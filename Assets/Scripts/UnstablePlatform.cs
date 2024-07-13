using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnstablePlatform : MonoBehaviour
{

    [SerializeField]
    private float destroyCooldown;

    [SerializeField]
    private float respawnCooldown;

    [SerializeField]
    private float flashTiming;

    private float _destroyCount;
    private float _respawnCount;
    private float _flashCount;
    private const float _flashingDelay = 0.1f;
    private bool _active = true;
    private bool _touched = false;
    private bool _flashing = false;

    [SerializeField]
    private Image image;
    [SerializeField]
    private BoxCollider col;
    [SerializeField]
    private MeshRenderer rend;
    void Start()
    {
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_touched)
        {
            _destroyCount += Time.deltaTime;
            if (destroyCooldown - _destroyCount < 1f)
            {
                _flashing = true;
            }
        } else if (!_active){
            _respawnCount += Time.deltaTime;
        }

        if (_flashing)
        {
            _flashCount += Time.deltaTime;
        }
        if (_flashCount > _flashingDelay)
        {
            _flashCount = 0;
            image.enabled = !image.enabled;
        }

        if (_destroyCount > destroyCooldown)
        {
            Deactivate();
        }

        if (_respawnCount > respawnCooldown)
        {
            Activate();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_active && !_touched)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                _touched = true;
                image.enabled = true;
                Debug.Log("touched platform");
            }
        }
    }

    private void Activate()
    {
        _respawnCount = 0;
        _active = true;
        col.enabled = true;
        rend.enabled = true;
    }

    private void Deactivate()
    {
        _destroyCount = 0;
        _touched = false;
        _active = false;
        _flashing = false;
        col.enabled = false;
        rend.enabled = false;
        image.enabled = false;
    }
}
