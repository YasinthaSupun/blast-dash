using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastDash
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer renderer;
        private PlayerMovementController playerMovementController;

        private static readonly int RunHash = Animator.StringToHash("run");
        private static readonly int GroundedHash = Animator.StringToHash("grounded");
        private static readonly int JumpHash = Animator.StringToHash("jump");
        private static readonly int DieHash = Animator.StringToHash("die");
        
        private void Start()
        {
            playerMovementController = GetComponentInParent<PlayerMovementController>();
            animator = GetComponent<Animator>();
            renderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            if (playerMovementController.Velocity.x < -.1f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                animator.SetBool(RunHash, true);
            }
            else if (playerMovementController.Velocity.x > .1f)
            {
                transform.localScale = Vector3.one;
                animator.SetBool(RunHash, true);
            }
            else
            {
                animator.SetBool(RunHash, false);
            }
        }

        public void PlayerDeath()
        {
            animator.SetBool(RunHash, true);
            animator.SetTrigger(DieHash);
        }

        public void Respawn()
        {
            animator.SetBool(RunHash, false);
        }
    }
}