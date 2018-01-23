using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public GameObject zPrefab;
    GameObject[] obstacle;
    obstacleScript[] blocks;
    List<int> index;
    Vector3 steer;
    List<Vehicle> all = new List<Vehicle>();
    public List<GameObject> humans;
    List<HumanScript> humanScripts;
    GameObject theVehicle;
    Vehicle vehicleScript;
    public List<GameObject> zombie;
    List<ZombieScript> zombieScripts;
    float distance;
    float temp;
    float humanFleeWeight;
    bool newZ;
    public bool toggle;

    // Use this for initialization
    void Start()
    {
        toggle = true;
        humanScripts = new List<HumanScript>();

        for (int i = 0; i < humans.Count; i++)
        {
            humanScripts.Add(humans[i].GetComponent<HumanScript>());
            all.Add(humans[i].GetComponent<Vehicle>());
        }

        zombieScripts = new List<ZombieScript>();

        for (int i = 0; i < zombie.Count; i++)
        {
            zombieScripts.Add(zombie[i].GetComponent<ZombieScript>());
            all.Add(zombie[i].GetComponent<Vehicle>());
        }

        obstacle = GameObject.FindGameObjectsWithTag("obstacle");
        blocks = new obstacleScript[obstacle.Length];

        for (int i = 0; i < obstacle.Length; i++)
            blocks[i] = obstacle[i].GetComponent<obstacleScript>();

        newZ = false;
    }

    // Update is called once per frame
    void Update()
    {
        temp = 50;

        for (int i = 0; i < humanScripts.Count; i++)
        {
            for (int j = 0; j < zombieScripts.Count; j++)
            {
                if (Vector3.Distance(humanScripts[i].transform.position, zombieScripts[j].transform.position) < temp)
                {
                    temp = Vector3.Distance(humanScripts[i].transform.position, zombieScripts[j].transform.position);
                    zombieScripts[j].seekTarget = humans[i];
                }

                if (Vector3.Distance(humanScripts[i].transform.position, zombieScripts[j].transform.position) < 5)
                {
                    newZ = true;
                    GameObject tempHuman = humans[i];
                    HumanScript tempScript = humanScripts[i];
                    Vehicle tempVehicle = humans[i].GetComponent<Vehicle>();
                    humans.Remove(tempHuman);
                    humanScripts.Remove(tempScript);

                    GameObject newZombie = Instantiate(zPrefab, tempHuman.transform.position, Quaternion.identity);
                    ZombieScript newZScript = newZombie.GetComponent<ZombieScript>();
                    newZScript.seekTarget = humans[i];
                    newZScript.maxSpeed = 10;
                    newZScript.seekWeight = 1;
                    tempVehicle.maxSpeed = 10;

                    zombie.Add(newZombie);
                    zombieScripts.Add(newZScript);

                    tempHuman.SetActive(false);

                    break;
                }

                Vehicle check = humanScripts[i].GetComponent<Vehicle>();

                if (Vector3.Distance(humanScripts[i].transform.position, zombieScripts[j].transform.position) < 15)
                {
                    check.mass = 0.25f;
                    steer = check.Evade(zombieScripts[j].transform.position, zombieScripts[j].transform.position + this.transform.position);
                    steer.y = 0;
                    check.ApplyForce(steer);
                }

                else
                {
                    Wander(check);
                }

                if (humanScripts[i].transform.position.x > 50 || humanScripts[i].transform.position.x < -50 || humanScripts[i].transform.position.z < -50 || humanScripts[i].transform.position.z > 50)
                {
                    steer = check.Seek(new Vector3(0, 0.5f, 0));
                    steer.y = 0;
                    check.ApplyForce(steer);
                }
            }

            if (newZ)
            {
                newZ = false;
                break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vehicle tempVehicle = humans[0].GetComponent<Vehicle>();
            GameObject newZombie = Instantiate(zPrefab, Vector3.zero, Quaternion.identity);
            ZombieScript newZScript = newZombie.GetComponent<ZombieScript>();
            newZScript.seekTarget = humans[0];
            newZScript.maxSpeed = 10;
            newZScript.seekWeight = 1;
            tempVehicle.maxSpeed = 10;

            zombie.Add(newZombie);
            zombieScripts.Add(newZScript);
        }


        if (humans.Count == 0)
        {
            for (int i = 0; i < zombieScripts.Count; i++)
            {
                Vehicle check = zombieScripts[i].GetComponent<Vehicle>();
                Wander(check);

                if (zombieScripts[i].transform.position.x > 50 || zombieScripts[i].transform.position.x < -50 || zombieScripts[i].transform.position.z < -50 || zombieScripts[i].transform.position.z > 50)
                {
                    steer = check.Seek(new Vector3(0, 0.5f, 0));
                    steer.y = 0;
                    check.ApplyForce(steer);
                }
            }
        }

        foreach (Vehicle v in all)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    Vector3 avoidForce = v.AvoidObstacle(blocks[i], 4.5f);
                    avoidForce.y = 0;
                    v.ApplyForce(avoidForce);
                }
            }

        if (Input.GetKeyDown(KeyCode.F))
            toggle = false;
        if (Input.GetKeyDown(KeyCode.D))
            toggle = true;
    }

    public void Wander(Vehicle v)
    {
        float strength = Random.Range(7.5f, 30);

        Vector2 offset2 = Random.insideUnitCircle;
        Vector3 offset3 = new Vector3(offset2.x, 0, offset2.y);
        offset3.Normalize();
        offset3 *= strength;

        v.ApplyForce(offset3);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 400, 20), "Press D to show debug lines. Press F to hide debug lines.");
        GUI.Label(new Rect(10, 30, 400, 20), "You can left mouse click to spawn additional zombies in the middle.");
    }
}
