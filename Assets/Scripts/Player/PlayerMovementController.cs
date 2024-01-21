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
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform warrior;
        [SerializeField] private float attackCooldown;
        
        [Networked] public Vector3 Velocity { get; set; }
        
        private InputController inputController;
        private NetworkRigidbody2D rb;
        private float cooldownTimer = Mathf.Infinity;
        
        private void Awake()
        {
            inputController = GetBehaviour<InputController>();
            rb = GetComponent<NetworkRigidbody2D>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput<InputData>(out var input))
            {   
                var pressed = input.GetButtonPressed(inputController.PrevButtons);
                inputController.PrevButtons = input.buttons;
                
                if (input.GetButton(InputButton.LEFT))
                {
                    rb.Rigidbody.velocity = new Vector2(-1 * 5, rb.Rigidbody.velocity.y);
                }
                else if (input.GetButton(InputButton.RIGHT))
                {
                    rb.Rigidbody.velocity = new Vector2(1 * 5, rb.Rigidbody.velocity.y);
                }
                if (input.GetButton(InputButton.JUMP))
                {
                    rb.Rigidbody.velocity = new Vector2(rb.Rigidbody.velocity.x, 7);
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