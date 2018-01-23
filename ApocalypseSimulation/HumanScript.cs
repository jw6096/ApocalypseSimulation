using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanScript : MonoBehaviour
{
    bool toggle;
    public Material[] material;
    public float seekWeight;
    public float fleeWeight;
    public float maxSpeed;
    public GameObject seekTarget;
    public GameObject fleeTarget;
    Vehicle vehicleScript;
    Vector3 steer;
    public GameObject manager;

    public Material[] materials;
    public Renderer rend;

    // Use this for initialization
    void Start()
    {
        vehicleScript = this.GetComponent<Vehicle>();
        vehicleScript.maxSpeed = this.maxSpeed;
        vehicleScript.mass = this.seekWeight;

        manager = GameObject.FindGameObjectWithTag("manager");
    }

    // Update is called once per frame
    void Update()
    {
        toggle = manager.GetComponent<AgentManager>().toggle;
        /*if (Vector3.Distance(this.transform.position, zombieScript.transform.position) < 7)
        {
            vehicleScript.mass = this.fleeWeight;
            steer = vehicleScript.Evade(zombieScript.transform.position, fleeTarget.transform.position + this.transform.position);
            steer.y = 0;
            vehicleScript.ApplyForce(steer);
        }
        else
        {
            vehicleScript.mass = this.seekWeight;

            float angle = Random.Range(0, 360);

            Vector3 futurePos = this.transform.position + vehicleScript.velocity * Time.deltaTime;
            futurePos.y = 0.5f;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            offset *= 10;
            steer = vehicleScript.Seek(futurePos + offset);
            vehicleScript.ApplyForce(steer);
            Debug.DrawLine(this.transform.position, futurePos, Color.green);
        }

        if (this.transform.position.x > 25 || this.transform.position.x < -25 || this.transform.position.z < -25 || this.transform.position.z > 25)
        {
            steer = vehicleScript.Seek(new Vector3(0, 0.5f, 0));
            steer.y = 0;
            vehicleScript.ApplyForce(steer);
        }*/
    }

    void OnRenderObject()
    {
        if (toggle)
        {
            material[0].SetPass(0);

            GL.Begin(GL.LINES);
            GL.Vertex(this.transform.position);
            GL.Vertex(this.transform.position + this.transform.right * 5);
            GL.End();

            material[1].SetPass(0);

            GL.Begin(GL.LINES);
            GL.Vertex(this.transform.position);
            GL.Vertex(this.transform.position + this.transform.forward * 5);
            GL.End();
        }
    }
}
