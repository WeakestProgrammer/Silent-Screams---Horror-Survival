using AdvancedHorrorFPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] AdvanceGhostAi ghostAi;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject shakeCam;
    [SerializeField] GameObject gameUi;
    [SerializeField] GameObject fadeUi;
    [SerializeField] Transform player;
    [SerializeField] Transform ghost;
    [SerializeField] Transform playerSpawnPoint;
    [SerializeField] Transform ghostSpawnPoint;
    [SerializeField] GameObject endUi;
    [SerializeField] GameObject playerObj;
    public GameObject MainCam;
    public bool OneTime;
    public static DeathScreen Instance;
    public bool twoTimes;
    public bool thirdTime;
    public bool oneTimeDeath;
    private void Awake()
    {
        thirdTime = false;
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        oneTimeDeath = false;
        OneTime = false;
        twoTimes = false;
        shakeCam.gameObject.SetActive(false);
        gameUi.gameObject.SetActive(true);
        fadeUi.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name =="Character"&&!twoTimes&&ghostAi.isCaught)
        {
                ghostAi.enabled = false;
                agent.acceleration = 0;
                playerObj.GetComponent<FirstPersonController>().enabled = false;
                shakeCam.gameObject.SetActive(true);
                ShakeEfx.instance.start = true;
                gameUi.gameObject.SetActive(false);
                StartCoroutine(FadeUi());
                OneTime = false;            
                oneTimeDeath=true;
        }
        if(other.gameObject.name== "Character"&&twoTimes&&ghostAi)
        {
            shakeCam.gameObject.SetActive(true);
            ShakeEfx.instance.start = true;
            gameUi.gameObject.SetActive(false);
            StartCoroutine(WaitForEnd());
            endUi.gameObject.SetActive(true);

        }
    }
    IEnumerator FadeUi()
    {
        yield return new WaitForSeconds(3.5f);
        shakeCam.gameObject.SetActive(false);
        MainCam.SetActive(true);        
        fadeUi.gameObject.SetActive(true);       
        StartCoroutine(StartGame());
        
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5f);
        playerObj.GetComponent<FirstPersonController>().enabled=true;
        gameUi.gameObject.SetActive(true);
        player.transform.position = playerSpawnPoint.position;
        ghost.transform.position = ghostSpawnPoint.position;
        StartCoroutine(AiSpawning());    }
    IEnumerator AiSpawning()
    {
        yield return new WaitForSeconds(2f);
        ghostAi.enabled = true;
        agent.acceleration = 5;
        twoTimes = true;
    }
    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(5f);
        Time.timeScale = 0;
    }
}
