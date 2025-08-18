using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hebilla : MonoBehaviour
{
    void Start()
    {
        // Asegurar que tiene el tag correcto
        if (!CompareTag("Hebilla"))
        {
            tag = "Hebilla";
        }
    }
}
