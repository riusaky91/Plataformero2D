using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogos : MonoBehaviour
{
    public TextMeshProUGUI visualizador;//referencia al componente textmeshPro

    public string aviso;

    public string[] oraciones; //Arreglo de cadenas que contendra las oraciones en los dialogos

    private int index;//indice para indicar la posicion del arreglo

    public float velocidadTecleo;//Velociada conla cual se ejecutara cada letra

    public GameObject botonContinuar;//Objeto para que la corrutina termine y no genere oraciones aleatorios

    public Animator visualizadorAnimacion;//referencia al componencte animator del textmesh

    private bool enColider;//Verifica si esta en collider

    public Rigidbody2D rb;//referencia al rigidbody del personaje

    SpriteRenderer sr;//refenecia a la imagen


    private void Awake()
    {
        sr = GetComponentInParent<SpriteRenderer>();//referencia al Sprite renderer de el dialogo    
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        visualizador.text = aviso;//escribi el texto aviso en el Textmesh
        AudioManager.PlayDialogoAudio();//genero el audio del dialogo
        sr.enabled = true;//habilito el sprite
        enColider = true;//habilito el validador para saber que hay collision

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Si hay un aviso ejecutandose lo limpio
        if(visualizador.text == aviso)
        {
            visualizador.text = "";
        }
        
        sr.enabled = false;//deshabilito el sprite
        enColider = false;//deshabilito el validador de collision
        index = 0;//la posiciono incial del string pasa a 0
    }

    private void Update()
    {
        //Si hay una oracion seguira activo el boton continuar
        if (visualizador.text == oraciones[index])
        {
            botonContinuar.SetActive(true);
        }

        //Si se oprimio el boton agachar y hay una colission (vacio el texto, muestro el texto letra por letra, deshabilito el validador de collision, detengo la posicion del jugador;
        if (Input.GetButtonDown("Crouch") && enColider)
        {
            visualizador.text = "";
            StartCoroutine(Teclear());//Metodo que muestra el texto 
            enColider = false;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;//Mientras el dialogo se presente el jugador quedara estatico (Freeze position x,y,Z)

        }
    }

    //corrutina para ejecutar un metodo en un tiempo determinado
    IEnumerator Teclear()
    {
        foreach(char letra in oraciones[index].ToCharArray())
        {
            visualizador.text += letra;
            yield return new WaitForSeconds(velocidadTecleo);
        }
    }


    //Metodo que añade la funcionalidad al boton de continuar del texto

    public void siguienteOracion()
    {
        visualizadorAnimacion.SetTrigger("cambio");//ejecuto la animacion
        botonContinuar.SetActive(false);//Mientras se muestra el texto se desactiva el boton continue

        if (index < oraciones.Length - 1)//Mientras haya oraciones se pasara a la otra, se limpierar el texto y se ejecutara la otra
        {
            index++;
            visualizador.text = "";
            StartCoroutine(Teclear());
        }
        else// si no se borrara el texto y se deshabilitara el boton de continuar y se dejeara mover al jugador
        {
            visualizador.text = "";
            sr.enabled = false;
            botonContinuar.SetActive(false);
            rb.constraints = RigidbodyConstraints2D.None;//Activo los ejes x,y z
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;//congelo el z
            index = 0;
        }
    }

}
