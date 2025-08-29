using UnityEngine;
using UnityEngine.UI;

public class RotarMesa : MonoBehaviour
{
    public Button BotonMesa;          // asignar el componente Button (no el GameObject solo)
    public Animator MesaAnimator;     // asignar el Animator del objeto Mesa
    
    public string playerTag = "Player";
    public string triggerName = "PlayAnim";

    private bool jugadorCerca = false;
    private bool animacionUsada = false;
    private bool listenerAgregado = false;

    void Awake()
    {
        MesaAnimator.SetBool("PlayAnim", false);
    }

    private void Update()
    {
        // Si el jugador está en el rango Y se presiona la tecla 'E'.
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!animacionUsada) ActivarAnimacion();
            else ActivarAnimacionCerrar();
        }
    }
  
    void OnTriggerEnter(Collider other)
    {
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

    public void Boton()
    {
        Debug.Log("Click");
        if (!animacionUsada && jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            ActivarAnimacion();
            Debug.Log("Animation");
        }
    }

    void ActivarAnimacion()
    { 
        if (MesaAnimator == null)
        {
            Debug.LogError("[RotarMesa] No hay Animator asignado a 'MesaAnimator'.");
            return;
        }

        Debug.Log("[RotarMesa] Disparando animación una sola vez.");
        MesaAnimator.SetBool("PlayAnim", true);

        animacionUsada = true;

        if (BotonMesa != null)
        {
            BotonMesa.interactable = false;
            BotonMesa.gameObject.SetActive(false);
        }
    }

    void ActivarAnimacionCerrar()
    {
        if (MesaAnimator == null)
        {
            Debug.LogError("[RotarMesa] No hay Animator asignado a 'MesaAnimator'.");
            return;
        }

        Debug.Log("[RotarMesa] Disparando animación una sola vez.");
        MesaAnimator.SetBool("PlayAnim", false);

        animacionUsada = false;

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
