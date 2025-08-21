using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucesRojas : MonoBehaviour
{
    public float tiempoEncendido = 1;
    public float tiempoApagado = 2;
    public Light luz;
    private int prendido = 1;
    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(Titilar());
    }
    void Update()
    {

    }

    // Update is called once per frame
    IEnumerator Titilar()
    {
        while (prendido == 1)
        {
            luz.enabled = true;
            yield return new WaitForSeconds(tiempoEncendido);
            luz.enabled = false;
            yield return new WaitForSeconds(tiempoApagado);
        }
    }
}   
