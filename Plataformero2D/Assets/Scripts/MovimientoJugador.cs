using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{

    public bool dibujarRayCasts = true;	//Activa los rayos para el jugador

    [Header("Propiedades de Movimiento")]
    public float velocidad = 8f;            //Velocidad del jugador
    public float velocidadAgachado = 3f;   //Velocidad de agachado
    public float duracionCoyote = .05f;     //Tiempo de salto para el juagador mientras cae
    public float velocidadMaximaCaida = -25f;       //Velocidad Maxima de caida

    [Header("Propiedades de Salto")]
    public float FuezaSalto = 6.3f;         //Fuerza  Inicial del Salto
    public float saltoAgachado = 2.5f;      //Fuerza del salto estando agachado
    public float saltoAgarrado = 15f;        //Fuerza del salto estando colgado
    public float saltoSostenido = 1.9f;      //Incremento de fuerza cuando se mantiene la tecla salto
    public float tiempoTeclaSalto = .1f;    //Cuento timepo se puede dejar la tecla de saltar sostenida

    [Header("Propiedades del entorno")]
    public float footOffset = .4f;          //Distancia Pies x
    public float eyeHeight = 1.5f;          //Mirada Jugador
    public float reachOffset = .7f;         //agarre distancia
    public float headClearance = .5f;       //espacio de cabeza
    public float groundDistance = .2f;      //Distancia donde se considera que el jugador toca el suelo
    public float grabDistance = .4f;        //Distancia de agarre
    public LayerMask groundLayer;           //Layer del suelo

    [Header("Estados del Jugador")]
    public bool estaEnSuelo;                
    public bool estaSaltando;                  
    public bool estaAgarrado;                 
    public bool estaAgachado;                
    public bool estaBloqueado;

    InputJugador input;                      
    BoxCollider2D cuerpoCollider;            
    Rigidbody2D rigidBody;                  

    float tiempoSalto;                         
    float tiempoCoyote;                       
    float alturaJugador;                     

    float escalaOriginaX;                   //Original escala en el eje x
    int direccion = 1;                      //direccion de la cara del jugador

    Vector2 tamañoJugadorDepie;              
    Vector2 tamañoJugadorDepieMoviendo;           
    Vector2 tamañoJugadorAgachado;             
    Vector2 tamañoJugadorAgachadoMoviendo;          

    const float cantidaPequeña = .05f;			//usado en el agarre


    void Start()
    {
        //Get a reference to the required components
        input = GetComponent<InputJugador>();
        rigidBody = GetComponent<Rigidbody2D>();
        cuerpoCollider = GetComponent<BoxCollider2D>();

        //Record the original x scale of the player
        escalaOriginaX = transform.localScale.x;

        //Record the player's height from the collider
        alturaJugador = cuerpoCollider.size.y;

        //Record initial collider size and offset
        tamañoJugadorDepie = cuerpoCollider.size;
        tamañoJugadorDepieMoviendo = cuerpoCollider.offset;

        //Calculate crouching collider size and offset
        tamañoJugadorAgachado = new Vector2(cuerpoCollider.size.x, cuerpoCollider.size.y / 2f);
        tamañoJugadorAgachadoMoviendo = new Vector2(cuerpoCollider.offset.x, cuerpoCollider.offset.y / 2f);
    }

    private void FixedUpdate()
    {
        //Check the environment to determine status
        chequeoFisicas();

        //Process ground and air movements
        movimientoTierra();
        movientoAire();
    }

    void chequeoFisicas()
    {
        //Start by assuming the player isn't on the ground and the head isn't blocked
        estaEnSuelo = false;
        estaBloqueado = false;

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);

        //If either ray hit the ground, the player is on the ground
        if (leftCheck || rightCheck)
            estaEnSuelo = true;

        //Cast the ray to check above the player's head
        RaycastHit2D headCheck = Raycast(new Vector2(0f, cuerpoCollider.size.y), Vector2.up, headClearance);

        //If that ray hits, the player's head is blocked
        if (headCheck)
            estaBloqueado = true;

        //Determine the direction of the wall grab attempt
        Vector2 grabDir = new Vector2(direccion, 0f);

        //Cast three rays to look for a wall grab
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direccion, alturaJugador), grabDir, grabDistance);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direccion, alturaJugador), Vector2.down, grabDistance);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direccion, eyeHeight), grabDir, grabDistance);

        //If the player is off the ground AND is not hanging AND is falling AND
        //found a ledge AND found a wall AND the grab is NOT blocked...
        if (!estaEnSuelo && !estaAgarrado && rigidBody.velocity.y < 0f &&
            ledgeCheck && wallCheck && !blockedCheck)
        {
            //...we have a ledge grab. Record the current position...
            Vector3 pos = transform.position;
            //...move the distance to the wall (minus a small amount)...
            pos.x += (wallCheck.distance - cantidaPequeña) * direccion;
            //...move the player down to grab onto the ledge...
            pos.y -= ledgeCheck.distance;
            //...apply this position to the platform...
            transform.position = pos;
            //...set the rigidbody to static...
            rigidBody.bodyType = RigidbodyType2D.Static;
            //...finally, set isHanging to true
            estaAgarrado = true;
        }
    }

    void movimientoTierra()
    {
        //If currently hanging, the player can't move to exit
        if (estaAgarrado)
            return;

        //Handle crouching input. If holding the crouch button but not crouching, crouch
        if (input.mantenerAgachado && !estaAgarrado && estaEnSuelo)
            Agacharse();
        //Otherwise, if not holding crouch but currently crouching, stand up
        else if (!input.mantenerAgachado && estaAgachado)
            Levantarse();
        //Otherwise, if crouching and no longer on the ground, stand up
        else if (!estaEnSuelo && estaAgachado)
            Levantarse();

        //Calculate the desired velocity based on inputs
        float xVelocity = velocidad * input.horizontal;

        //If the sign of the velocity and direction don't match, flip the character
        if (xVelocity * direccion < 0f)
            vueltaDireccionPersonaje();

        //If the player is crouching, reduce the velocity
        if (estaAgachado)
            xVelocity /= velocidadAgachado;

        //Apply the desired velocity 
        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

        //If the player is on the ground, extend the coyote time window
        if (estaEnSuelo)
            tiempoCoyote = Time.time + duracionCoyote;
    }

    void movientoAire()
    {
        //If the player is currently hanging...
        if (estaAgarrado)
        {
            //If crouch is pressed...
            if (input.agacharse)
            {
                //...let go...
                estaAgarrado = false;
                //...set the rigidbody to dynamic and exit
                rigidBody.bodyType = RigidbodyType2D.Dynamic;
                return;
            }

            //If jump is pressed...
            if (input.saltar)
            {
                //...let go...
                estaAgarrado = false;
                //...set the rigidbody to dynamic and apply a jump force...
                rigidBody.bodyType = RigidbodyType2D.Dynamic;
                rigidBody.AddForce(new Vector2(0f, saltoAgarrado), ForceMode2D.Impulse);
                //...and exit
                return;
            }
        }

        //If the jump key is pressed AND the player isn't already jumping AND EITHER
        //the player is on the ground or within the coyote time window...
        if (input.saltar && !estaSaltando && (estaEnSuelo || tiempoCoyote > Time.time))
        {
            //...check to see if crouching AND not blocked. If so...
            if (estaAgachado && !estaBloqueado)
            {
                //...stand up and apply a crouching jump boost
                Levantarse();
                rigidBody.AddForce(new Vector2(0f, saltoAgachado), ForceMode2D.Impulse);
            }

            //...The player is no longer on the groud and is jumping...
            estaEnSuelo = false;
            estaSaltando = true;

            //...record the time the player will stop being able to boost their jump...
            tiempoSalto = Time.time + tiempoTeclaSalto;

            //...add the jump force to the rigidbody...
            rigidBody.AddForce(new Vector2(0f, FuezaSalto), ForceMode2D.Impulse);

            //...and tell the Audio Manager to play the jump audio
            //AudioManager.PlayJumpAudio();
        }
        //Otherwise, if currently within the jump time window...
        else if (estaSaltando)
        {
            //...and the jump button is held, apply an incremental force to the rigidbody...
            if (input.mantenerSalto)
                rigidBody.AddForce(new Vector2(0f, FuezaSalto), ForceMode2D.Impulse);

            //...and if jump time is past, set isJumping to false
            if (tiempoSalto <= Time.time)
                estaSaltando = false;
        }

        //If player is falling to fast, reduce the Y velocity to the max
        if (rigidBody.velocity.y < velocidadMaximaCaida)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, velocidadMaximaCaida);
    }

    void vueltaDireccionPersonaje()
    {
        //Turn the character by flipping the direction
        direccion *= -1;

        //Record the current scale
        Vector3 scale = transform.localScale;

        //Set the X scale to be the original times the direction
        scale.x = escalaOriginaX * direccion;

        //Apply the new scale
        transform.localScale = scale;
    }

    void Agacharse()
    {
        //The player is crouching
        estaAgachado = true;

        //Apply the crouching collider size and offset
        cuerpoCollider.size = tamañoJugadorAgachado;
        cuerpoCollider.offset = tamañoJugadorAgachadoMoviendo;
    }

    void Levantarse()
    {
        //If the player's head is blocked, they can't stand so exit
        if (estaBloqueado)
            return;

        //The player isn't crouching
        estaAgachado = false;

        //Apply the standing collider size and offset
        cuerpoCollider.size = tamañoJugadorDepie;
        cuerpoCollider.offset = tamañoJugadorDepieMoviendo;
    }


    //These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
    //functionality
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        //Call the overloaded Raycast() method using the ground layermask and return 
        //the results
        return Raycast(offset, rayDirection, length, groundLayer);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        //Record the player's position
        Vector2 pos = transform.position;

        //Send out the desired raycasr and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        //If we want to show debug raycasts in the scene...
        if (dibujarRayCasts)
        {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        //Return the results of the raycast
        return hit;
    }

    void Update()
    {
        
    }
}
