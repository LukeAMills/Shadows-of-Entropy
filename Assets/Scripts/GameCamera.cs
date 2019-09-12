using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour {

    public GameObject player;
    private Transform target;
    private Vector3 cameraTarget;

    // Use this for initialization
    void Start () {

        target = GameObject.FindGameObjectWithTag("player").transform;

    }

    // Update is called once per frame
    void Update () {
        
        cameraTarget = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.deltaTime * 8);

    }
}
