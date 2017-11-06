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
		HexCoordinates b = new HexCoordinates(16, 21);
		HexCoordinates start = new HexCoordinates(15, 22);

		List<HexCoordinates> blocked = new List<HexCoordinates> ();
		blocked.Add (a);
		blocked.Add (b);
		blocked.Add (new HexCoordinates(16, 22));
		blocked.Add (new HexCoordinates(12, 24));
		blocked.Add (new HexCoordinates(12, 23));
		List<HexCoordinates> range = HexCoordintaeTools.GetMovementRange (start, 3, blocked);
		//HexCoordinates[] range1 = HexCoordintaeTools.GetRange(a, 8);
		//HexCoordinates[] range2 = HexCoordintaeTools.GetRange(b, 8);
		foreach(HexCoordinates hc in range) {
			Destroy (map[hc.X + hc.Z/2, hc.Z]);
			map [hc.X + hc.Z/2, hc.Z] = HexFactory.CreateGrass (hc.X + hc.Z/2, hc.Z);
		}
		foreach(HexCoordinates hc in blocked) {
			Destroy (map[hc.X + hc.Z/2, hc.Z]);
			map [hc.X + hc.Z/2, hc.Z] = HexFactory.CreateSand (hc.X + hc.Z/2, hc.Z);
		}
		//  HexCoordinates[] intersection = HexCoordintaeTools.ZoneIntersection (range1, range2);
		// foreach(HexCoordinates hc in intersection) {
		Destroy (map[start.X + start.Z/2, start.Z]);
		map [start.X + start.Z/2, start.Z] = HexFactory.CreateMountain (start.X +start.Z/2, start.Z);
		// }


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
