using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // References
    public string[] textColors;
    public TextMeshProUGUI orderText;
    public GameObject titleScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Image orderUI;
    public List<Image> ordersUI;

    // Modulate for difficulty
    [SerializeField] private int orderDelay = 8;
    [SerializeField] private int startingValue = 1000;
    [SerializeField] private int minusValue = 200;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private int repeatNum = 4;
    [SerializeField] private int tooManyOrders = 1;

    // Keep track of orders and their score. topOrderDelay keeps track of where to place the new order.
    public List<List<string>> orders = new List<List<string>>();
    public int topOrder = 0;
    public int topOrderDelay = 0;
    public int score;

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

    public bool isGameActive;

    // Start is called before the first frame update
    void Start()
    {
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

    public void StartGame() {
        // Remove title text
        titleScreen.gameObject.SetActive(false);

        isGameActive = true;

        // Start spawning orders
        InvokeRepeating("Orders", 2, orderDelay);

        scoreText.text += " 0";
    }

    private void Update() {
        if (orders.Count > tooManyOrders) {
            GameOver();
        }
    }

    private void Orders() {
        //Create new order
        Image newOrder = ordersUI[0];

        // Because orderUI is funky, the new color placement needs to be topOrderDelay unless order.Count is at 0
        if (orders.Count != 0) {
            newOrder = ordersUI[(topOrderDelay) % ordersUI.Count];
        }

        // Add payment text to order
        newOrder.transform.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Payment:\n" + startingValue;

        // Start decreasing value of the order.
        StartCoroutine(decreaseValue(newOrder, waitTime, repeatNum));

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
            newOrder.transform.GetComponentsInChildren<Image>()[i + 1].color = (Color)colorHash[currOrder.ElementAt(i)];
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

        while (true && repeatNum > 0) {
            yield return new WaitForSecondsRealtime(waitTime);
            currValue -= minusValue;
            newOrder.transform.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Payment:\n" + currValue;
            repeatNum--;
        }
    }

    public void UpdateScore(int addScore) {
        score += addScore;
        scoreText.text = "Score: " + score;
    }

    private void GameOver() {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        CancelInvoke();
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
