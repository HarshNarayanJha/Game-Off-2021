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
    [SerializeField] private CameraController.CameraZoomState targetCamState = CameraController.CameraZoomState.Mid;

    [Header("Signals Broadcasting On")]
    [SerializeField] private CamSignalSO camSignal;

    [Header("Signals Listening On")]
    [SerializeField] private VoidSignalSO restartLevelSignal;

    [Header("Others")]
    [SerializeField] bool startMoving;
    [SerializeField] private Rigidbody2D rigidBody;
    Vector2 currentTargetPosition;
    Vector2 position;
    int currentNode;

    private void OnEnable()
    {
        restartLevelSignal.OnSignalRaised += LevelRestarted;
    }

    private void OnDisable()
    {
        restartLevelSignal.OnSignalRaised -= LevelRestarted;
    }

    private void Start()
    {
        currentTargetPosition = pathNodes[currentNode].position;
        rigidBody.position = currentTargetPosition;

        position.x = rigidBody.position.x;
        position.y = rigidBody.position.y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("SetStartMoving", startDelay);
            camSignal.RaiseSignal(targetCamState);
            GetComponent<BoxCollider2D>().enabled = false;
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
        if (currentNode == 1)
            Invoke("SetStartMoving", 0f);
        else
            Invoke("SetStartMoving", pauseTime);
    }

    private void SetStartMoving()
    {
        startMoving = true;
    }

    private void LevelRestarted()
    {
        startMoving = false;
        currentNode = 0;
        currentTargetPosition = pathNodes[currentNode].position;
        position = currentTargetPosition;
        rigidBody.position = position;

        GetComponent<BoxCollider2D>().enabled = true;
    }
}
