using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool outline = false;
    Outline outlineObj;

    public bool isPickedUp = false;
    void Start()
    {
        outlineObj = gameObject.GetComponent<Outline>();
        outlineObj.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(outline){
            outlineObj.enabled = true;
        }
        else{
            outlineObj.enabled = false;
        }
    }
}
