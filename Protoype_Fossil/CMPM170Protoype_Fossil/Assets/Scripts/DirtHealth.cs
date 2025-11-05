using UnityEngine;

public class DirtHealth : MonoBehaviour
{
    [SerializeField] private float health = 40f;


    public void Scrub()
    {
        health -= 2;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
