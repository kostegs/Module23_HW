using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BombVisibleRadius : MonoBehaviour
{
    private const float VisibleRadiusCoefficiency = 1.88f;

    private float _cachedExplosionRadius;
    private Queue<Color> _shimmerColors;
    private Material _material;

    private bool _isInitialized;
    private Coroutine _colorChangeCoroutine;

    public void Initialize(Color[] shimmerColors)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        _shimmerColors = new Queue<Color>(shimmerColors);

        _material = meshRenderer.material;
        
        Color materialColor = _shimmerColors.Dequeue();
        _material.color = materialColor; // start color
        _shimmerColors.Enqueue(materialColor);

        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized == false)
            return;

        if (_colorChangeCoroutine == null)
            _colorChangeCoroutine = StartCoroutine(ChangeVisibleSphereColor());
    }

    public void CheckVisibleRadius(float explosiveRadius)
    {
        if (explosiveRadius != _cachedExplosionRadius)
        {
            float scale = explosiveRadius * VisibleRadiusCoefficiency;

            transform.localScale =
                new Vector3(scale, scale, scale);
            _cachedExplosionRadius = explosiveRadius;
        }
    }   

    IEnumerator ChangeVisibleSphereColor()
    {
        float elapsedTime = 0;
        Color startColor = _material.color;
        Color targetColor = _shimmerColors.Dequeue();
        _shimmerColors.Enqueue(targetColor);
        float scaleTime = 1f;

        while (elapsedTime < scaleTime)
        {
            _material.color = Color.Lerp(startColor, targetColor, elapsedTime / scaleTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _colorChangeCoroutine = null;
    }
}