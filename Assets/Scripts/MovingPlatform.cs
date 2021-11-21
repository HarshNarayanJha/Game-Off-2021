using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] private Transform[] pathNodes;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float startDelay;
    [SerializeField] private float pauseTime;

    [Header("Signals Broadcasting On")]
    [SerializeField] private CamSignalSO camSignal;

    [Header("Others")]
    [SerializeField] bool startMoving;
    [SerializeField] private Rigidbody2D rigidBody;
    Vector2 currentTargetPosition;
    Vector2 position;
    int currentNode;

    private void Start()
    {
        currentTargetPosition = pathNodes[currentNode].position;

        position.x = rigidBody.position.x;
        position.y = rigidBody.position.y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("SetStartMoving", startDelay);
            camSignal.RaiseSignal(CameraController.CameraZoomState.Mid);
        }
    }

    private void FixedUpdate()
    {
        if (!startMoving)
            return;

        if (position != currentTargetPosition)
        {
            position = Vector2.MoveTowards(position, currentTargetPosition, moveSpeed);
            // Debug.Log("Moving Towards " + currentNode);
        } else {
            // Debug.Log("Currently On " + currentNode);
            
            if (currentNode < pathNodes.Length - 1)
            {
                startMoving = false;
                currentNode++;
                CheckNode();

            } else {
                startMoving = false;
                currentNode = 0;
                CheckNode();
            }
        }

        rigidBody.MovePosition(position);
    }

    private void CheckNode()
    {
        currentTargetPosition = pathNodes[currentNode].position;
        Invoke("SetStartMoving", pauseTime);
    }

    private void SetStartMoving()
    {
        startMoving = true;
    }
}
