using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class menu : MonoBehaviour{
	
	//объявляем и устанавливаем состояние меню
	public bool isOpened = false;
	
	//объявляем переменные для настроек игры
	public float tVis;
	public float tSin;
	public float tRes;
	public int numOfPic;
	public string nameOfPic;
	
	//объявляем объекты на которых будет отображаться информация об очках
	public GameObject Vis;
	public GameObject Sin;
	public GameObject Res;
	public GameObject Drop;
	public GameObject winWind;
	public GameObject loseWind;
	public GameObject ratWind;
	public int win=0;
	public int lose=0;
	public int rat=0;
	
	//объявляем скрипт, для записи переменных в скрипт GameScript
	public GameObject gScript;
	private GameScript actionTarget;
	
	//объявляем подменю насстроек для переключения их в будущем
	public GameObject MineMenu;
	public GameObject settings;
		
    // Start is called before the first frame update
    void Start(){
		
		//подгрузка объектов
        gScript = GameObject.Find("background");
		Vis = GameObject.Find("Canvas/settings/mem");
		Sin = GameObject.Find("Run");
		Res = GameObject.Find("Bet");
		Drop = GameObject.Find("Dropdown");
		winWind = GameObject.Find("win");
		loseWind = GameObject.Find("lose");
		ratWind = GameObject.Find("ratio");
		MineMenu = GameObject.Find("MineMenu");
		settings = GameObject.Find("settings");
		
		//так как на старте все меню активно, нам надо скрыть половину, поэтому отключаем отображение настроек
		settings.SetActive(false);
		
		//подгружаем скрипт
		actionTarget = gScript.GetComponent<GameScript>();
    }

    // Update is called once per frame
	/*
	проводим проверку нажатия кнопок
	*/
   void Update(){
		
		//при нажатии Escape переводим игру в остановку, и сбрасываем меню до главного
		if(Input.GetKey(KeyCode.Escape)) {
			actionTarget.stopGame();
			MineMenu.SetActive(true);
			settings.SetActive(false);
		}
	}
	
	/*
	метод для отображения статистики в случае победы
	*/
	public void winR(){
	
	//увеличиваем счетчик
	win++;
		//проверка на крайние значения, чтобы избежать деления на ноль и бесконечность
		if ((lose==0)&(win>0)){		
			rat=100;
		}else if((lose==0)&(win==0)){
			rat=0;
		}else{
			//считаем процент выйгрыша
			rat=(win*100)/(lose+win);
		}
	//обновляем текст на интерфейсе
	winWind.GetComponent<TMP_Text>().text=win+"";
	loseWind.GetComponent<TMP_Text>().text=lose+"";
	ratWind.GetComponent<TMP_Text>().text=rat+"%";
	}
	
	/*
	метод для отображения статистики в случае проигрыша
	*/
	public void loseR(){
	
	//увеличиваем счетчик
	lose++;
		//проверка на крайние значения, чтобы избежать деления на ноль и бесконечность
		if ((lose==0)&(win>0)){		
			rat=100;
		}else if((lose==0)&(win==0)){
			rat=0;
		}else{
			//считаем процент выйгрыша
			rat=(win*100)/(lose+win);
		}
	//обновляем текст на интерфейсе
	winWind.GetComponent<TMP_Text>().text=win+"";
	loseWind.GetComponent<TMP_Text>().text=lose+"";
	ratWind.GetComponent<TMP_Text>().text=rat+"%";
	}
	
	/*
	Метод для сохранения настроек
	*/
	public void saveSettings(){
	
	//берем значения из интерфейса и сохраняем в переменные
	tVis = (float)Vis.GetComponent<Slider>().value;	
	tSin = (float)Sin.GetComponent<Slider>().value;	
	tRes = (float)Res.GetComponent<Slider>().value;	
	numOfPic = Drop.GetComponent<TMP_Dropdown>().value;		
	
	//конвертируем числовые значения из выпадающего меню в названия наборов спрайтов
	switch(numOfPic){
	case 0:
        nameOfPic = "gems";
    break;
	case 1:
        nameOfPic = "potions";
    break;
	case 2:
        nameOfPic = "books";
    break;
	}
	
	//вызываем метод из GameScript, для записи настроек
	actionTarget.setSettings(tVis, tSin, tRes, nameOfPic);
	}
	
	/*
	Метод для запуска игры
	*/
	public void getStart(){
	
	//вызываем метод из GameScript, для записи настроек	
	actionTarget.startRound();
	}
}
