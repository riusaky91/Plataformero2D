using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Biblioteca para la gestion de escenas

public class AdministrarCreditos : MonoBehaviour
{
    public ElementoInteractivo pantalla;//referencia al script

    void Update()
    {
        if (/*Input.GetButtonDown("Fire1")*/pantalla.pulsado)//si se pulsa el eje fuego 1 o se da click 
        {
            //Vidas.vidas = 3;//reinicio el valor de vidas
            //EmpezarPartida.nombre = string.Empty;//reinicio el nombre
            //AdministrarPuntuacion.puntuacion = 0;//reinicio el valor de puntos
            SceneManager.LoadScene("MenuPrincipal");// cambia laa escena nivel01
        }

    }

    
}
