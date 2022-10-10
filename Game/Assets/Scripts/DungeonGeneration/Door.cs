using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom, leftw, rightw, topw, bottomw
    }
    public DoorType doorType;
    
    public GameObject doorCollider;

    public SpriteRenderer open;

    public SpriteRenderer closed;

}
