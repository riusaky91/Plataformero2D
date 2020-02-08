using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdministrarCamaras : MonoBehaviour
{
    

    public GameObject camaraVirtual;//referencia a la camara del cuarto


    static bool salio;// confirmar si salio de la camara



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Estado estado = collision.GetComponent<Estado>();//Referencia el stado del jugador

        BoxCollider2D muroNivel = GetComponent<BoxCollider2D>();// Referencia al box Collider del objeto

        PlayerMovement movimientoJugador = collision.GetComponent<PlayerMovement>();//Tomo el componente player movement del jugador

        if (collision.CompareTag("Player")&& !collision.isTrigger )//Si el jugador entra y hay una colision, y el istrigger del jugador no esta activo
        {
            camaraVirtual.SetActive(true);
            Debug.Log("Entrar camara");

            if (GameManager.reseteo)//si se reseteo
            {
                if (muroNivel != null)//si el nivel tiene instancia del muro lo activa
                {
                    muroNivel.enabled = true;
                }
               
                GameManager.reseteo = false;
            }

            if (!camaraVirtual.CompareTag("camara1") && salio)//Si la camara es diferente a la 1 y el jugador salio de un collider
            {
                estado.posicion = collision.transform.position;//Guardo la poscion de mihway en la variable posicion del estado
                GestorDeEstado.Guardar(estado);//guardo

                if(muroNivel != null)
                {
                    muroNivel.enabled = true;
                }

                salio = false;
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string bodyType = collision.attachedRigidbody.bodyType.ToString();

        if (collision.CompareTag("Player") && !collision.isTrigger && bodyType == "Dynamic" )
        {
            
            camaraVirtual.SetActive(false);
            Debug.Log("Salir camara");

            salio = true;
        }
    }


}
