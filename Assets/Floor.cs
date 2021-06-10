using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Floor : MonoBehaviour
{
    public static List<GameObject> roomList = new List<GameObject>();
    public static List<Door> unopenedDoors = new List<Door>();
    public static float spawnTime = .05f;
    public static float spawnTimer = 0;
    public static int maxRooms = 10;
    private GameObject wallFound;
    private Door bossDoor;

    private bool called = false;
    private bool bossRoomSpawned = false;
    private void Start()
    {
        //Invoke("MakeBossRoom", 6f);
    }
    private void Update()
    {
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnTime;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
        
        
    }
    private void LateUpdate()
    {
        if (roomList.Count >= maxRooms)
        {
            if (!called)
            {
                called = true;
                Invoke("RoomsSpawned", .5f);
            }
        }
    }
    private void RoomsSpawned()
    {
        Debug.Log("RoomsSpawned");
        foreach (Door door in unopenedDoors)
        {
            Collider2D[] data = Physics2D.OverlapCircleAll(door.transform.position, 1f);
            List<Collider2D> cols = new List<Collider2D>();
            int roomCount = 0;
            int wallCount = 0;

            foreach (Collider2D i in data)
            {
                if (i.transform.tag.Equals("Wall"))
                {
                    wallCount++;
                    wallFound = i.gameObject;

                }
                else if (i.transform.tag.Equals("Room"))
                {

                    roomCount++;
                }
            }

            if (roomCount >= 2)
            {
                if (wallCount >= 1)
                {

                    GameObject newWall1 = Instantiate(Resources.Load("Wall") as GameObject, wallFound.transform.parent) as GameObject;
                    GameObject newWall2 = Instantiate(Resources.Load("Wall") as GameObject, wallFound.transform.parent) as GameObject;

                    newWall1.transform.position = wallFound.transform.position;
                    newWall2.transform.position = wallFound.transform.position;
                    if (door.type == "DoorTop" || door.type == "DoorBot")
                    {
                        Debug.Log("H");
                        newWall1.transform.position = new Vector3(newWall1.transform.position.x +
                                                                        ((wallFound.transform.localScale.x - 3) / 4) + 1.5f
                                                                        + ((door.transform.position.x - wallFound.transform.position.x) / 2)
                                                                        ,
                                                                        newWall1.transform.position.y, 0);
                        newWall2.transform.position = new Vector3(newWall2.transform.position.x -
                                                                        ((wallFound.transform.localScale.x - 3) / 4) - 1.5f
                                                                        + ((door.transform.position.x - wallFound.transform.position.x) / 2)
                                                                        ,
                                                                        newWall2.transform.position.y, 0);
                        newWall1.transform.localScale = new Vector3(((wallFound.transform.localScale.x - 3) / 2)
                                                                    - (door.transform.position.x - wallFound.transform.position.x)
                                                                    , 1, 1);
                        newWall2.transform.localScale = new Vector3(((wallFound.transform.localScale.x - 3) / 2)
                                                                    + (door.transform.position.x - wallFound.transform.position.x)
                                                                    , 1, 1);
                    }
                    else
                    {
                        Debug.Log("V");
                        newWall1.transform.localPosition = new Vector3(newWall1.transform.localPosition.x, newWall1.transform.localPosition.y +
                                                                        ((wallFound.transform.localScale.y - 3) / 4) + 1.5f +
                                                                        ((door.transform.position.y - wallFound.transform.position.y) / 2), 0);
                        newWall2.transform.localPosition = new Vector3(newWall2.transform.localPosition.x, newWall2.transform.localPosition.y -
                                                                        ((wallFound.transform.localScale.y - 3) / 4) - 1.5f +
                                                                        ((door.transform.position.y - wallFound.transform.position.y) / 2), 0);
                        newWall1.transform.localScale = new Vector3(1, ((wallFound.transform.localScale.y - 3) / 2) - (door.transform.position.y - wallFound.transform.position.y), 1);
                        newWall2.transform.localScale = new Vector3(1, ((wallFound.transform.localScale.y - 3) / 2) + (door.transform.position.y - wallFound.transform.position.y), 1);
                    }
                    Destroy(wallFound.gameObject);
                    Destroy(door.gameObject);
                }
            }
        }
        StartCoroutine("SpawnBossRoom");
        
    }

    public IEnumerator SpawnBossRoom()
    {
        
        while (GameObject.Find("Boss Room(Clone)") == null)
        {
            
            var rand = Random.Range(0, unopenedDoors.Count);
            Door door = unopenedDoors[rand].GetComponent<Door>();

            Room room = (Instantiate(Resources.Load("Rooms/Boss Room") as GameObject, transform) as GameObject).GetComponent<Room>();
            if (room != null)
            {
                room.transform.position = door.transform.position;
                room.target = door.type;
                room.previous = door.gameObject.transform.parent.GetComponent<Room>();
                door.isSpawned = true;
                bossDoor = door;
            }

            yield return new WaitForSeconds(1);
        }


        yield return new WaitForSeconds(3);
        unopenedDoors.Remove(bossDoor);
        foreach (Door unopened in unopenedDoors)
        {

            var wall = (Instantiate(Resources.Load("Wall") as GameObject, unopened.transform.parent) as GameObject);
            wall.transform.localPosition = unopened.transform.localPosition;
            
            if (unopened.type == "DoorTop" || unopened.type == "DoorBot") wall.transform.localScale = new Vector3(3, 1, 1);
            else wall.transform.localScale = new Vector3(1, 3, 1);

            
            
            if (unopened.bossRoom)
            {
                wall.GetComponent<SpriteRenderer>().color = Color.red;
                wall.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            }
            Destroy(unopened.gameObject);
        }

    }
    
    void MakeBossRoom()
    {
        Room last = roomList[roomList.Count-1].GetComponent<Room>();
        foreach(Transform child in last.transform)
        {
            if (child.transform.tag.Equals("Wall"))
            {
                
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (child.transform.tag.Equals("Light"))
            {
                Debug.Log("thing");
                child.gameObject.GetComponent<Light2D>().color = Color.red;
            }
        }

        foreach (Transform child in roomList[roomList.Count - 3].GetComponent<Room>().transform)
        {
            if (child.transform.tag.Equals("Wall"))
            {
                
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            if (child.transform.tag.Equals("Light"))
            {
                Debug.Log("thing");
                child.gameObject.GetComponent<Light2D>().color = Color.yellow;
            }
        }
    }
}
