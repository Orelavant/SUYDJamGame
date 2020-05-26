using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    // References
    public string[] textColors;
    public TextMeshProUGUI orderText;

    public Image orderUI;
    public List<Image> ordersUI;

    // Modulate for harder game
    [SerializeField] private int orderDelay = 8;
    public int startingValue = 400;

    // Keep track of orders. topOrderDelay keeps track of where to place the new order.
    public List<List<string>> orders = new List<List<string>>();
    public int topOrder = 0;
    public int topOrderDelay = 0;

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
        //Create new order
        Image newOrder = ordersUI[0];

        // Because orderUI is funky, the new color placement needs to be topOrderDelay unless order.Count is at 0
        if (orders.Count != 0) {
            newOrder = ordersUI[topOrderDelay];
        }

        // Add payment text to order
        newOrder.transform.GetComponentsInChildren<TextMeshProUGUI>()[0].text += "" + startingValue;

        // Start decreasing value of the order.
        StartCoroutine(decreaseValue(newOrder, 2, 4));

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

            // Update orderUI
            newOrder.transform.GetComponentsInChildren<Image>()[i+1].color = (Color)colorHash[currOrder.ElementAt(i)];
        }
        // Add to order text and increment topOrderDelay.
        orders.Add(currOrder);
        orderText.text += orderToString(currOrder);
        topOrderDelay++;
    }

    private string orderToString(List<string> currOrder) {
        string orderString = " [";

        foreach (string color in currOrder) {
            orderString += " " + color;
        }

        orderString += " ]";
        return orderString;
    }

    private IEnumerator decreaseValue(Image newOrder, float waitTime, int repeatNum) {
        int currValue = startingValue;

        while (repeatNum > 0) {
            while (true) {
                yield return new WaitForSecondsRealtime(waitTime);
                currValue -= 100;
                newOrder.transform.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Payment:\n" + currValue;
            }
        }
    }
}
