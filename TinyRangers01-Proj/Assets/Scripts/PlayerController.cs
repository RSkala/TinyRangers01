using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] Transform _projectileWeapon;

    // Components
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    Animator _animator;

    // Player input values
    Vector2 _movementDirectionInput;
    Vector2 _rightStickLookDirectionInput;
    Vector2 _mouseLookDirectionInput;

    Camera _mainCamera;
    bool _useMouseLook;

    // Temp weapon sprite value
    SpriteRenderer _weaponSpriteRenderer; // TEMP

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

        _mainCamera = Camera.main;
        _weaponSpriteRenderer = _projectileWeapon.GetComponentInChildren<SpriteRenderer>();

        // Sprite faces right by default
        _playerFacingDirection = GameManager.SpriteFacingDirection.Right;
        _projectileWeaponFacingDirection = GameManager.SpriteFacingDirection.Right;

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
        
    }

    void FixedUpdate()
    {
        UpdatePlayerAnimStates();

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
                Vector2 dirPlayerToMousePos = (_mouseLookDirectionInput - _rigidbody2D.position).normalized; // TODO: Try weapon point for better accuracy

                Vector3 cross = Vector3.Cross(Vector2.up, dirPlayerToMousePos);
                float flipValue = cross.z < 0.0f ? -1.0f : 1.0f;
                float rotateAngle = Vector2.Angle(Vector2.up, dirPlayerToMousePos) * flipValue;
                //_projectileWeaponRotationPoint.rotation = Quaternion.Euler(0.0f, 0.0f, rotateAngle);
                _projectileWeapon.rotation = Quaternion.Euler(0.0f, 0.0f, rotateAngle);

                // Update the gun facing based on the player's mouse cursor direction
                _projectileWeaponFacingDirection = cross.z >= 0.0f ? GameManager.SpriteFacingDirection.Left : GameManager.SpriteFacingDirection.Right;
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
        Vector3 mouseScreenPosition = inputValue.Get<Vector2>();

        // Convert the mouse screen position to the position in the game world
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
    }
}
