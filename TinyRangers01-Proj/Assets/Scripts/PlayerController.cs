using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;

    // TEMP
    [SerializeField] Transform _projectileWeapon;
    [SerializeField] Transform _weaponFirePointR; // Fire point of weapon when facing right
    [SerializeField] Transform _weaponFirePointL; // Fire point of weapon when facing left

    [Header("Debug")]
    [SerializeField] bool _disableMouseLookInput; // This is used for weapon position adjustment without the mouse look interfering.

    // Components
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    PlayerInput _playerInput;

    // Player input values
    Vector2 _movementDirectionInput;
    Vector2 _rightStickLookDirectionInput;
    Vector2 _mouseLookDirectionInput;

    Camera _mainCamera;
    bool _useMouseLook;

    // Temp weapon sprite value
    SpriteRenderer _weaponSpriteRenderer; // TEMP
    LineRenderer _lineRenderer;

    // Facing
    GameManager.SpriteFacingDirection _playerFacingDirection = GameManager.SpriteFacingDirection.Invalid;
    GameManager.SpriteFacingDirection _projectileWeaponFacingDirection = GameManager.SpriteFacingDirection.Invalid;

    // Animation
    int animParam_IsMoving = Animator.StringToHash("IsMoving");

    void Start()
    {
        // Initialize Components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();

        _mainCamera = Camera.main;
        _weaponSpriteRenderer = _projectileWeapon.GetComponentInChildren<SpriteRenderer>();

        // Sprite faces right by default
        _playerFacingDirection = GameManager.SpriteFacingDirection.Right;
        _projectileWeaponFacingDirection = GameManager.SpriteFacingDirection.Right;

        _lineRenderer = gameObject.AddComponent<LineRenderer>();

        CheckValidFields();
    }

    void CheckValidFields()
    {
        if(Mathf.Approximately(_moveSpeed, 0.0f))
        {
            Debug.LogWarning("Invalid _moveSpeed on " + gameObject.name + ". Player will not move. Check Inspector values.");
        }
    }

    void Update()
    {
        //var inputActions = _playerInput.actions;
        //var fireHoldInputAction = _playerInput.actions["FireHold"];
        //Debug.Log("fireHoldInputAction: " + fireHoldInputAction);
        //Debug.Log("-----------------------");
        //Debug.Log("fireHoldInputAction.IsInProgress:          " + fireHoldInputAction.IsInProgress());
        //Debug.Log("fireHoldInputAction.IsPressed:             " + fireHoldInputAction.IsPressed());
        //Debug.Log("-");

        //var fireInputAction = _playerInput.actions["Fire"];
        //Debug.Log("fireInputAction.IsInProgress:          " + fireInputAction.IsInProgress());
        //Debug.Log("fireInputAction.IsPressed:             " + fireInputAction.IsPressed());

        //Debug.Log("fireHoldInputAction.WasPerformedThisFrame: " + fireHoldInputAction.WasPerformedThisFrame());
        //Debug.Log("fireHoldInputAction.WasPressedThisFrame:   " + fireHoldInputAction.WasPressedThisFrame());
        //Debug.Log("fireHoldInputAction.WasReleasedThisFrame:  " + fireHoldInputAction.WasReleasedThisFrame());
        
    }

    void FixedUpdate()
    {
        //UpdatePlayerAnimStates();

        // Update Movement
        if(!_movementDirectionInput.Equals(Vector2.zero))
        {
            // Move to the new position using the movement direction input
            Vector2 movement = _rigidbody2D.position + _movementDirectionInput * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(movement);

            // Update the player facing based on the player's movement direction input
            if(Mathf.Abs(_movementDirectionInput.x) > 0.0f)
            {
                _playerFacingDirection = _movementDirectionInput.x > 0.0f ? GameManager.SpriteFacingDirection.Right : GameManager.SpriteFacingDirection.Left;
            }
        }

        // Update Look
        if(!_rightStickLookDirectionInput.Equals(Vector2.zero))
        {
            Vector3 cross = Vector3.Cross(Vector2.up, _rightStickLookDirectionInput);
            float flipValue = cross.z < 0.0f ? -1.0f : 1.0f;
            float rotateAngle = Vector2.Angle(Vector2.up, _rightStickLookDirectionInput) * flipValue;
            //_projectileWeaponRotationPoint.rotation = Quaternion.Euler(0.0f, 0.0f, rotateAngle);
            _projectileWeapon.rotation = Quaternion.Euler(0.0f, 0.0f, rotateAngle);

            // Update the gun facing based on the player's look input direction
            _projectileWeaponFacingDirection = cross.z >= 0.0f ? GameManager.SpriteFacingDirection.Left : GameManager.SpriteFacingDirection.Right;
        }
        else
        {
            if(_useMouseLook)
            {
                // Get the direction from the player character to the mouse position
                //Vector2 dirPlayerToMousePos = (_mouseLookDirectionInput - _rigidbody2D.position).normalized; // TODO: Try weapon point for better accuracy
                Vector2 projectileWeaponPos = new Vector2(_projectileWeapon.position.x, _projectileWeapon.position.y);
                Vector2 dirWeaponToMousePos = (_mouseLookDirectionInput - projectileWeaponPos).normalized; // TODO: Try weapon point for better accuracy
                dirWeaponToMousePos = (_mouseLookDirectionInput - new Vector2(_weaponFirePointR.position.x, _weaponFirePointR.position.y)).normalized;

                //Vector3 cross = Vector3.Cross(Vector2.up, dirPlayerToMousePos);
                Vector3 cross = Vector3.Cross(Vector2.up, dirWeaponToMousePos);
                float flipValue = cross.z < 0.0f ? -1.0f : 1.0f;
                //float rotateAngle = Vector2.Angle(Vector2.up, dirPlayerToMousePos) * flipValue;
                float rotateAngle = Vector2.Angle(Vector2.up, dirWeaponToMousePos) * flipValue;

                rotateAngle = Mathf.Atan2(dirWeaponToMousePos.y, dirWeaponToMousePos.x) * Mathf.Rad2Deg - 90.0f;
                //Debug.Log("rotateAngle: " + rotateAngle);
                //_projectileWeaponRotationPoint.rotation = Quaternion.Euler(0.0f, 0.0f, rotateAngle);
                _projectileWeapon.rotation = Quaternion.Euler(0.0f, 0.0f, rotateAngle);

                // Update the gun facing based on the player's mouse cursor direction
                _projectileWeaponFacingDirection = cross.z >= 0.0f ? GameManager.SpriteFacingDirection.Left : GameManager.SpriteFacingDirection.Right;

                //
                Vector2 lineEndPos = projectileWeaponPos + dirWeaponToMousePos * 20.0f;
                _lineRenderer.SetPosition(0, _weaponFirePointR.position);
                _lineRenderer.SetPosition(1, lineEndPos);
                _lineRenderer.startWidth = 0.1f;
                _lineRenderer.endWidth = 0.1f;
            }
        }

        // Update player and weapon facing directions
        UpdatePlayerSpriteFacingDirection();
        UpdateProjectileWeaponSpriteFacingDirection();

        
    }

    void UpdatePlayerAnimStates()
    {
        if(!_movementDirectionInput.Equals(Vector2.zero))
        {
            _animator.SetBool(animParam_IsMoving, true);
        }
        else
        {
            _animator.SetBool(animParam_IsMoving, false);
        }
    }

    void UpdatePlayerSpriteFacingDirection()
    {
        switch(_playerFacingDirection)
        {
            case GameManager.SpriteFacingDirection.Right: _spriteRenderer.flipX = false; break;
            case GameManager.SpriteFacingDirection.Left: _spriteRenderer.flipX = true; break;
            default: break;
        }
    }

    void UpdateProjectileWeaponSpriteFacingDirection()
    {
        switch(_projectileWeaponFacingDirection)
        {
            case GameManager.SpriteFacingDirection.Right: _weaponSpriteRenderer.flipY = false; break;
            case GameManager.SpriteFacingDirection.Left: _weaponSpriteRenderer.flipY = true; break;
            default: break;
        }
    }

    void OnMove(InputValue inputValue)
    {
        _movementDirectionInput = inputValue.Get<Vector2>();
    }

    void OnLook(InputValue inputValue)
    {
        _rightStickLookDirectionInput = inputValue.Get<Vector2>();

        // The player is using their gamepad's right thumbstick for aiming, so do not use mouse look for aiming the gun
        _useMouseLook = false;
    }

    void OnMouseMove(InputValue inputValue)
    {
        if(_disableMouseLookInput)
        {
            return;
        }

        Vector3 mouseScreenPosition = inputValue.Get<Vector2>();

        // Convert the mouse screen position to the position in the game world
        mouseScreenPosition.z = _mainCamera.nearClipPlane;
        Vector3 mouseWorldPoint = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        _mouseLookDirectionInput = mouseWorldPoint;

        // The player has moved their mouse, so use mouse look for the player's gun direction
        _useMouseLook = true;

        // Clear the lookInput (gamepad right thumbstick)
        _rightStickLookDirectionInput = Vector2.zero;
    }

    void OnFire(InputValue inputValue)
    {
        Debug.Log("OnFire");

        // TEMP
        //Weapon_Fire_Point_R
        //Weapon_Fire_Point_L
        //Transform projectileFirePoint = _projectileWeapon.Find("Weapon_Fire_Point_R");
        Transform projectileFirePoint;

        projectileFirePoint = _projectileWeaponFacingDirection == GameManager.SpriteFacingDirection.Right ? _weaponFirePointR : _weaponFirePointL;
        // switch(_projectileWeaponFacingDirection)
        // {
        //     case GameManager.SpriteFacingDirection.Left: _weaponFirePointL; break;
        // }

        // Always fire the first projectile straight from the weapon firepoint
        ProjectileBase newProjectile = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, projectileFirePoint.position, _projectileWeapon.rotation);
        newProjectile.Init(8.0f, 3.0f, 0.0f);

        // Handle Spread Gun
        //int totalBulletsSpawned = 1;
        //float angleMultiple = 1.0f;
    }

    void OnFireHold(InputValue inputValue)
    {
        var inputVal = inputValue.Get();
        Debug.Log("OnFireHold - inputValue: " + inputValue.Get().ToString());
    }
}
