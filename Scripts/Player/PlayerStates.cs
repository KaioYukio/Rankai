using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public static PlayerStates instance;

    public enum playerStates { noControl, moving, attacking, dodging, flinch};
    public playerStates state;

    public string estadoAtual;
    public string atacando;
    public string podeAtacar;
    public string livre;
    public string desviando;
    public string flinch;
    public string bloqueando;
    public string parry;
    public string morto;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        state = playerStates.moving;
        estadoAtual = livre;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MudarEstadoPara(string estadoDesejado)
    {
        estadoAtual = estadoDesejado;
    }
}
