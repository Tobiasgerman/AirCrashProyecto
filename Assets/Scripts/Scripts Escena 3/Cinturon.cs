using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinturon : MonoBehaviour
{
    [SerializeField] private float maxHeight = 2f;
    [SerializeField] private GameObject targetObject; // El objeto que debe tocar para quedarse pegado
    [SerializeField] private float snapDistance = 0.5f; // Distancia para considerar que está tocando

    private bool isDragging = false;
    private bool isLocked = false; // Para evitar que se pueda arrastrar después de tocar el target
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 initialPosition;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            mainCamera = FindObjectOfType<Camera>();

        initialPosition = transform.position;
    }

    void Update()
    {
        if (isLocked) return; // Si está bloqueado, no hacer nada

        HandleMouseInput();

        if (isDragging)
        {
            DragObject();
            CheckTargetCollision();
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    StartDragging();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDragging();
        }
    }

    void StartDragging()
    {
        isDragging = true;

        // Calcular offset entre la posición del objeto y la posición del mouse en el mundo
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        offset = transform.position - mouseWorldPos;
    }

    void StopDragging()
    {
        isDragging = false;
    }

    void DragObject()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        Vector3 newPosition = mouseWorldPos + offset;

        // Mantener la Z original (no se mueve en profundidad)
        newPosition.z = initialPosition.z;

        // Limitar la altura máxima
        newPosition.y = Mathf.Clamp(newPosition.y, initialPosition.y, initialPosition.y + maxHeight);

        transform.position = newPosition;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Vector3.Distance(mainCamera.transform.position, transform.position);
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    void CheckTargetCollision()
    {
        if (targetObject == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);

        if (distanceToTarget <= snapDistance)
        {
            // Pegar al lado del objeto target
            SnapToTarget();

            // Bloquear para que no se pueda arrastrar más
            isLocked = true;
            isDragging = false;

            Debug.Log(gameObject.name + " se ha pegado a " + targetObject.name);
        }
    }

    void SnapToTarget()
    {
        // Calcular la posición al lado del objeto target
        Vector3 direction = (transform.position - targetObject.transform.position).normalized;

        // Obtener los bounds de ambos objetos para calcular la distancia correcta
        Bounds targetBounds = GetObjectBounds(targetObject);
        Bounds myBounds = GetObjectBounds(gameObject);

        float snapDistance = (targetBounds.size.x + myBounds.size.x) / 2f;

        Vector3 snapPosition = targetObject.transform.position + direction * snapDistance;
        snapPosition.y = transform.position.y; // Mantener la altura actual
        snapPosition.z = initialPosition.z; // Mantener la Z original

        transform.position = snapPosition;
    }

    Bounds GetObjectBounds(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
            return renderer.bounds;

        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
            return collider.bounds;

        // Si no tiene renderer ni collider, usar bounds por defecto
        return new Bounds(obj.transform.position, Vector3.one);
    }

    // Método público para resetear el objeto (opcional)
    public void ResetPosition()
    {
        transform.position = initialPosition;
        isLocked = false;
        isDragging = false;
    }

    // Para visualizar el rango de arrastre en el editor
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(initialPosition.x, initialPosition.y + maxHeight / 2f, initialPosition.z),
                               new Vector3(10f, maxHeight, 0.1f));
        }

        if (targetObject != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetObject.transform.position, snapDistance);
        }
    }
}