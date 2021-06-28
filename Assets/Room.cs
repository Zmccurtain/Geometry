using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    public Door[] doors;
    public string target;
    public Room previous;
    public bool isSpawnRoom = false;
    public bool aggro = false;
    public bool bossRoom = false;
    private bool NotSpawned = true;
    void Start()
    {
        Floor.roomList.Add(gameObject);
        if (!isSpawnRoom)
        {
            foreach (Door i in doors)
            {
                if (target.Equals(i.name))
                {
                    i.isSpawned = true;
                    transform.position -= i.transform.localPosition;
                    break;
                }
            }
            
        }
        if (Floor.roomList.Count >= Floor.maxRooms)
        {
            Invoke("MakeStatic", 1f);
        }
        
    }
    
    private void MakeStatic()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Room"))
        {
            Room room = collision.GetComponent<Room>();
            if (!(room == previous))
            {
                if (!CheckRight(room))
                {
                    Floor.roomList.Remove(gameObject);
                    Destroy(gameObject);
                }

            }
        }
        else if (collision.transform.tag.Equals("Player"))
        {
            aggro = true;
            if (bossRoom && NotSpawned)
            {
                Instantiate(Resources.Load("Next") as GameObject, transform);
                NotSpawned = false;
            }
            
            GlobalControl.SetLayerRecursively(gameObject, 11);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Player"))
        {
            aggro = false;
        }   
    }
    private bool CheckRight(Room room)
    {
        if (room != null)
        {
            var other = Floor.roomList.FindIndex(a => a == room.gameObject);
            var current = Floor.roomList.FindIndex(a => a == gameObject);

            if (other != -1)
            {
                if (other > current)
                {
                    return true;
                }
                return false;
            }
        }
        return true;
    }

    private void OnDestroy()
    {
        string spawnType = "";
        if (target.Equals("DoorTop"))
        {
            spawnType = "DoorBot";
        }
        if (target.Equals("DoorRight"))
        {
            spawnType = "DoorLeft";
        }
        if (target.Equals("DoorBot"))
        {
            spawnType = "DoorTop";
        }
        if (target.Equals("DoorLeft"))
        {
            spawnType = "DoorRight";
        }

        foreach (Door i in doors)
        {
            if (i != null)
            {
                if (i.name.Equals(spawnType))
                {
                    i.isSpawned = false;
                    return;
                }
            }
        }
    }
}
