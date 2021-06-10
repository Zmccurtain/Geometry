using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isSpawned = false;
    public string[] target;
    public string type;
    public Room newRoom = null;
    private float timeToDestroy = .1f;
    private float timer;
    private GameObject wallFound;
    public bool bossRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        if (isSpawned)
        {
            Destroy(gameObject);
            return;
        }
        Floor.unopenedDoors.Add(this);
        target = new string[1];
        type = "";
        if (gameObject.name.Equals("DoorTop"))
        {
            target = GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().bot;
            type = "DoorBot";
        }
        else if (gameObject.name.Equals("DoorRight"))
        {
            target = GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().left;
            type = "DoorLeft";
        }
        else if (gameObject.name.Equals("DoorBot"))
        {
            target = GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().top;
            type = "DoorTop";
        }
        else if (gameObject.name.Equals("DoorLeft"))
        {
            target = GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().right;
            type = "DoorRight";
        }
    }
    void Spawn()
    {
        var rand = 0;
        rand = Random.Range(0, target.Length);
        Floor floor = GameObject.Find("Floor").GetComponent<Floor>();
        newRoom = (Instantiate(Resources.Load("Rooms/" + target[rand]) as GameObject, floor.transform) as GameObject).GetComponent<Room>();
        newRoom.transform.position = transform.position;
        newRoom.target = type;
        newRoom.previous = gameObject.transform.parent.GetComponent<Room>();
        isSpawned = true;

    }
    // Update is called once per frame
    void Update()
    {
        if (Floor.spawnTimer <= 0)
        {
            if (newRoom == null)
            {
                isSpawned = false;
            }
            if (!isSpawned)
            {
                timer = timeToDestroy;
                if(Floor.roomList.Count < Floor.maxRooms) Spawn();

            }
            else
            {
                if(timer <= 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    timer -= Time.deltaTime;
                }
                
            }
        }
    }
    private void OnDestroy()
    {
        Floor.unopenedDoors.Remove(this);

    }
}
