using UnityEngine;
using UnityEngine.UI;

public class RotarMesa : MonoBehaviour
{
    [Header("Referencias")]
    public Button BotonMesa;          // asignar el componente Button (no el GameObject solo)
    public Animator MesaAnimator;     // asignar el Animator del objeto Mesa

    [Header("Opciones")]
    public string playerTag = "Player";
    public string triggerName = "PlayAnim";

    private bool jugadorCerca = false;
    private bool animacionUsada = false;
    private bool listenerAgregado = false;

    void Awake()
    {
        if (BotonMesa != null)
        {
            BotonMesa.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("[RotarMesa] Falta asignar 'BotonMesa' (Button) en el Inspector.");
        }

        if (MesaAnimator == null)
        {
            Debug.LogError("[RotarMesa] Falta asignar 'MesaAnimator' (Animator) en el Inspector.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (animacionUsada) return;
        if (other.CompareTag(playerTag))
        {
            jugadorCerca = true;
            Debug.Log("[RotarMesa] Player entró al trigger.");

            if (BotonMesa != null)
            {
                BotonMesa.gameObject.SetActive(true);

                if (!listenerAgregado)
                {
                    BotonMesa.onClick.AddListener(ActivarAnimacion);
                    listenerAgregado = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            jugadorCerca = false;
            Debug.Log("[RotarMesa] Player salió del trigger.");

            if (BotonMesa != null)
                BotonMesa.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!animacionUsada && jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            ActivarAnimacion();
        }
    }

    void ActivarAnimacion()
    {
        if (animacionUsada) return;

        if (MesaAnimator == null)
        {
            Debug.LogError("[RotarMesa] No hay Animator asignado a 'MesaAnimator'.");
            return;
        }

        Debug.Log("[RotarMesa] Disparando animación una sola vez.");
        MesaAnimator.ResetTrigger(triggerName); // opcional
        MesaAnimator.SetTrigger(triggerName);

        animacionUsada = true;

        if (BotonMesa != null)
        {
            BotonMesa.interactable = false;
            BotonMesa.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (BotonMesa != null && listenerAgregado)
        {
            BotonMesa.onClick.RemoveListener(ActivarAnimacion);
            listenerAgregado = false;
        }
    }
}
