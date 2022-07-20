using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class melody : MonoBehaviour{
	
		//мелодии которые будут проигрываться
		private AudioClip melody_1;
		private AudioClip melody_2;
		private AudioClip melody_3;
		//массив для кругового доступа
		private AudioClip[] melodies;
		
		//номер трека
		private int num=0;
		
		//переменная для хронения длины проигрываемого трека
		private float tMelody=0.0f;
		
		//доступ к AudioSource
		private AudioSource m_AudioSource;
		
    // Start is called before the first frame update
    void Start(){
		
		//получаем доступ к AudioSource объекта на котором висит скрипт
		m_AudioSource = GetComponent<AudioSource>();
		
		//получаем мелодии из папки с ресурсами
        melody_1 = Resources.Load<AudioClip>("1_melody");
		melody_2 = Resources.Load<AudioClip>("2_melody");
		melody_3 = Resources.Load<AudioClip>("3_melody");
		
		//объявляем массив с музыкой
		melodies = new AudioClip[3];
		
		//присваемваем треки в массив
		melodies[0]=melody_1;
		melodies[1]=melody_2;
		melodies[2]=melody_3;
		
		//отправляем первую мелодию на AudioSource
		m_AudioSource.clip = melodies[0];
		
		//присваеваем переменной длину трека
		tMelody = m_AudioSource.clip.length;
		
		//запускаем трек
		m_AudioSource.Play();
		
    }

    // Update is called once per frame
    void Update(){
		
		//измеряем время до конца трека		
		tMelody -= Time.deltaTime;
			
			//когда время кончается переключаемся на следующий трек
			if (tMelody<0){				
				++num; 	
				// проверяем есть ли еще треки, если нет возвращаемся на первый
				if (num == melodies.Length){				
				num=0;
				}
				
				//останавливаем трек
				m_AudioSource.Stop();
				//вызываем метод, для переключения трека
				changeMelody(num);
					
				}
		
    }
	
	/*
	данный метод меняет трек в AudioSource по индексу трека
	*/
	void changeMelody (int i){	
	
		//присвеваем новый трек
		m_AudioSource.clip = melodies[i];
		//обновляем переменную длины трека
		tMelody = m_AudioSource.clip.length;
		//запускаем новый трек
		m_AudioSource.Play();		
	}
	
}
