using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexCoordinateTools
{	

	private static HexCoordinates[] directions = new HexCoordinates[6] { 
		new HexCoordinates(1, -1,  0),
		new HexCoordinates(1, 0,  -1),
		new HexCoordinates(0, 1, -1),
		new HexCoordinates(-1, 1,  0),
		new HexCoordinates(-1, 0,  1),
		new HexCoordinates(0, -1,  1)
	};

	public static HexCoordinates HexDirection(int direction) {
		return directions [direction];
	}

	public static HexCoordinates HexNeighbour(HexCoordinates hex, int direction) {
		return HexAddition (hex, HexDirection (direction));
	} 

	// sums the coordinates
	public static HexCoordinates HexAddition(HexCoordinates A, HexCoordinates B) {
		return new HexCoordinates (A.X + B.X, A.Y + B.Y, A.Z + B.Z);
	}

	// returns the neighbours of given input hex coordinate
	public static HexCoordinates[] GetNeighbours (HexCoordinates input)
	{
		HexCoordinates[] result = new HexCoordinates[6];
		result [0] = new HexCoordinates (input.X, input.Y - 1, input.Z + 1);
		result [1] = new HexCoordinates (input.X, input.Y + 1, input.Z - 1);
		result [2] = new HexCoordinates (input.X + 1, input.Y - 1, input.Z);
		result [3] = new HexCoordinates (input.X - 1, input.Y + 1, input.Z);
		result [4] = new HexCoordinates (input.X + 1, input.Y, input.Z - 1);
		result [5] = new HexCoordinates (input.X - 1, input.Y, input.Z + 1);

		return result;
	}

	// returns the diagonals of given input hex coordinate
	public static HexCoordinates[] GetDiagonals (HexCoordinates input)
	{
		HexCoordinates[] result = new HexCoordinates[6];
		result [0] = new HexCoordinates (input.X + 2, input.Y - 1, input.Z - 1);
		result [1] = new HexCoordinates (input.X - 2, input.Y + 1, input.Z + 1);
		result [2] = new HexCoordinates (input.X + 1, input.Y + 1, input.Z - 2);
		result [3] = new HexCoordinates (input.X - 1, input.Y - 1, input.Z + 2);
		result [4] = new HexCoordinates (input.X + 1, input.Y - 2, input.Z + 1);
		result [5] = new HexCoordinates (input.X - 1, input.Y + 2, input.Z - 1);

		return result;
	}

	// returns the distance between 2 hex coordinates a and b
	public static int DistanceBetween (HexCoordinates a, HexCoordinates b)
	{
		int xDif = Mathf.Abs (a.X - b.X);
		int yDif = Mathf.Abs (a.Y - b.Y);
		int zDif = Mathf.Abs (a.Z - b.Z);

		return Mathf.Max (xDif, yDif, zDif);
	}

	private static float lerp (float a, float b, float t)
	{ // for floats
		return a + (b - a) * t;
	}

	private static HexCoordinates HexLerp (HexCoordinates a, HexCoordinates b, float t)
	{ // for hexes
		int[] coordinates = HexCoordinateRounding (lerp (a.X, b.X , t),
			                    lerp (a.Y , b.Y, t),
			                    lerp (a.Z , b.Z, t));
		return new HexCoordinates (coordinates [0], coordinates [1], coordinates [2]); 
	}

	private static int[] HexCoordinateRounding (float x, float y, float z)
	{
		int[] result = new int[3];
		result [0] = (int)Mathf.Round (x);
		result [1] = (int)Mathf.Round (y);
		result [2] = (int)Mathf.Round (z);

		float x_diff = Mathf.Abs (result [0] - x);
		float y_diff = Mathf.Abs (result [1] - y);
		float z_diff = Mathf.Abs (result [2] - z);

		if (x_diff > y_diff && x_diff > z_diff) {
			result [0] = -result [1] - result [2];
		} else if (y_diff > z_diff) {
			result [1] = -result [0] - result [2];
		} else { 
			result [2] = -result [0] - result [1];
		}

		return result;
	}

	public static HexCoordinates[] DrawLine (HexCoordinates a, HexCoordinates b)
	{
		int N = DistanceBetween (a, b);
		Debug.Log (N);
		HexCoordinates[] results = new HexCoordinates[N + 1];
		for (int i = 0; i <= N; i++) {
			results [i] = (HexLerp (a, b, (1f / N) * i));
		}
		return results;
	}

	// gets range of n	
	public static HexCoordinates[] GetRange (HexCoordinates input, int N)
	{
		int size = 1;
		for (int i = 1; i <= N; i++) {
			size += i * 6;
		}

		HexCoordinates[] result = new HexCoordinates[size];
		int count = 0;
		for (int i = -N; i <= N; i++) { 
			for (int j = Mathf.Max (-N, - i - N); j <= Mathf.Min (N, -i + N); j++) {  
				int k = -i - j;
				result [count] = new HexCoordinates (input.X + i, input.Y + j, input.Z + k);
				count++;

			}
		}
		return result;
	}

	// gets the intersection of two hexCoordinates[] zones
	// TODO: optimize
	public static HexCoordinates[] ZoneIntersection(HexCoordinates[] A, HexCoordinates[] B) {
		HexCoordinates[] tempResult = new HexCoordinates[Mathf.Max(A.Length, B.Length)];
		int count = 0;
		for (int i = 0; i < A.Length; i++) {
			for (int j = 0; j < B.Length; j++) {
				if (A [i].Equals (B [j])) {
					tempResult [count] = A [i];
					count++;
				}
			}
		}
		HexCoordinates[] result = new HexCoordinates[count];
		for(int i = 0; i < count; i++){
			result[i] = tempResult[i];
		}
		return result;
	}


	public static List<HexCoordinates> GetMovementRange(HexCoordinates start, int movementRange, List<HexCoordinates> blocked) {
		
		List<HexCoordinates> visited = new List<HexCoordinates> ();
		List<HexCoordinates>[] fringles = new List<HexCoordinates>[movementRange + 1];
		visited.Add (start);
		fringles [0] = new List<HexCoordinates> ();
		fringles [0].Add (start);
		for (int i = 1; i <= movementRange; i++) {
			fringles [i] = new List<HexCoordinates> ();
			foreach (HexCoordinates hc in fringles[i-1]) {
				HexCoordinates[] neighbours = GetNeighbours (hc);
				foreach (HexCoordinates neighbour in neighbours) {
					if (!visited.Contains (neighbour) && !blocked.Contains (neighbour)) {
						visited.Add (neighbour);
						fringles [i].Add (neighbour);
					}
				}
			}
		}
		return visited;
	}

	public static HexCoordinates[] GetRing(HexCoordinates center, int radius) {
		HexCoordinates[] result;
		if (radius <= 0) {
			result = new HexCoordinates[1];
			result[0] = center;
			return result;
		} 

		result = new HexCoordinates[radius * 6];
		int count = 0;
		HexCoordinates hex = new HexCoordinates (center.X - radius, center.Y, center.Z + radius);
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < radius; j++) {
				result[count] = hex;
				count++; 
				hex = HexNeighbour(hex, i);
			}
		}
		return result;
	}

}
