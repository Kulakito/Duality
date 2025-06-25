using UnityEngine;
using UnityEngine.UI;
using System;

public class GhostPowerTracker : MonoBehaviour
{
    float _ghostPower;
    public float ghostPower
    {
        get { return _ghostPower; }
        set
        {
            if (value <= 0)
            {
                _ghostPower = 0;
                GameOver();
            }
            else
            {
                _ghostPower = value;
            }
        }
    }

    float powerAtRoomStart;

    [SerializeField] float maxPower = 1f;
    [SerializeField] float drainingSpeed = 1f;

    [SerializeField] Image meter;

    PlayerVisibility visibility;

    public static event Action OnGhostPowerEnd;

    private void OnEnable()
    {
        RoomManager.OnReset += ResetPower;
        RoomManager.OnRoomLoad += UpdatePower;
    }

    private void OnDisable()
    {
        RoomManager.OnReset -= ResetPower;
        RoomManager.OnRoomLoad -= UpdatePower;
    }

    void Start()
    {
        ghostPower = maxPower;
        powerAtRoomStart = ghostPower;
        visibility = FindFirstObjectByType<PlayerVisibility>();
    }

    void Update()
    {
        if (visibility.isGhost && ghostPower > 0f)
        {
            ghostPower -= Time.deltaTime * drainingSpeed;
        }
        meter.fillAmount = ghostPower / maxPower;
    }

    void GameOver()
    {
        print("Ur otta power");
        OnGhostPowerEnd?.Invoke();
    }

    void UpdatePower()
    {
        powerAtRoomStart = ghostPower;
    }

    void ResetPower()
    {
        ghostPower = powerAtRoomStart;
    }
}
