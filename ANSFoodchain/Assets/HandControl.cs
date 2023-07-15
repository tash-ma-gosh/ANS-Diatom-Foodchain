using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HandControl : MonoBehaviour
{
    public static BodySourceView bodySourceView;
    private static GameObject playerBody;
    private static GameObject handLeft;
    private static GameObject handRight;
    private static GameObject head;

    public static GameObject playerHand;
    
    private static float gameStartCountDown;
    private static float countDownMax = 3;

    private static float timeOut;

    private TextMeshProUGUI gameStateText;
    private SpriteRenderer poseGuide;
    
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
    public static GameState previousGameState;

    public static float[] playerBoundary = StartGameSequence.playerBoundary;

    public GameObject playerPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        bodySourceView = gameObject.GetComponent<BodySourceView>();
        gameStateText  = GameObject.Find("GameStateText").GetComponent<TextMeshProUGUI>();
        poseGuide  = GameObject.Find("PoseGuide").GetComponent<SpriteRenderer>();
        timeOut = 11.0f;

        gameState = GameState.StartGame;
        previousGameState = GameState.StartGame;
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
                timeOut = 0;
                Debug.Log("Reset timer");
                poseGuide.gameObject.SetActive(true);
            }
            if(timeOut <= 10){
                gameState = GameState.WaitPlayerReturn;
                gameStateText.text = $"Please return to the circle\n{10-(int)timeOut}";
                timeOut += Time.deltaTime;
            } else {
                SceneManager.LoadScene("Menu");
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
