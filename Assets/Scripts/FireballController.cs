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
        private float lifetime;

        private Animator animator;
        private BoxCollider2D boxCollider;
        private static readonly int ExplodeHash = Animator.StringToHash("explode");

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

            lifetime += Time.deltaTime;
            if (lifetime > 5)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            hit = true;
            boxCollider.enabled = false;
            animator.SetTrigger(ExplodeHash);
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
            lifetime = 0;
        }

        private void DeactivateFireball()
        {
            ObjectPooler.Instance.BackToPool(Utils.FireballTag, this.gameObject);
        }
    }
}