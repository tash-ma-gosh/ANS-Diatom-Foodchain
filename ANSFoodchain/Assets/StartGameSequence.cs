using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGameSequence : MonoBehaviour
{

    // public gameObject head;
    // public gameObject rightHand;
    // public gameObject leftHand;
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
    private SpriteRenderer poseGuideR;
    private SpriteRenderer poseGuideL;
    private Sprite[] sprites = new Sprite[5];
    
    public enum HandState{
        NoHands,
        LeftRaise,
        RightRaise,
        BothRaise
    }
    public static HandState handState = HandState.NoHands;

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

    public static float[] playerBoundary = new float[4];

    public LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        bodySourceView = gameObject.GetComponent<BodySourceView>();
        gameStateText  = GameObject.Find("GameStateText").GetComponent<TextMeshProUGUI>();
        poseGuideR  = GameObject.Find("PoseGuideR").GetComponent<SpriteRenderer>();
        poseGuideL  = GameObject.Find("PoseGuideL").GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("silhouette");
        timeOut = 11.0f;

        gameState = GameState.NoPlayer;
        previousGameState = GameState.NoPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        //If there is body
        if (bodySourceView.bodyContainer.transform.childCount >= 1){
            //Check if player return or player replaced and reassign hand
            if (gameState == GameState.WaitPlayerReturn){
                gameState = previousGameState;

                playerBody = bodySourceView.bodyContainer.transform.GetChild(0).gameObject;
                handRight = playerBody.transform.Find("HandRight").gameObject;
                handLeft = playerBody.transform.Find("HandLeft").gameObject;
                head = playerBody.transform.Find("Head").gameObject;
                
                if (handState == HandState.RightRaise){
                    playerHand = handRight;
                    poseGuideL.gameObject.SetActive(false);
                } else if (handState == HandState.LeftRaise){
                    playerHand = handLeft;
                    poseGuideR.gameObject.SetActive(false);
                }
            } else if (gameState == GameState.NoPlayer){
                gameState = GameState.Idle;
                
                playerBody = bodySourceView.bodyContainer.transform.GetChild(0).gameObject;
                handRight = playerBody.transform.Find("HandRight").gameObject;
                handLeft = playerBody.transform.Find("HandLeft").gameObject;
                head = playerBody.transform.Find("Head").gameObject;
            } else if (playerHand == null){
                playerBody = bodySourceView.bodyContainer.transform.GetChild(0).gameObject;
                handRight = playerBody.transform.Find("HandRight").gameObject;
                handLeft = playerBody.transform.Find("HandLeft").gameObject;
                head = playerBody.transform.Find("Head").gameObject;
                
                if (handState == HandState.RightRaise){
                    playerHand = handRight;
                    poseGuideL.gameObject.SetActive(false);
                } else if (handState == HandState.LeftRaise){
                    playerHand = handLeft;
                    poseGuideR.gameObject.SetActive(false);
                }
            }
        
            switch (gameState){
            case GameState.Idle:
                poseGuideR.sprite = sprites[1];
                poseGuideL.sprite = sprites[1];
                HandCheck();
                break;
            case GameState.CountDown:
                gameStateText.text = $"Hold your hand there\n{3-(int)gameStartCountDown}";
                poseGuideR.sprite = sprites[1];
                poseGuideL.sprite = sprites[1];
                HandCheck();
                break;
            case GameState.CalibrationDown:
                Calibration();
                break;
            case GameState.CalibrationRight:
                Calibration();
                break;
            case GameState.CalibrationLeft:
                Calibration();
                break;
            case GameState.StartGame:
                //gameStateText.text =  string.Join(",", playerBoundary);
                
                levelLoader.LoadLevel("Introduction");
                break;
            }
        } else {
            if (gameState != GameState.NoPlayer && gameState != GameState.WaitPlayerReturn){
                previousGameState = gameState;
                timeOut = 0;
                Debug.Log("Reset timer");
            }
            poseGuideL.gameObject.SetActive(true);
            poseGuideR.gameObject.SetActive(true);
            poseGuideR.sprite = sprites[0];
            poseGuideL.sprite = sprites[0];
            if(timeOut <= 10){
                gameState = GameState.WaitPlayerReturn;
                gameStateText.text = $"Please return to the circle\n{10-(int)timeOut}";
                timeOut += Time.deltaTime;
            } else {
                gameState = GameState.NoPlayer;
                gameStateText.text = "Stand on the circle to play";
                handState = HandState.NoHands;
            }
            

        }
        

        // IF BODY NOT IN SCENE WAIT FOR 5 SECONDS
        // if (playerBody == null){
        //     GameState = 
        // }
        
        
        //Get head and hand positions
    }

    void HandCheck(){

        float headY = head.transform.position.y;
        float handRightY = handRight.transform.position.y;
        float handLeftY = handLeft.transform.position.y;

        //Check both hands
        if (handRightY > headY && handLeftY > headY)
        {
            gameStateText.text = "Raise ONLY ONE hand above your head to Start";
            handState = HandState.BothRaise;
            StopCountDown();
        }
        else if (handRightY <= headY && handLeftY <= headY){
            gameStateText.text = "Raise your dominant hand above your head to Start";
            handState = HandState.NoHands;
            StopCountDown();
        }
        else if (handRightY > headY){
            if (handState != HandState.RightRaise){
                StopCountDown();
            }
            handState = HandState.RightRaise;
            StartCountDown();
        }
        else if (handLeftY > headY){
            if (handState != HandState.LeftRaise){
                StopCountDown();
            }
            handState = HandState.LeftRaise;
            StartCountDown();
        }

        // //Switch Case
        // switch (handState){
        //     case HandState.BothRaise:
        //         //Stop Time
        //         break;
        //     case HandState.RightRaise:
                
        //         break;
        //     case HandState.LeftRaise:
        //         StartCountDown();
        //         break;
        //     case HandState.NoHands:
        //         break;
        // }
    }

    void StartCountDown(){
        //start
        if (gameState == GameState.Idle){
            gameState = GameState.CountDown;

            if (handState == HandState.RightRaise){
                    playerHand = handRight;
                    poseGuideL.gameObject.SetActive(false);
                } else if (handState == HandState.LeftRaise){
                    playerHand = handLeft;
                    poseGuideR.gameObject.SetActive(false);
                }
        }
        //continue
        else{
            gameStartCountDown += Time.deltaTime;
            if (gameStartCountDown >= countDownMax){
                gameState = GameState.CalibrationDown;
                if (handState == HandState.RightRaise){
                    playerHand = handRight;
                    poseGuideL.gameObject.SetActive(false);
                } else if (handState == HandState.LeftRaise){
                    playerHand = handLeft;
                    poseGuideR.gameObject.SetActive(false);
                }
                playerBoundary[0] = playerHand.transform.position.y;
                gameStartCountDown = 0;
                
            }
        }
    }

    void StopCountDown(){
        gameStartCountDown = 0;
        gameState = GameState.Idle;
        handState = HandState.NoHands;
        poseGuideL.gameObject.SetActive(true);
        poseGuideR.gameObject.SetActive(true);
    }

    void Calibration(){
        //if (handState == HandState.RightRaise){

        switch (gameState){
            case GameState.CalibrationDown:
                poseGuideR.sprite = sprites[2];
                poseGuideL.sprite = sprites[2];
                if (playerHand.transform.position.y < head.transform.position.y){
                    gameStartCountDown += Time.deltaTime;
                    gameStateText.text = $"Hold your hand DOWN\n{3-(int)gameStartCountDown}";
                } else{
                    gameStateText.text = "Put your hand DOWN";
                    gameStartCountDown = 0;
                }
                
                if (gameStartCountDown >= 3){
                    if (handState == HandState.RightRaise){
                        gameState = GameState.CalibrationLeft;
                    } 
                    if (handState == HandState.LeftRaise){
                        gameState = GameState.CalibrationRight;
                    }
                    playerBoundary[1] = playerHand.transform.position.y;
                    gameStartCountDown = 0;
                }
                break;
            case GameState.CalibrationRight:
                poseGuideR.sprite = sprites[3];
                poseGuideL.sprite = sprites[4];
                if (playerHand.transform.position.x > head.transform.position.x){
                    gameStartCountDown += Time.deltaTime;
                    gameStateText.text = $"Hold your hand to the RIGHT\n{3-(int)gameStartCountDown}";
                } else{
                    gameStateText.text = "Put your hand to the RIGHT";
                    gameStartCountDown = 0;
                }
                if (gameStartCountDown >= 3){
                    if (handState == HandState.LeftRaise){
                        gameState = GameState.CalibrationLeft;
                    } else {
                        gameState = GameState.StartGame;
                    }
                    playerBoundary[2] = playerHand.transform.position.x;
                    gameStartCountDown = 0;
                }
                break;
            case GameState.CalibrationLeft:
                poseGuideR.sprite = sprites[4];
                poseGuideL.sprite = sprites[3];
                if (playerHand.transform.position.x < head.transform.position.x){
                    gameStartCountDown += Time.deltaTime;
                    gameStateText.text = $"Hold your hand to the LEFT\n{3-(int)gameStartCountDown}";
                } else{
                    gameStateText.text = "Put your hand to the LEFT";
                    gameStartCountDown = 0;
                }
                if (gameStartCountDown >= 3){
                    if (handState == HandState.RightRaise){
                        gameState = GameState.CalibrationRight;
                    } else {
                        gameState = GameState.StartGame;
                    }
                    playerBoundary[3] = playerHand.transform.position.x;
                    gameStartCountDown = 0;
                }
                break;

        }
        //}
    }


}
