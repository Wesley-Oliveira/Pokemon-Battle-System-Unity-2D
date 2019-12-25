using UnityEngine;
using System.Collections;

public class PokemonInimigo : MonoBehaviour
{
    private PokemonPlayer pokemonPlayer;
    private BattleController battleController;

    public string nome;
    public int level;
    public int xp;
    public float pvMax, pv, percentualDeVida; //ponto vida maximo

    public string[] acoes;
    public int[] danoAcoes;
    public GameObject[] animacoes;

    private string fala;

    private Transform barraHP;
    private Vector3 vetor3;

    private int idComando, hit, idFase;

    // Use this for initialization
    void Start ()
    {
        pokemonPlayer = FindObjectOfType(typeof(PokemonPlayer)) as PokemonPlayer;
        battleController = FindObjectOfType(typeof(BattleController)) as BattleController;

        pv = pvMax;
        barraHP = GameObject.Find("HPInimigo").transform;
        percentualDeVida = pv / pvMax;
        vetor3 = barraHP.localScale;
        vetor3.x = percentualDeVida;
        barraHP.localScale = vetor3;

    }

	public void TomarDano(int hit)
    {
        pv -= hit;
        if(pv <= 0)
        {
            pv = 0;
            GetComponent<SpriteRenderer>().enabled = false;
        }

        percentualDeVida = pv / pvMax;
        vetor3 = barraHP.localScale;
        vetor3.x = percentualDeVida;
        barraHP.localScale = vetor3;
    }

    public IEnumerator AcaoInicial()
    {
        idComando = Random.Range(0, acoes.Length);
        yield return new WaitForSeconds(1f);
        StartCoroutine("comando", idComando);
    }

    public IEnumerator comando(int idAcao)
    {
        switch (idAcao)
        {
            case 0:
                StartCoroutine("AplicarDano");
                break;

            case 1:
                StartCoroutine("AplicarDano");
                break;

            case 2:
                StartCoroutine("AplicarDano");
                break;

            case 3:
                StartCoroutine("AplicarDano");
                break;

            case 4:
                StartCoroutine("AplicarDano");
                break;
        }

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator dialogo(string txt)
    {
        int letra = 0;
        battleController.texto.text = "";

        while (letra <= txt.Length - 1)
        {
            battleController.texto.text += txt[letra];
            letra += 1;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);

        switch (idFase)
        {
            case 1:
                StartCoroutine("AplicarDano");
                break;

            case 2:
                pokemonPlayer.ComandoIncial();
                break;
        }
    }

    public IEnumerator AplicarDano()
    {
        GameObject tempPrefab = Instantiate(animacoes[idComando]) as GameObject;
        tempPrefab.transform.position = pokemonPlayer.transform.position;

        hit = Random.Range(1, danoAcoes[idComando]); // retirar esse random e colocar o valor real de cada golpe subtraido da defesa do inimigo
        fala = nome + " usou " + acoes[idComando] + " e causou " + hit + " de dano.";
        StartCoroutine("dialogo", fala);
        yield return new WaitForSeconds(1f);
        pokemonPlayer.TomarDano(hit);
        Destroy(tempPrefab);

        idFase = 2;
    }
}
