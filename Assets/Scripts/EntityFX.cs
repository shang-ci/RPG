using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;//要改成的材料
    [SerializeField] private float flashDuration;//闪光的时间
    private Material originalMat;//原来的材料

    [Header("Aliment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }


    //变透明
    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }



    public void IgniteFxFor(float _second)
    {
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    public void ShockFxFor(float _second)
    {
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    public void ChillFxFor(float _second)
    {
        InvokeRepeating("ChillColor", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    private void IgniteColorFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }


    private void ChillColor()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
}
