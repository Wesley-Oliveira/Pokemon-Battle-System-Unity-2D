using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PokemonPlayer : MonoBehaviour
{
    private PokemonInimigo pokemonInimigo;
    private BattleController battleController;

    public string nome;
    public int level;
    public int xp;
    public float pvMax, pv, percentual; //ponto vida maximo

    public string[] acoes;
    public int[] danoAcoes;
    public GameObject[] animacoes;

    private string fala;

    private Transform barraHP, barraXp;
    private Vector3 vetor3;

    private GameObject botaoA, botaoB, botaoC, botaoD;

    private int hit;
    private int idComando;
    private int idFase;

    // Use this for initialization
    void Start()
    {
        pokemonInimigo = FindObjectOfType(typeof(PokemonInimigo)) as PokemonInimigo;
        battleController = FindObjectOfType(typeof(BattleController)) as BattleController;

        pv = pvMax;
        barraHP = GameObject.Find("HPPlayer").transform;
        barraXp = GameObject.Find("XPPlayer").transform;
        percentual = pv / pvMax;
        vetor3 = barraHP.localScale;
        vetor3.x = percentual;
        barraHP.localScale = vetor3;

        percentual = xp / 100;
        vetor3 = barraXp.localScale;
        vetor3.x = percentual;
        barraXp.localScale = vetor3;
    }

    public void TomarDano(int hit)
    {
        pv -= hit;
        if (pv <= 0)
        {
            pv = 0;
            GetComponent<SpriteRenderer>().enabled = false;
        }

        percentual = pv / pvMax;
        vetor3 = barraHP.localScale;
        vetor3.x = percentual;
        barraHP.localScale = vetor3;
    }

    public void renomearBotoes()
    {
        botaoA = GameObject.Find("TextoA");
        botaoB = GameObject.Find("TextoB");
        botaoC = GameObject.Find("TextoC");
        botaoD = GameObject.Find("TextoD");

        botaoA.GetComponent<Text>().text = acoes[0];
        botaoB.GetComponent<Text>().text = acoes[1];
        botaoC.GetComponent<Text>().text = acoes[2];
        botaoD.GetComponent<Text>().text = acoes[3];

    }

    public IEnumerator comando(int idAcao)
    {
        switch (idAcao)
        {
            case 1:
                idComando = 0;
                fala = nome + " use " + acoes[idComando] + "!";
                StartCoroutine("dialogo", fala);
                break;

            case 2:
                idComando = 1;
                fala = nome + " use " + acoes[idComando] + "!";
                StartCoroutine("dialogo", fala);
                break;

            case 3:
                idComando = 2;
                fala = nome + " use " + acoes[idComando] + "!";
                StartCoroutine("dialogo", fala);
                break;

            case 4:
                idComando = 3;
                fala = nome + " use " + acoes[idComando] + "!";
                StartCoroutine("dialogo", fala);
                break;
        }
        idFase = 1;
        return null;
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

        switch(idFase)
        {
            case 1:
                StartCoroutine("AplicarDano");
                break;

            case 2:
                pokemonInimigo.StartCoroutine("AcaoInicial");
                break;

            case 3:
                battleController.menuA.SetActive(true);
                break;

            case 4:
                fala = pokemonInimigo.nome + " foi derrotado!";
                StartCoroutine("dialogo", fala);
                idFase = 5;
                break;

            case 5:
                StartCoroutine("ganhaXP", pokemonInimigo.xp);
                idFase = 6;
                break;

            case 6:
                //vitória e etc
                break;
        }
    }

    public IEnumerator AplicarDano()
    {
        GameObject tempPrefab = Instantiate(animacoes[idComando]) as GameObject;
        tempPrefab.transform.position = pokemonInimigo.transform.position;

        hit = Random.Range(1, danoAcoes[idComando]); // retirar esse random e colocar o valor real de cada golpe subtraido da defesa do inimigo
        fala = nome + " usou " + acoes[idComando] + " e causou " + hit + " de dano.";
        StartCoroutine("dialogo", fala);
        yield return new WaitForSeconds(1f);
        pokemonInimigo.TomarDano(hit);
        Destroy(tempPrefab);

        if (pokemonInimigo.pv <= 0)
        {
            idFase = 4;
        }
        else
        {
            idFase = 2;
        }      
    }

    public void ComandoIncial()
    {
        fala = "O que fazer?";
        StartCoroutine("dialogo", fala);
        idFase = 3;
    }

    public IEnumerator ganhaXP( int xpGanho)
    {
        fala = nome + " recebeu " + xpGanho + " xp";
        
        StartCoroutine("dialogo", fala);
        xp += xpGanho;

        percentual = xp / 100f;
        vetor3 = barraXp.localScale;
        vetor3.x = percentual;
        barraXp.localScale = vetor3;

        idFase = 5;

        return null;
    }
}