using UnityEngine;
using UnityEngine.UI;

public class RotarMesa : MonoBehaviour
{
    public GameObject BotonMesa; // botón en la UI
    public float anguloRotacion = 90f; // grados a rotar

    private bool jugadorCerca = false;

    void Start()
    {
        if (BotonMesa != null)
            BotonMesa.SetActive(false); // ocultar el botón al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SimpleFPSController")
        {
            jugadorCerca = true;
            BotonMesa.SetActive(true);
            BotonMesa.GetComponent<Button>().onClick.AddListener(RotarObjeto);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SimpleFPSController")
        {
            jugadorCerca = false;
            BotonMesa.SetActive(false);
            BotonMesa.GetComponent<Button>().onClick.RemoveListener(RotarObjeto);
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
