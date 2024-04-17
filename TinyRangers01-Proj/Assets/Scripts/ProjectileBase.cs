using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    float _moveSpeed;
    float _lifetimeSeconds;
    float _timeAlive;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _timeAlive = 0.0f;
    }

    public void Init(float moveSpeed, float lifetimeSeconds, float damage)
    {
        _moveSpeed = moveSpeed;
        _lifetimeSeconds = lifetimeSeconds;
    }

    void FixedUpdate()
    {
        // By default, move directly forward (2D up) direction
        // Use the "Up" vector as that is actually the forward vector in Unity 2D (Note: "forward" refers to the Z direction, i.e. in the camera facing direction)
        Vector2 movementDirection = _rigidbody2D.transform.up;
        Vector2 newPos = _rigidbody2D.position + movementDirection * _moveSpeed * Time.fixedDeltaTime;
        _rigidbody2D.MovePosition(newPos);

        // Destroy owning GameObject if time alive has exceeded the lifetime
        _timeAlive += Time.fixedDeltaTime;
        if(_timeAlive >= _lifetimeSeconds)
        {
            Destroy(gameObject);
        }
    }
}
