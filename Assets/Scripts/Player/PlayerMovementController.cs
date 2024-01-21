using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace BlastDash
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpPower;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform warrior;
        [SerializeField] private float attackCooldown;
        [SerializeField] private LayerMask groundLayer;
        
        [Networked] public Vector3 Velocity { get; set; }
        
        private InputController inputController;
        private NetworkRigidbody2D rb;
        private float cooldownTimer = Mathf.Infinity;
        private BoxCollider2D boxCollider;
        
        private void Awake()
        {
            inputController = GetBehaviour<InputController>();
            rb = GetComponent<NetworkRigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput<InputData>(out var input))
            {   
                var pressed = input.GetButtonPressed(inputController.PrevButtons);
                inputController.PrevButtons = input.buttons;
                
                if (input.GetButton(InputButton.LEFT))
                {
                    rb.Rigidbody.velocity = new Vector2(-1 * moveSpeed, rb.Rigidbody.velocity.y);
                }
                else if (input.GetButton(InputButton.RIGHT))
                {
                    rb.Rigidbody.velocity = new Vector2(1 * moveSpeed, rb.Rigidbody.velocity.y);
                }
                if (input.GetButton(InputButton.JUMP) && IsGrounded())
                {
                    rb.Rigidbody.velocity = new Vector2(rb.Rigidbody.velocity.x, jumpPower);
                }
                if (input.GetButton(InputButton.SHOOT) && cooldownTimer > attackCooldown)
                {
                    FireAttack();
                }
            }
            
            Velocity = rb.Rigidbody.velocity;
        }

        private void Update()
        {
            cooldownTimer += Time.deltaTime;
        }
        
        private bool IsGrounded()
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
                Vector2.down, 0.1f, groundLayer);
            
            return raycastHit.collider != null;
        } 

        private void FireAttack()
        {
            if (Object.HasInputAuthority)
            {
                RPC_FireballAttack(spawnPoint.position);
            }
            
            cooldownTimer = 0;
        }
        
        [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
        private void RPC_FireballAttack(Vector3 fireballSpawnPoint)
        {
            GameObject fireball = ObjectPooler.Instance.SpawnFromPool(Utils.FireballTag, fireballSpawnPoint, Quaternion.identity);
            fireball.GetComponent<FireballController>().InitFireball(Mathf.Sign(warrior.localScale.x));
        }
    }
}