using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBullet : MonoBehaviour
{
    void Update()
    {
        transform.Rotate (0,0,360*Time.deltaTime);
    }
}