using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeletransportarJugador : MonoBehaviour
{


    private Transform primerDestino;//destino final del jugador
    private Transform segundoDestino;//destino final del jugador

    public float posicionX;//Posicion adicional donde aparecera el jugador

    public GameObject portalHermanoCaminando;//Portal Hermano
    public GameObject portalHermanoAgachado;//Portal Hermano

    PlayerMovement estadoJugador;

    public bool estaAgachadao;


    private void Start()
    {
        if (portalHermanoCaminando != null)//Si hay una referencia a un portal caminando
        {
            primerDestino = portalHermanoCaminando.transform;
        }

        if(portalHermanoAgachado != null)//Si hay una referencia a un portal agachado
        {
            segundoDestino = portalHermanoAgachado.transform;
        }

        estadoJugador =  GameObject.Find("Mihway").GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        estaAgachadao = estadoJugador.isCrouching;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(Vector2.Distance(transform.position, collision.transform.position) > 0.3f && !estaAgachadao && primerDestino != null)//Si la distancia entre el portal y el jugador es mayor a 0.3 y NO esta Agachado y hay un primer destino
        {
            collision.transform.position = new Vector2(primerDestino.position.x + posicionX, primerDestino.position.y);// muevo al jugador al destino + una posicion x dependiendo de la ubicación del portal
            //GameManager.PlayerDead();
            AudioManager.PlayDeathAudio();
        }
        else if(Vector2.Distance(transform.position, collision.transform.position) > 0.3f && estaAgachadao && segundoDestino != null)
        {
            collision.transform.position = new Vector2(segundoDestino.position.x + posicionX, segundoDestino.position.y);// muevo al jugador al destino + una posicion x dependiendo de la ubicación del portal
            //GameManager.PlayerDead();
            AudioManager.PlayDeathAudio();
        }

    }
}
