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
        sr = GetComponentInParent<SpriteRenderer>();
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        visualizador.text = aviso;
        sr.enabled = true;
        enColider = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if(visualizador.text == aviso)
        {
            visualizador.text = "";
        }
        
        sr.enabled = false;
        enColider = false;
        index = 0;
    }

    private void Update()
    {
        if(visualizador.text == oraciones[index])//Si hay una oracion seguira activo el boton continuar
        {
            botonContinuar.SetActive(true);
        }

        if (Input.GetButtonDown("Crouch") && enColider)
        {
            visualizador.text = "";
            StartCoroutine(Teclear());//Metodo que muestra el texto 
            enColider = false;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;//Mientras el dialogo se presente el jugador quedara estatico

        }
    }


    IEnumerator Teclear()//corrutina para ejecutar un metodo en un tiempo determinado
    {
        foreach(char letra in oraciones[index].ToCharArray())
        {
            visualizador.text += letra;
            yield return new WaitForSeconds(velocidadTecleo);
        }
    }


    //Metodo que añde la funcionalidad al boton de continuar del texto

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
