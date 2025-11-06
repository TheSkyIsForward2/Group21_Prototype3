using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DirtPainter : MonoBehaviour
{
    [Header("Mask Settings")]
    public int maskTextureWidth = 1024;
    public int maskTextureHeight = 1024;

    [Header("Brush Settings")]
    public int brushSize = 50;

    private Texture2D maskTexture;
    private Material carMaterial;
    private Color[] brushPixels;
    private bool needsUpdate = false;

    void Start()
    {
        carMaterial = GetComponent<Renderer>().material;

        maskTexture = new Texture2D(maskTextureWidth, maskTextureHeight);

        Color[] whitePixels = new Color[maskTextureWidth * maskTextureHeight];
        for (int i = 0; i < whitePixels.Length; i++)
        {
            whitePixels[i] = Color.white;
        }
        maskTexture.SetPixels(whitePixels);
        maskTexture.Apply();

        carMaterial.SetTexture("_MaskTexture", maskTexture);

        CreateBrush();
    }

    void OnValidate()
    {
        if (brushPixels == null || brushPixels.Length != brushSize * brushSize)
        {
            CreateBrush();
        }
    }

    void CreateBrush()
    {
        brushPixels = new Color[brushSize * brushSize];
        for (int i = 0; i < brushPixels.Length; i++)
        {
            brushPixels[i] = Color.black;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == this.gameObject)
            {
                Vector2 uv = hit.textureCoord;

                uv.x = uv.x - Mathf.Floor(uv.x);
                uv.y = uv.y - Mathf.Floor(uv.y);

                int x = (int)(uv.x * maskTextureWidth);
                int y = (int)(uv.y * maskTextureHeight);

                int paintX = x - (brushSize / 2);
                int paintY = y - (brushSize / 2);

                paintX = Mathf.Clamp(paintX, 0, maskTextureWidth - brushSize);
                paintY = Mathf.Clamp(paintY, 0, maskTextureHeight - brushSize);

                maskTexture.SetPixels(paintX, paintY, brushSize, brushSize, brushPixels);

                needsUpdate = true;
            }
        }

        if (needsUpdate)
        {
            maskTexture.Apply();
            needsUpdate = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Start();
        }
    }
}