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
    [SerializeField] private int orderDelay = 7;

    // Keep track of orders
    public List<List<string>> orders = new List<List<string>>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Orders", 2, orderDelay);
    }

    private void Orders() {
        // Values to be excluded
        List<int> exclude = new List<int>();

        // Create an order of 3 unique random colors.
        // Random numbers are checked against exclude to make sure they are unique. 
        // Sloppy, change if possible.
        List<string> currOrder = new List<string>();
        for (int i = 0; i < 3; i++) {
            int randIndex = Random.Range(0, colors.Length);
            while (exclude.Contains(randIndex)) {
                randIndex = Random.Range(0, colors.Length);
            }
            exclude.Add(randIndex);
            currOrder.Add(colors[randIndex]);
        }

        orders.Add(currOrder);
        orderText.text += orderToString(currOrder);
    }

    private string orderToString(List<string> currOrder) {
        string orderString = " [";

        foreach (string color in currOrder) {
            orderString += " " + color;
        }

        orderString += " ]";
        return orderString;
    }
}
