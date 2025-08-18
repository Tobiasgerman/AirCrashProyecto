using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinturon : MonoBehaviour
{
    private bool puedeMoverse = true;
    private bool arrastrando = false;
    private Vector3 offsetMouse;
    private Camera camara;

    void Start()
    {
        camara = Camera.main;
    }

    void OnMouseDown()
    {
        if (!puedeMoverse) return;

        arrastrando = true;
        Vector3 posicionMundo = camara.ScreenToWorldPoint(Input.mousePosition);
        offsetMouse = transform.position - posicionMundo;
    }

    void OnMouseDrag()
    {
        if (!puedeMoverse || !arrastrando) return;

        Vector3 posicionMouse = camara.ScreenToWorldPoint(Input.mousePosition);
        posicionMouse.z = transform.position.z; 

        Vector3 nuevaPosicion = transform.position;
        nuevaPosicion.x = posicionMouse.x + offsetMouse.x;
        transform.position = nuevaPosicion;
    }

    void OnMouseUp()
    {
        arrastrando = false;
    }

    public void BloquearMovimiento()
    {
        puedeMoverse = false;
        arrastrando = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hebilla"))
        {
            BloquearMovimiento();

            Vector3 posicionHebilla = other.transform.position;
            posicionHebilla.x = other.transform.position.x;
            transform.position = posicionHebilla;
        }
    }
}