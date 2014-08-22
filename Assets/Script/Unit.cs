using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	//public float angle = 30.0f;
	//public float velocity = 1.0f;

	enum unitClass {
		enemy5star = -15,
		enemy4star = -14,
		enemy3star = -13,
		enemy2star = -12,
		enemy1star = -11,
		enemyColonel = -10,
		enemyLTCol = -9,
		enemyMajor = -8,
		enemyCaptain = -7,
		enemy1stLT = -6,
		enemy2ndLT = -5,
		enemySergeant = -4,
		enemyPrivate = -3,
		enemySpy = -2,
		enemyFlag = -1,
		noUnit = 0,
		myFlag = 1,
		mySpy = 2,
		myPrivate = 3,
		mySergeant = 4,
		my2ndLT = 5,
		my1stLT = 6,
		myCaptain = 7,
		myMajor = 8,
		myLTCol = 9,
		myColonel = 10,
		my1star = 11,
		my2star = 12,
		my3star = 13,
		my4star = 14,
		my5star = 15
	};
	public int unitClassId;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
