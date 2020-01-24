using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR //Este codigo entre # solo se ejecutara cuando nos encontremos en el editor de Unity
using UnityEditor;
#endif
using UnityEngine.Audio;//Espacio de nombre que sirve para poder interactuar con audioMixer 

public class AdministrarPause : MonoBehaviour {

    

    Canvas canvas;//variable que contendra la referencia al componente (Canvas)

    

    
    void Start()
    {
        canvas = GetComponent<Canvas>();//haciendo referencia al componente (Canvas)
        canvas.enabled = false;//Deshabilito el componente canvas
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//Si oprimimos la tecla Esc en el teclado
        {
            Pause();//Ejecuto metodod pause
        }
    }
    

    //Metodo para pausar el juego
    public void Pause()
    {
        canvas.enabled = !canvas.enabled;//va ha cambiar el estado del canvas independientemente en el que se encuentre
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;//Si esta pausado el juedo se despausa o a la inversa
        
    }
    

    //metodo apra salir del Juego
    public void Salir()
    {
        #if UNITY_EDITOR //Este codigo entre # solo se ejecutara cuando nos encontremos en el editor de Unity
        EditorApplication.isPlaying = false;//Detiene la ejecucion de la aplicacion
        #else 
        Application.Quit();//salgo de la aplicacion
        #endif
        

    }
}