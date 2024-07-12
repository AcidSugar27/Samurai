using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;
        public int totalCoins = 3; // Número total de monedas en la escena
        public int lives = 3;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText;

        public Image[] hearts;
        public Sprite fullHeart;
        public Sprite emptyHeart;
        public GameObject gameOverPanel;
        public GameObject winPanel; // Panel para el logo de "Ganaste"

        // Nombre para guardar vidas en PlayerPrefs
        private string livesPlayerPrefsKey = "PlayerLives";

        void Start()
        {
            player = playerGameObject.GetComponent<PlayerController>(); // Obtener el PlayerController del GameObject del jugador
            gameOverPanel.SetActive(false);
            winPanel.SetActive(false); // Asegurarse de que el panel esté desactivado al inicio
            UpdateHearts();
            UpdateCoins(); // Inicializar el contador de monedas

            // Cargar vidas guardadas si existen
            if (PlayerPrefs.HasKey(livesPlayerPrefsKey))
            {
                lives = PlayerPrefs.GetInt(livesPlayerPrefsKey);
                UpdateHearts();
            }
        }

        void Update()
        {
            coinText.text = coinsCounter.ToString();
        }

        public void HandlePlayerDeath()
        {
            playerGameObject.SetActive(false);
            GameObject deathPlayer = Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
            deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);
            player.deathState = false;
            Invoke("HandleDeath", 1); // Reduce la vida y maneja la lógica después de 1 segundo
        }

        private void HandleDeath()
        {
            LoseLife();
            if (lives > 0)
            {
                playerGameObject.SetActive(true);
                player.Respawn();
            }
            else
            {
                GameOver();
            }
        }

        public void LoseLife()
        {
            if (lives > 0)
            {
                lives--;
                UpdateHearts();

                // Guardar vidas actualizadas en PlayerPrefs
                PlayerPrefs.SetInt(livesPlayerPrefsKey, lives);
                PlayerPrefs.Save();

                if (lives <= 0)
                {
                    GameOver();
                }
            }
        }

        void UpdateHearts()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < lives)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }
            }
        }

        void GameOver()
        {
            gameOverPanel.SetActive(true);
            StartCoroutine(RestartGame());
        }

        IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(3); // Espera 3 segundos antes de reiniciar
            SceneManager.LoadScene(0); // Cambia "0" al índice de la primera escena
        }

        public void UpdateCoins()
        {
            coinText.text = coinsCounter.ToString();
            // Lógica para comprobar si todos los coins han sido recolectados
            if (coinsCounter >= totalCoins)
            {
                if (IsLastScene())
                {
                    ShowWinPanel();
                }
                else
                {
                    LoadNextLevel();
                }
            }
        }

        private bool IsLastScene()
        {
            // Verifica si la escena actual es la última en el Build Settings
            return SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;
        }

        private void ShowWinPanel()
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
        }

        private void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
