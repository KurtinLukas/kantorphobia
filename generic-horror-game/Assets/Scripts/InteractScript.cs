using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    public GameObject PlayerCamera;
    public GameObject ZaHando;
    public LayerMask layerMask;
    public float interactDistance = 5;

    Vector3 physicsBeamStart;
    Vector3 physicsBeamEnd;

    bool touchingInteractible = false;
    GameObject LastTouchedInteractible;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouching(touchingInteractible, LastTouchedInteractible);
    }

    void HandleTouching(bool isTouch, GameObject WhatTouch = null) {
        if(WhatTouch == null) return;
        if(!touchingInteractible) {
            switch(WhatTouch.tag){
            case "Item":
                WhatTouch.gameObject.GetComponent<KeyScript>().outline = false;
                break;
            case "NonInvItem":
                WhatTouch.gameObject.GetComponent<KeyScript>().outline = false;
                break;
            case "Interactible":
                break;
            }
            return; 
        }
        switch(WhatTouch.tag){
            case "Item":
                WhatTouch.gameObject.GetComponent<KeyScript>().outline = true;
                break;
            case "NonInvItem":
                WhatTouch.gameObject.GetComponent<KeyScript>().outline = true;
                break;
            case "Interactible":
                break;
        }
        if(!Input.GetKeyDown(KeyCode.F)) 
            return;
        switch(LastTouchedInteractible.tag){
            case "Item":
                //get to inventory idk
                Destroy(LastTouchedInteractible);
                break;
            case "NonInvItem":
                //pick up item not accessible through inventory
                WhatTouch.gameObject.GetComponent<KeyScript>().isPickedUp = true;
                LastTouchedInteractible.SetActive(false);
                break;
            case "Interactible":
                //open za door
                DoorScript ds = LastTouchedInteractible.GetComponent<DoorScript>();
                ds.isOpened = !ds.isOpened;
                break;
        }
    }

    void FixedUpdate() {
        physicsBeamStart = PlayerCamera.transform.position;// + PlayerCamera.transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(physicsBeamStart,PlayerCamera.transform.forward, out hit, interactDistance, layerMask)){
            LastTouchedInteractible = hit.collider.gameObject;
            ZaHando.SetActive(true);
            touchingInteractible = true;
        }
        else{
            ZaHando.SetActive(false);
            touchingInteractible = false;
        }
    }
}
