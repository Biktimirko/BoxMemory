using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour{
    
	//объявляем игровые объекты
	public GameObject pic1;
	public GameObject pic2;
	public GameObject pic3;
	public GameObject pic4;
	public GameObject pic5;
	public GameObject pic6;
	
	public GameObject leftPic;
	public GameObject rightPic;
	public GameObject leftPod;
	public GameObject rightPod;
	
	public GameObject canva;
	
	//Объявляем звуки победы и проигрыш
	public AudioClip good;
	public AudioClip bad;
	
	//флаги состояния игры
	public bool inGame;			//флаг, что идет игра
	public bool opinionChange;	//флаг, что идет время запоминания
	public bool responseTime;	//флаг, что идет время выбора ответа
	public bool reStart;		//флаг, что идет время перезапуска игры
	public bool win;			//флаг, была ли попытка выигрышная
	
	public int choice;			//переменная в которой храниться выбор игрока
	
	//переменные определяющие время игры
	public float timeVisibility = 3;	//время на запоминание
	public float timeGames = 3;			//время на выбор
	public float timeReStart = 2;		//время рестарта игры
	
	//переменные определяющие графику
	public string spriteNames = "gems";				//название спрайтов картинок
	private string spriteInvizible = "invizible";	//название невидимого спрайта
	private string spritePodlogka = "podlogka";		//название спрайта рубашки
   
	//массив для хранения спрайтов картинок
    public Sprite[] sprites;
	
	//спрайт рубашки
	private Sprite Podlogka;
	
	//спрайт невидимого квадрата
	private Sprite Invizible;
	
	//массив для хранения случайных номеров случайных спрайтов
	private ArrayList numbers;
	
	//переменная которая хранит выигрышный  квадрат
	private int winNum;
	
	//переменная которая хранит выигрышный  сторону
	private int winSide;
	
	//переменная которая хранит выигрышный  картинку
	private int winPic;
	
	//переменная отвечающая за скорость изменения цвета в квадрате выбора картинки
	private float colchange; 
	
	//переменные хранения времени
	private float timeRemaining;	//время для запоминания
	private float timeResponse;		//время для ответа
	private float downtime;			//время рестарта
	
	//переменная для цвета квадрата выбора
	private Color m_NewColor;
	
	//скрипт отвечающий за разворот при смене с рубашки на картинку и обратно
	private rotare actionTarget;
	
	//скрипт для взаимодействия с меню настроек
	private menu scriptTarget;
	
	
	
	/*
	метод инициализации скрипта
	*/
    void Start(){
		
		//устанавливаем статус игры
		inGame = false;
		responseTime = false;
		reStart = false;
		
		//сбрасываем цвет квадрата выбора
		colchange=1;
		
		// подгружаем изображения для объектов
        sprites = Resources.LoadAll<Sprite>(spriteNames);		//набор картинок
		Invizible = Resources.Load<Sprite>(spriteInvizible);	//спрайт для невидемых объектов
		Podlogka = Resources.Load<Sprite>(spritePodlogka);		//спрайт для рубашки
		
		// подгрузка звуков выйгрыша и проигрыша
		good = Resources.Load<AudioClip>("good_beep");
		bad = Resources.Load<AudioClip>("bad_beep");
		
		//подгрузка объектов игры	
		pic1 = GameObject.Find("First");
		pic2 = GameObject.Find("Second");
		pic3 = GameObject.Find("Third");
		pic4 = GameObject.Find("Fourth");
		pic5 = GameObject.Find("Fifth");
		pic6 = GameObject.Find("Sixth");
		rightPic = GameObject.Find("Right");
		leftPic = GameObject.Find("Left");		
		rightPod = GameObject.Find("RightPod");
		leftPod = GameObject.Find("LeftPod");
		
		//делаем квадраты выбора невидимыми
		rightPod.GetComponent<SpriteRenderer>().sprite=Invizible;
		leftPod.GetComponent<SpriteRenderer>().sprite=Invizible;

		//подгружаем меню
		canva = GameObject.Find("Canvas");
		//подгружаем скрипт меню
		scriptTarget = canva.GetComponent<menu>();
    }

	/*
	данный метод принимает параметры из меню настроек, для установки параметров игры
	tVis - время для запоминания
	tSin - время для ответа
	tRes - время до рестарта раунда
	name - имя картинок
	*/
	public void setSettings(float tVis,float tSin, float tRes, string name){
	
		//останавливаем игру
		inGame = false;
		responseTime = false;
		reStart = false;
		
		//обновляем переменные
		timeVisibility = tVis;
		timeGames = tSin;
		timeReStart = tRes;
		spriteNames = name;
		
		//сбрасываем цвет квадрата выбора
		colchange=1;
		
		//обновляем коллекцию картинок
        sprites = Resources.LoadAll<Sprite>(spriteNames);
		
		//прячем квадраты выбора
		rightPod.GetComponent<SpriteRenderer>().sprite=Invizible;
		leftPod.GetComponent<SpriteRenderer>().sprite=Invizible;
	}

	/*
	метод начинает раунд
	*/
	public void startRound(){
		
		//создаем массив для случайного выбора изображений по количеству спрайтов
		numbers = new ArrayList ();
		 
		 //наполняем массив случайными значениями
		 for (int i = 0; i < sprites.Length; i++) {
			 numbers.Add(i);
		 }
		
		//получаем скрипты от картинок которые показывают варианты выбора
		//и переключаем их на рубашку
		actionTarget = rightPic.GetComponent<rotare>();		
		actionTarget.goPic(Podlogka);
		actionTarget = leftPic.GetComponent<rotare>();		
		actionTarget.goPic(Podlogka);
		
		//устанавливаем флаг в игре, и сбрасываем флаг рестарт игры
		inGame = true;
		reStart = false;
		
		//сбрасываем переменные от предыдущей игры
		choice=0;
		winNum=0;
		winSide=0;
		winPic=0;
		
		//запуск метода показа и перемешивания картинок
		shufflePictures(numbers);
		
		//сброс значения таймера
		timeRemaining=timeVisibility;
		
		//скрываем квадраты выбора
		rightPod.GetComponent<SpriteRenderer>().sprite=Invizible;
		leftPod.GetComponent<SpriteRenderer>().sprite=Invizible;
	}

	/*
	метод остановки раунда
	*/
	 void stopRound(){
		
		//смена флагов игры
		inGame = false;
		responseTime = false;
		reStart = true;
		
		//сброс значения таймера
		timeRemaining=timeVisibility;
		downtime = timeReStart;
		
		//проверка выйграл ли игрок, запуск скрипта обновления счета, проигрывание музыки
		if (choice == winSide){
			win = true;
			scriptTarget.winR();
			this.GetComponent<AudioSource>().clip = good;
			this.GetComponent<AudioSource>().Play();
		} else {
			win = false;
			scriptTarget.loseR();
			this.GetComponent<AudioSource>().clip = bad;
			this.GetComponent<AudioSource>().Play();
		}
		
		//показываем картинку которая выйграла из картинок выйгрыша 
		if(winSide==1){
		changePik(winNum, leftPic);
		actionTarget = rightPic.GetComponent<rotare>();		
		actionTarget.goPic(Podlogka);
		}else{
		changePik(winNum, rightPic);
		actionTarget = leftPic.GetComponent<rotare>();		
		actionTarget.goPic(Podlogka);
		}
		
		//показываем картинку которая выйграла из которых надо запоминанть
		switch (winPic) {

		case 1:
        changePik(winNum, pic1);
		Debug.Log("1");
        break;

		case 2:
        changePik(winNum, pic2);
		Debug.Log("2");
        break;
		
		case 3:
        changePik(winNum, pic3);
		Debug.Log("3");
        break;

		case 4:
        changePik(winNum, pic4);
		Debug.Log("4");
        break;
		
		case 5:
        changePik(winNum, pic5);
		Debug.Log("5");
        break;

		case 6:
        changePik(winNum, pic6);
		Debug.Log("6");
        break;
		}
	}
	
	/*
	метод остановки игры
	*/
	public	void stopGame(){
		
		//выставляем флаги в остановку игры
		inGame = false;
		responseTime = false;
		reStart = false;
		
		// прячем картинки
		hidePic(Podlogka);
		
		//прячем квадраты выбора
		rightPod.GetComponent<SpriteRenderer>().sprite=Invizible;
		leftPod.GetComponent<SpriteRenderer>().sprite=Invizible;

		//прячем картинки выбора
		actionTarget = rightPic.GetComponent<rotare>();		
		actionTarget.goPic(Podlogka);
		actionTarget = leftPic.GetComponent<rotare>();		
		actionTarget.goPic(Podlogka);
		
		//сбрасываем параметры выигрыша и выбора игрока
		winNum=0;
		winSide=0;
		winPic=0;
	}
	
	/*
	метод фиксированного обновления, отслеживаем нажатие клавиш и считаем время
	*/
	private void FixedUpdate(){
		
		//если игра запущена
		if (inGame){
			//если идет время выбора
			if (responseTime){
				//проверка на нажатие клавиши
				//выход игры в состояние остановки, установка выборов игрока
				if (Input.GetKey(KeyCode.LeftArrow)){
					choice=1;
					opinionChange=true;
					stopRound();				
				}else if (Input.GetKey(KeyCode.RightArrow)){
					choice=2;
					opinionChange=true;
					stopRound();				
				}
				
				//отсчет времени
				timeResponse -= Time.deltaTime;
				//проверка на окончание времени остановка игры
				if (timeResponse<0){	
					stopRound();
				}
			//иначе идет время перезапуск
			}else{
				
				timeRemaining -= Time.deltaTime;
				if (timeRemaining<0){
				
					shuffleDownPic(numbers);
					timeResponse=timeGames;
					responseTime=true;
					hidePic(Podlogka);
				}
			}
		}
		
		//если сейчас рестарт, то отсчитывается время до конца рестарта
		if (reStart){
			downtime -= Time.deltaTime;			
			if (downtime<0){				
				//запускаем следующий раунд
				startRound();
			}
		}
	}

	/*
	метод не фиксированного обновления, обновляем цвет квадрата выбора	
	*/
	private void Update(){
		
		//проверяем флаги игры
		if (reStart){
			//проверяем, сделан ли выбор, если сделан, то отображаем квадрат
			if (opinionChange){
				if (choice==1){				
					leftPod.GetComponent<SpriteRenderer>().sprite=Podlogka;
					rightPod.GetComponent<SpriteRenderer>().sprite=Invizible;
					opinionChange=false;
				}else if (choice==2){				
					rightPod.GetComponent<SpriteRenderer>().sprite=Podlogka;
					leftPod.GetComponent<SpriteRenderer>().sprite=Invizible;
					opinionChange=false;				
				}
			}else{	
			//после выбора, начинаем менять прозрачность квадрата выбора
			//если правильно, то подкрашиваем зеленым
			//если не правильно, то красным
				colchange+=0.01f;			
				if (win){
				m_NewColor = new Color(0, 1, 0, ((Mathf.Sin(colchange)*0.5f)+0.7f));			
				}else{			
				m_NewColor = new Color(1, 0, 0, ((Mathf.Sin(colchange)*0.5f)+0.7f));
				}
				//устанавливаем цвет на квадрат
				rightPod.GetComponent<SpriteRenderer>().color = m_NewColor;
				leftPod.GetComponent<SpriteRenderer>().color = m_NewColor;
			}
		}
	}
	
	/*
	Метод изменяет спрайт на объекте win, на спрайт под номером picNumber
	*/
	void changePik(int picNumber, GameObject win){
		//вызываем скрипт от объекта
		actionTarget = win.GetComponent<rotare>();
		actionTarget.goPic(sprites[picNumber]);
	}
	
	/*
	Метод для рандомного удаления значения из массива ArrayList
	На выход отправляется содержимое удаленной ячейки enter
	*/	
	int arrayRandomDelete (ArrayList mass){		
	
		int deleteInt=Random.Range(0, mass.Count);
		//из mass.Count, потому что какой-то пидор посчитал что для Random.Range int надо исключить верхнюю границу
		int enter = (int)(mass[deleteInt]);
		mass.RemoveAt(deleteInt);
		return enter;
	}
	
	/*
	перемешиваем картинки, выбираем победившую сторону
	ArrayList - массив перемешанных значений
	*/
	void shufflePictures(ArrayList num){
		
		//выбираем картинку победителя
		winPic=Random.Range(1, 7);
		//выбираем сторону победы
		winSide=Random.Range(1, 3);
		
		//устанавливаем картинку победителя
		if (winPic==1){
			winNum = arrayRandomDelete(num);
			changePik(winNum, pic1);
		}else{
			changePik(arrayRandomDelete(num), pic1);
		}
		if (winPic==2){
			winNum = arrayRandomDelete(num);
			changePik(winNum, pic2);
		}else{
			changePik(arrayRandomDelete(num), pic2);
		}
		if (winPic==3){
			winNum = arrayRandomDelete(num);
			changePik(winNum, pic3);
		}else{
			changePik(arrayRandomDelete(num), pic3);
		}
		if (winPic==4){
			winNum = arrayRandomDelete(num);
			changePik(winNum, pic4);
		}else{
			changePik(arrayRandomDelete(num), pic4);
		}
		if (winPic==5){
			winNum = arrayRandomDelete(num);
			changePik(winNum, pic5);
		}else{
			changePik(arrayRandomDelete(num), pic5);
		}
		if (winPic==6){
			winNum = arrayRandomDelete(num);
			changePik(winNum, pic6);
		}else{
			changePik(arrayRandomDelete(num), pic6);
		}
	}
	
	/*
	устанавливаем картинку выигрыша и проигрыша
	ArrayList - массив с замешанными номерами,, для выбора неиспользованной картинкой для проигрышного варианта
	*/
	void shuffleDownPic(ArrayList num){
		
		//проверка выигрышной стороны и установка картинок
		if(winSide==1){
		changePik(winNum, leftPic);
		changePik(arrayRandomDelete(num), rightPic);
		}else{
		changePik(winNum, rightPic);
		changePik(arrayRandomDelete(num), leftPic);	
		}		
	}
	
	/*
	Меняем все картинки на спрайт Pod
	*/
	void hidePic(Sprite Pod){
		
		//вызываем для компонентов скрипт переворота
		actionTarget = pic1.GetComponent<rotare>();		
		actionTarget.goPic(Pod);
		actionTarget = pic2.GetComponent<rotare>();		
		actionTarget.goPic(Pod);
		actionTarget = pic3.GetComponent<rotare>();		
		actionTarget.goPic(Pod);
		actionTarget = pic4.GetComponent<rotare>();		
		actionTarget.goPic(Pod);
		actionTarget = pic5.GetComponent<rotare>();		
		actionTarget.goPic(Pod);
		actionTarget = pic6.GetComponent<rotare>();		
		actionTarget.goPic(Pod);
	}
}
