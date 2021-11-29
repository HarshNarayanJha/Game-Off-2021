using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bug : MonoBehaviour
{
    [SerializeField] private float health;

    [Header("Signals Broadcasting On")]
    [SerializeField] private VoidSignalSO fadeInSignal;
    [SerializeField] private VoidSignalSO fadeOutSignal;
    [SerializeField] private VoidSignalSO restartLevelSignal;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    [Header("Others")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private BoxCollider2D boxCollider;

    private MaterialPropertyBlock materialPropertyBlock;
    private SpriteRenderer spriteRenderer;
    private bool isDissolving;
    private float fade;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();    
        spriteRenderer = GetComponent<SpriteRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();

        isDissolving = false;
        fade = 1;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDissolving)
            return;
            
        if (other.gameObject.CompareTag("Player"))
        {
            inputReader.DisablePlayerInput();
            other.gameObject.GetComponent<PlayerController>().Kill();
            boxCollider.enabled = false;

            StartCoroutine(RestartLevel());
        }
    }

    public void Damage(float amount)
    {
        if ((health - amount) < 0)
            health = 0;
        else
            health -= amount;

        if (health == 0)
            Kill();
    }

    private void Update()
    {
        if (isDissolving)
        {
            spriteRenderer.GetPropertyBlock(materialPropertyBlock);
            fade -= Time.deltaTime * 2f;
             if (fade <= 0f)
             {
                fade = 0;
                isDissolving = false;
                gameObject.SetActive(false);
             }

            materialPropertyBlock.SetFloat("_Fade", fade);
            spriteRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }

    private void Kill()
    {
        // Debug.Log(gameObject.name + " Was Killed by you");
        isDissolving = true;
        // gameObject.SetActive(false);
    }

    private IEnumerator RestartLevel()
    {
        impulseSource.GenerateImpulse();
        yield return new WaitForSeconds(0.8f);

        fadeInSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);
        restartLevelSignal.RaiseSignal();
        yield return new WaitForSeconds(0.1f);
        fadeOutSignal.RaiseSignal();
        yield return new WaitForSeconds(0.5f);

        boxCollider.enabled = true;
        inputReader.EnablePlayerInput();
    }
}
