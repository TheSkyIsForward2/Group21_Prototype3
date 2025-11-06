using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    
    void Start()
    {
        float x = Random.Range(-1.8f, 1.8f);
        float y = Random.Range(-1.8f, 1.8f);
        float z = 0;

        gameObject.transform.position = new Vector3(x, y, z);
    }

    
}
