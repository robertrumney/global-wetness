using UnityEngine;
using System.Collections;

public class GlobalWetness : MonoBehaviour
{
    [Header("Configuration")]
    // Layers to apply wetness
    public LayerMask layerMask; 
    public bool activated = true;

    // Default smoothness
    [Range(0f, 1f)]
    public float initialGlossiness = 1.0f; 

    // Target smoothness for transition
    [Range(0f, 1f)]
    public float targetGlossiness = 0.0f; 

    // Duration of smoothness transition in seconds
    public float interpolationDuration = 5.0f; 

    private void Awake()
    {
        if (!activated) return;

        // Apply initial smoothness on startup
        ApplyGlossiness(initialGlossiness); 
    }

    public void TriggerInterpolationToTarget(float newTargetGlossiness)
    {
        StartCoroutine(InterpolateGlossiness(newTargetGlossiness, interpolationDuration));
    }

    public void ResetGlossiness()
    {
        // Stop any ongoing interpolation
        StopAllCoroutines();

        // Revert to initial smoothness
        ApplyGlossiness(initialGlossiness); 
    }

    private void ApplyGlossiness(float glossiness)
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            if (((1 << renderer.gameObject.layer) & layerMask) != 0)
            {
                MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    renderer.GetPropertyBlock(propBlock, i);
                    propBlock.SetFloat("_Glossiness", glossiness);
                    renderer.SetPropertyBlock(propBlock, i);
                }
            }
        }
    }

    private IEnumerator InterpolateGlossiness(float targetGlossiness, float duration)
    {
        float time = 0;
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();

        while (time < duration)
        {
            float lerpValue = time / duration;
            float glossiness = Mathf.Lerp(initialGlossiness, targetGlossiness, lerpValue);

            foreach (MeshRenderer renderer in renderers)
            {
                if (((1 << renderer.gameObject.layer) & layerMask) != 0)
                {
                    MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
                    for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                    {
                        renderer.GetPropertyBlock(propBlock, i);
                        propBlock.SetFloat("_Glossiness", glossiness);
                        renderer.SetPropertyBlock(propBlock, i);
                    }
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final value is set
        ApplyGlossiness(targetGlossiness); 
    }
}
