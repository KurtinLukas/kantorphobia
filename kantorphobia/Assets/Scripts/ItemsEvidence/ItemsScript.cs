using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsScript : MonoBehaviour
{
    public GameObject playerHead;
    public bool placed = false;
    private bool basePosition = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown("Mouse 1")){
            //if(Physics.Raycast(playerHead.transform.position, playerHead.transform.forward, 5f, Layerout hit))
        }
    }
}
