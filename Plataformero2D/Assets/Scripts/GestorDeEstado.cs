using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorDeEstado : MonoBehaviour
{


    //metodo que recibira la componente para guardar sus datos
    public static void Guardar(MonoBehaviour componente)
    {
        //Estado estado = (Estado)componente;//casting de variable estado

        //PlayerPrefs.SetInt("Nivel", estado.nivel);//Guardado estado es entero de la variable nivel
        //PlayerPrefs.SetInt("Vidas", estado.vidas);

        PlayerPrefs.SetString("slot", JsonUtility.ToJson(componente));//Serializo en un Json los datos del componennte slot

        Debug.Log("Guardando...");
        

    }


    //metodo que recibira al componente para cargar sus datos
    public static void Cargar(MonoBehaviour componente)
    {
        //Estado estado = (Estado)componente;//casting de variable estado

        //estado.nivel = PlayerPrefs.GetInt("Nivel");//Cargo el valor entergo guardado con playerprefbs
        //estado.nivel = PlayerPrefs.GetInt("Vidas");

        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("slot"), componente);//cargo los datos del componente serializados en un JSON llamdo slot
        Debug.Log("Cargando...");
        //componente.transform.position = new Vector3(3,4,0);
    }
}
