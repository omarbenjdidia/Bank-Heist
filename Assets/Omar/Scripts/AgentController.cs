using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    private NavMeshAgent agentNav;
    public Transform[] destinations;
    private int currentDestinationIndex = 0;
    public float detectionRadius = 3f;
    public Material lineMaterial;
    private bool playerDetected;
    private bool distractionDetected;
    private LineRenderer lineRenderer;
    private float normalSpeed = 1f;
    private float boostedSpeed = 1.25f;

    public GameObject objectToInstantiate;
    private int maxClicks = 0;
    public Vector3 Distraction_position;
    private Animator agentanim;
    private GameObject _distraction;
    private bool cbon;


    void Start()
    {
        agentNav = GetComponent<NavMeshAgent>();
        NavigateToDestination(currentDestinationIndex);
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        agentanim = GetComponent<Animator>();
    }

    void Update()
    {
        AgentDetectionRange();
        DrawDetectionDisk();
        Check_If_Destination_Reached();
        Distraction_Instance();
    }

    private void Distraction_Instance()
    {
        if (Input.GetMouseButtonDown(0) && maxClicks < 3)
        {
            _distraction= Instantiate(objectToInstantiate, Distraction_position, Quaternion.identity);
            maxClicks++;
        }
    }

    private void Check_If_Destination_Reached()
    {
        if (agentNav.remainingDistance < 0.1f && !agentNav.pathPending)
        {
            currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;
            NavigateToDestination(currentDestinationIndex);
          
        }
    }

    private void AgentDetectionRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Collider[] colliders1 = Physics.OverlapSphere(transform.position, detectionRadius + 10f);

        bool playerInRange = false;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                //Debug.Log("Player detected!");
                float distanceToPlayer = Vector3.Distance(transform.position, collider.gameObject.transform.position);
                if (distanceToPlayer <= detectionRadius)
                {
                    agentNav.SetDestination(collider.gameObject.transform.position);
                    playerInRange = true;
                    playerDetected = true;
                    distractionDetected = false;
                }
            }
        }

        foreach (Collider collider1 in colliders1)
        {
            if (collider1.CompareTag("Distraction") && collider1.gameObject != null)
            {
                //Debug.Log("Distraction detected!");
                agentNav.SetDestination(Distraction_position);
                distractionDetected = true;
                Destroy(collider1.gameObject, 10f);
                if (_distraction == null)
                {
                    NavigateToDestination(currentDestinationIndex);
                    distractionDetected = false;
                }
            }
        }

        if (playerDetected || distractionDetected)
        {
            if (playerInRange)
            {
                agentNav.speed = boostedSpeed;
                agentanim.SetBool("reached", true);
                agentanim.SetBool("patrolling", false);
            }
            else
            {
                agentNav.speed = normalSpeed;
                agentanim.SetBool("reached", false);
                agentanim.SetBool("patrolling", true);
            }
        }
        else
        {
            agentNav.speed = normalSpeed;
            agentanim.SetBool("reached", false);
            agentanim.SetBool("patrolling", true);
        }
    }

    private void NavigateToDestination(int destinationIndex)
    {
        agentNav.SetDestination(destinations[destinationIndex].position);
    }

    private void DrawDetectionDisk()
    {
        float circumference = 2 * Mathf.PI * detectionRadius;
        int numSegments = Mathf.RoundToInt(circumference / 0.1f);
        numSegments = Mathf.Max(numSegments, 3);
        float angleDelta = 360f / numSegments;
        Vector3[] positions = new Vector3[numSegments + 1];
        positions[0] = transform.position;
        for (int i = 1; i <= numSegments; i++)
        {
            float angle = (i - 1) * angleDelta;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * detectionRadius;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * detectionRadius;
            positions[i] = transform.position + new Vector3(x, 0f, z);
        }
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

}