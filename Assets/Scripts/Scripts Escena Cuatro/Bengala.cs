using UnityEngine;

public class Bengala : MonoBehaviour
{
    [Header("Flare Settings")]
    public GameObject flarePrefab;
    public GameObject flareProjectilePrefab; // Proyectil que se dispara
    public Transform firePoint;
    public float flareForce = 2000f; // Aumentado para más velocidad y distancia
    public float pickupRange = 2f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;

    [Header("UI")]
    public GameObject pickupPrompt;

    private Camera playerCamera;
    private bool isHoldingFlare = false;
    private GameObject currentFlare;
    private Rigidbody flareRigidbody;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
            playerCamera = FindObjectOfType<Camera>();

        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);

        // Debug para verificar configuración
        if (firePoint == null)
        {
            Debug.LogError("FlareController: firePoint no asignado! Crea un GameObject hijo vacío y asígnalo.");
        }
    }

    void Update()
    {
        HandleFlareInteraction();
        HandleFlareFiring();
    }

    void HandleFlareInteraction()
    {
        if (!isHoldingFlare)
        {
            // Buscar bengalas cercanas
            GameObject nearbyFlare = FindNearbyFlare();

            if (nearbyFlare != null)
            {
                ShowPickupPrompt(true);

                if (Input.GetKeyDown(pickupKey))
                {
                    PickupFlare(nearbyFlare);
                }
            }
            else
            {
                ShowPickupPrompt(false);
            }
        }
        else
        {
            // Soltar bengala
            if (Input.GetKeyDown(dropKey))
            {
                DropFlare();
            }
        }
    }

    void HandleFlareFiring()
    {
        if (isHoldingFlare && Input.GetMouseButtonDown(0))
        {
            FireFlare();
        }
    }

    GameObject FindNearbyFlare()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, pickupRange);

        Debug.Log($"Buscando bengalas... Objetos encontrados: {nearbyObjects.Length}");

        foreach (Collider col in nearbyObjects)
        {
            Debug.Log($"Objeto encontrado: {col.name}, Tag: {col.tag}");

            if (col.CompareTag("Flare") && col.GetComponent<BengalaItem>() != null)
            {
                Debug.Log($"¡Bengala encontrada: {col.name}!");
                return col.gameObject;
            }
        }

        return null;
    }

    void PickupFlare(GameObject flare)
    {
        currentFlare = flare;
        flareRigidbody = flare.GetComponent<Rigidbody>();

        // Configurar la bengala como equipada
        if (flareRigidbody != null)
        {
            flareRigidbody.isKinematic = true;
        }

        // Posicionar bengala en la mano del jugador
        flare.transform.SetParent(firePoint);
        flare.transform.localPosition = Vector3.zero;
        flare.transform.localRotation = Quaternion.identity;

        // Desactivar el collider mientras está equipada
        Collider flareCollider = flare.GetComponent<Collider>();
        if (flareCollider != null)
            flareCollider.enabled = false;

        isHoldingFlare = true;
        ShowPickupPrompt(false);
    }

    void DropFlare()
    {
        if (currentFlare == null) return;

        // Liberar la bengala
        currentFlare.transform.SetParent(null);

        if (flareRigidbody != null)
        {
            flareRigidbody.isKinematic = false;
        }

        // Reactivar el collider
        Collider flareCollider = currentFlare.GetComponent<Collider>();
        if (flareCollider != null)
            flareCollider.enabled = true;

        // Posicionar ligeramente adelante del jugador
        currentFlare.transform.position = transform.position + transform.forward * 1f;

        currentFlare = null;
        flareRigidbody = null;
        isHoldingFlare = false;
    }

    void FireFlare()
    {
        if (currentFlare == null || flareProjectilePrefab == null) return;

        // Crear un nuevo proyectil de bengala
        GameObject projectile = Instantiate(flareProjectilePrefab, firePoint.position, firePoint.rotation);

        // Configurar la dirección de disparo
        Vector3 fireDirection = playerCamera.transform.forward;

        // Aplicar fuerza al proyectil
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.AddForce(fireDirection * flareForce);
        }

        // Activar efectos del proyectil
        BengalaProyectil flareProjectile = projectile.GetComponent<BengalaProyectil>();
        if (flareProjectile != null)
        {
            flareProjectile.Ignite();
        }

        Debug.Log("¡Bengala disparada!");

        // La bengala (arma) sigue en la mano - NO se resetea currentFlare
    }

    void ShowPickupPrompt(bool show)
    {
        if (pickupPrompt != null)
            pickupPrompt.SetActive(show);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
