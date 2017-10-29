using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator
{
	
	// generates a terrain based on a simple algorithm
	public static TerrainZone[] GenerateTerrain (int length, int width, string[] texturePaths)
	{		
		int[,] occupationMap = new int[length, width];
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < width; j++) {
				occupationMap [i, j] = 0;
			}
		}

		// set starting positions
		TerrainZone[] terrains = new TerrainZone[texturePaths.Length];
		for (int i = 0; i < terrains.Length; i++) {
			terrains [i] = new TerrainZone ();
			terrains [i].SetTexturePath (texturePaths [i]);
		}

		for (int i = 0; i < terrains.Length; i++) {
			int[] position;
			do {
				position = new int[]{ Random.Range (0, length), Random.Range (0, width) };
			} while (occupationMap [position [0], position [1]] != 0);
			terrains [i].AddHexInside (position);
			occupationMap [position [0], position [1]] = 1;
			terrains [i].AddMultipleHexBorder (GetEmptyNeighbours (position [0], position [1], occupationMap));
		}

		// main generating loop
		while (true) {

			for (int i = 0; i < terrains.Length; i++) {

				if (!terrains [i].isFinished ()) {

					int[] pos;
					do {	
						pos = terrains [i].ChooseRandomBorder ();
						if (pos == null) {
							break;
						}
						if (occupationMap [pos [0], pos [1]] == 1) {	
							terrains [i].DeleteBorder (pos);
						}
					} while (occupationMap [pos [0], pos [1]] != 0);	

					if (pos != null) {
						terrains [i].AddBorderInside (pos);
						occupationMap [pos [0], pos [1]] = 1;
						terrains [i].AddMultipleHexBorder (GetEmptyNeighbours (pos [0], pos [1], occupationMap));
					}

				}
			}

			// checking if all zones can't be expanded
			int zonesDone = 0;
			for (int i = 0; i < terrains.Length; i++) {
				if (terrains [i].isFinished ()) {
					zonesDone += 1;
				}
			}
			if (zonesDone == terrains.Length) {
				break;
			}
		}
		return terrains;
	}

	// get neighbours that has not been initialized yet
	private static List<int[]> GetEmptyNeighbours (int i, int j, int[,] map)
	{
		List<int[]> neighbours = new List<int[]> ();
		neighbours.Add (new int[]{ i + 1, j });
		neighbours.Add (new int[]{ i - 1, j });
		neighbours.Add (new int[]{ i, j + 1 });
		neighbours.Add (new int[]{ i, j - 1 });
		neighbours.Add (new int[]{ i - 1, j - 1 });
		neighbours.Add (new int[]{ i - 1, j + 1 });

		List<int[]> result = new List<int[]> ();
		foreach (int[] position in neighbours) {
			if (position [0] >= 0 && position [1] >= 0 && position [0] < map.GetLength (0) && position [1] < map.GetLength (1) && map [position [0], position [1]] != 1)
				result.Add (position);
		}
		return result;
	}

	// generating height map
	private static float[,] GenerateHeightMapPerlin(int length, int width) {
		float[,] heightMap = new float[length, width];
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < width; j++) {
				float xCoord = 1f + ((float)i / (float)length);
				float yCoord = 14f + ((float)j / (float)width);
				heightMap [i, j] = Mathf.PerlinNoise (xCoord, yCoord);
			}
		}
		return heightMap;
	}

	public static TerrainZone[] GenerateHightMapped(int length, int width, string[] texturePaths, float[] maxHeight, float[] minHeight){
		float[,] heightMap = GenerateHeightMapPerlin (length, width);
		TerrainZone[] terrains = new TerrainZone[texturePaths.Length];
		for (int i = 0; i < terrains.Length; i++) {
			terrains [i] = new TerrainZone ();
			terrains [i].SetTexturePath (texturePaths [i]);
			terrains [i].SetMaxHeight (maxHeight [i]);
			terrains [i].SetMinHeight (minHeight [i]);
		}
		foreach (TerrainZone tz in terrains) {
			for (int i = 0; i < length; i++) {
				for (int j = 0; j < length; j++) {
					
					if (heightMap [i, j] < tz.GetMaxHeight () && heightMap [i, j] >= tz.GetMinHeight ()) {
						tz.AddHexInside (new int[]{i, j});
					}
				}
			}
		}
		return terrains;
	}
		
	// generates a bell curve
	float[] GaussianBell(float mean, float stdDev, int size) {
		float[] data = new float[size];

		for (int i = 0; i < size; i++) {
			float u1 = Random.Range(0.0f, 1.0f);
			float u2 = Random.Range(0.0f, 1.0f);
			float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
			data[i] = mean + stdDev * randStdNormal;
		}

		return data;
	}

	 
}
