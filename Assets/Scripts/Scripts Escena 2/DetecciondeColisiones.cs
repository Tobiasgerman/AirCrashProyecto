using UnityEngine;
using UnityEngine.SceneManagement;

public class DetecciondeColisiones : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Contacto con: " + col.gameObject.name);

        if (col.gameObject.name == "SimpleFPSController")
        {
            Debug.Log("Recargando escena...");
            SceneManager.LoadScene("escenaTres");
        }
    }
}
