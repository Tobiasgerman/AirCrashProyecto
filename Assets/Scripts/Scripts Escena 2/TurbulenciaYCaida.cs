using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbulenciaYCaida : MonoBehaviour
{
    public float duracionTurbulencia = 10f; // Duración total de la turbulencia en segundos
    public float intensidad = 0.3f;         // Qué tan fuerte se mueve el objeto (vibración)

    Rigidbody rb;
    float tiempoRestante;
    bool turbulenciaActiva = false;

    Vector3 posicionBase; // Guarda la posición al comenzar la turbulencia

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public IEnumerator EsperarYComenzarTurbulencia()
    {
        yield return new WaitForSeconds(5f);

        // Guardamos la posición actual como base para vibrar
        posicionBase = transform.localPosition;

        tiempoRestante = duracionTurbulencia;
        turbulenciaActiva = true;
    }

    void Update()
    {
        if (!turbulenciaActiva)
            return;

        if (tiempoRestante > 0)
        {
            // Movimiento aleatorio de vibración (turbulencia)
            float offsetX = Random.Range(-intensidad, intensidad);
            float offsetY = Random.Range(-intensidad, intensidad);
            float offsetZ = Random.Range(-intensidad, intensidad);

            transform.localPosition = posicionBase + new Vector3(offsetX, offsetY, offsetZ);

            tiempoRestante -= Time.deltaTime;
        }
        else if (!rb.useGravity)
        {
            // Antes de activar la gravedad, dejamos el objeto donde esté
            rb.useGravity = true;
        }
    }
}
