using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour {

    public int health;

    public void takeHit(int power)
    {
        health -= power;

        if (health <= 0) {

            Destroy(this.gameObject);
        }
    }
}
