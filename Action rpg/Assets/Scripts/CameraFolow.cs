using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFolow : MonoBehaviour {

    public GameObject player;
    public float FollowSpeed = 2f;

    // Update is called once per frame
    void Update () {
        
        Vector3 target =  new Vector3(player.transform.position.x + player.GetComponent<PlayerController>().direction.x , player.transform.position.y + player.GetComponent<PlayerController>().direction.y, -10);
        transform.position = Vector3.Slerp(transform.position, target, FollowSpeed * Time.deltaTime);
    }
}
