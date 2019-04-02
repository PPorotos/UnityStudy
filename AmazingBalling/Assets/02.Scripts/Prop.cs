using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public ParticleSystem explosionParticle;

    public int score = 5;

    public float hp = 10.0f;

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            ParticleSystem instance = Instantiate(explosionParticle, transform.position, transform.rotation);
            Destroy(instance.gameObject, instance.duration);

            AudioSource explosionAudio = instance.GetComponent<AudioSource>();
            explosionAudio.Play();

            gameObject.SetActive(false);


        }
    }
}
