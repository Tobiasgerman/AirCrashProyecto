using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PonerMascara : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject BotonMascara;     // Asignar el botón desde el Inspector
    public Animator MascaraAnimator;    // Asignar el Animator de la máscara

    [Header("Ajustes")]
    public Vector3 ajustePosicion = new Vector3(0f, 0.2f, 0.3f); // Offset para la cara

    private Transform jugador;
    private bool jugadorCerca = false;
    private bool colocada = false;
    private Button botonUI;

    void Start()
    {
        if (BotonMascara != null)
        {
            botonUI = BotonMascara.GetComponent<Button>();
            BotonMascara.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SimpleFPSController" && !colocada)
        {
            jugadorCerca = true;
            jugador = other.transform;

            if (BotonMascara != null)
            {
                BotonMascara.SetActive(true);
                if (botonUI != null)
                {
                    botonUI.onClick.RemoveListener(ColocarMascara);
                    botonUI.onClick.AddListener(ColocarMascara);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SimpleFPSController" && !colocada)
        {
            jugadorCerca = false;
            jugador = null;

            if (BotonMascara != null)
            {
                BotonMascara.SetActive(false);
                if (botonUI != null)
                    botonUI.onClick.RemoveListener(ColocarMascara);
            }
        }
    }

    void Update()
    {
        // También funciona con la tecla K
        if (jugadorCerca && !colocada && Input.GetKeyDown(KeyCode.K))
        {
            ColocarMascara();
        }

        // Si ya está colocada, seguir al jugador
        if (colocada && jugador != null)
        {
            Vector3 pos = jugador.position;
            transform.position = pos + ajustePosicion;
        }
    }

    void ColocarMascara()
    {
        if (colocada || jugador == null) return;

        colocada = true;

        // Ocultar botón
        if (BotonMascara != null)
        {
            if (botonUI != null) botonUI.onClick.RemoveListener(ColocarMascara);
            BotonMascara.SetActive(false);
        }

        // Desactivar rigidbody si existe
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        // Posicionar de inmediato en la cara del jugador
        transform.position = jugador.position + ajustePosicion;

        // 🔥 Activar la animación
        if (MascaraAnimator != null)
        {
            MascaraAnimator.SetTrigger("Colocar"); // asegúrate que en el Animator exista un Trigger llamado "Colocar"
        }
    }

    void OnDestroy()
    {
        if (botonUI != null)
            botonUI.onClick.RemoveListener(ColocarMascara);
    }
}
