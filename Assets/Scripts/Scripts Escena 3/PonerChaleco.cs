using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PonerChaleco : MonoBehaviour
{
    public GameObject BotonChaleco;
    public float velocidadColocacion = 5f;
    public Vector3 ajustePosicion = Vector3.zero;
    public Vector3 rotacionFinal = new Vector3(270f, 0f, 0f); // Rotación fija

    private bool jugadorCerca = false;
    private bool colocado = false;
    private Transform jugador;
    public Animator chaleco;

    void Start()
    {
        if (BotonChaleco != null)
            BotonChaleco.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SimpleFPSController" && !colocado)
        {
            jugadorCerca = true;
            jugador = other.transform;
            BotonChaleco.SetActive(true);
            BotonChaleco.GetComponent<Button>().onClick.AddListener(ColocarChaleco);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SimpleFPSController" && !colocado)
        {
            jugadorCerca = false;
            BotonChaleco.SetActive(false);
            BotonChaleco.GetComponent<Button>().onClick.RemoveListener(ColocarChaleco);
        }
    }

    void Update()
    {
        if (jugadorCerca && !colocado && Input.GetKeyDown(KeyCode.E))
        {
            ColocarChaleco();
        }
    }

    void ColocarChaleco()
    {
        if (jugador != null && !colocado)
        {
            StartCoroutine(MoverHaciaJugador());
        }
    }

    IEnumerator MoverHaciaJugador()
    {
        colocado = true;
        BotonChaleco.SetActive(false);
        chaleco.SetBool("Chaleco", true);
        yield return new WaitForSeconds(5f);

        if (GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().isKinematic = true;

        Vector3 posicionObjetivo = jugador.position + ajustePosicion;

        while (Vector3.Distance(transform.position, posicionObjetivo) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, posicionObjetivo, Time.deltaTime * velocidadColocacion);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotacionFinal), Time.deltaTime * velocidadColocacion);
            yield return null;
        }

        // Pegar al jugador y fijar rotación final
        transform.SetParent(jugador);
        transform.localPosition = ajustePosicion;
        transform.localRotation = Quaternion.Euler(rotacionFinal);
    }
}
