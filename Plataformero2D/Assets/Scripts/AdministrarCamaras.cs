using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdministrarCamaras : MonoBehaviour
{
    

    public GameObject camaraVirtual;//referencia a la camara del cuarto



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Estado estado = collision.GetComponent<Estado>();//Referencia el stado del jugador

        BoxCollider2D muroNivel = GetComponent<BoxCollider2D>();// Referencia al box Collider del objeto

        if (collision.CompareTag("Player")&& !collision.isTrigger)//Si el jugador entra y hay una colision, y el istrigger del jugador no esta activo
        {
            camaraVirtual.SetActive(true);
            Debug.Log("Entrar camara");

            if (!camaraVirtual.CompareTag("camara1"))//Si la camara es diferente a la 1
            {
                estado.posicion = collision.transform.position;//Guardo la poscion de mihway en la variable posicion del estado
                GestorDeEstado.Guardar(estado);//guardo

                if(muroNivel != null)
                {
                    muroNivel.enabled = true;
                }
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
            
            
        }
    }


}
