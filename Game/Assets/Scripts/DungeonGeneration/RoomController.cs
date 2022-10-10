﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{

    public static RoomController instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    Room currRoom;

    bool roomClosed = false;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRooms = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        UpdateRoomQueue();
        foreach(Room room in loadedRooms)
            {
                room.RemoveUnconnectedDoors();
        }
        
    }

    void UpdateRoomQueue()
    {
        if(isLoadingRoom)
        {
            return;
        }

        if(loadRoomQueue.Count == 0)
        {
            if(!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            } 
            else if(spawnedBossRoom && !updatedRooms)
            {

                foreach(Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));

    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    public void LoadRoom( string name, int x, int y)
    {
        if(DoesRoomExist(x, y) == true)
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom( Room room)
    {
        if(!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
            );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if(loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }

    }

    public bool DoesRoomExist( int x, int y)
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom( int x, int y)
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[] {
            "Empty",
            "Basic"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;

        StartCoroutine(RoomCoroutine());
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    int count = 0;
    bool updated = false;
    public void UpdateRooms()
    {
        if(!updated){
        EnemyController[] enemies1 = currRoom.GetComponentsInChildren<EnemyController>();

        count = enemies1.Length;
        updated = true;
        }



        foreach(Room room in loadedRooms)
        {
            if(currRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies != null)
                {
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;

                    }

                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                        
                        door.open.enabled = true;
                        door.closed.enabled = false;
                    }
                }
                else
                {
                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                        
                        door.open.enabled = true;
                        door.closed.enabled = false;
                        

                    }
                }
            }
            else
            {
              EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();

              
                if(enemies.Length > 0)
                {
                    
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                    }
                    
                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                    
                        door.doorCollider.SetActive(true);
                        door.open.enabled = false;
                        door.closed.enabled = true;
                        
                    }
                    roomClosed = true;
                    if(enemies.Length == count){
                    FindObjectOfType<AudioManager>().Play("doorClose");
                    }

                }

                else
                {
                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                        door.open.enabled = true;
                        door.closed.enabled = false;
                        
                        FindObjectOfType<AudioManager>().Play("doorOpen");
                        updated = false;

                        
                    } 
                }

            }
        }
    }
}