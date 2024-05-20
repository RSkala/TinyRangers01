using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBase : MonoBehaviour
{
    [Header("ProjectileWeaponBase Fields")]
    [SerializeField] ProjectileBase _projectilePrefab;
    //[SerializeField] AudioManager.SFX _projectileFireSfx = AudioManager.SFX.None;
    [SerializeField] float _projectileShotsPerSecond; // Rapid fire speed
    [SerializeField] Transform _firePoint; // RKS: MARKED FOR DEATH
    [SerializeField] Transform _weaponFirePointR; // Fire point of weapon when player is facing right
    [SerializeField] Transform _weaponFirePointL; // Fire point of weapon when player is facing left
    [SerializeField] float _projectileSpeed; // Base speed at which a projectile fired
    [SerializeField] float _projectileLifetime; // RKS: MARKED FOR DEATH
    [SerializeField] float _projectileDistance; // Base distance traveled by a projectile fired from this weapon
    [SerializeField] float _projectileDamage; // Base damage dealt by a projectile fired from this weapon

    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }
}
