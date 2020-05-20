﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Movement
    public float speed = 5f;
    private Rigidbody2D rb;
    Vector2 movement;

    // Dash
    public float boostForce;
    private bool boostBool = true;

    //References to game objects
    public GameObject canvas;
    public TextMeshProUGUI playerColorsText;
    public GameObject crazyBuckets;
    public GameObject eventSystem;
    private GameManager gameManagerScript;

    // Color storage and ref to currColor being touched.
    List<string> colorStorage = new List<string>();
    private string currColor;
    
    //Booleans
    private bool crazyIsActive = true;
    private bool touchPaint = false;

    private void Start() {
        gameManagerScript = eventSystem.GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() { 
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //View switch
        if (Input.GetKeyDown("space")) {
            if (crazyIsActive) {
                crazyBuckets.SetActive(false);
                crazyIsActive = false;
            } else {
                crazyBuckets.SetActive(true);
                crazyIsActive = true;
            }
        }

        //Interact
        if (Input.GetKeyDown("j") && touchPaint) {
            UpdateColors(currColor);
        }

        //Dump paint
        if (Input.GetKeyDown("k")) {
            colorStorage.Clear();
            playerColorsText.text = "Colors:";
        }

        //Boost
        if (Input.GetKeyDown("l")) {
            boostBool = true;
        }
    }

    void FixedUpdate() {
        //Movement
        if (crazyIsActive) {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            if (boostBool) {
                rb.MovePosition(rb.position + movement * boostForce);
                boostBool = false;
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject && collision.gameObject != canvas) {
            touchPaint = true;
            currColor = collision.gameObject.name;
        } else if (collision.gameObject == canvas) {
            SendOrder();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject != canvas) {
            touchPaint = false;
        }
    }

    private void UpdateColors(string color) {
        //Update color storage and text.
        if (colorStorage.Count < 3) {
            colorStorage.Add(color);
            playerColorsText.text += " " + color;
        }
    }

    private void SendOrder() {
        List<List<string>> orderList = gameManagerScript.orders;

        // If colors are 3 and equal to the top order, deliver.
        // If any color is not present in the order, break;
        bool deliverStatus = false;
        if (colorStorage.Count == 3) {
            for (int i = 0; i < colorStorage.Count; i++) {
                if (orderList[0][i].Equals(colorStorage[i])) {
                    deliverStatus = true;
                } else {
                    deliverStatus = false;
                    break;
                }
            }
        }

        //Clear storage, reset colors text, remove top order, and adjust order text
        if (deliverStatus) {
            colorStorage.Clear();
            playerColorsText.text = "Colors:";
            orderList.Remove(orderList[0]);

            //Adjusting order text to remove top order.
            int i = 7;
            char c = gameManagerScript.orderText.text[i];
            while (!(c.Equals(']'))) {
                c = gameManagerScript.orderText.text[i];
                i++;
            }

            gameManagerScript.orderText.text = "Orders:" + gameManagerScript.orderText.text.Substring(i);
        }
    }
}
