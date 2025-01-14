using UnityEngine;
using System.Collections;

public class GlobalWetness : MonoBehaviour
{
    [Header("Configuration")]
    public LayerMask layerMask; // Layers to apply wetness
    public bool activated = true;

    [Range(0f, 1f)]
    public float initialGlossiness = 1.0f; // Default smoothness
    [Range(0f, 1f)]
    public float targetGlossiness = 0.0f; // Target smoothness for transition
    public float interpolationDuration = 5.0f; // Duration of smoothness transition in seconds

    private void Awake()
    {
        if (!activated) return;
        ApplyGlossiness(initialGlossiness); // Apply initial smoothness on startup
    }

    public void TriggerInterpolationToTarget(float newTargetGlossiness)
    {
        StartCoroutine(InterpolateGlossiness(newTargetGlossiness, interpolationDuration));
    }

    public void ResetGlossiness()
    {
        StopAllCoroutines(); // Stop any ongoing interpolation
        ApplyGlossiness(initialGlossiness); // Revert to initial smoothness
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

        ApplyGlossiness(targetGlossiness); // Ensure final value is set
    }
}
