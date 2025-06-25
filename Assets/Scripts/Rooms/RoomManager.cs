using UnityEngine;
using System;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Room[] rooms;
    [SerializeField] Transform camTr, playerTr;

    int curId;

    public static Action OnReset;
    public static Action OnRoomLoad;

    /*void Start()
    {
        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[curId].roomObj.SetActive(false);
        }
    }*/

    void Update()
    {
        camTr.position = Vector3.Lerp(camTr.position, rooms[curId].cameraRoomPivot.position, .15f);
        if (curId == 0)
            return;
        if (Vector3.Distance(rooms[curId].cameraRoomPivot.position, camTr.position) <= .97f && rooms[curId - 1].roomObj.activeSelf)
        {
            rooms[curId-1].roomObj.SetActive(false);
        }
    }

    public void LoadNextRoom()
    {
        curId++;
        rooms[curId].roomObj.SetActive(true);
        OnRoomLoad?.Invoke();
        playerTr.position = rooms[curId].playerStartingPoint.position;
    }

    public void ResetRoom()
    {
        OnReset?.Invoke();
        playerTr.position = rooms[curId].playerStartingPoint.position;
    }
}

[System.Serializable]
public class Room
{
    public GameObject roomObj;
    public Transform cameraRoomPivot, playerStartingPoint;
}
