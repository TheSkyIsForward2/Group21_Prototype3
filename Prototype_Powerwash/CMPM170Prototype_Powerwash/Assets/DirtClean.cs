using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DirtPainter : MonoBehaviour
{
    [Header("Mask Settings")]
    public int maskTextureWidth = 1024;
    public int maskTextureHeight = 1024;

    [Header("Brush Settings")]
<<<<<<< Updated upstream
    public int brushSize = 50; // You can change this in the Inspector
=======
    public int brushSize = 50; 

    // Public variable for the manager script to read
    public int cleanedPixelCount = 0;
>>>>>>> Stashed changes

    private Texture2D maskTexture;
    private Material carMaterial;
    private Color[] brushPixels;
    private bool needsUpdate = false;

    void Start()
    {
        // 1. Get the material instance for this specific object
        carMaterial = GetComponent<Renderer>().material;

        // 2. Create the mask texture
        maskTexture = new Texture2D(maskTextureWidth, maskTextureHeight);

        // 3. Fill the mask with WHITE (which means "fully dirty")
        Color[] whitePixels = new Color[maskTextureWidth * maskTextureHeight];
        for (int i = 0; i < whitePixels.Length; i++)
        {
            whitePixels[i] = Color.white;
        }
        maskTexture.SetPixels(whitePixels);
        maskTexture.Apply();

        // 4. Assign the new mask to the material's "_MaskTexture" property
        carMaterial.SetTexture("_MaskTexture", maskTexture);
<<<<<<< Updated upstream

        // 5. Create the brush stamp
=======
        
>>>>>>> Stashed changes
        CreateBrush();
    }

    // This function runs if you change the brushSize in the Inspector while playing
    void OnValidate()
    {
        // Re-create the brush if the size changes
        if (brushPixels == null || brushPixels.Length != brushSize * brushSize)
        {
            CreateBrush();
        }
    }

    void CreateBrush()
    {
        // This creates a solid black square brush "stamp"
        brushPixels = new Color[brushSize * brushSize];
        for (int i = 0; i < brushPixels.Length; i++)
        {
            brushPixels[i] = Color.black;
        }
    }

    void Update()
    {
        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits *this* object
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == this.gameObject)
            {
                // We hit! Get the UV coordinate of the hit point.
                Vector2 uv = hit.textureCoord;

                // --- TILING FIX ---
                // Wrap UV coordinates to prevent tiling issues
                uv.x = uv.x - Mathf.Floor(uv.x);
                uv.y = uv.y - Mathf.Floor(uv.y);

                // Convert the (0-1) UV coordinate to (0-width) pixel coordinates
                int x = (int)(uv.x * maskTextureWidth);
                int y = (int)(uv.y * maskTextureHeight);
<<<<<<< Updated upstream

                // Offset by half the brush size to center it
=======
                
>>>>>>> Stashed changes
                int paintX = x - (brushSize / 2);
                int paintY = y - (brushSize / 2);

                // --- EDGE CLEANING FIX ---
                // "Clamp" the brush position so it can't go off the edge
                // This allows the brush to paint all the way to the border
                paintX = Mathf.Clamp(paintX, 0, maskTextureWidth - brushSize);
                paintY = Mathf.Clamp(paintY, 0, maskTextureHeight - brushSize);

<<<<<<< Updated upstream
                // Now we can paint, guaranteed to be safely inside the bounds
                maskTexture.SetPixels(paintX, paintY, brushSize, brushSize, brushPixels);

                // Set a flag that the texture needs to be applied
=======
                // --- NEW LOGIC: Count pixels before painting them ---
                Color[] currentPixels = maskTexture.GetPixels(paintX, paintY, brushSize, brushSize);
                for (int i = 0; i < currentPixels.Length; i++)
                {
                    // If the pixel is currently white (r > 0.5), we are about to clean it.
                    if (currentPixels[i].r > 0.5f)
                    {
                        cleanedPixelCount++;
                    }
                }
                // ----------------------------------------------------
                
                maskTexture.SetPixels(paintX, paintY, brushSize, brushSize, brushPixels);
>>>>>>> Stashed changes
                needsUpdate = true;
            }
        }

        // We only call Apply() ONCE per frame, after all painting is done.
        // This is a crucial optimization.
        if (needsUpdate)
        {
            maskTexture.Apply();
            needsUpdate = false;
        }

        // Optional: Press 'R' to reset the dirt
        if (Input.GetKeyDown(KeyCode.R))
        {
<<<<<<< Updated upstream
            Start(); // Just re-run the start logic to reset everything
=======
            // Reset counter if we reset the car
            cleanedPixelCount = 0;
            Start(); 
>>>>>>> Stashed changes
        }
    }
}