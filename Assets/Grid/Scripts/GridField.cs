using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridField : MonoBehaviour
{

	public int length;
	public int width;

	GameObject[,] map;

	string mode;


	// Use this for initialization
	void Start ()
	{
		// SetMap (50, 50);
		GenerateBase (50, 50);
		mode = "";
	}

	void OnGUI () {
		// Make a background box
		GUI.Box(new Rect(10,10,100,180), "Terrain type");


		if(GUI.Button(new Rect(20,40,80,20), "Water")) {
			this.mode = "Water";
		}
			
		if(GUI.Button(new Rect(20,70,80,20), "Grass")) {
			this.mode = "Grass";
		}

		if(GUI.Button(new Rect(20,100,80,20), "Sand")) {
			this.mode = "Sand";
		}

		if(GUI.Button(new Rect(20,130,80,20), "Mountain")) {
			this.mode = "Mountain";
		}
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

	public void ChangeHex(int x, int z) {
		Destroy (map [x + z/2, z]);
		map [x + z/2, z] = HexFactory.CreateHexOfType (x + z/2, z, mode);
		map [x + z/2, z].transform.SetParent (this.transform);
	}

		
}
