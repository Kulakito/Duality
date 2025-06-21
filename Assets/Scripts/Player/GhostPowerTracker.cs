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

    [SerializeField] float maxPower = 1f;
    [SerializeField] float drainingSpeed = 1f;

    [SerializeField] Image meter;

    PlayerVisibility visibility;

    public static event Action OnGhostPowerEnd;

    void Start()
    {
        ghostPower = maxPower;
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
}
