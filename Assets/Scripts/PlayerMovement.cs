using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastDash
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask wallLayer;
        private Rigidbody2D body;
        private Animator animator;
        private BoxCollider2D boxCollider;
        private float wallJumpCooldown = 0f;
        
        private static readonly int RunHash = Animator.StringToHash("run");
        private static readonly int GroundedHash = Animator.StringToHash("grounded");
        private static readonly int JumpHash = Animator.StringToHash("jump");

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            
            // Flip player direction
            if (horizontalInput > 0.01f)
            {
                transform.localScale = Vector3.one;
            }
            else if(horizontalInput < -0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            
            animator.SetBool(RunHash, horizontalInput != 0);
            animator.SetBool(GroundedHash, IsGrounded());

            // Wall Jump Logic
            if (wallJumpCooldown < 0.2f)
            {
                if (Input.GetKey(KeyCode.Space) && IsGrounded())
                {
                    Jump();
                }
                
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

                if (OnWall() && !IsGrounded())
                {
                    body.gravityScale = 10;
                    body.velocity = Vector2.zero;
                }
                else
                {
                    body.gravityScale = 3;
                }
            }
            else
            {
                wallJumpCooldown += Time.deltaTime;
            }
        }

        private void Jump()
        {
            body.velocity = new Vector2(body.velocity.x, speed);
            animator.SetTrigger(JumpHash);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        private bool IsGrounded()
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
                Vector2.down, 0.1f, groundLayer);
            
            return raycastHit.collider != null;
        } 
        
        private bool OnWall()
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
                new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
            
            return raycastHit.collider != null;
        }
    }
}
