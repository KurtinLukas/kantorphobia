using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject DoorKey;
    public Transform DoorTransform;
    public Transform HingeTransform;
    public bool isOpened = false;
    public float xSpeed = 0.5f;
    private float startingAngle;

    float rotation = 0f;
    float xVal = 0f;

    public enum rotationFunction {
        linear,
        exponential,
        reverseExponentioal
    }

    public rotationFunction func;

    // Start is called before the first frame update
    void Start()
    {
        startingAngle = gameObject.transform.eulerAngles.y;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() 
    {
        if(DoorKey && !DoorKey.GetComponent<KeyScript>().isPickedUp)
        {
            isOpened = false;
            return;
        }
        
        if(isOpened){
            xVal = Mathf.Clamp01(rotation + xSpeed * Time.fixedDeltaTime);
        }
        else{
            xVal = Mathf.Clamp01(rotation - xSpeed * Time.fixedDeltaTime);
        }
        switch(func){
            case rotationFunction.linear:
                rotation = xVal;
                break;
            case rotationFunction.exponential:
                rotation = xVal * xVal;
                break;
            case rotationFunction.reverseExponentioal:
                rotation = Mathf.Sqrt(xVal);
                break;
        }
        transform.RotateAround(HingeTransform.position, new Vector3(0f,1f,0f), rotation * 90 - transform.localRotation.eulerAngles.y + startingAngle);
    }

    public void Open(){
        
        
    }
}
