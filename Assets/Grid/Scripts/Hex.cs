using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public struct HexCoordinates {

	[SerializeField]
	private int x, y, z;

	public int X {
		get {
			return x;
		}
	}

	public int Y {
		get {
			return y;
		}
	}

	public int Z {
		get {
			return z;
		}
	}

	public HexCoordinates (int x, int z) {
		this.x = x;
		this.z = z;
		this.y = -x - z;
	}

	public HexCoordinates (int x, int y, int z) {
		this.x = x;
		this.z = z;
		this.y = y;
	}

	public int getX(){
		return this.x;
	}

	public int getY(){
		return this.Y;
	}

	public int getZ(){
		return this.Z;
	}

	public override string ToString () {
		return "(" +
			X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
	}

	public string ToStringOnSeparateLines () {
		return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
	}

	public static HexCoordinates FromOffsetCoordinates (int x, int z) {
		return new HexCoordinates(x, z);
	}
}

public class Hex : MonoBehaviour
{

	int height;
	public int x;
	public int z;
	static float outerRadius = 1f;
	static float innerRadius = outerRadius * 0.866025404f;
	Vector3[] corners;
	bool locked;
	public HexCoordinates hexCoords;

	// Use this for initialization
	void Start ()
	{
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	// sets the properties of hex and sets its position
	public void Initialize (int x, int z)
	{
		this.x = x - z / 2;
		this.z = z;
		hexCoords = FromOffsetCoordinates (x, z);

		Vector3 transformation = new Vector3 (transform.position.x + x * 0.866025404f * 2f, transform.position.y, transform.position.z + z * 1.5f);
		if (z % 2 != 0) {
			transformation.x += 0.866025404f;
		}
		transform.localPosition = transformation;
		GetCorners ();
		DrawMesh ();
		DrawBorders ();
		locked = false;
	}

	public static HexCoordinates FromOffsetCoordinates (int x, int z) {
		return new HexCoordinates(x - z / 2, z);
	}


	public void Lock ()
	{
		locked = true;
	}

	public bool IsLocked ()
	{
		return locked;
	}

	public HexCoordinates GetHexCoord(){
		return this.hexCoords;
	}

	//locates the corners of hex
	private void GetCorners ()
	{
		Vector3 currentPosition = this.transform.localPosition;

		corners = new Vector3[] {
			currentPosition + new Vector3 (0f, 0f, outerRadius),
			currentPosition + new Vector3 (innerRadius, 0f, 0.5f * outerRadius),
			currentPosition + new Vector3 (innerRadius, 0f, -0.5f * outerRadius),
			currentPosition + new Vector3 (0f, 0f, -outerRadius),
			currentPosition + new Vector3 (-innerRadius, 0f, -0.5f * outerRadius),
			currentPosition + new Vector3 (-innerRadius, 0f, 0.5f * outerRadius),
			currentPosition + new Vector3 (0f, 0f, outerRadius)
		};
	
	}

	// locates the local positions of hex corners
	private Vector3[] GetCornersLocal ()
	{
		Vector3 currentPosition = this.transform.localPosition;

		Vector3[] corners = new Vector3[] {
			new Vector3 (0f, 0f, outerRadius),
			new Vector3 (innerRadius, 0f, 0.5f * outerRadius),
			new Vector3 (innerRadius, 0f, -0.5f * outerRadius),
			new Vector3 (0f, 0f, -outerRadius),
			new Vector3 (-innerRadius, 0f, -0.5f * outerRadius),
			new Vector3 (-innerRadius, 0f, 0.5f * outerRadius),
			new Vector3 (0f, 0f, outerRadius)
		};

		return corners;
	}

	// makes hex's borders
	public void DrawBorders ()
	{
		GameObject borders = new GameObject ();
		borders.name = "borders";
		int i = 0;
		while (i < 6) {

			GameObject border = Instantiate (Resources.Load ("Prefabs/Border")) as GameObject;
			border.transform.position = (corners [i + 1] + corners [i]) / 2;
			border.transform.rotation = Quaternion.Euler (0, 30 + 60 * i, 0);
			border.transform.SetParent (borders.transform);
			i++;
		}

		borders.transform.SetParent (this.transform);
	}

	// makes hex's mesh
	public void DrawMesh ()
	{
		gameObject.AddComponent<MeshFilter> ();
		gameObject.AddComponent<MeshRenderer> ();
		Mesh mesh = GetComponent<MeshFilter> ().mesh;

		mesh.Clear (); 

		Vector3[] vertices = new Vector3[7];
		Vector3[] corners_local = this.GetCornersLocal ();
		for (int j = 0; j < corners_local.Length - 1; j++) {
			vertices [j] = corners_local [j];
		}
		vertices [6] = new Vector3 (0, 0, 0);

		Vector2[] uvs = new Vector2[vertices.Length];

		for (int j = 0; j < uvs.Length; j++) {
			uvs [j] = new Vector2 (vertices [j].x, vertices [j].z);
		}
			
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = new int[] { 0, 1, 6, 1, 2, 6, 2, 3, 6, 3, 4, 6, 4, 5, 6, 5, 0, 6 };
	}


	// applies the texture to the mesh
	public void ApplyTexture (string path)
	{
		Renderer rend = GetComponent<Renderer> ();
		Material newMat = Resources.Load (path, typeof(Material)) as Material;
		rend.material = newMat;
	}


}
