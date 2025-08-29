using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgarrarCaja : MonoBehaviour
{

    public GameObject jugador;   // El jugador
    public GameObject caja;      // La caja a agarrar
    public Button BotonCaja;     // Botón que aparece al acercarse
    public float distanciaMax = 3f; // Distancia máxima para que aparezca el botón y se pueda agarrar

    private bool agarrada = false;
    private Rigidbody rb;

    void Start()
    {
        rb = caja.GetComponent<Rigidbody>(); 
    }

    void Update()
    {
        float distancia = Vector3.Distance(jugador.transform.position, caja.transform.position);

        // Mostrar u ocultar el botón según la distancia
        if (BotonCaja != null)
        {
            BotonCaja.gameObject.SetActive(distancia <= distanciaMax);
        }

        // Agarrar o soltar con E si está cerca
        if (Input.GetKeyDown(KeyCode.E) && distancia <= distanciaMax)
        {
            agarrada = !agarrada;

            if (agarrada)
            {
                rb.isKinematic = true; // Desactivamos física para poder moverla
            }
            else
            {
                rb.isKinematic = false; // Activamos física para que caiga
            }
        }

        // Si está agarrada, la caja sigue al jugador
        if (agarrada)
        {
            Vector3 posicionFrente = jugador.transform.position + jugador.transform.forward * 2f + Vector3.up * 1f;
            caja.transform.position = posicionFrente;
        }
    }
}
