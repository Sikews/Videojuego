using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Siki;

    // Update is called once per frame
    void Update()
    {
        if (Siki != null)
        {
            Vector3 position = transform.position;
            position.x = Siki.transform.position.x;
            transform.position = position;
        }
    }
}