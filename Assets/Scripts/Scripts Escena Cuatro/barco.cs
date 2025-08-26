using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class barco : MonoBehaviour
{
    [SerializeField] NavMeshAgent Barco;
    public Transform Destino;
    // Start is called before the first frame update
    void Awake()
    {
        Barco = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Barco.destination = Destino.position;
        Debug.Log("Distancia al destino: " + Barco.remainingDistance);
    }
}
