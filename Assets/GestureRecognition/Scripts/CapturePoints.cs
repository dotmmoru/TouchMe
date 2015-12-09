using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CapturePoints : MonoBehaviour {

    private List<Vector2> points = new List<Vector2>();

    public GameObject gestureOnScreen;
    private LineRenderer gestureLineRenderer;
    private int vertexCount = 0;

    private string message;
    private RuntimePlatform platform;

    private Vector3 virtualKeyPosition = Vector2.zero;
    private Rect drawArea;

    private string newGestureName = "";
    private GestureLibrary gl;

    private string libraryToLoad;
    bool SwitchLibrary = true, EndGame= false;
    
    //nGUI
    public GameObject WinLoseBar;
    public UILabel lblTimeValue, lblLevelValue, lblResult; 
    public UISprite TaskSprite;// спрайт с заданием 
    public List<Sprite> TaskImg = new List<Sprite>();// Список изображений , которые попадут в очередь 
    // End

    // Time values
    float maxTime, curTime, timeDecreaser = 0;

    // Indexes
    int Index_of_TaskArray = 0, LvlCounter = 1, Score = 0;

    //Music
    private AudioSource AS_Music;
    public AudioClip WinMelody, LoseMelody, lvlComplateMelody;

    void Start() {

        libraryToLoad = "shapes";

        gl = new GestureLibrary(libraryToLoad);

        platform = Application.platform;

        gestureLineRenderer = gestureOnScreen.GetComponent<LineRenderer>();

        drawArea = new Rect(145, 0, Screen.width - 145, Screen.height);

        AS_Music = GetComponent<AudioSource>();

        // Таймер 
        maxTime = TaskImg.Count+3;  // стартовое время = к-во элементов из задания + 3 секунды 

        curTime = maxTime;
        // End

        // Задаем начальный спрайт 
        TaskSprite.spriteName = TaskImg[Index_of_TaskArray].name;
        
        //стартовое значение уровня 
        lblLevelValue.text = LvlCounter.ToString();
        //End
    }


    void Update() {

        ChooseLibrary();
        TimeEscape();

        
        if (Input.GetMouseButton(0))
        {
            virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        }
        

        if (drawArea.Contains(virtualKeyPosition)&& !EndGame) {

            if (Input.GetMouseButtonDown(0)) {
                ClearDesk();
            }

            if (Input.GetMouseButton(0)) {
                points.Add(new Vector2(virtualKeyPosition.x, -virtualKeyPosition.y));

                gestureLineRenderer.SetVertexCount(++vertexCount);
                gestureLineRenderer.SetPosition(vertexCount - 1, WorldCoordinateForGesturePoint(virtualKeyPosition));
            }


            //
            //Распознавание 
            //
            if (Input.GetMouseButtonUp(0)) {
                Gesture g = new Gesture(points);
                Result result = g.Recognize(gl, true);
                ClearDesk();

                if (TaskSprite.spriteName == result.Name)
                {
                    Score++;
                    if (Index_of_TaskArray < TaskImg.Count-1)
                    {
                        timeDecreaser++;
                        curTime = maxTime - timeDecreaser;

                        Index_of_TaskArray++;
                        LvlCounter++;
                        lblLevelValue.text = LvlCounter.ToString();
                        TaskSprite.spriteName = TaskImg[Index_of_TaskArray].name;

                        AS_Music.PlayOneShot(lvlComplateMelody);
                    }
                    else
                    {
                        EndGame = true;
                        WinLoseBar.SetActive(EndGame);
                        lblResult.text = "You WIN!!!\n"+"Score : "+Score;
                        
                        AS_Music.PlayOneShot(WinMelody);
                    }
                }
            }
            //End
            //

        }

    }

    //Выбор библиотеки в зависимости от уровня 
    // Если честно, думаю можно было и лучше реализовать , но это самое простое что я придумал
    private void ChooseLibrary()
    {
        if (SwitchLibrary)
        {
            if (LvlCounter == 4)
            {
                libraryToLoad = "numbers";
                gl = new GestureLibrary(libraryToLoad);
                SwitchLibrary = false;
            }
        }
    }
    //End



    // Обучение программы новым фигурам 
   /*
  //  ----------------------------------------------------------
    void OnGUI() {
        GUI.skin.label.fontSize = 20;
        GUI.Label(new Rect(10, Screen.height - 40, 500, 50), message);

      //  GUI.Label(new Rect(Screen.width - 340, 10, 70, 30), "Add as: ");
        newGestureName = GUI.TextField(new Rect(Screen.width - 270, 10, 200, 30), newGestureName);

        if (GUI.Button(new Rect(10, 10, 50, 30), "Add")) {
            Gesture newGesture = new Gesture(points, newGestureName);
            gl.AddGesture(newGesture);
        }
    }
   // -------------------------------------------------------------
   */ 

    
    // Отсчет времени на каждую попытку
    private void TimeEscape()
    {
        if (!EndGame)
        {
            if (curTime > 0)
            {
                curTime -= Time.deltaTime;
                lblTimeValue.text = Mathf.Round(curTime).ToString();
            }
            else
            {
                EndGame = true;
                WinLoseBar.SetActive(EndGame);
                lblResult.text = "You Lose!!!\n" + "Score : " + Score;
                ClearDesk();
                AS_Music.PlayOneShot(LoseMelody);
            }
        }
    }
    // End

    private void ClearDesk()
    {
        points.Clear();
        gestureLineRenderer.SetVertexCount(0);
        vertexCount = 0;
    }

    private Vector3 WorldCoordinateForGesturePoint(Vector3 gesturePoint) {
        Vector3 worldCoordinate = new Vector3(gesturePoint.x, gesturePoint.y, 10);
        return Camera.main.ScreenToWorldPoint(worldCoordinate);
    }
}
