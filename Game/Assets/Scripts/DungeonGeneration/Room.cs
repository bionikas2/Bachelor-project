using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public float Width;
    public float Height;
    public int X;
    public int Y;

    private bool updatedDoors = false;

    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;
    public Door leftDoorw;
    public Door rightDoorw;
    public Door topDoorw;
    public Door bottomDoorw;

    public List<Door> doors = new List<Door>();

    // Start is called before the first frame update
    void Start()
    {

        Door[] ds = GetComponentsInChildren<Door>(); 
        foreach(Door d in ds)
        {
            doors.Add(d);
            switch(d.doorType)
            {
                case Door.DoorType.right:
                rightDoor = d;
                break;
                case Door.DoorType.left:
                leftDoor = d;
                break;
                case Door.DoorType.top:
                topDoor = d;
                break;
                case Door.DoorType.bottom:
                bottomDoor = d;
                break;                
                case Door.DoorType.rightw:
                rightDoorw = d;
                break;
                case Door.DoorType.leftw:
                leftDoorw = d;
                break;
                case Door.DoorType.topw:
                topDoorw = d;
                break;
                case Door.DoorType.bottomw:
                bottomDoorw = d;
                break;
            }
        }

        RoomController.instance.RegisterRoom(this);
    }

    void Update()
    {   
        if(name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    public void RemoveUnconnectedDoors()
    {

        foreach(Door door in doors)
        {

            switch(door.doorType)
            {

                case Door.DoorType.rightw:
                    if(GetRight() == null)
                        door.gameObject.SetActive(true);
                    else
                        door.gameObject.SetActive(false);
                break;

                case Door.DoorType.leftw:
                    if(GetLeft() == null)
                    door.gameObject.SetActive(true);
                else
                        door.gameObject.SetActive(false);
                break;

                case Door.DoorType.topw:
                if(GetTop() == null)
                    door.gameObject.SetActive(true);
                else
                        door.gameObject.SetActive(false);
                break;

                case Door.DoorType.bottomw:
                    if(GetBottom() == null)
                    door.gameObject.SetActive(true);
                    else
                        door.gameObject.SetActive(false);

                break;
            }
        }
    }

    public Room GetRight()
    {
        if(RoomController.instance.DoesRoomExist(X + 1, Y))
        {
            return RoomController.instance.FindRoom(X + 1, Y);
        }
        return null;
    }
    public Room GetLeft()
    {
        if(RoomController.instance.DoesRoomExist(X - 1, Y))
        {
            return RoomController.instance.FindRoom(X - 1, Y);
        }
        return null;
    }
    public Room GetTop()
    {
        if(RoomController.instance.DoesRoomExist(X, Y + 1))
        {
            return RoomController.instance.FindRoom(X, Y + 1);
        }
        return null;
    }
    public Room GetBottom()
    {
        if(RoomController.instance.DoesRoomExist(X, Y - 1))
        {
            return RoomController.instance.FindRoom(X, Y - 1);
        }
        return null;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3( X * Width, Y * Height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
}
