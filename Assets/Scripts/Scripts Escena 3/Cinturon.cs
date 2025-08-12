using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinturon : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 originalPosition;
    private bool isAttached = false;
    private Vector3 dragOffset;

    [SerializeField] private Transform hebilla;
    [SerializeField] private float snapDistance = 0.5f;
    [SerializeField] private Vector3 attachOffset = Vector3.zero;
    [SerializeField] private bool lockXAxis = false;
    [SerializeField] private bool lockYAxis = false;
    [SerializeField] private bool lockZAxis = false;

    void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
            gameObject.AddComponent<BoxCollider>();
    }

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (!isAttached)
        {
            isDragging = true;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            dragOffset = transform.position - Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    void OnMouseDrag()
    {
        if (isDragging && !isAttached)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePos) + dragOffset;

            Vector3 newPosition = transform.position;

            if (!lockXAxis) newPosition.x = targetPosition.x;
            if (!lockYAxis) newPosition.y = targetPosition.y;
            if (!lockZAxis) newPosition.z = targetPosition.z;

            transform.position = newPosition;
        }
    }

    void OnMouseUp()
    {
        if (isDragging && !isAttached)
        {
            isDragging = false;

            if (hebilla != null)
            {
                float distance = Vector3.Distance(transform.position, hebilla.position);

                if (distance <= snapDistance)
                {
                    Vector3 attachPosition = hebilla.position + attachOffset;
                    transform.position = attachPosition;
                    isAttached = true;

                    Rigidbody rb = GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }
        }
    }

    public void Desabrochar()
    {
        isAttached = false;
        transform.position = originalPosition;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}