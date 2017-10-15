using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainZone {

	List<int[]> hexBorders;
	List<int[]> hexInside;
	bool finished;

	public TerrainZone(){
		hexBorders = new List<int[]> ();
		hexInside =  new List<int[]> ();
		finished = false;
	}

	public void AddHexBorder(int[] coordinate){
		hexBorders.Add (coordinate);
	}

	public void AddHexInside(int[] coordinate){
		hexInside.Add (coordinate);
	}

	public void DeleteBorder(int[] coordinate) {
		hexBorders.Remove (coordinate);
		finishCheck ();
	}

	public List<int[]> GetHexBorders(){
		return hexBorders;
	}

	public List<int[]> GetHexInside(){
		return hexInside;
	}

	public void AddMultipleHexBorder(List<int[]> coordinates){
		foreach(int[] elem in coordinates){
			AddHexBorder(elem);
		}
	}

	public int[] ChooseRandomBorder(){
		if (hexBorders.Count > 0) {
			return hexBorders [Random.Range (0, hexBorders.Count - 1)];
		} else {
			return null;
		}
	}

	public void AddBorderInside(int[] coordinate){
		if (hexBorders.Contains (coordinate)) {
			hexInside.Add (coordinate);
			hexBorders.Remove (coordinate);
		} else {
			Debug.Log("There is no such coordinate in hexBorders: "
				+ coordinate[0] + ", " + coordinate[1]);
		}
		finishCheck ();
	}

	public void finishCheck(){
		if (hexBorders.Count <= 0) {
			finished = true;
		}
	}

	public bool isFinished(){
		finishCheck ();
		return finished;
	}



}
