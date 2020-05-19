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

    private bool crazyIsActive = true;

    // Color storage list
    List<string> colorStorage = new List<string>();

    private void Start() {
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
        if (collision.gameObject) {
            UpdateColors(collision.gameObject.name);
        }
    }

    private void UpdateColors(string color) {
        //Update color storage and text.
        if (colorStorage.Count < 3) {
            colorStorage.Add(color);
            playerColorsText.text += " " + color;
        }
    }
}
