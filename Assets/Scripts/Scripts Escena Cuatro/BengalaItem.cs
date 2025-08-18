using UnityEngine;

public class BengalaItem : MonoBehaviour
{
    private void Start()
    {
        // Asegurarse de que tenga el tag correcto
        if (!CompareTag("Flare"))
            tag = "Flare";
    }
}

