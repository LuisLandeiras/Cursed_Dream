using System.Collections.Generic;
using UnityEngine;

/*
Aqui é para adicionar as frases que podem aparecer no puzzle de escolha de frases
*/
public class Sentences : MonoBehaviour{
    private readonly List<string> sentences = new List<string>();

    public void SetSentences(){
        sentences.Add("You are mine");
        sentences.Add("Feel the corruption");
        sentences.Add("No salvation never");
        sentences.Add("Surrender your soul");
        sentences.Add("Bow before me");
        sentences.Add("Embrace the darkness");
        sentences.Add("Obey my will");
        sentences.Add("Eternal suffering awaits");
    }

    public List<string> GetSentences(){return sentences;}
}
