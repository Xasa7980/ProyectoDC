using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSkill : MonoBehaviour
{
    [SerializeField] Specialist_SO skillSet;
    [SerializeField] SkillSO skill;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float explosionTime = 3f;
    [SerializeField] GameObject effect;
    [SerializeField] Renderer surfaceRenderer;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] Gradient explosionGradient;
    float explosionTimer;
    Color originalColor;

    private void OnEnable()
    {
        originalColor = surfaceRenderer.material.color;
    }
    void Update()
    {
        explosionTimer += Time.deltaTime;

        float alpha = 0 + (explosionTimer / explosionTime);
        surfaceRenderer.material.color = explosionGradient.Evaluate(alpha);

        if (explosionTimer >= explosionTime)
        {
            surfaceRenderer.material.color = originalColor;
            surfaceRenderer.gameObject.SetActive(false);
            Explode();
        }

    }
    void Explode()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);
        effect.SetActive(true);
        foreach (Collider target in targets)
        {
            if(target.TryGetComponent<iDamageable>(out iDamageable damageable))
            {
                damageable.ApplyDamage(skill.damage);
                Debug.Log("ea");
            }
        }
    }
}