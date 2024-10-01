using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdvanceGhostAi : MonoBehaviour
{
    [System.Serializable]
    public class SoundEvent
    {
        public Vector3 position;
        public float timeStamp;
    }

    public enum GhostState
    {
        Patrolling,
        Chasing,
        Investigating
    }

    [Header("Movement")]
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float rotationSpeed = 120f;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float hearingRadius = 15f;
    [SerializeField] private float fieldOfView = 120f;  // Updated to 120 degrees
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int numberOfRays = 5;  // New variable for number of rays
    [SerializeField] private float rayHeight = 5f;

    [Header("Scare Mechanics")]
    [SerializeField] private float scareRadius = 5f;
    [SerializeField] private AudioClip[] scarySounds;
    [SerializeField] private float minTimeBetweenScares = 5f;
    [SerializeField] private float maxVolume = 1f;  // Maximum volume for the scary sound
    [SerializeField] private float minVolume = 0.1f;

    private NavMeshAgent agent;
    public Transform player;
    private AudioSource audioSource;
    private GhostState currentState;
    private int currentWaypointIndex;
    private Queue<SoundEvent> soundEvents = new Queue<SoundEvent>();
    private float lastScareTime;
    public bool isCaught = false;
    private bool isPlayingScarySound = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        ConfigureAudioSource();
    }

    private void Start()
    {
        currentState = GhostState.Patrolling;
        currentWaypointIndex = 0;
        SetNextPatrolPoint();
        if (player == null)
        {
            Debug.LogError("Player Transform not assigned. Attempting to find player.");
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                Debug.Log("Player found and assigned.");
            }
            else
            {
                Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
            }
        }
        else
        {
            Debug.Log("Player Assigned");
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case GhostState.Patrolling:
                Patrol();
                break;
            case GhostState.Chasing:
                ChasePlayer();
                break;
            case GhostState.Investigating:
                Investigate();
                break;
        }
        RestartTheAi();
        CheckForPlayerVisibility();
        ProcessSoundEvents();
        CheckForScareRadius();
    }


    private void Patrol()
    {
        if (agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            SetNextPatrolPoint();
        }
    }

    private void SetNextPatrolPoint()
    {
        if (waypoints.Count > 0)
        {
            agent.speed = patrolSpeed;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        RotateTowards(player.position);
        Debug.Log($"Chasing player. Ghost position: {transform.position}, Player position: {player.position}");
    }

    private void Investigate()
    {
        if (agent.remainingDistance < 0.5f)
        {
            currentState = GhostState.Patrolling;
            SetNextPatrolPoint();
        }
    }

    private void CheckForPlayerVisibility()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < fieldOfView / 2)
            {
                if (CastMultipleRays())
                {
                    Debug.Log("Player detected! Switching to chase state.");
                    currentState = GhostState.Chasing;
                    isCaught = true;
                }
                else
                {
                    Debug.Log("Player not detected or blocked by obstacle.");
                    isCaught = false;
                }
            }
        }
            Debug.Log($"Current Ghost State: {currentState}");

    }
    private bool CastMultipleRays()
    {
        Vector3 rayStart = transform.position + Vector3.up * rayHeight;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = (fieldOfView / (numberOfRays - 1)) * i - fieldOfView / 2;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(rayStart, direction, out hit, detectionRadius, playerLayer | obstacleLayer))
            {
                Debug.Log($"Raycast {i} hit: {hit.transform.name}, Layer: {LayerMask.LayerToName(hit.transform.gameObject.layer)}");
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Character"))
                {
                    // Player detected and not blocked by obstacle
                    return true;
                }
            }
        }

        // Player not detected by any of the three raycasts
        return false;
    }
    private void ConfigureAudioSource()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.spatialBlend = 0.8f;
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, AnimationCurve.Linear(0, 1, 1, 0));
        audioSource.spatialBlend = 1f;  // 1 = full 3D sound
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = scareRadius;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spread = 60f;
        audioSource.priority = 0;
        audioSource.maxDistance = scareRadius * 1.5f;
        audioSource.minDistance = 1f;
    }



    private void ProcessSoundEvents()
    {
        while (soundEvents.Count > 0 && Time.time - soundEvents.Peek().timeStamp > 5f)
        {
            soundEvents.Dequeue();
        }

        if (soundEvents.Count > 0 && currentState != GhostState.Chasing)
        {
            SoundEvent soundToInvestigate = soundEvents.Dequeue();
            currentState = GhostState.Investigating;
            agent.SetDestination(soundToInvestigate.position);
        }
    }

    private void CheckForScareRadius()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= scareRadius)
        {
            if (!isPlayingScarySound)
            {
                PlayScarySound();
            }

            // Adjust volume based on distance
            float volumeLevel = Mathf.Lerp(maxVolume, minVolume, distanceToPlayer / scareRadius);
            audioSource.volume = volumeLevel;
        }
        else
        {
            if (isPlayingScarySound)
            {
                StopScarySound();
            }
        }
    }

    private void PlayScarySound()
    {
        if (scarySounds.Length > 0 && !audioSource.isPlaying&&currentState==GhostState.Patrolling)
        {
            AudioClip clipToPlay = scarySounds[Random.Range(0, scarySounds.Length)];
            audioSource.clip = clipToPlay;
            audioSource.Play();
            isPlayingScarySound = true;
            lastScareTime = Time.time;
        }
    }
    private void StopScarySound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            isPlayingScarySound = false;
        }
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    public void HearSound(Vector3 soundPosition)
    {
        if (Vector3.Distance(transform.position, soundPosition) <= hearingRadius)
        {
            soundEvents.Enqueue(new SoundEvent { position = soundPosition, timeStamp = Time.time });
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scareRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(fieldOfView / 2, transform.up) * transform.forward * detectionRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fieldOfView / 2, transform.up) * transform.forward * detectionRadius;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        // Draw the three raycasts
        Vector3 rayStart = transform.position + Vector3.up * rayHeight;
        Gizmos.color = Color.red;
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = (fieldOfView / (numberOfRays - 1)) * i - fieldOfView / 2;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            Gizmos.DrawRay(rayStart, direction * detectionRadius);
        }
    }
    public void RestartTheAi()
    {
        if(DeathScreen.Instance.oneTimeDeath)
        {
            currentState = GhostState.Patrolling;
            DeathScreen.Instance.oneTimeDeath = false;
        }
        else {
            return;
        }
    }
}

