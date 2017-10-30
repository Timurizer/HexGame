using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridField : MonoBehaviour
{

	public int length;
	public int width;

	GameObject[,] map;


	// Use this for initialization
	void Start ()
	{
		// SetMap (50, 50);
		GenerateBase (50, 50);
		HexCoordinates a = new HexCoordinates(15, 23);
		HexCoordinates b = new HexCoordinates(8, 12);
		HexCoordinates[] line = HexCoordintaeTools.DrawLine(a, b);
		// HexCoordinates[] line = HexCoordintaeTools.GetRange(a, 8);
		foreach(HexCoordinates hc in line) {
			Destroy (map[hc.getX () + hc.getZ()/2, hc.getZ ()]);
			map [hc.getX () + hc.getZ()/2, hc.getZ ()] = HexFactory.CreateGrass (hc.getX () + hc.getZ()/2, hc.getZ ());
		}


		// string[] paths = { "Water", "Grass", "Mountain" };
		// float[] minHeights = {0f, 0.2f, 0.4f};
		// float[] maxHeights = {0.2f, 0.4f, 1f};
		// SetTerrainZones (Generator.GenerateHightMapped (length, width, paths, maxHeights, minHeights));
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	// Generates a basic hex map
	void GenerateBase (int length, int width)
	{
		this.length = length;
		this.width = width;
		map = new GameObject[length, width];
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < width; j++) {
				GameObject hex = HexFactory.CreateWater (i, j);
				hex.transform.SetParent (this.transform);
				map [i, j] = hex;
			}
		}
	}

	// sets the main parameters of a hex map
	void SetMap (int length, int width)
	{
		map = new GameObject[length, width];
		this.length = length;
		this.width = width;
	}

	void SetTerrainZones (TerrainZone[] terrains)
	{
		foreach (TerrainZone tz in terrains) {
			foreach (int[] pos in tz.GetHexInside()) {
				GameObject hex = HexFactory.CreateHexOfType (pos [0], pos [1], tz.GetTexturePath ());
				hex.transform.SetParent (this.transform);
				map [pos [0], pos [1]] = hex;
			}
		}
	}









		
}
