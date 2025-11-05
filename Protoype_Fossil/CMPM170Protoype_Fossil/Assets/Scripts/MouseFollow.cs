using System;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        gameObject.transform.position = new Vector3(Input.mousePosition.x + 31, Input.mousePosition.y + 39, 0);
    }
}
