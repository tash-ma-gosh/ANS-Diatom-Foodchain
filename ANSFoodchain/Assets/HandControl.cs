using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HandControl : MonoBehaviour
{
    public BodySourceView bodySourceView;
    private GameObject playerBody;
    private GameObject handLeft;
    private GameObject handRight;
    private GameObject head;

    public GameObject playerHand;
    
    private float gameStartCountDown;

    private float timeOut;

    private TextMeshProUGUI gameStateText;
    private SpriteRenderer poseGuide;

    public bool devMode = false;
    
    public enum HandState{
        NoHands,
        LeftRaise,
        RightRaise,
        BothRaise
    }
    public static HandState handState = (HandState)StartGameSequence.handState;

    public enum GameState{
        NoPlayer,
        WaitPlayerReturn,
        Idle,
        CountDown,
        CalibrationDown,
        CalibrationRight,
        CalibrationLeft,
        StartGame
    }
    public static GameState gameState;
    public GameState previousGameState;

    public static float[] playerBoundary = StartGameSequence.playerBoundary;

    public GameObject playerPrefab;
    public LevelLoader levelLoader;
    // Start is called before the first frame update
    void Start()
    {
        bodySourceView = gameObject.GetComponent<BodySourceView>();
        gameStateText  = GameObject.Find("GameStateText").GetComponent<TextMeshProUGUI>();
        poseGuide  = GameObject.Find("PoseGuide").GetComponent<SpriteRenderer>();
        timeOut = 11.0f;

        gameState = GameState.StartGame;
        previousGameState = GameState.StartGame;
        //if (devMode){
        if (handState == HandState.NoHands){
            handState = HandState.RightRaise;
            playerBoundary = new float[4]{3,-3,6,-6};
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bodySourceView.bodyContainer.transform.childCount >= 1){
            //Check if player return or player replaced and reassign hand
            if (gameState == GameState.WaitPlayerReturn){
                gameState = previousGameState;
                poseGuide.gameObject.SetActive(false);
                gameStateText.text = "";
                playerPrefab.SetActive(true);

                playerBody = bodySourceView.bodyContainer.transform.GetChild(0).gameObject;
                handRight = playerBody.transform.Find("HandRight").gameObject;
                handLeft = playerBody.transform.Find("HandLeft").gameObject;
                head = playerBody.transform.Find("Head").gameObject;
                
                if (handState == HandState.RightRaise){
                    playerHand = handRight;
                } else if (handState == HandState.LeftRaise){
                    playerHand = handLeft;
                }
            } else if (playerHand == null){
                playerBody = bodySourceView.bodyContainer.transform.GetChild(0).gameObject;
                handRight = playerBody.transform.Find("HandRight").gameObject;
                handLeft = playerBody.transform.Find("HandLeft").gameObject;
                head = playerBody.transform.Find("Head").gameObject;
                
                if (handState == HandState.RightRaise){
                    playerHand = handRight;
                } else if (handState == HandState.LeftRaise){
                    playerHand = handLeft;
                }
            }
            switch (gameState){
                case GameState.StartGame:
                    //gameStateText.text =  string.Join(",", playerBoundary);
                    MoveCharacter();
                    break;
            }
        } else {
            if (gameState != GameState.NoPlayer && gameState != GameState.WaitPlayerReturn){
                previousGameState = gameState;
                playerPrefab.SetActive(false);
                timeOut = 0;
                Debug.Log("Reset timer");
                poseGuide.gameObject.SetActive(true);
            }
            if(timeOut <= 10){
                gameState = GameState.WaitPlayerReturn;
                if(timeOut >= 0.5f){
                    gameStateText.text = $"Please return to the circle\n{10-(int)timeOut}";
                }
                timeOut += Time.deltaTime;
            } else {
                levelLoader.LoadLevel("Menu");
            } 
        }
    }

    void MoveCharacter(){
        float yMin = playerBoundary[1];
        float yMax = playerBoundary[0];
        float xMax = playerBoundary[3];
        float xMin = playerBoundary[2];
        float[] remapTo = new float[4] {-30,30,56,-56};
        float currY = playerHand.transform.position.y;
        float currX = playerHand.transform.position.x;

        float remappedY = remapTo[0] + (currY-yMin)*(remapTo[1]-remapTo[0])/(yMax-yMin);
        float remappedX = remapTo[2] + (currX-xMin)*(remapTo[3]-remapTo[2])/(xMax-xMin);

        playerPrefab.transform.position = Vector2.Lerp(transform.position, new Vector2(remappedX,remappedY), 1f);
    }
}
