using UnityEngine;

public class RoomChanger : MonoBehaviour
{
    RoomManager roman;

    void Start()
    {
        roman = FindAnyObjectByType<RoomManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            roman.LoadNextRoom();
        }
    }
}
