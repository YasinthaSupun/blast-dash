using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastDash
{
    public class FireballController : MonoBehaviour
    {
        [SerializeField] private float speed;
        private float direction;
        private bool hit;

        private Animator animator;
        private BoxCollider2D boxCollider;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (hit)
            {
                return;
            }
            float movementSpeed = speed * Time.deltaTime * direction;
            transform.Translate(movementSpeed, 0, 0);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            hit = true;
            boxCollider.enabled = false;
            animator.SetTrigger("explode");
        }

        public void InitFireball(float fireDirection)
        {
            this.direction = fireDirection;
            hit = false;
            boxCollider.enabled = true;

            float localScaleX = transform.localScale.x;
            if (Mathf.Sign(localScaleX) != direction)
            {
                localScaleX = -localScaleX;
            }

            transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        }

        private void DeactivateFireball()
        {
            ObjectPooler.Instance.BackToPool("Fireball", this.gameObject);
        }
    }
}