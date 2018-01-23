using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    bool toggle;
    public Material[] material;
    public float seekWeight;
    public float maxSpeed;
    public GameObject seekTarget;
    Vehicle vehicleScript;
    Vector3 steer;
    public GameObject manager;
    // Use this for initialization
    void Start()
    {
        vehicleScript = this.GetComponent<Vehicle>();
        vehicleScript.maxSpeed = this.maxSpeed;
        vehicleScript.mass = this.seekWeight;
        vehicleScript.target = this.seekTarget;
        
        manager = GameObject.FindGameObjectWithTag("manager");
    }

    // Update is called once per frame
    void Update()
    {
        vehicleScript.target = this.seekTarget;

        steer = vehicleScript.Pursue(seekTarget.transform.position, seekTarget.transform.position - this.transform.position);
        vehicleScript.ApplyForce(steer);

        toggle = manager.GetComponent<AgentManager>().toggle;
    }

    void OnRenderObject()
    {
        if (toggle)
        {
            material[0].SetPass(0);

            GL.Begin(GL.LINES);
            GL.Vertex(this.transform.position);
            GL.Vertex(seekTarget.transform.position);
            GL.End();

            material[1].SetPass(0);

            GL.Begin(GL.LINES);
            GL.Vertex(this.transform.position);
            GL.Vertex(this.transform.position + this.transform.right * 5);
            GL.End();

            material[2].SetPass(0);

            GL.Begin(GL.LINES);
            GL.Vertex(this.transform.position);
            GL.Vertex(this.transform.position + this.transform.forward * 5);
            GL.End();
        }
    }
}
