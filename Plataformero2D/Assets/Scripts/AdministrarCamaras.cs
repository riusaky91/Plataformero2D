using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdministrarCamaras : MonoBehaviour
{
    public GameObject camaraVirtual;//referencia a la camara del cuarto

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")&& !collision.isTrigger)//Si el jugador entra y hay una colision
        {
            camaraVirtual.SetActive(true);
            Debug.Log("Entrar camara");
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
