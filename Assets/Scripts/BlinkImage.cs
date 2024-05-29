using System.Collections;
using UnityEngine;

public class BlinkColor : MonoBehaviour
{
    public SpriteRenderer spriteToBlink; // El SpriteRenderer que quieres hacer parpadear
    public Color blinkColor = Color.red; // El color a parpadear
    public float blinkTime = 1f; // El tiempo en segundos que quieres que parpadee
    public float totalBlinkTime = 5f; // El tiempo total de parpadeo

    private Color originalColor; // El color original del SpriteRenderer

    private void Start()
    {
        // Guarda el color original del SpriteRenderer
        originalColor = spriteToBlink.color;
    }

    public IEnumerator Blink() // Haz esta función pública
    {
        float endTime = Time.time + totalBlinkTime;

        while (Time.time < endTime)
        {
            // Alterna el color del SpriteRenderer
            spriteToBlink.color = spriteToBlink.color == originalColor ? blinkColor : originalColor;

            // Espera por el tiempo especificado antes de continuar
            yield return new WaitForSeconds(blinkTime);
        }

        // Asegúrate de que el SpriteRenderer vuelva a su color original al final
        spriteToBlink.color = originalColor;
    }
}
