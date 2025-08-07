using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecciondeColisiones : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Contacto");

        if (col.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
    }
}

