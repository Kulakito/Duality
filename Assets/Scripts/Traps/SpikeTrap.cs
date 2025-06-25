using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpikeTrap : Trap
{
    [SerializeField] float actTime = 5f, inactTime = 1f; // actTime - время для активации; inactTime - время для деактивации
    [SerializeField] Material actMat, inactMat;
    [SerializeField] MeshRenderer rend;
    [SerializeField] Image indicator;

    [SerializeField] Vector3 boxShape;
    [SerializeField] LayerMask mask;

    PlayerVisibility vis;
    float i;
    bool isResetting;

    private void OnEnable()
    {
        RoomManager.OnReset += ResetTrap;
    }

    private void OnDisable()
    {
        RoomManager.OnReset -= ResetTrap;
    }

    void Start()
    {
        vis = FindFirstObjectByType<PlayerVisibility>();

        StartCoroutine(LifeCycle());
    }

    void Update()
    {
        if (isResetting)
            return;

        if (!isActive)
        {
            indicator.fillAmount = i / actTime;
        }
        else
        {
            indicator.fillAmount = i / inactTime;

            Collider[] cs = Physics.OverlapBox(transform.position, boxShape, Quaternion.identity, mask);
            foreach (Collider c in cs)
            {
                if (c.gameObject.tag == "Player" && vis.isGhost == effectGhost)
                {
                    GameOver();
                }
            }
        }
    }

    IEnumerator LifeCycle()
    {
        if (!isActive)
        {
            rend.material = inactMat;
            indicator.color = Color.green;
            for (i = actTime; i > 0; i -= Time.deltaTime)
            {
                if (isResetting)
                    break;
                yield return null;
            }
        }
        else
        {
            rend.material = actMat;
            indicator.color = Color.red;
            for (i = 0; i < inactTime; i += Time.deltaTime)
            {
                if (isResetting)
                    break;
                yield return null;
            }
        }
        if (!isResetting)
        {
            isActive = !isActive;
            StartCoroutine(LifeCycle());
        }
    }

    protected override void ResetTrap()
    {
        StartCoroutine(Res());
    }

    IEnumerator Res()
    {
        isResetting = true;
        StopCoroutine(LifeCycle());
        isActive = false;
        yield return null;
        isResetting = false;
        StartCoroutine(LifeCycle());
        yield return null;
    }

    void GameOver() // temp solution... or so?
    {
        print("RIP bozo");
        FindFirstObjectByType<RoomManager>().ResetRoom();
    }
}
