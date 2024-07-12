using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text pressEnterText; // Referencia al texto "Press Enter to Play"

    void Start()
    {
        // Puedes hacer cualquier configuración inicial aquí si es necesario
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Detecta la tecla Enter
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Carga la siguiente escena en el índice
    }
}
