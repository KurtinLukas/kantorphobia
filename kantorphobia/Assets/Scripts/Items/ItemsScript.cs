using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsScript : MonoBehaviour
{
    public Vector3 targetPosition;
    private bool basePosition = false;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetPosition;
    }
}
