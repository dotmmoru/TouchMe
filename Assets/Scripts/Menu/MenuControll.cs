using UnityEngine;
using System.Collections;

public class MenuControll : MonoBehaviour {

    //
    //Управление кнопками меню
    //

    // Загружаем основную сцену 
    public void StartLvl()
    {
        Application.LoadLevel(1);
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    // Выходим из приложения 
    public void Exit ()
    {
        Application.Quit();
    }

}
