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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        visualizador.text = aviso;
        enColider = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if(visualizador.text == aviso)
        {
            visualizador.text = "";

        }

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
            rb.bodyType = RigidbodyType2D.Static;//Mientras el dialogo se presente el jugador quedara estatico

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
        else// si no se borrara el texto y se deshabilitara el boton de continuar y se volvera a dinamico al jugador
        {
            visualizador.text = "";
            botonContinuar.SetActive(false);
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

}
