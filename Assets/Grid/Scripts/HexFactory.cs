using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexFactory : MonoBehaviour {

	protected static HexFactory instance;

	void Start()
	{	
		instance = this;
	}

	// instantiating and setting the position of hex
	private static GameObject CreateHexBase(int x, int z){
		GameObject hex = Instantiate(Resources.Load("Prefabs/Hex")) as GameObject;
		hex.GetComponent<Hex>().Initialize(x, z);
		hex.GetComponent<Hex>().Draw();
		return hex;
	}

	// just a basic case
	public static GameObject CreateHex(int x, int z){
		GameObject hex = CreateHexBase (x, z);
		hex.GetComponent<Hex> ().ApplyTexture ("Materials/defaultMat");
		return hex;
	}

	public static GameObject CreateHexOfType(int x, int z, string materialPath = "defaultMat"){
		GameObject hex = CreateHexBase (x, z);
		hex.GetComponent<Hex> ().ApplyTexture ("Materials/" + materialPath);
		return hex;
	}

	public static GameObject CreateSand(int x, int z){
		return CreateHexOfType(x, z, "Sand");
	}

	public static GameObject CreateWater(int x, int z){
		return CreateHexOfType(x, z, "Water");
	}

	public static GameObject CreateGrass(int x, int z){
		return CreateHexOfType(x, z, "Grass");
	}
		
}
