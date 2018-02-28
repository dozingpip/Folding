using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thingsWithArraysOfDoubleArrays : MonoBehaviour 
{
    [System.Serializable]
    public class thingsWithArrays
    {
        public double[] array;
    }
    public thingsWithArrays[] array;

 //    public double this[int i]
 //    {
	//     get{return array[i];}
	// 	set{array[i] = value;}
	// }
}