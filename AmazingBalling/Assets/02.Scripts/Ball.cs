using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public LayerMask whatIsProp;

    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;

    public float maxDamage = 100.0f;
    public float explosionForce = 1000.0f;
    public float lifeTime = 10.0f;
    public float explosionRadius = 20.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {

        Collider[] colliders =  Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);
        explosionParticle.transform.parent = null;

        for (int i = 0; i< colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            Prop targetProp = colliders[i].GetComponent<Prop>();

            float damage = CalculateDamage(colliders[i].transform.position);

            targetProp.TakeDamage(damage);
        }

        explosionParticle.Play();
        explosionAudio.Play();

        GameManager.instance.OnBallDestroy();

        Destroy(explosionParticle.gameObject, explosionParticle.duration);

        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPositon)
    {
        Vector3 explosionToTarget = targetPositon - transform.position;

        float distance = explosionToTarget.magnitude;

        float edgeToCenterDistance = explosionRadius - distance;

        float percentage = edgeToCenterDistance / explosionRadius;

        float damage = maxDamage * percentage;

        damage = Mathf.Max(0, damage);

        return damage;
    }
}
