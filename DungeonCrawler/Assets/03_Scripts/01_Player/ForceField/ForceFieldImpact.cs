using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldImpact : MonoBehaviour
{
    //    // tiempo hasta que la onda de impacto se disipe
    //    [Range(0.1f, 5), SerializeField] float dampenTime = 1.5f;

    //    //Desplazamiento maximo al impactar
    //    [Range(0.1f, 5), SerializeField] float impactRippleAmplitude = 0.005f;
    //    [Range(0.1f, 5), SerializeField] float impactRippleMaxRadius = 0.35f;

    //    //Añadimos clicks del raton para probar 
    //    [SerializeField] bool clickToImpact;

    //    //Breve retardo entre clicks para prevenir spameo de balas
    //    const float coolDownMax = 0.25f;
    //    float coolDownWindow;

    //    //Camara
    //    Camera cam;

    //    //Referencia al meshRenderer o SkinnedMeshRenderer
    //    [SerializeField] MeshRenderer meshRenderer;
    //    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

    //    void Start()
    //    {
    //        if (cam == null & Camera.main != null) cam = Camera.main;
    //        //meshRenderer = GetComponent<MeshRenderer>(); LOS REFERENCIARE MANUALMENTE A TRAVES DEL INSPECTOR
    //        //skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    //        coolDownWindow = 0;
    //    }
    //    void Update()
    //    {
    //        if (clickToImpact)
    //        {
    //            UpdateMouse();
    //        }
    //        Dampen();
    //    }
    //    public void EnableRipple(bool state = false)
    //    {
    //        int onOff = (state) ? 1 : 0;
    //        meshRenderer?.material.SetFloat("_EnableRipple", onOff);
    //    }
    //    public void EnableRimGlow(bool state = false)
    //    {
    //        int onOff = (state) ? 1 : 0;
    //        meshRenderer?.material.SetFloat("_EnableGlowColor", onOff);
    //    }
    //    public void EnableScanLine2D(bool state = false)
    //    {
    //        int onOff = (state) ? 1 : 0;
    //        meshRenderer?.material.SetFloat("_EnableScanLine2D", onOff);
    //    }
    //    public void EnabledFillingTexture(bool state = false)
    //    {
    //        int onOff = (state) ? 1 : 0;
    //        meshRenderer?.material.SetFloat("_EnabledFillingTexture", onOff);
    //    }
    //    public void EnableIntersection(bool state = false)
    //    {
    //        int onOff = (state) ? 1 : 0;
    //        meshRenderer?.material.SetFloat("_EnableInteractionFromDepth", onOff);
    //    }
    //    //Impactar contra el ForceField, pasando el punto y la direccion de impacto

    //    public void ApplyImpact(Vector3 hitPoint, Vector3 rippleDirection)
    //    {
    //        if (meshRenderer != null)
    //        {
    //            EnableRipple(true);
    //            meshRenderer.material.SetFloat("_ImpactRippleMaxRadius", impactRippleMaxRadius);
    //            meshRenderer.material.SetFloat("_ImpactRippleAmplitude", impactRippleAmplitude);
    //            meshRenderer.material.SetVector("_ImpactRippleDirection", rippleDirection);
    //            meshRenderer.material.SetVector("_ImpactPoint", hitPoint);
    //        }
    //    }

    //    //Impactar al ForceField, pasando en RaycastHit

    //    public void ApplyImpact(RaycastHit hit)
    //    {
    //        if (meshRenderer != null)
    //        {
    //            EnableRipple(true);
    //            meshRenderer.material.SetFloat("_ImpactRippleMaxRadius", impactRippleMaxRadius);
    //            meshRenderer.material.SetFloat("_ImpactRippleAmplitude", impactRippleAmplitude);
    //            meshRenderer.material.SetVector("_ImpactRippleDirection", hit.normal);
    //            meshRenderer.material.SetVector("_ImpactPoint", hit.point);
    //        }
    //    }

    //    void Dampen()
    //    {
    //        if (meshRenderer != null)
    //        {
    //            //Recoger la amplitud actual
    //            float currentAmplitude = meshRenderer.material.GetFloat("_ImpactRippleAmplitude");
    //            //Disminuye por una pequeña cantidad por frame
    //            float newAmplitude = currentAmplitude - (impactRippleAmplitude * Time.deltaTime / dampenTime);
    //            //Clampea a valores positivos
    //            newAmplitude = Mathf.Clamp(newAmplitude, 0, newAmplitude);
    //            //Si es negativo, desactivamos el ripple si no establecemos una nueva amplitud
    //            if (newAmplitude <= 0) EnableRipple(false);
    //            else meshRenderer.material.SetFloat("_ImpactRippleAmplitude", newAmplitude);
    //        }
    //    }

    //    private void UpdateMouse()
    //    {
    //        coolDownWindow -= Time.deltaTime;

    //        if (coolDownWindow <= 0)
    //        {
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                ClickToImpact();
    //            }
    //        }
    //    }

    //    // Agrega clicks del mouse para impactar contra el forcefield
    //    private void ClickToImpact()
    //    {
    //        if (cam == null)
    //            return;

    //        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit, 15))
    //        {
    //            Transform hitXform = hit.transform;

    //            if (hitXform == this.transform)
    //            {
    //                coolDownWindow = coolDownMax;

    //                ApplyImpact(hit.point, hit.normal);
    //            }
    //            else
    //            {
    //                // Para debuggear si impactamos otro objeto
    //                Debug.Log("Hit " + hitXform.name);
    //            }
    //        }
    //    }
    //}
    // time until impact ripple dissipates
    [Range(0.1f, 5f)]
    [SerializeField] private float dampenTime = 1.5f;

    // maximum displacement on impact
    [Range(0.002f, 0.1f)]
    [SerializeField] private float impactRippleAmplitude = 0.005f;
    [Range(0.05f, 0.5f)]
    [SerializeField] private float impactRippleMaxRadius = 0.35f;

    // allow mouse clicks for testing 
    [SerializeField] private bool clickToImpact;

    //// slight delay between clicks to prevent spamming
    private const float coolDownMax = 0.25f;
    private float coolDownWindow;

    // main camera
    private Camera cam;

    // reference to this MeshRenderer
    private MeshRenderer meshRenderer;

    void Start()
    {
        if (cam == null && Camera.main != null)
        {
            cam = Camera.main;
        }

        meshRenderer = GetComponent<MeshRenderer>();

        coolDownWindow = 0;

    }

    #region DIAGNOSTIC 
    private void UpdateMouse()
    {
        coolDownWindow -= Time.deltaTime;

        if (coolDownWindow <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickToImpact();
            }
        }
    }

    // allow mouse clicks to test forcefield - useful for diagnostic
    private void ClickToImpact()
    {
        if (cam == null)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Transform hitXform = hit.transform;

            if (hitXform == this.transform)
            {
                coolDownWindow = coolDownMax;

                ApplyImpact(hit.point, hit.normal);
            }
            else
            {
                // for debugging if we hit another object instead
                Debug.Log("Hit " + hitXform.name);
            }
        }
    }
    #endregion

    public void EnableRipple(bool state = false)
    {
        int onOff = (state) ? 1 : 0;
        meshRenderer?.material.SetFloat("_EnableRipple", onOff);
    }

    public void EnableRimGlow(bool state = false)
    {
        int onOff = (state) ? 1 : 0;
        meshRenderer?.material.SetFloat("_EnableGlowColor", onOff);
    }

    public void EnableScanLine(bool state = false)
    {
        int onOff = (state) ? 1 : 0;
        meshRenderer?.material.SetFloat("_EnableScanLine2D", onOff);
    }

    public void EnableFillTexture(bool state = false)
    {
        int onOff = (state) ? 1 : 0;
        meshRenderer?.material.SetFloat("_EnabledFillingTexture", onOff);
    }

    public void EnableIntersection(bool state = false)
    {
        int onOff = (state) ? 1 : 0;
        meshRenderer?.material.SetFloat("_EnableInteractionFromDepth", onOff);
    }


    // impact Forcefield, passing in hit point and direction
    public void ApplyImpact(Vector3 hitPoint, Vector3 rippleDirection)
    {
        if (meshRenderer != null)
        {
            EnableRipple(true);
            meshRenderer.material.SetFloat("_ImpactRippleMaxRadius", impactRippleMaxRadius);
            meshRenderer.material.SetFloat("_ImpactRippleAmplitude", impactRippleAmplitude);
            meshRenderer.material.SetVector("_ImpactRippleDirection", rippleDirection);
            meshRenderer.material.SetVector("_ImpactPoint", hitPoint);

        }
    }

    // impact Forcefield, passing in RaycastHit
    public void ApplyImpact(RaycastHit hit)
    {
        if (meshRenderer != null)
        {
            EnableRipple(true);
            meshRenderer.material.SetFloat("_ImpactRippleMaxRadius", impactRippleMaxRadius);
            meshRenderer.material.SetFloat("_ImpactRippleAmplitude", impactRippleAmplitude);
            meshRenderer.material.SetVector("_ImpactRippleDirection", hit.normal);
            meshRenderer.material.SetVector("_ImpactPoint", hit.point);

        }
    }

    // gradually slow ripple motion 
    private void Dampen()
    {
        if (meshRenderer != null)
        {
            // get the current amplitude
            float currentAmplitude = meshRenderer.material.GetFloat("_ImpactRippleAmplitude");

            // decrement by a small amount per frame
            float newAmplitude = currentAmplitude - (impactRippleAmplitude * Time.deltaTime / dampenTime);

            // clamp to positive values
            newAmplitude = Mathf.Clamp(newAmplitude, 0f, newAmplitude);

            // if negative, disable the ripple; otherwise, set the new amplitude
            meshRenderer.material.SetFloat("_ImpactRippleAmplitude", newAmplitude);
        }
    }

    void Update()
    {
        if (clickToImpact)
        {
            UpdateMouse();
        }

        Dampen();
    }
}
