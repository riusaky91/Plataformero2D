using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdministrarCamaras : MonoBehaviour
{
    

    public GameObject camaraVirtual;//referencia a la camara del cuarto


    static bool salio;// confirmar si salio de la camara



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Estado estado = collision.GetComponent<Estado>();//Referencia el estado del jugador

        BoxCollider2D muroNivel = GetComponent<BoxCollider2D>();// Referencia al box Collider del objeto

        PlayerMovement movimientoJugador = collision.GetComponent<PlayerMovement>();//Tomo el componente player movement del jugador

        camaraVirtual.SetActive(true);// activo la camara actual


        //Si el objeto que colisiona es el jugador
        if (collision.CompareTag("Player") )
        {
            
            Debug.Log("Entrar camara");


            //si se reinicio el juego
            if (GameManager.reseteo)
            {
                //si hay un boxcolider en el objeto {se activa el muro}
                if (muroNivel != null)
                {
                    muroNivel.enabled = true;
                }
               
                GameManager.reseteo = false;//cambio el valor de la variable por que ya se ha reiniciado al jugador
            }

            //Si la camara es diferente a la 1 y el jugador ya salio de una camara anterior
            if (!camaraVirtual.CompareTag("camara1") && salio)
            {
                estado.posicion = collision.transform.position;//Guardo la poscion de el jugador en la variable posicion del estado
                GestorDeEstado.Guardar(estado);//guardo estado

                //si hay un boxcolider en el objeto {se activa el muro}
                if (muroNivel != null)
                {
                    muroNivel.enabled = true;
                }

                salio = false;//cambio el valor de salio ya que se entro a una nueva camara
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string bodyType = collision.attachedRigidbody.bodyType.ToString();//referencio el bodytype del objeto que colisiono (jugador) con la camara

        //Si el objeto que colisiono es el jugador y su boditype es dinamico
        if (collision.CompareTag("Player") && bodyType == "Dynamic" )
        {
            
            camaraVirtual.SetActive(false);//desactivo la camara referenciada
            Debug.Log("Salir camara");

            salio = true;//cambio el valor a verdadero ya que se salio de la camara
        }
    }


}
