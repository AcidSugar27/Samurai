using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;
        private bool isDead = false;

        private bool isGrounded;
        public Transform groundCheck;

        private new Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        // Variables para guardar el estado inicial del jugador
        private Vector3 initialPosition;
        private Vector3 initialScale;
        private Quaternion initialRotation;

        public AudioSource audioSource;
        public AudioClip stepSound;
        private bool isWalking = false;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            // Guardar el estado inicial del jugador
            initialPosition = transform.position;
            initialScale = transform.localScale;
            initialRotation = transform.rotation;
        }

        private void FixedUpdate()
        {
            if (!isDead)
            {
                CheckGround();
            }
        }

        void Update()
        {
            if (isDead) return;

            if (Input.GetButton("Horizontal"))
            {
                moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                animator.SetInteger("playerState", 1); // Turn on run animation

                if (!isWalking)
                {
                    isWalking = true;
                    StartCoroutine(PlayStepSound());
                }
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
                isWalking = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }

            if (!isGrounded) animator.SetInteger("playerState", 2); // Turn on jump animation

            if (facingRight == true && moveInput > 0)
            {
                Flip();
            }
            else if (facingRight == false && moveInput < 0)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (isDead) return;

            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
                gameManager.HandlePlayerDeath();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isDead) return;

            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
                gameManager.UpdateCoins();
            }
        }

        public void ResetPlayer()
        {
            // Restaurar el estado inicial del jugador
            transform.position = initialPosition;
            transform.localScale = initialScale;
            transform.rotation = initialRotation;
            rigidbody.velocity = Vector2.zero; // Reiniciar la velocidad
            animator.SetInteger("playerState", 0); // Configurar la animación en idle
            isDead = false;
        }

        public void Die()
        {
            isDead = true;
            rigidbody.velocity = Vector2.zero;
            // Desactivar colisiones
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }
            // Opcional: desactivar renderers si deseas hacer invisible al jugador al morir
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }

        public void Respawn()
        {
            // Reactivar colisiones
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = true;
            }
            // Reactivar renderers si estaban desactivados
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }
            ResetPlayer();
        }

        private IEnumerator PlayStepSound()
        {
            while (isWalking)
            {
                audioSource.PlayOneShot(stepSound);
                yield return new WaitForSeconds(0.5f); // Adjust the interval between steps as needed
            }
        }
    }
}
