using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BengalaProyectil : MonoBehaviour
{
    [Header("Flare Effects")]
    public ParticleSystem flareParticles;
    public TrailRenderer flareTrail;
    public Light flareLight;
    public float burnDuration = 10f;
    public float lightIntensity = 2f;
    public Color flareColor = Color.red;

    [Header("Physics")]
    public float gravityMultiplier = 0.5f; // Reducido para que vaya más lejos
    public float airResistance = 0.98f; // Resistencia del aire

    private Rigidbody rb;
    private bool isIgnited = false;
    private float igniteTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetupComponents();
    }

    void SetupComponents()
    {
        // Configurar sistema de partículas si no existe
        if (flareParticles == null)
        {
            GameObject particlesGO = new GameObject("FlareParticles");
            particlesGO.transform.SetParent(transform);
            particlesGO.transform.localPosition = Vector3.zero;

            flareParticles = particlesGO.AddComponent<ParticleSystem>();
            var main = flareParticles.main;
            main.startColor = flareColor;
            main.startSize = 0.5f;
            main.startSpeed = 2f;
            main.startLifetime = 2f;
            main.maxParticles = 50;

            var emission = flareParticles.emission;
            emission.rateOverTime = 25f;

            var shape = flareParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 15f;
        }

        // Configurar trail renderer si no existe
        if (flareTrail == null)
        {
            flareTrail = gameObject.AddComponent<TrailRenderer>();

            // Crear material para el trail
            Material trailMat = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            trailMat.color = flareColor;
            flareTrail.material = trailMat;

            flareTrail.time = 3f; // Duración de la estela
            flareTrail.startWidth = 0.5f;
            flareTrail.endWidth = 0.1f;
            flareTrail.minVertexDistance = 0.1f;
            flareTrail.enabled = false; // Se activa al encender
        }

        // Configurar luz si no existe
        if (flareLight == null)
        {
            GameObject lightGO = new GameObject("FlareLight");
            lightGO.transform.SetParent(transform);
            lightGO.transform.localPosition = Vector3.zero;

            flareLight = lightGO.AddComponent<Light>();
            flareLight.type = LightType.Point;
            flareLight.color = flareColor;
            flareLight.intensity = lightIntensity;
            flareLight.range = 10f;
        }

        // Inicialmente apagados
        if (flareParticles != null) flareParticles.Stop();
        if (flareLight != null) flareLight.enabled = false;
        if (flareTrail != null) flareTrail.enabled = false;
    }

    void Update()
    {
        if (isIgnited)
        {
            // Aplicar gravedad reducida para mayor alcance
            if (rb != null)
            {
                rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
                // Aplicar resistencia del aire gradual
                rb.velocity *= airResistance;
            }

            // Verificar si debe apagarse
            if (Time.time - igniteTime >= burnDuration)
            {
                Extinguish();
            }

            // Efecto de parpadeo cerca del final
            if (Time.time - igniteTime >= burnDuration * 0.8f)
            {
                float flicker = Mathf.Sin(Time.time * 20f) * 0.5f + 0.5f;
                if (flareLight != null)
                    flareLight.intensity = lightIntensity * flicker;
            }
        }
    }

    public void Ignite()
    {
        if (isIgnited) return;

        isIgnited = true;
        igniteTime = Time.time;

        Debug.Log("¡Bengala encendida! Activando efectos...");

        // Activar efectos
        if (flareParticles != null)
        {
            flareParticles.Play();
            Debug.Log("Partículas activadas");
        }

        if (flareLight != null)
        {
            flareLight.enabled = true;
            Debug.Log("Luz activada");
        }

        if (flareTrail != null)
        {
            flareTrail.enabled = true;
            flareTrail.Clear(); // Limpiar cualquier trail previo
            Debug.Log("Trail activado");
        }

        // Cambiar a tag de proyectil
        tag = "FlareProjectile";
    }

    void Extinguish()
    {
        isIgnited = false;

        // Apagar efectos
        if (flareParticles != null)
        {
            flareParticles.Stop();
        }

        if (flareLight != null)
        {
            flareLight.enabled = false;
        }

        if (flareTrail != null)
        {
            flareTrail.enabled = false;
        }

        // Convertir de nuevo en item recogible
        tag = "Flare";

        // Opcional: destruir después de un tiempo
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Reducir velocidad al impactar
        if (rb != null && isIgnited)
        {
            rb.velocity *= 0.3f;
            rb.angularVelocity *= 0.5f;
        }
    }
}
