using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Room[] rooms;
    [SerializeField] Transform camTr, playerTr;

    int curId;

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
        playerTr.position = rooms[curId].playerStartingPoint.position;
        if (FindFirstObjectByType<GhostPowerTracker>() != null)
            FindFirstObjectByType<GhostPowerTracker>().UpdatePower();
    }

    public void ResetRoom()
    {
        foreach (Trap trap in rooms[curId].roomObj.transform.GetComponentsInChildren<Trap>())
        {
            trap.ResetTrap();
        }
        playerTr.position = rooms[curId].playerStartingPoint.position;
        if (FindFirstObjectByType<GhostPowerTracker>() != null)
            FindFirstObjectByType<GhostPowerTracker>().ResetPower();
    }
}

[System.Serializable]
public class Room
{
    public GameObject roomObj;
    public Transform cameraRoomPivot, playerStartingPoint;
}
