using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	enum unitClass {
		Enemy5star = -15,
		Enemy4star = -14,
		Enemy3star = -13,
		Enemy2star = -12,
		Enemy1star = -11,
		EnemyColonel = -10,
		EnemyLTCol = -9,
		EnemyMajor = -8,
		EnemyCaptain = -7,
		Enemy1stLT = -6,
		Enemy2ndLT = -5,
		EnemySergeant = -4,
		EnemyPrivate = -3,
		EnemySpy = -2,
		EnemyFlag = -1,
		NoUnit = 0,
		MyFlag = 1,
		MySpy = 2,
		MyPrivate = 3,
		MySergeant = 4,
		My2ndLT = 5,
		My1stLT = 6,
		MyCaptain = 7,
		MyMajor = 8,
		MyLTCol = 9,
		MyColonel = 10,
		My1star = 11,
		My2star = 12,
		My3star = 13,
		My4star = 14,
		My5star = 15
	};

	unitClass[,] map = new unitClass[9, 8];
	int[] myCount = new int[15] {1, 2, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
	//int[] enemyCount = new int[15] {1, 2, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};

	unitClass id = unitClass.NoUnit;

	bool placing = false;
	bool placeComplete = false;
	bool gameStart = false;
	bool myTurn = false;
	bool enemyTurn = false;
	bool moving = false;

	RaycastHit hit = new RaycastHit();

	// Use this for initialization
	void Start () {
		int i, j;
		for(j = 0; j < 8; j++)
			for(i = 0; i < 9; i++)
				map[i, j] = unitClass.NoUnit;
	}
	
	// Update is called once per frame
	void Update () {
		int i;
		placeComplete = true;
		for (i=0; i<15; i++) {
			if (myCount [i] > 0) {
				placeComplete = false;
				break;
			}
		}

		if(Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if(placing) {
					if(ShellY(hit.point.y) > 2) Debug.Log("You Cannot Place Your Unit There (Not Your Area)");
					else if(map[ShellX(hit.point.x), ShellY(hit.point.y)] != unitClass.NoUnit) Debug.Log("You Cannot Place Your Unit There (There Is Another Unit Already)");
					else {
						GameObject newUnit = GameObject.Find(id.ToString());
						Instantiate(newUnit, new Vector3((float)ShellPositionX(ShellX(hit.point.x)),(float)ShellPositionY(ShellY(hit.point.y)),-25.4f), Quaternion.identity);
						map[ShellX(hit.point.x), ShellY(hit.point.y)] = id;
						myCount[((int)id - 1)]--;
						placing = false;
					}
				}
				/*if(gameStart && myTurn) {
					int startX, startY;
					unitClass startClass;
					if(map[ShellX(hit.point.x), ShellY(hit.point.y)] != unitClass.NoUnit) {
						moving = true;
						startX = ShellX(hit.point.x);
						startY = ShellY(hit.point.y);
						startClass = map[startX, startY];
					}
					if(moving) {
						if(((startX == ShellX(hit.point.x)) && (((startY - ShellY(hit.point.y)) == 1) || ((startY - ShellY(hit.point.y)) == -1))) ||
						   ((startY == ShellY(hit.point.y)) && (((startX - ShellX(hit.point.x)) == 1) || ((startX - ShellX(hit.point.x)) == -1)))) {
						   
						}
					}
				}*/
			}
		}
	}
	
	void OnGUI () {
		string[] classes = new string[15];
		int i, j;
		for(i = 0; i < 15; i++) classes[i] = ((unitClass)(i + 1)).ToString() + " : " + myCount[i];

		for(j = 0; j < 8; j++)
			for(i = 0; i < 9; i++)
				GUI.Label (new Rect(1500 + i * 30, 300 - j * 30, 30, 30), ((map[i, j] < 0) ? "?" : map[i, j].ToString("d")));

		GUI.Label (new Rect (1500, 330, 300, 30), "Mouse Point Ray Hit On " + hit.point.ToString ());
		GUI.Label (new Rect (1500, 360, 300, 30), "Shell Position Of Ray Hit : (" + ShellX(hit.point.x) + ", " + ShellY(hit.point.y) + ")");

		if (gameStart == false) {		
			GUI.Box (new Rect (135, 90, 240, 360), "Place Your Units Manually");
			int sg = GUI.SelectionGrid (new Rect (150, 135, 210, 300), -1, classes, 2);
			if (sg >= 0) {
				if (myCount [sg] <= 0) Debug.Log ("You Can't Place It Any More");
				else {
					id = (unitClass)sg + 1;
					placing = true;
				}
			}
			if(placeComplete == false) {
				if (GUI.Button (new Rect (150, 465, 150, 30), "Place Randomly")) {
					placeRandomly();
						placeComplete = true;
				}
			}
			if (placeComplete == true) {
				if (GUI.Button (new Rect (150, 500, 150, 30), "Game Start")) {
					placeEnemyUnits();
					Debug.Log ("Game Start");
					if(Random.Range(0,2) == 1) {
						myTurn = true;
						enemyTurn = false;
						Debug.Log("You First.");
					}
					else {
						enemyTurn = true;
						myTurn = false;
						Debug.Log("Enemy First.");
					}
					gameStart = true;
				}
			}
		}
	}

	int ShellX (float x) {
		if (x >= 37.7) return 0;
		if ((x >= 26.9) && (x < 37.7)) return 1;
		if ((x >= 16.2) && (x < 26.9)) return 2;
		if ((x >= 5.4) && (x < 16.2)) return 3;
		if ((x >= -5.4) && (x < 5.4)) return 4;
		if ((x >= -16.2) && (x < -5.4)) return 5;
		if ((x >= -26.9) && (x < -16.2)) return 6;
		if ((x >= -37.7) && (x < -26.9)) return 7;
		if (x < -37.7) return 8;
		return -1;
	}

	int ShellY (float y) {
		if (y < -36.5) return 0;
		if ((y >= -36.5) && (y < -24.6)) return 1;
		if ((y >= -24.6) && (y < -12.7)) return 2;
		if ((y >= -12.7) && (y < 0)) return 3;
		if ((y >= 0) && (y < 12.7)) return 4;
		if ((y >= 12.7) && (y < 24.6)) return 5;
		if ((y >= 24.6) && (y < 36.5)) return 6;
		if (y >= 36.5) return 7;
		return -1;
	}

	float ShellPositionX (int sx) {
		switch (sx) {
		case 0 :
			return 43.0f;
		case 1 :
			return 32.25f;
		case 2 :
			return 21.5f;
		case 3 :
			return 10.75f;
		case 4 :
			return 0.0f;
		case 5 :
			return -10.75f;
		case 6 :
			return -21.5f;
		case 7 :
			return -32.25f;
		case 8 :
			return -43.0f;
		default :
			return 0f;
		}
	}

	float ShellPositionY (int sy) {
		switch (sy) {
			case 0 :
				return -42.4f;
			case 1 :
				return -30.6f;
			case 2 :
				return -18.8f;
			case 3 :
				return -7.0f;
			case 4 :
				return 6.6f;
			case 5 :
				return 18.4f;
			case 6 :
				return 30.2f;
			case 7 :
				return 42.0f;
			default :
				return 0f;
		}
	}

	void placeRandomly () {
		int spare = 0;
		int blank = 0;
		int i, j;

		for (i = 0; i < 15; i++)
			spare += (myCount[i]);
		int[] spareStore = new int[spare];
		for (i = 0, j = 0; i < 15; i++) {
			while (myCount[i] > 0) {
				spareStore[j] += (i + 1);
				myCount[i]--;
				j++;
			}
		}
		arrayShuffle (spareStore, spare);

		for (i = 0; i < 18; i++)
			if (map [(i % 9), (i / 9)] == unitClass.NoUnit)
				blank++;
		int[] blankStore = new int[blank];
		for (i = 0, j = 0; i < 18; i++) {
			if (map [(i % 9), (i / 9)] == unitClass.NoUnit) {
				blankStore[j] = i;
				j++;
			}
		}
		arrayShuffle (blankStore, blank);

		for (i = 0; i < blank; i++) {
			GameObject newUnit = GameObject.Find(((unitClass)spareStore[i]).ToString());
			Instantiate(newUnit, new Vector3((float)ShellPositionX(blankStore[i] % 9), (float)ShellPositionY(blankStore[i] / 9), -25.4f), Quaternion.identity);
			map[(blankStore[i] % 9), (blankStore[i] / 9)] = (unitClass)spareStore[i];
		}

		int[] lastLine = new int[9];
		for (i = 0; i < 9; i++) {
			if(i < (spare - blank)) lastLine[i] = spareStore[(spare - i - 1)];
			else lastLine[i] = 0;
		}
		arrayShuffle (lastLine, 9);

		for (i = 0; i < 9; i++) {
			if(lastLine[i] != 0) {
				GameObject newUnit = GameObject.Find(((unitClass)lastLine[i]).ToString());
				Instantiate(newUnit, new Vector3((float)ShellPositionX(i % 9), (float)ShellPositionY(2), -25.4f), Quaternion.identity);
				map[(i % 9), 2] = (unitClass)lastLine[i];
			}
		}
	}

	void placeEnemyUnits () {
		int i;
		int[] spareStore = new int[21] {-1, -2, -2, -3, -3, -3, -3, -3, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15};
		int[] blankStore = new int[18] {71, 70, 69, 68, 67, 66, 65, 64, 63, 62, 61, 60, 59, 58, 57, 56, 55, 54};
		arrayShuffle (spareStore, 21);
		arrayShuffle (blankStore, 18);

		for (i = 0; i < 18; i++) {
			GameObject newUnit = GameObject.Find(((unitClass)spareStore[i]).ToString());
			Instantiate(newUnit, new Vector3((float)ShellPositionX(blankStore[i] % 9),(float)ShellPositionY(blankStore[i] / 9),-25.4f), Quaternion.identity);
			map[(blankStore[i] % 9), (blankStore[i] / 9)] = (unitClass)spareStore[i];
		}
		
		int[] lastLine = new int[9];
		for (i = 0; i < 9; i++) {
			if(i < 3) lastLine[i] = spareStore[(20 - i)];
			else lastLine[i] = 0;
		}
		arrayShuffle (lastLine, 9);
		
		for (i = 0; i < 9; i++) {
			if(lastLine[i] != 0) {
				GameObject newUnit = GameObject.Find(((unitClass)lastLine[i]).ToString());
				Instantiate(newUnit, new Vector3((float)ShellPositionX(i % 9),(float)ShellPositionY(5),-25.4f), Quaternion.identity);
				map[(i % 9), 5] = (unitClass)lastLine[i];
			}
		}
	}

	void arrayShuffle (int[] array, int size) {
		int i, r, temp = 0;

		for (i = 0; i < size; i++) {
			r = Random.Range(i, size);
			temp = array[i];
			array[i] = array[r];
			array[r] = temp;
		}
	}
}
