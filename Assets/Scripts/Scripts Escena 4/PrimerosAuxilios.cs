using UnityEngine;
using UnityEngine.UI;

public class PrimerosAuxilios : MonoBehaviour
{
    public Button BotonBotiquin;          // asignar el componente Button (no el GameObject solo)
    public Animator BotiquinAnimator;     // asignar el Animator del objeto Mesa

    private bool jugadorCerca = false;
    private bool animacionUsada = false;
    private bool listenerAgregado = false;
    public GameObject UI;

    private string playerTag = "Player";

    void Awake()
    {
        BotiquinAnimator.SetBool("PlayAnim", false);
    }
    
    void Start()
    {
        UI.SetActive(false);
    }

    private void Update()
    {
        // Si el jugador está en el rango Y se presiona la tecla 'B'.
        if (Input.GetKeyDown(KeyCode.B))
        {
            ActivarAnimacion();
        }
    }
  
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Botiquin"))
        {
            UI.SetActive(true);
            if (animacionUsada) return;
            if (other.CompareTag(playerTag))
            {
                jugadorCerca = true;
                Debug.Log("[RotarMesa] Player entró al trigger.");

                if (BotonBotiquin != null)
                {
                    BotonBotiquin.gameObject.SetActive(true);

                    if (!listenerAgregado)
                    {
                        BotonBotiquin.onClick.AddListener(ActivarAnimacion);
                        listenerAgregado = true;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Botiquin"))
        {
            UI.SetActive(false);
            jugadorCerca = false;
            Debug.Log("[RotarMesa] Player salió del trigger.");

            if (BotonBotiquin != null)
                BotonBotiquin.gameObject.SetActive(false);
        }
    }

    public void Boton()
    {
        Debug.Log("Click");
        if (!animacionUsada && jugadorCerca && Input.GetKeyDown(KeyCode.B))
        {
            ActivarAnimacion();
            Debug.Log("Animation");
        }
    }

    void ActivarAnimacion()
    {
        if (animacionUsada) return;

        if (BotiquinAnimator == null)
        {
            Debug.LogError("[RotarMesa] No hay Animator asignado a 'MesaAnimator'.");
            return;
        }
        UI.SetActive(false);
        Debug.Log("[RotarMesa] Disparando animación una sola vez.");
        BotiquinAnimator.SetBool("PlayAnim", true);

        animacionUsada = true;

        if (BotonBotiquin != null)
        {
            BotonBotiquin.interactable = false;
            BotonBotiquin.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (BotonBotiquin != null && listenerAgregado)
        {
            BotonBotiquin.onClick.RemoveListener(ActivarAnimacion);
            listenerAgregado = false;
        }
    }
}
