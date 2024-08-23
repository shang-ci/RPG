using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;
    }

    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    private void CancelRedBlik()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
