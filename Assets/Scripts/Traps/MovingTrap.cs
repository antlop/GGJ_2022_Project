using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingTrap : MonoBehaviour
{
    public ParticleSystem CollisionParticle;
    public string[] CollidableTags;
    public Vector2Int DamageRange;

    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(ArrayContainsTag(collision.transform.tag))
        {
            GameObject particleObj = Instantiate(CollisionParticle.gameObject, new Vector3(0f, 0.5f, 0f) + collision.transform.position, Quaternion.identity, collision.transform);
            Destroy(particleObj, CollisionParticle.main.duration + 1);

            collision.transform.GetComponent<Character_Core_Manager>().TakeDamage(Random.Range(DamageRange.x, DamageRange.y));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ArrayContainsTag(other.transform.tag))
        {
            GameObject particleObj = Instantiate(CollisionParticle.gameObject, new Vector3(0f, 0.5f, 0f) + other.transform.position, Quaternion.identity, other.transform);
            Destroy(particleObj, CollisionParticle.main.duration + 1);

            other.transform.GetComponent<Character_Core_Manager>().TakeDamage(Random.Range(DamageRange.x, DamageRange.y));
        }
    }

    bool ArrayContainsTag(string tag)
    {
        foreach(string _tag in CollidableTags)
        {
            if(_tag.Equals(tag)) { return true; }
        }

        return false;
    }
}
