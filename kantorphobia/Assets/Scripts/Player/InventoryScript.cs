using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public GameObject[] inventory = new GameObject[3];
    private int itemCount = 0;
    public Camera cam;
    public GameObject hand;
    private int itemInHand = 0;
    public GameObject emptyGameObject;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++)
            inventory[i] = emptyGameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 3f, LayerMask.GetMask("Ground","Item"))) //, LayerMask.NameToLayer("Ground")
            {
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Item")){
                    if(itemCount < 3){
                        itemInHand = itemCount;
                        itemCount++;
                        inventory[itemInHand] = hit.collider.transform.parent.gameObject;
                    }
                }
                else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")){
                    if(itemCount > 0){
                        itemCount--;
                        inventory[itemInHand].transform.position = hit.point;
                        Debug.Log(inventory[itemInHand].transform.rotation.y);
                        inventory[itemInHand].transform.rotation = Quaternion.Euler(0f, inventory[itemInHand].transform.eulerAngles.y, 0f);
                        inventory[itemInHand] = emptyGameObject;
                        if(itemInHand > 0) itemInHand--;
                    }
                }
            }
        }
        foreach(GameObject item in inventory){
            if(inventory[itemInHand] == item){
                item.transform.SetPositionAndRotation(hand.transform.position, hand.transform.rotation);
            }
            else item.transform.position = new Vector3(0f, 0f, 0f);
        }
    }
}
