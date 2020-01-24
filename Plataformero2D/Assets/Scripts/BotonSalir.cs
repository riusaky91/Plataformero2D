using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Biblioteca para cambiar de escenas

public class BotonSalir : MonoBehaviour
{

    public bool salir; //variable para confirmar en que escena se ecuentra

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//si se oprime escape
        {
            if (salir)//si salir es vardadero
            {
                Application.Quit();//Salir del juego
            }
            else// si no 
            {
                SceneManager.LoadScene("MenuPrincipal");//me envia al menu principal
            }
        }
    }
}
