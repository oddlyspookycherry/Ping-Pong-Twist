using UnityEngine;

public class ImageEffect : MonoBehaviour
{
    [SerializeField]
    private Material effectMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, effectMaterial);
    }
}
