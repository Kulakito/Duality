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

    void Start()
    {
        vis = FindFirstObjectByType<PlayerVisibility>();

        StartCoroutine(LifeCycle());
    }

    void Update()
    {
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
        Perform();
        if (!isActive)
        {
            for (i = actTime; i > 0; i -= Time.deltaTime)
            {
                yield return null;
            }
        }
        else
        {
            for (i = 0; i < inactTime; i += Time.deltaTime)
            {
                yield return null;
            }
        }
        isActive = !isActive;
        StartCoroutine(LifeCycle());
    }

    protected override void Perform()
    {
        if (isActive)
        {
            rend.material = actMat;
            indicator.color = Color.red;
        }
        else
        {
            rend.material = inactMat;
            indicator.color = Color.green;
        }
    }

    public override void ResetTrap()
    {
        
    }

    void GameOver() // temp solution
    {
        print("RIP bozo");
    }
}
