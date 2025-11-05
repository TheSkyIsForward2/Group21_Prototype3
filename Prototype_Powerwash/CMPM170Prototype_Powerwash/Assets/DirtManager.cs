using UnityEngine;
using UnityEngine.UI;

public class DirtManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider progressBar;
    public Image fillImage;

    [Header("Color Settings")]
    public Color startColor = Color.red;
    public Color midColor = Color.yellow; // --- NEW Explicit Yellow ---
    public Color endColor = Color.green;

    private DirtPainter[] painters;
    private long totalPixels = 0;

    void Start()
    {
        painters = GetComponentsInChildren<DirtPainter>();
        foreach (DirtPainter painter in painters)
        {
            totalPixels += (long)painter.maskTextureWidth * painter.maskTextureHeight;
        }
    }

    void Update()
    {
        if (painters == null || painters.Length == 0) return;

        long totalCleaned = 0;
        foreach (DirtPainter painter in painters)
        {
            totalCleaned += painter.cleanedPixelCount;
        }

        float progress = (float)totalCleaned / totalPixels;

        if (progressBar != null)
        {
            progressBar.value = progress;
        }

        if (fillImage != null)
        {
            // --- NEW 3-COLOR BLEND LOGIC ---
            if (progress < 0.5f)
            {
                // First half: Blend from Red to Yellow
                // We multiply progress by 2 to go from 0-0.5 up to 0-1 for the Lerp
                fillImage.color = Color.Lerp(startColor, midColor, progress * 2f);
            }
            else
            {
                // Second half: Blend from Yellow to Green
                // We map 0.5-1.0 to 0-1 for the Lerp
                fillImage.color = Color.Lerp(midColor, endColor, (progress - 0.5f) * 2f);
            }
        }
    }
}