using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucesRojas : MonoBehaviour
{
    public float tiempoEncendido = 0.5f;
    public float tiempoApagado = 0.5f;
    public Light luz;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Titilar());
    }

    // Update is called once per frame
    IEnumerator Titilar()
    {
        luz.enabled = true;
        yield return new WaitForSeconds(tiempoEncendido);
        luz.enabled = false;
        yield return new WaitForSeconds(tiempoApagado);
    }
}
