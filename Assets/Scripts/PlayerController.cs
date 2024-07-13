using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _runningSpeed = 7.0f;
    [SerializeField]
    private float _turningSpeed = 10.0f;
    [SerializeField]
    private float _attackStrength = 3.0f;
    [SerializeField]
    private float _attackDuration = 1.0f;
    [SerializeField]
    private float _attackDelay = 0.2f;
    [SerializeField]
    private float _attackCooldown = 1.2f;
    [SerializeField]
    private float _attackRadius = 10.0f;
    [SerializeField]
    private float _attackAngle = 90.0f;
    [SerializeField]
    private float _forceResistance = 0.5f;
    private float _currentForceResistance;
    [SerializeField]
    private GameObject _weaponPrefab;
    [SerializeField]
    private GameObject _weaponHolder;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private CapsuleCollider _capsuleCollider;
    [SerializeField]
    private AudioClip _attackSound;
    [SerializeField]
    private AudioClip _bonkSound;
    [SerializeField]
    private AudioClip _bumpSound;
    [SerializeField]
    private AudioClip _fallSound;


    private PlayerInput playerInput;

    private bool forwardAction;
    private bool backwardAction;
    private bool rightturnAction;
    private bool leftturnAction;

    [SerializeField]
    private GameObject _attackArea;

    [SerializeField]
    public Canvas _canvas;

    private int _playerIndex = 0;
    private Vector3 _lookDirection = Vector3.zero;
    private bool _canAttack = true;
    private bool _isAttacking = false;
    private bool _isStunned = false;
    private GameObject _weapon;
    private int _playerLayer = 0;
    private int _weaponLayer = 0;

    private Vector3 _forceVector = new Vector3();
    private float _forceMagnitude = 0.0f;
    private bool _fallStarted = false;

    private List<GameObject> bonkedPlayersDuringCurrentAttack = new List<GameObject>{};

    // Start is called before the first frame update
    void Start()
    {
        GameUtils.playerCount++;
        _playerIndex = GameUtils.playerCount - 1;
        _lookDirection = new Vector3(0, 0, 1);
        _weapon = Instantiate(_weaponPrefab, _weaponHolder.transform.position, _weaponHolder.transform.rotation);
        _weapon.transform.parent = gameObject.transform;
        gameObject.layer = _playerLayer;
        transform.Find("Physics").gameObject.layer = _playerLayer;
        _weapon.layer = _weaponLayer;
        _weapon.transform.Find("Physics").gameObject.layer = _weaponLayer;
        _currentForceResistance = _forceResistance;
        playerInput = GetComponent<PlayerInput>();
        int playerActionMapIndex = _playerIndex + 1;
        playerInput.SwitchCurrentActionMap("Player" + playerActionMapIndex);
        Debug.Log("Player" + playerActionMapIndex);
        Color attackAreaColor = GameUtils.Instance.GetPlayerColor(_playerIndex);
        attackAreaColor.a = 0.6f;
        _attackArea.GetComponent<Image>().color = attackAreaColor;
    }

    public void ForwardRead(InputAction.CallbackContext context)
    {
        forwardAction = context.ReadValueAsButton();
    }

    public void BackwardRead(InputAction.CallbackContext context)
    {
        backwardAction = context.ReadValueAsButton();
    }

    public void LeftTurnRead(InputAction.CallbackContext context)
    {
        leftturnAction = context.ReadValueAsButton();
    }

    public void RightTurnRead(InputAction.CallbackContext context)
    {
        rightturnAction = context.ReadValueAsButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -5)
        {
            Death();
            GameUtils.Instance.KillPlayer(_playerIndex);
        }

        Vector3 turningVector = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        _forceVector.y = 0.0f;
        Vector3 pushed = _forceVector * _forceMagnitude;
        if (leftturnAction)
        {
            velocity.x -= _runningSpeed;
            turningVector.x -= Time.deltaTime * _turningSpeed;
            //if (Mathf.Approximately(_lookDirection.normalized.x, 1.0f))
            //{
            //    turningVector.z -= Time.deltaTime * _turningSpeed * 5f;
            //}
        }
        if (rightturnAction)
        {
            velocity.x += _runningSpeed;
            turningVector.x += Time.deltaTime * _turningSpeed;
            //if (Mathf.Approximately(_lookDirection.normalized.x, -1.0f))
            //{
            //    turningVector.z += Time.deltaTime * _turningSpeed * 5f;
            //}
        }
        if (backwardAction)
        {
            velocity.z -= _runningSpeed;
            turningVector.z -= Time.deltaTime * _turningSpeed;
            //if (Mathf.Approximately(_lookDirection.normalized.z, 1.0f))
            //{
            //    turningVector.x += Time.deltaTime * _turningSpeed * 5f;
            //}
        }
        if (forwardAction) // Input.GetKey(forwardKeys[_playerIndex])
        {
            velocity.z += _runningSpeed;
            turningVector.z += Time.deltaTime * _turningSpeed;
            //if (Mathf.Approximately(_lookDirection.normalized.z, -1.0f))
            //{
            //    turningVector.x -= Time.deltaTime * _turningSpeed * 5f;
            //}
        }
        if (Vector3.Angle(_lookDirection, velocity) > 90)
        {
            _lookDirection = Vector3.Slerp(_lookDirection.normalized, velocity.normalized, Time.deltaTime * _turningSpeed);
        }
        else
            _lookDirection += turningVector;
        velocity.Normalize();
        velocity = velocity * _runningSpeed;
        _animator.SetFloat("Speed", _isStunned ? 0f : velocity.magnitude);
        _isStunned = _forceMagnitude > 0.0f;

        bool raycastHit = Physics.Raycast(_capsuleCollider.bounds.center, Vector3.down, _capsuleCollider.bounds.extents.y + 0.5f);
        Debug.DrawRay(_capsuleCollider.bounds.center, Vector3.down * (_capsuleCollider.bounds.extents.y + 0.5f));
        if (!raycastHit || transform.position.y < -0.05)
        {
            Debug.Log("Fall");
            velocity.y -= 15f;
            if (!_fallStarted && transform.position.y < -1f)
            {
                _fallStarted = true;
                SoundManager.Instance.PlaySound(_fallSound);
            }
        }
        if (!_isStunned)
        {
            _forceVector = velocity;
            transform.position = GameUtils.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        }
        transform.position += pushed * Time.deltaTime;
        transform.position = GameUtils.Instance.positionClippedIntoGameArea(transform.position);
        _lookDirection.Normalize();
        transform.rotation = Quaternion.LookRotation(_lookDirection);
        _weapon.transform.position = _weaponHolder.transform.position;
        _weapon.transform.rotation = _weaponHolder.transform.rotation;
        if (_forceMagnitude > 0.0f)
        {
            _forceMagnitude -= Time.deltaTime * _currentForceResistance;
            _currentForceResistance = _currentForceResistance + Time.deltaTime * _forceResistance;
        }
        else
        {
            _forceMagnitude = 0.0f;
            _currentForceResistance = _forceResistance;
        }
        
        if (_isAttacking)
        {
            List<GameObject> _players = GameUtils.Instance.getPlayers();
            for (int i = 0; i < _players.Count; i++)
            {
                Vector2 _batPos = new Vector2(_weapon.transform.GetChild(2).transform.position.x, _weapon.transform.GetChild(2).transform.position.z);
                Vector2 _playerPos = new Vector2(transform.position.x, transform.position.z);
                Vector2 _batVector = _batPos - _playerPos;

            
                PlayerController enemyController = _players[i].GetComponent<PlayerController>();
                if (bonkedPlayersDuringCurrentAttack.Contains(_players[i])) continue;
                if (enemyController == null) continue;
                if (enemyController == this) continue;

                Vector2 _enemyPos = new Vector2(enemyController.transform.position.x, enemyController.transform.position.z);
                Vector2 _enemyVector = _enemyPos - _playerPos;
                if (IsTargetInAttackArea(enemyController.gameObject) && Vector2.SignedAngle(_batVector, _enemyVector) < 0 && 
                    Vector2.Angle(_batVector, _enemyVector) < 60)
                {
                    enemyController.GetPushed(this);
                    SoundManager.Instance.PlaySound(_bonkSound);
                    bonkedPlayersDuringCurrentAttack.Add(_players[i]);
                    Debug.Log("player got bonked");
                }
            }
        }


    }

    public void MakeAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            _isAttacking = false;
            _animator.SetTrigger("Attack");
            bonkedPlayersDuringCurrentAttack.Clear();
            StartCoroutine(DelayAttack());
            StartCoroutine(ResetAttackDuration());
            StartCoroutine(ResetAttackCooldown());
        }
    }

    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(_attackDelay);
        _isAttacking = true;
        SoundManager.Instance.PlaySound(_attackSound);
    }

    IEnumerator ResetAttackDuration()
    {
        yield return new WaitForSeconds(_attackDuration);
        _isAttacking = false;
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
        _animator.ResetTrigger("Attack");
    }

    public void SetLayers(int playerLayer, int weaponLayer)
    {
        _playerLayer = playerLayer;
        _weaponLayer = weaponLayer;
    }
    
    private void Death()
    {
        Debug.Log("Player:" + _playerIndex + " has been slain");
        Destroy(this.gameObject);
        Debug.Log("Player: " + _playerIndex + "has been slain");
    }
    



    public bool IsTargetInAttackArea(GameObject target)
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 targetDirection = targetPosition - transform.position;
        bool mag = targetDirection.magnitude <= _attackRadius;
        bool dot = Vector3.Dot(targetDirection.normalized, transform.forward) > Mathf.Cos(_attackAngle * 0.5f * Mathf.Deg2Rad);
        return (mag && 
                dot);
    }

    public void GetPushed(PlayerController attacker)
    {
        _forceVector = (transform.position - attacker.transform.position).normalized;
        _forceMagnitude = attacker.GetAttackStrength();
        _isStunned = true;
    }

    public float GetAttackStrength()
    {
        return _attackStrength;
    }

    public void SetAttackStrength(float newStrength)
    {
        _attackStrength = newStrength;
    }

    public float GetAttackRadius()
    {
        return _attackRadius;
    }

    public void SetAttackRadius(float newRadius)
    {
        _attackRadius = newRadius;
    }

    public void EnlargeWeapon(float factor)
    {
        _weapon.transform.localScale *= factor;
    }

    public Vector3 GetForceVector()
    {
        return _forceVector;
    }

    public void SetForceVector(Vector3 forceVector)
    {
        _forceVector = forceVector;
    }

    public float GetForceMagnitude()
    {
        return _forceMagnitude;
    }

    public void SetForceMagnitude(float forceMagnitude)
    {
        _forceMagnitude = forceMagnitude;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        
        //hit by bat collider
        PlayerController enemyController = collision.collider.transform.parent?.parent?.GetComponent<PlayerController>();
        /*
        if (enemyController != null && enemyController._isAttacking && enemyController.IsTargetInAttackArea(gameObject))
        {
            string colLayer = collision.collider.gameObject.layer.ToString();
            Debug.Log("Layer " + colLayer + " hit layer " + gameObject.layer);
            GetPushed(enemyController);
            SoundManager.Instance.PlaySound(_bonkSound);
            return;
        }*/
        //hit by player collider
        enemyController = collision.collider.transform.parent?.GetComponent<PlayerController>();
        if (enemyController != null)
        {
            Debug.Log("Bumped into player");
            SoundManager.Instance.PlaySound(_bumpSound);
            _forceVector = -_forceVector;
            if (!_isStunned)
            {
                _forceMagnitude = GameUtils.Instance.BounceFactor;
            }
        }
    }

    public int GetPlayerIndex()
    {
        return _playerIndex;
    }

    private void OnDestroy()
    {
        GameUtils.playerCount--;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Color c = new Color(0.8f, 0, 0, 0.4f);
        UnityEditor.Handles.color = c;

        Vector3 rotatedForward = Quaternion.Euler(
            0,
            -_attackAngle * 0.5f,
            0) * transform.forward;

        UnityEditor.Handles.DrawSolidArc(
            transform.position,
            Vector3.up,
            rotatedForward,
            _attackAngle,
            _attackRadius);

    }
#endif
}
