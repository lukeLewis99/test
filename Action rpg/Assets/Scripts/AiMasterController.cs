using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMasterController : MonoBehaviour {

    [Header("Unit lists")]
    public List<GameObject> frendlyUnits;
    public List<GameObject> enemyUnits;


    [Space]

    [Header("Movement point lists")]
    
    public List<movementPoint> frendlyUnitPath;
    public List<movementPoint> enemyUnitPath;
    // Use this for initialization
    void Start () {
        //initiates unit lists with all aplicible units
        frendlyUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        enemyUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        // initiates path lists DONT TOUCH OR VISUAL STUDIO WILL YELL AT YOU
        frendlyUnitPath = new List<movementPoint>();
        frendlyUnitPath = new List<movementPoint>();
    }
}

[System.Serializable]
public class movementPoint {
    public Vector3 position;
    public float radius;
}
