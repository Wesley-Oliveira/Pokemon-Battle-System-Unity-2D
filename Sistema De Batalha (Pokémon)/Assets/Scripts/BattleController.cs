using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleController : MonoBehaviour
{
    private PokemonPlayer pokemonPlayer;
    private PokemonInimigo pokemonInimigo;

    public string fala;
    public Text texto;
    public int idFase;

    private Transform treinador, pokemon, posA, posB;
    public GameObject menuA, menuB;

	// Use this for initialization
	void Start ()
    {
        pokemonPlayer = FindObjectOfType(typeof(PokemonPlayer)) as PokemonPlayer;
        pokemonInimigo = FindObjectOfType(typeof(PokemonInimigo)) as PokemonInimigo;

        treinador = GameObject.Find("Treinador").transform;
        pokemon = pokemonPlayer.transform;
        posA = GameObject.Find("PosA").transform;
        posB = GameObject.Find("PosB").transform;

        menuA.SetActive(false);
        menuB.SetActive(false);

        idFase = 0;
        fala = "Um " + pokemonInimigo.nome + " apareceu!";
        StartCoroutine("dialogo", fala);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(idFase == 1) // Animação treinador saindo e pokemon entrando
        {
            treinador.GetComponent<Animator>().SetBool("Lancar", true);

            float step = 2 * Time.deltaTime; //velocidade de movimento dos objetos
            treinador.position = Vector3.MoveTowards(treinador.position, posB.position, step);
            pokemon.position = Vector3.MoveTowards(pokemon.position, posA.position, step);
        }
	}

    public IEnumerator dialogo(string txt)
    {
        int letra = 0;
        texto.text = "";

        while(letra <= txt.Length - 1)
        {
            texto.text += txt[letra];
            letra += 1;
            yield return new WaitForSeconds(0.05f);
        }        
        yield return new WaitForSeconds(1);
        idFase += 1;

        switch (idFase)
        {
            case 1:
                fala = pokemonPlayer.nome + " eu escolho você!";
                StartCoroutine("dialogo", fala);
                break;

            case 2:
                pokemonPlayer.ComandoIncial();
                break;
        }        
    }

    public void Lutar()
    {
        menuA.SetActive(false);
        menuB.SetActive(true);
        pokemonPlayer.renomearBotoes();
    }

    public void comando(int idComando)
    {
        menuB.SetActive(false);
        pokemonPlayer.StartCoroutine("comando", idComando);
    }    
}
