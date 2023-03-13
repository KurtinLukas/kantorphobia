using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Light flashlight;
    public AudioSource flashAudio;
    public GameObject ghostObject;
    public GameObject playerRoom;
    private GhostScript GS;

    [Range(0,100)] public float sanity = 100f;
    public float sanityDrain = 7.2f; //per minute

    private bool flashOn = true;

    private float generalTimer;

    // Start is called before the first frame update
    void Start()
    {
        GS = ghostObject.GetComponent<GhostScript>();
    }

    // Update is called once per frame
    void Update()
    {
        generalTimer += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.T)){
            if(flashOn)
                {
                    flashlight.intensity = 0;
                    sanityDrain *= 1.5f;
                }
            else
                {
                    flashlight.intensity = 2;
                    sanityDrain /= 1.5f;
                }
            flashOn = !flashOn;
            flashAudio.Play();
        }

        sanity = Mathf.Clamp(sanity - (sanityDrain/60f) * Time.deltaTime, 0, 100);
        if(generalTimer > 1f)
        {
            generalTimer = 0f;
            
            if(!GS.isHunting && (Random.Range(0f, 10000f)) <= 100f - sanity)
            {
                GS.StartHunt();
            }
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.CompareTag("Room"))
        {
            playerRoom = coll.gameObject;
            
        }
    }
}
