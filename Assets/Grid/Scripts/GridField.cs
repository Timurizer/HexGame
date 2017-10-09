using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridField : MonoBehaviour {

	public int length;
	public int width;

	GameObject[,] map;


	// Use this for initialization
	void Start () {
		Generate (20, 20);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Generate(int length, int width) {
		this.length = length;
		this.width = width;
		map = new GameObject[length, width];
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < width; j++) {
				GameObject hex = HexFactory.CreateSand(i, j);
				hex.transform.SetParent(this.transform);
				map [i, j] = hex;
			}
		}
	}

	void GenerateTerrain(){
		
	}

	// get neighbours that has not been initialized yet
	private List<int[]> GetEmptyNeighbours(int i, int j){
		List<int[]> neighbours = new List<int[]> ();
		neighbours.Add (new int[]{ i + 1, j });
		neighbours.Add(new int[]{i - 1, j});
		neighbours.Add (new int[]{ i, j + 1});
		neighbours.Add (new int[]{ i, j - 1 });
		neighbours.Add (new int[]{ i - 1, j - 1 });
		neighbours.Add (new int[]{ i - 1, j - 1 });
		neighbours.Add (new int[]{ i - 1, j + 1 });

		List<int[]> result = new List<int[]> ();
		foreach (int[] position in neighbours) {
			// map [position [0], position [1]].GetComponent<Renderer> ().material.name == "defaultMat" &&
			if (position [0] >= 0 && position [1] >= 0 && position [0] < map.GetLength (0) && position [1] < map.GetLength (1))
				result.Add (position);
		}

		return result;

	}
		
}
