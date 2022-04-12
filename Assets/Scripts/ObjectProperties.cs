using System;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectProperties : MonoBehaviour
{
    // Class that holds the properties of an object in the scene
    
    private Rigidbody2D objectRigidbody;
    private Collider2D objectCollider;

    public bool isMovable;
    public bool isBreakable;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();
        objectRigidbody.isKinematic = !isMovable;
    }
    
    public void SetIsMovable(bool isMovable)
    {
        this.isMovable = isMovable;
        objectRigidbody.isKinematic = !isMovable;
    }
    
}