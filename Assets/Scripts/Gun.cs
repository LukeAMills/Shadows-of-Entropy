using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class Gun : MonoBehaviour {

    public enum GunType { Semi, Burst, Auto};
    public GunType gunType;
    public float rpm;           //rounds per minute

    //Components

    public Transform spawn;
    public Transform shellEjectionPoint;
    public Rigidbody shell;
    private LineRenderer tracer;

    //Sounds

    AudioSource Gun1;           //variable for sounds

    //System
    private float secondsBetweenShots;
    private float nextPossibleShootTime;

    private void Start()
    {
        secondsBetweenShots = 60 / rpm;

        if (GetComponent<LineRenderer>())
        {
            tracer = GetComponent<LineRenderer>();

        }
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            Ray ray = new Ray(spawn.position, spawn.forward);
            RaycastHit hit;
            float shotDistance = 20;

            if (Physics.Raycast(ray, out hit, shotDistance))
            {
                shotDistance = hit.distance;
            }
            nextPossibleShootTime = Time.time + secondsBetweenShots;

            Gun1 = GetComponent<AudioSource>();     //Fetches sound from variable in unity
            Gun1.Play();                            //Plays fetched sound

            if (tracer) //if there is a tracer 
            {
                StartCoroutine("RenderTracer", ray.direction * shotDistance); //draw tracer
            }

            //Rigidbody newShell = Instantiate(shell, shellEjectionPoint.position, Quaternion.identity) as Rigidbody;
            //Rigidbody newShell = Instantiate(shell, spawn.position, Quaternion.identity) as Rigidbody;
            //Rigidbody newShell = Instantiate<Rigidbody>(shell, shellEjectionPoint.position, Quaternion.identity);
            //newShell.AddForce(shellEjectionPoint.forward * Random.Range(150f, 200f) + spawn.forward * Random.Range(-10f, 10f));
            //newShell.AddForce(spawn.forward * Random.Range(150f, 200f));

            //Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 1);    //draws rays
        }

    }

    public void ShootContinuous()   //for full auto
    {
        if(gunType == GunType.Auto)
        {
            Shoot();
        }
    }

    private bool CanShoot() //determines whether or not the next shot can be fired
    {
        bool canShoot = true;

        if(Time.time < nextPossibleShootTime)
        {
            canShoot = false;
        }

        return canShoot;
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, spawn.position);              //start
        tracer.SetPosition(1, spawn.position + hitPoint);   //finish
       
        yield return null;  //single frame, can change null to other for different amounts of time
        tracer.enabled = false;
    }
}
