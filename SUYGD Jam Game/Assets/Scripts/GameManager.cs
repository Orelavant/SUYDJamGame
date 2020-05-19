using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // References
    public string[] colors;
    public TextMeshProUGUI orderText;

    // Modulate for harder game
    int timer = 7;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Orders", 2, timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Orders() {

        // Values to be excluded
        List<int> exclude = new List<int>();

        // Create an order of 3 unique random colors.
        // Random numbers are checked against exclude to make sure they are unique. 
        // Sloppy, change if possible.
        orderText.text += "[";
        for (int i = 0; i < 2; i++) {
            int randIndex = Random.Range(0, colors.Length);
            while (exclude.Contains(randIndex)) {
                randIndex = Random.Range(0, colors.Length);
            }
            exclude.Add(randIndex);
            orderText.text += colors[randIndex] + " ";
        }
        // The last color is here to avoid space at the end when adding text.
        // Changed randIndex to Int due to scope error
        int randInt = Random.Range(0, colors.Length);
        while (exclude.Contains(randInt)) {
            randInt = Random.Range(0, colors.Length);
        }
        orderText.text += colors[randInt];
        orderText.text += "] ";
    }
}
