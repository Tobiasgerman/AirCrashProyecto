using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaidaDemorada : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Desactiva la gravedad al iniciar
        rb.useGravity = false;

        // Inicia la corutina que activa la gravedad después de 10 segundos
        StartCoroutine(ActivarCaida());
    }

    public System.Collections.IEnumerator ActivarCaida()
    {
        yield return new WaitForSeconds(5f); // Cae despues de 10 segundos

        rb.useGravity = true;
    }
}