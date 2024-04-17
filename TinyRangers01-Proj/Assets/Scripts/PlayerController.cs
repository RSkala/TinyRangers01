using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;

    // Components
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    Animator _animator;

    // Player input values
    Vector2 _movementDirectionInput;

    // Facing
    GameManager.SpriteFacingDirection _playerFacingDirection = GameManager.SpriteFacingDirection.Invalid;

    // Animation
    int animParam_IsMoving = Animator.StringToHash("IsMoving");

    void Start()
    {
        // Components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        // Sprite faces right by default
        _playerFacingDirection = GameManager.SpriteFacingDirection.Right;

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

        // Update player facing direction
        UpdatePlayerSpriteFacingDirection();
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
        //Debug.Log("_playerFacingDirection: " + _playerFacingDirection);
        switch(_playerFacingDirection)
        {
            case GameManager.SpriteFacingDirection.Right: _spriteRenderer.flipX = false; break;
            case GameManager.SpriteFacingDirection.Left: _spriteRenderer.flipX = true; break;
            default: break;
        }
    }

    void OnMove(InputValue inputValue)
    {
        _movementDirectionInput = inputValue.Get<Vector2>();
    }
}
