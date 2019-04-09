using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leanTouchAxis : MonoBehaviour {

	
		//rotationAxis
		//0 = x axis
		//1 = y axis
		//2 = z axis
		public int rotationAxis = 0;
		//sets the rotation axis, called from x y z buttons on link panel
		public void setRotationAxis(int x){
			rotationAxis = x;
		}

		public void addRotationAxis()
		{
			rotationAxis++;
			if (rotationAxis == 3)
			{
				rotationAxis = 0;
			}
		}

}
