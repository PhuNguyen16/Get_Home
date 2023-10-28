﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    public Transform holdSpot;
    public LayerMask pickUpMask;
    public float pickupRadius = 0.9f;

    public Vector3 Direction {get; set;}
    private GameObject itemHolding;
    public float placeDownOffSet = 0f;
    private Vector3 yOffset = new Vector3 (0.0f, -0.3f, 0.0f);

    //sound FX
    public AudioSource pickUpTrashSound;
    public AudioSource throwTrashSound;

    // public GameObject DestroyEffect;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Direction);
        if (Input.GetKeyDown(KeyCode.T))
        {

            if (itemHolding)
            {
                if (Mathf.Abs(Direction.x) > 0)
                {
                    //sound FX
                    throwTrashSound.Play();

                    itemHolding.transform.parent = null;
                    itemHolding.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    itemHolding.GetComponent<ThrownObjectsHitDetect>().thrown = true;

                    itemHolding.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                    itemHolding.GetComponent<Rigidbody2D>().AddForce(Direction * 1000);
                    GetComponent<PlayerCombatMelee>().enabled = true;
                    
                    if (itemHolding.GetComponent<Rigidbody2D>())
                        itemHolding.GetComponent<Rigidbody2D>().simulated = true;
                    Destroy(itemHolding, 0.5f);
                    itemHolding = null;
                }
                else
                {
                    //sound FX
                    pickUpTrashSound.Play();

                    if(GetComponent<PlayerMovement>().lookingRight == true)
                    {
                        itemHolding.transform.position = new Vector3(holdSpot.position.x, holdSpot.position.y - placeDownOffSet, 0.0f) + new Vector3(0.6f, 0.3f, 0);
                        GetComponent<PlayerCombatMelee>().enabled = true;
                        itemHolding.transform.parent = null;
                        if (itemHolding.GetComponent<Rigidbody2D>())
                            itemHolding.GetComponent<Rigidbody2D>().simulated = true;
                        itemHolding = null;
                    }
                    else
                    {
                        itemHolding.transform.position = new Vector3(holdSpot.position.x, holdSpot.position.y - placeDownOffSet, 0.0f) + new Vector3(-0.6f, 0.3f, 0);
                        GetComponent<PlayerCombatMelee>().enabled = true;
                        itemHolding.transform.parent = null;
                        if (itemHolding.GetComponent<Rigidbody2D>())
                            itemHolding.GetComponent<Rigidbody2D>().simulated = true;
                        itemHolding = null;
                    }
                  
                }
                
            }
            else
            {
                Collider2D pickUpItem = Physics2D.OverlapCircle(transform.position, pickupRadius, pickUpMask);
                if (pickUpItem)
                {
                    itemHolding = pickUpItem.gameObject;
                    itemHolding.transform.position = holdSpot.position;
                    itemHolding.transform.parent = transform;
                    GetComponent<PlayerCombatMelee>().enabled = false;
                    if (itemHolding.GetComponent<Rigidbody2D>())
                        itemHolding.GetComponent<Rigidbody2D>().simulated = false;
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (holdSpot == null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

}
