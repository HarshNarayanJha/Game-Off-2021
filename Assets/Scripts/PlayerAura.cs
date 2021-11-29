using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAura : MonoBehaviour
{
    [Header("Aura Settings")]
    [SerializeField] private float auraPower;
    [SerializeField] private float auraRange;
    [SerializeField] private LayerMask bugLayerMask;
    [SerializeField] private GameObject auraObject;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    Collider2D[] hitBugsColliders;
    private bool canUseAura;
    private bool isaAuraActive;

    public bool IsAuraActive { get => isaAuraActive; }

    private void Start()
    {
        hitBugsColliders = new Collider2D[5];
        isaAuraActive = false;
    }

    private void OnEnable()
    {
        inputReader.spaceEvent += ActivateAura;
        inputReader.spaceCanceledEvent += DeactivateAura;
    }

    private void OnDisable()
    {
        inputReader.spaceEvent -= ActivateAura;
        inputReader.spaceCanceledEvent -= DeactivateAura;
    }

    private void Update()
    {
        if (!canUseAura)
        {
            DeactivateAura();
            return;
        }
        if (isaAuraActive)
        {
            int num = Physics2D.OverlapCircleNonAlloc(transform.position, auraRange, hitBugsColliders, bugLayerMask);
            // Debug.Log(num + " Bugs in Aura Range");
            if (num == 0)
                return;

            for (int i=0; i < num; i++)
            {
                hitBugsColliders[i].GetComponent<Bug>().Damage(this.auraPower);
            }

            hitBugsColliders = new Collider2D[5];
        }
    }

    private void ActivateAura()
    {
        if (!canUseAura)
            return;

        auraObject.SetActive(true);
        isaAuraActive = true;
    }

    private void DeactivateAura()
    {
        auraObject.SetActive(false);
        isaAuraActive = false;
    }

    public void SetCanUseAura(bool canUse)
    {
        canUseAura = canUse;
    }

    private void OnDrawGizmosSelected()
    {
        Color origColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, auraRange);
        Gizmos.color = origColor;
    }
}
