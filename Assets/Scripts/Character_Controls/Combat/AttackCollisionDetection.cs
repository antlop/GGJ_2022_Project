using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AttackCollisionDetection : MonoBehaviour
{
    public List<string> tagsToLookFor;
    public Vector2Int DamageToApply;

    private void OnCollisionEnter(Collision collision)
    {
        if( collidedWithObjectWeAreLookingFor(collision.transform.tag))
        {
            Debug.Log("Hit " + collision.transform.name);

            collision.transform.GetComponent<Character_Core_Manager>().TakeDamage(Random.Range(DamageToApply.x, DamageToApply.y+1));
        }
    }

    bool collidedWithObjectWeAreLookingFor(string tag)
    {
        foreach(string searchTag in tagsToLookFor)
        {
            if (tag.Equals(searchTag)) { return true; }
        }
        return false;
    }
}
