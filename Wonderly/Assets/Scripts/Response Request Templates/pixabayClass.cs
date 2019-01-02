/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for response of web call to Pixabay api 
              to return results after a search
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class pixabayClass {
public int total = 0;
public int totalHits;
public pixabayHitClass[] hits = new pixabayHitClass[50];
}
