using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;
//using System;

public class GhostScript : MonoBehaviour
{
    public GameObject[] destArray;
    private NavMeshAgent agent;
    public GameObject deathVideoPlayer;
    private float deathTimer;
    private float generalTimer = 0f;
    private bool playVideo;
    public float triggerDistance = 10f;
    public Camera cameraView;
    public GameObject ghostObject;

    public enum GhostType {Spirit, Mare, Phantom, Wraith, Demon, Poltergheist};
    public GhostType ghostType;
    //[orbs, writing, fingerprints, freezing, spiritbox, emf]
    private bool[] currentEvidence = new bool[6];

    public GameObject[] evidencePrefabs;
    public float eventTimer = 0;

    public PostProcessVolume ppv;
    private Grain grain;
    private ChromaticAberration chAbb;

    public GameObject favRoom;
    public float idleTimer = 0f;
    private bool ghostIdle = false;
    public GameObject targetRoom;
    public GameObject currentRoom;

    private bool isTargetingPlayer;
    public GameObject playerObject;

    public bool isHunting = false;
    private bool huntingTimer;

    public AudioSource audioSrc;
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    public bool deathable = true;

    private Vector3 spawnPoint = new Vector3(0f, 7f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        ppv.profile.TryGetSettings(out grain);
        ppv.profile.TryGetSettings(out chAbb);
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        switch(Random.Range(0, 5)){
            case 0: ghostType = GhostType.Spirit; currentEvidence[1] = true; currentEvidence[4] = true; currentEvidence[5] = true; break;
            case 1: ghostType = GhostType.Mare; currentEvidence[0] = true; currentEvidence[2] = true; currentEvidence[3] = true; break;
            case 2: ghostType = GhostType.Phantom; currentEvidence[0] = true; currentEvidence[2] = true; currentEvidence[5] = true; break;
            case 3: ghostType = GhostType.Wraith; currentEvidence[1] = true; currentEvidence[3] = true; currentEvidence[4] = true; break;
            case 4: ghostType = GhostType.Demon; currentEvidence[0] = true; currentEvidence[1] = true; currentEvidence[3] = true; break;
            case 5: ghostType = GhostType.Poltergheist; currentEvidence[1] = true; currentEvidence[2] = true; currentEvidence[5] = true; break;
            default: ghostType = GhostType.Spirit; currentEvidence[1] = true; currentEvidence[4] = true; currentEvidence[5] = true; break;
        }


        favRoom = destArray[Random.Range(0, destArray.Length)];
        agent.destination = favRoom.transform.position;

        currentRoom = favRoom;
    }

    // Update is called once per frame
    void Update()
    {
        generalTimer += Time.deltaTime;
        //if(generalTimer <= 15f) return;   //peaceful timer

        if(isHunting)
        {
            if(Physics.Raycast(ghostObject.transform.position, playerObject.transform.Find("AgentTarget").transform.position - ghostObject.transform.position, out RaycastHit hit, 30f) && hit.collider.CompareTag("Player"))
            {
                Debug.Log(hit.collider.gameObject.name);
                agent.destination = playerObject.transform.position;
                idleTimer = generalTimer;
                isTargetingPlayer = true;
            }
            else isTargetingPlayer = false;
            //Camera effects
            if(isTargetingPlayer){
                float playerDistance = GhostToPlayerDistance(triggerDistance);
                grain.intensity.value = playerDistance;
                grain.size.value = 2 * playerDistance;
                chAbb.intensity.value = playerDistance;
                audioSrc.volume = playerDistance / 2;
                noise.m_AmplitudeGain  = playerDistance / 2;
                noise.m_FrequencyGain = playerDistance * 3;
            }
        }

        //random roaming
        if(agent.remainingDistance <= 1f && !ghostIdle){  //is roaming
            idleTimer = generalTimer;
            ghostIdle = true;
        }

        if(generalTimer - idleTimer >= (targetRoom == favRoom ? 6f : 3f) && ghostIdle) //idle in room timer
        {
            Roam();
            idleTimer = 0f;
            ghostIdle = false;
        }

        //actions in favourite room
        if(favRoom == currentRoom)
        {
            eventTimer += Time.deltaTime;
            if(currentEvidence[0]){
                //instantiate a ghost orb preset
                if(Random.Range(0, 1000) * eventTimer * 0.03f >= 999){
                    Destroy(Instantiate(evidencePrefabs[0], ghostObject.transform.position, new Quaternion(1f,0f,0f,0f)), 10f);
                    eventTimer = 0;
                }
            }
        }

        //quit after death
        if(playVideo && generalTimer - deathTimer >= 3f){
            Application.Quit();
            deathVideoPlayer.SetActive(false);
            playVideo = false;
        }
        
        //ghost blinking
        if((int)generalTimer % 3 == 0) 
            ghostObject.SetActive(true);
        else if((int)generalTimer % 3 == 1)
            ghostObject.SetActive(false);
        
        
    }

    private float GhostToPlayerDistance(float startDistance)
    {
        float distance = agent.remainingDistance;
        if(distance > startDistance || distance <= 0f) return 0;
        return 1 - (distance / startDistance);
    }

    private void Roam()
    {
        int range = Random.Range(0, (int)(destArray.Length * 3.5f));
        agent.destination = (range >= destArray.Length ? favRoom : destArray[range]).transform.position + new Vector3(Random.Range(1f, 3f), 0f, Random.Range(1f, 3f));
        targetRoom = (range >= destArray.Length ? favRoom : destArray[range]);
    }

    public void StartHunt()
    {
        isHunting = true;
        agent.speed = 2.8f;
    }
    public void StopHunt()
    {
        isHunting = false;
        agent.speed = 1.5f;
        isTargetingPlayer = false;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player") && deathable){
            deathVideoPlayer.SetActive(true);
            deathTimer = generalTimer;
            playVideo = true;
        }
        else if(coll.gameObject.CompareTag("Room"))
        {
            currentRoom = coll.gameObject;
        }
    }

    bool IsTargetVisible(Camera c, GameObject go)
     {
         var planes = GeometryUtility.CalculateFrustumPlanes(c);
         var point = go.transform.position;
         foreach (var plane in planes)
         {
             if (plane.GetDistanceToPoint(point) < 0)
                 return false;
         }
         return true;
     }
}