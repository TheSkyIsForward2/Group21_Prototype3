using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtManager : MonoBehaviour
{
    private static DirtManager _instance;
    public static DirtManager Instance { get { return _instance; } }

    [Header("Spawn Settings")]
    public Vector2 spawnAreaSize;
    public int layers = 5;
    public float layerSpacing = 0.1f;
    public int particlesPerLayer = 50;

    [Header("Cleaning Settings")]
    public float cleanRadius = 0.5f;
    public float scrubDistanceModifier = 0.05f;

    private List<GameObject> allDirtParticles = new List<GameObject>();
    [SerializeField] private Camera cam;

    private Vector3 _scrubPosition;
    private float _lastScrubTime;
    private float _scrubInterval = 1f;
    
    
    private void Awake()
    {
        // Singleton enforcement
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CreateFossilDirt();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _scrubPosition = Input.mousePosition;
            Debug.Log(_scrubPosition);
        }
        
        if (Input.GetMouseButton(0))
        {
            CleanAtMouse();
        }
    }

    private void CreateFossilDirt()
    {
        for (int l = 0; l < layers; l++)
        {
            for (int i = 0; i < particlesPerLayer; i++)
            {
                GameObject t = SpawnDirt(l);
                if (t != null) allDirtParticles.Add(t);
            }
        }
    }

    private GameObject SpawnDirt(int layer)
    {
        GameObject t = ObjectPool.SharedInstance.GetPooledObject();
        if (t == null) return null;

        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float y = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        float z = -layer * layerSpacing;

        t.transform.position = new Vector3(x, y, z);
        t.SetActive(true);

        return t;
    }

    private void CleanAtMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane + 1f; // distance from camera
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        if (!(Vector3.Distance(worldPos, _scrubPosition) > scrubDistanceModifier))
            return;
        
        // Find all active particles in the radius
        for (int layer = layers - 1; layer >= 0; layer--) // top layer first
        {
            foreach (var dirt in allDirtParticles)
            {
                if (!dirt.activeInHierarchy) continue;
                float zLayer = -layer * layerSpacing;
                if (Mathf.Abs(dirt.transform.position.z - (zLayer)) > 0.01f) continue;

                float dist = Vector2.Distance(new Vector2(dirt.transform.position.x, dirt.transform.position.y),
                                              new Vector2(worldPos.x, worldPos.y));

                if (dist <= cleanRadius)
                {
                    dirt.GetComponent<DirtHealth>().Scrub();
                }
            }
        }

        if (_lastScrubTime + _scrubInterval <= Time.unscaledTime)
        {
            _lastScrubTime = Time.unscaledTime;
            _scrubPosition = mousePos;
        }
    }
}
