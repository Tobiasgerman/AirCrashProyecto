using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbulenciaYCaida : MonoBehaviour
{
    public float duracionTurbulencia = 10f; // Duración total de la turbulencia en segundos
    public float intensidad = 0.3f;         // Qué tan fuerte se mueve el objeto (vibración)

    Vector3 posicionInicial;        // Guarda la posición original para regresar después de vibrar
    Rigidbody rb;                   // Referencia al Rigidbody del objeto
    float tiempoRestante;           // Tiempo que queda de turbulencia

    bool turbulenciaActiva = false; // Bandera para saber cuándo empieza la turbulencia

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        posicionInicial = transform.localPosition;

        // Iniciar la corutina que espera 5 segundos antes de activar la turbulencia
        StartCoroutine(EsperarYComenzarTurbulencia());
    }

    IEnumerator EsperarYComenzarTurbulencia()
    {
        yield return new WaitForSeconds(5f); // Espera 5 segundos
        tiempoRestante = duracionTurbulencia;
        turbulenciaActiva = true; //  empieza la turbulencia
    }

    void Update()
    {
        if (!turbulenciaActiva)
            return; // Si aún no es tiempo de comenzar, salir del Update

        if (tiempoRestante > 0)
        {
            // Movimiento aleatorio de vibración (turbulencia)
            float offsetX = Random.Range(-intensidad, intensidad);
            float offsetY = Random.Range(-intensidad, intensidad);
            float offsetZ = Random.Range(-intensidad, intensidad);

            transform.localPosition = posicionInicial + new Vector3(offsetX, offsetY, offsetZ);

            tiempoRestante -= Time.deltaTime;
        }
        else if (!rb.useGravity)
        {
            transform.localPosition = posicionInicial;
            rb.useGravity = true; // Activa la caída al terminar la turbulencia
        }
    }
}

