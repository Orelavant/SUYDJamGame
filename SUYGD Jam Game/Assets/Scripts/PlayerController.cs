using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    public Rigidbody2D rb;

    Vector2 movement;

    //References to game objects
    public GameObject canvas;
    public TextMeshProUGUI playerColorsText;
    public GameObject crazyBuckets;
    public GameObject eventSystem;
    private GameManager gameManagerScript;

    // Color and orders storage
    List<string> colorStorage = new List<string>();
    
    private bool crazyIsActive = true;

    private void Start() {
        gameManagerScript = eventSystem.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() { 
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

    }

    void FixedUpdate() {
        //Movement
        if (crazyIsActive) {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }

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
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject && collision.gameObject != canvas) {
            UpdateColors(collision.gameObject.name);
        } else if (collision.gameObject == canvas) {
            SendOrder();
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
        // If colors are 3 and equal to the top order, deliver.
        // If any color is not present in the order, break;
        bool deliverStatus = false;
        if (colorStorage.Count == 3) {
            foreach(string color in colorStorage) {
                if (gameManagerScript.orders[0].Contains(color)) {
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
            gameManagerScript.orders.Remove(gameManagerScript.orders[0]);

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
