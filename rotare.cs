using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotare : MonoBehaviour{
	
	//вектора отвечающие за скорость переворота
	private Vector3 scaleChange, positionChange;
	
	//флаги состояния
	public bool razvorot;
	public bool zamena;
	
	//спрайт на который будет меняться картинка
	private Sprite Change;
	
    // Start is called before the first frame update
    void Start(){
		
		//задаем скорости
		scaleChange = new Vector3(-0.5f, 0, 0);
		//задаем направление смещения
        positionChange = new Vector3(0.0f, 0, 0);
		//устанавливаем флаги
		razvorot = true;
		zamena = false;
    }

    // Update is called once per frame
    void Update(){
		
		//проверяем флаги и запускаем смену картинки
		if (zamena){
		swap(Change);	
		}
	}
	
	/*
	метод где происходит переварот картинки
	spriteSwap - картинка на которую меняем
	*/
	private void swap(Sprite spriteSwap){
		
		//делаем смещение позиции и размера
		this.transform.localScale += scaleChange;
        this.transform.position += positionChange;
        // Move upwards when the sphere hits the floor or downwards
        // when the sphere scale extends 1.0f.
		//здесь проверяем, не произошёл выход за границы
        if (this.transform.localScale.x < 0.1f || this.transform.localScale.x > 5.0f){
            //если перешли за пределы, то меняем направление
			scaleChange = -scaleChange;
            positionChange = -positionChange;
			//проверяем куда происходит смещение
			if (razvorot == true){
				razvorot = false;
				//как только картинка достаточно сузилась, мы меняем изображение
				this.GetComponent<SpriteRenderer>().sprite=spriteSwap;
			}else{
				//после замены картинки выставляем флаги
				zamena = false;
				razvorot = true;
			}
		}
	}
	
	/*
	публичный метод, для свапа картинки из скрипта отвечающего за игру
	spriteSwap - спрайт на который будет меняться изображение
	*/
	public void goPic(Sprite spriteSwap){
		
		//установка спрайта
		Change = spriteSwap;
		//установка флага
		zamena = true;
	}
}
