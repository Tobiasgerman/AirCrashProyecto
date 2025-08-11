using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PonerChaleco : MonoBehaviour
{
    public GameObject BotonChaleco; // botón en la UI
    public float anguloRotacion = 90f; // grados a rotar

    private bool jugadorCerca = false;

    void Start()
    {
        if (BotonChaleco != null)
            BotonChaleco.SetActive(false); // ocultar el botón al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SimpleFPSController")
        {
            jugadorCerca = true;
            BotonChaleco.SetActive(true);
            BotonChaleco.GetComponent<Button>().onClick.AddListener(RotarObjeto);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SimpleFPSController")
        {
            jugadorCerca = false;
            BotonChaleco.SetActive(false);
            BotonChaleco.GetComponent<Button>().onClick.RemoveListener(RotarObjeto);
        }
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            RotarObjeto();
        }
    }

    void RotarObjeto()
    {
        if (jugadorCerca)
        {
            transform.Rotate(anguloRotacion, 0f, 0f);
        }
    }
}

