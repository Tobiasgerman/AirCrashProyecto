using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotarMesa : MonoBehaviour
{
    public GameObject BotonMesa; // Referencia al botón UI
    private GameObject mesa;     // Referencia a la mesa

    private bool jugadorCerca = false;

    void Start()
    {
        // Buscar la mesa por nombre en la escena
        mesa = GameObject.Find("Mesa");

        // Ocultar el botón al inicio
        BotonMesa.SetActive(false);

        // Agregar escucha al botón
        BotonMesa.GetComponent<Button>().onClick.AddListener(RotarObjeto);
    }

    void Update()
    {
        // Si el jugador está cerca y presiona la tecla E (opcional), se puede mostrar el botón también
        if (jugadorCerca)
        {
            BotonMesa.SetActive(true);
        }
        else
        {
            BotonMesa.SetActive(false);
        }
    }

    private void RotarObjeto()
    {
        if (mesa != null)
        {
            mesa.transform.Rotate(0, 90, 0); // Rota 90° en el eje Y
            BotonMesa.SetActive(false);     // Oculta el botón luego de hacer clic
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Mesa")
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Mesa")
        {
            jugadorCerca = false;
        }
    }
}
