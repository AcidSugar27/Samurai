using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EnemyAI : MonoBehaviour
    {
        public float moveSpeed = 1f;
        public Transform leftPoint;
        public Transform rightPoint;

        private Rigidbody2D rigidbody;
        private bool movingRight = true;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            Move();
        }

        void Move()
        {
            if (movingRight)
            {
                rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
                if (transform.position.x >= rightPoint.position.x)
                {
                    movingRight = false;
                    Flip();
                }
            }
            else
            {
                rigidbody.velocity = new Vector2(-moveSpeed, rigidbody.velocity.y);
                if (transform.position.x <= leftPoint.position.x)
                {
                    movingRight = true;
                    Flip();
                }
            }
        }

        private void Flip()
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
    }
}
