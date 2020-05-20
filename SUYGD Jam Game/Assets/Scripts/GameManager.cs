using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // References
    public string[] textColors;
    public TextMeshProUGUI orderText;

    // Modulate for harder game
    [SerializeField] private int orderDelay = 7;

    // Keep track of orders
    public List<List<string>> orders = new List<List<string>>();

    //Colors
    private static Color blue = new Color(48 / 255f, 96 / 255f, 130 / 255f);
    private static Color purple = new Color(118 / 255f, 66 / 255f, 138 / 255f);
    private static Color red = new Color(172 / 255f, 50 / 255f, 50 / 255f);
    private static Color orange = new Color(223 / 255f, 113 / 255f, 38 / 255f);
    private static Color yellow = new Color(251 / 255f, 242 / 255f, 54 / 255f);
    private static Color green = new Color(106 / 255f, 190 / 255f, 48 / 255f);
    private static Color pink = new Color(215 / 255f, 123 / 255f, 186 / 255f);
    private static Color black = new Color(0f, 0f, 0f);
    private static Color brown = new Color(102 / 255f, 57 / 255f, 49 / 255f);
    public Hashtable colorHash = new Hashtable();

    //Paint Storage sprite
    public GameObject paintStorageIcon;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Orders", 2, orderDelay);

        // Colors to hashtable.
        colorHash.Add("blue", blue);
        colorHash.Add("purple", purple);
        colorHash.Add("red", red);
        colorHash.Add("orange", orange);
        colorHash.Add("yellow", yellow);
        colorHash.Add("green", green);
        colorHash.Add("pink", pink);
        colorHash.Add("black", black);
        colorHash.Add("brown", brown);
    }

    private void Orders() {
        // Values to be excluded
        List<int> exclude = new List<int>();

        // Create an order of 3 unique random colors.
        // Random numbers are checked against exclude to make sure they are unique. 
        // Sloppy, change if possible.
        List<string> currOrder = new List<string>();
        for (int i = 0; i < 3; i++) {
            int randIndex = Random.Range(0, textColors.Length);
            while (exclude.Contains(randIndex)) {
                randIndex = Random.Range(0, textColors.Length);
            }
            exclude.Add(randIndex);
            currOrder.Add(textColors[randIndex]);
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
