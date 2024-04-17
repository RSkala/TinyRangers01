using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field:SerializeField] public ProjectileBase ProjectilePrefab { get; private set; }

    public static GameManager Instance { get; private set; }

    public enum SpriteFacingDirection
    {
        Invalid,
        Right,
        Left
    }
    
    void Start()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    void Update()
    {
        
    }
}
