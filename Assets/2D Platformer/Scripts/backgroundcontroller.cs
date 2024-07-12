using UnityEngine;

public class BackgroundSetup : MonoBehaviour
{
    public Sprite backgroundSprite;

    void Start()
    {
        GameObject background = new GameObject("Background");
        SpriteRenderer sr = background.AddComponent<SpriteRenderer>();
        sr.sprite = backgroundSprite;
        sr.sortingOrder = -1; // Asegura que el fondo esté detrás de otros elementos

        // Ajustar la posición y escala si es necesario
        background.transform.position = new Vector3(0, 0, 10);
        background.transform.localScale = new Vector3(10, 10, 1); // Cambia según sea necesario
    }
}
