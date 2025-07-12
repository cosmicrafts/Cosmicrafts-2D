using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{
    private Transform target;
    public float range = 15f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public ParticleSystem StartBeamEffect;
    public GameObject StartBeamObject;

    public string enemyTag = "Enemy";
    public Transform firePoint;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }

    void Update()
    {
        if (target == null)
        {

            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                StartBeamEffect.Stop();
            }         
            return;
        }
        else
        {
        }
        //LockOnTarget();
        Laser();
    }

    void Laser()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            StartBeamEffect.Play();
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.position = target.position;//+ dir.normalized;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }
}
