using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAura : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float auraPower;
    [SerializeField] private float auraRange;
    [SerializeField] private LayerMask bugLayerMask;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    Collider2D[] hitBugsColliders;

    private void Start()
    {
        hitBugsColliders = new Collider2D[5];
    }

    private void OnEnable()
    {
        inputReader.spaceEvent += ReleaseAura;
    }

    private void OnDisable()
    {
        inputReader.spaceEvent -= ReleaseAura;
    }


    private void ReleaseAura()
    {
        int num = Physics2D.OverlapCircleNonAlloc(transform.position, auraRange, hitBugsColliders, bugLayerMask);
        Debug.Log(num + " Bugs in Aura Range");
        if (num == 0)
            return;

        for (int i=0; i < num; i++)
        {
            hitBugsColliders[i].GetComponent<Bug>().Damage(auraPower);
        }

        hitBugsColliders = new Collider2D[5];
    }

    private void OnDrawGizmosSelected()
    {
        Color origColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, auraRange);
        Gizmos.color = origColor;
    }
}
