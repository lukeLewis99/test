using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour {

    BoxCollider2D collider;
    public bool hasHitEnemy;
    bool doDamage = true;

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //make the touched entity take a hit
        if (collision.gameObject.tag == "Player" && doDamage)
        {
            //collision.gameObject.GetComponent<EnemyHealthController>().takeHit(3);
            //hasHitEnemy = true;
        }
    }

    private void FixedUpdate()
    {
        //dont do  damage after one hit
        if (hasHitEnemy)
        {

            doDamage = false;
        }
    }
}
