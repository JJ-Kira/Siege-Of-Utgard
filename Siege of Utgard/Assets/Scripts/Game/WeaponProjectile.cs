using System;
using Entities.Enemies;
using UltimateXR.Mechanics.Weapons;
using UnityEngine;

namespace Game {
    public class WeaponProjectile : MonoBehaviour {
        [SerializeField] private int damage = 10;     // Amount of damage the projectile will deal to enemies

        [SerializeField] private UxrFirearmWeapon weapon;

        private void Update()
        {
            if (weapon.CurrentShotTarget.collider != null)
            {
                Shoot(weapon.CurrentShotTarget.collider);
            }
        }

        private void Shoot(Collider other) {
            Enemy enemy = other.GetComponent<Enemy>();
            Defender.Instance.DealDamage(damage, enemy);
        }
    }
}