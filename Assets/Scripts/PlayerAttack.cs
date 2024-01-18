using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastDash
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float attackCooldown;
        [SerializeField] private Transform firePoint;
        private Animator animator;
        private PlayerMovement playerMovement;
        private float cooldownTimer = Mathf.Infinity;
        private static readonly int AttackHash = Animator.StringToHash("attack");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.CanAttack())
            {
                Attack();
            }

            cooldownTimer += Time.deltaTime;
        }

        private void Attack()
        {
            animator.SetTrigger(AttackHash);
            cooldownTimer = 0;

            GameObject fireball = ObjectPooler.Instance.SpawnFromPool(Utils.FireballTag, firePoint.position, Quaternion.identity);
            fireball.GetComponent<FireballController>().InitFireball(Mathf.Sign(transform.localScale.x));
        }
    }
}