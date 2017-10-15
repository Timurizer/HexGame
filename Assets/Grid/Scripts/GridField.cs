using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridField : MonoBehaviour {

	public int length;
	public int width;

	GameObject[,] map;


	// Use this for initialization
	void Start () {
		string[] paths = { "Sand", "Water", "Grass" };
		GenerateTerrain (20, 20, 3, paths);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenerateBase(int length, int width) {
		this.length = length;
		this.width = width;
		map = new GameObject[length, width];
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < width; j++) {
				GameObject hex = HexFactory.CreateHexBase(i, j);
				hex.transform.SetParent(this.transform);
				map [i, j] = hex;
			}
		}
	}

	void GenerateTerrain(int length, int width, int zoneAmount, string[] texturePaths){
		map = new GameObject[length, width];
		this.length = length;
		this.width = width;
		int[,] occupationMap = new int[length, width];
		for (int i = 0; i < length; i ++) {
			for (int j = 0; j < width; j ++) {
				occupationMap[i, j] = 0;
			}
		}

		// set starting positions
		TerrainZone[] terrains = new TerrainZone[zoneAmount];

		for(int i = 0; i < terrains.Length; i++) {
			terrains[i] = new TerrainZone ();
			int[] position;
			do {
				position = new int[]{ Random.Range (0, length), Random.Range (0, width) };
			} while (occupationMap [position [0], position [1]] != 0);
			terrains[i].AddHexInside(position);
			occupationMap[position[0], position[1]] = 1;
			terrains[i].AddMultipleHexBorder(GetEmptyNeighbours (position [0], position [1], occupationMap));
		}

		// main generating loop
		while (true) {

			for(int i = 0; i < terrains.Length; i++){

				if (!terrains[i].isFinished ()) {
					
					int[] pos;
					do {	
						pos = terrains[i].ChooseRandomBorder ();
						if(pos == null){
							break;
						}
						if (occupationMap [pos [0], pos [1]] == 1) {	
							terrains[i].DeleteBorder (pos);
						}
					} while ( occupationMap [pos [0], pos [1]] != 0);	

					if (pos != null) {
						terrains [i].AddBorderInside (pos);
						occupationMap [pos [0], pos [1]] = 1;
						terrains[i].AddMultipleHexBorder (GetEmptyNeighbours (pos [0], pos [1], occupationMap));
					}

				}
			}

			// checking if all zones can't be expanded
			int zonesDone = 0;
			for(int i = 0; i < terrains.Length; i++){
				if(terrains[i].isFinished ()) {
					zonesDone += 1;
				}
			}
			if (zonesDone == terrains.Length) {
				break;
			}
		}
		// instantiating hexes
		for(int i =0; i < texturePaths.Length; i ++) {
			foreach (int[] pos in terrains[i].GetHexInside()) {
				GameObject hex = HexFactory.CreateHexOfType(pos[0], pos[1], texturePaths[i]);
				hex.transform.SetParent(this.transform);
				map [pos[0], pos[1]] = hex;
			}
		}

	}



	// get neighbours that has not been initialized yet
	private List<int[]> GetEmptyNeighbours(int i, int j, int[,] map){
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
			if ( position [0] >= 0 && position [1] >= 0 && position [0] < map.GetLength (0) && position [1] < map.GetLength (1) && map[position[0], position[1]] != 1)
				result.Add (position);
		}

		return result;

	}




		
}
