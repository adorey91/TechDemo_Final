using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public int X = 0;
    public int Y = 0;
    public int Z = 0;

    void Update()
    {
        transform.Rotate(new Vector3(X, Y, Z) * Time.deltaTime);
    }
}
