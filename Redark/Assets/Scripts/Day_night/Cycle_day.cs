using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Cycle_day : MonoBehaviour
{    
     public float duracao_dia;
     private float tempo_trasicao_luz;
     private float Tempo;
     private float dia;
     [SerializeField] private TextMeshProUGUI Contagem_dia;
     [SerializeField] private Volume globalVolume;
     [SerializeField] private float pesoDia = 1f; 
     [SerializeField] private float pesoNoite = 0f;
     private float tempo_aux;

    void Start()
    {
           dia = 86400 / duracao_dia;
           Tempo = 0;
           tempo_trasicao_luz = duracao_dia / 24;
    }


    void Update()
    { 
        Tempo += Time.deltaTime * dia;

        if (Tempo > 86400) {
            Tempo = 0;
            tempo_aux = 0;
        }
        CalcularHorario();

        if (Tempo >= 14400 && Tempo <= 18000) { 
            tempo_aux += Time.deltaTime;
          if (globalVolume == null) return;

        float intensityMultiplier = Mathf.Clamp01(0 + (tempo_aux / tempo_trasicao_luz));
        globalVolume.weight = Mathf.Lerp(pesoDia, pesoNoite, intensityMultiplier);
    }  
         
         if (Tempo > 18000 && Tempo < 64800) {
            tempo_aux = 0;
         }

        if (Tempo >= 64800 && Tempo <= 68400 ) {
            tempo_aux += Time.deltaTime;
            if (globalVolume == null) return;

        float intensityMultiplier = Mathf.Clamp01(1 - (tempo_aux / tempo_trasicao_luz));
        globalVolume.weight = Mathf.Lerp(pesoDia, pesoNoite, intensityMultiplier);
        }
}
    private void CalcularHorario() {
        Contagem_dia.text = TimeSpan.FromSeconds(Tempo).ToString(@"hh\:mm");
    }
} 

