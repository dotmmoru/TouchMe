using UnityEngine;
using System.Collections;

public class btnController : MonoBehaviour {

    // заново начать уровень 
    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    //End

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    //Выход в главное меню
    public void ExitToMenu()
    {
        Application.LoadLevel(0);
    }
    //End
}
