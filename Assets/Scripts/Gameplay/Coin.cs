using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private IntSignalSO coinCollectedSignal;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<CircleCollider2D>().enabled = false;
            coinCollectedSignal.RaiseSignal(points);
            gameObject.SetActive(false);
        }
    }
}
