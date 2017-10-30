using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexCoordintaeTools
{

	// returns the neighbours of given input hex coordinate
	public static HexCoordinates[] GetNeighbours (HexCoordinates input)
	{
		HexCoordinates[] result = new HexCoordinates[6];
		result [0] = new HexCoordinates (input.getX (), input.getY () - 1, input.getZ () + 1);
		result [1] = new HexCoordinates (input.getX (), input.getY () + 1, input.getZ () - 1);
		result [2] = new HexCoordinates (input.getX () + 1, input.getY () - 1, input.getZ ());
		result [3] = new HexCoordinates (input.getX () - 1, input.getY () + 1, input.getZ ());
		result [4] = new HexCoordinates (input.getX () + 1, input.getY (), input.getZ () - 1);
		result [5] = new HexCoordinates (input.getX () - 1, input.getY (), input.getZ () + 1);

		return result;
	}

	// returns the diagonals of given input hex coordinate
	public static HexCoordinates[] GetDiagonals (HexCoordinates input)
	{
		HexCoordinates[] result = new HexCoordinates[6];
		result [0] = new HexCoordinates (input.getX () + 2, input.getY () - 1, input.getZ () - 1);
		result [1] = new HexCoordinates (input.getX () - 2, input.getY () + 1, input.getZ () + 1);
		result [2] = new HexCoordinates (input.getX () + 1, input.getY () + 1, input.getZ () - 2);
		result [3] = new HexCoordinates (input.getX () - 1, input.getY () - 1, input.getZ () + 2);
		result [4] = new HexCoordinates (input.getX () + 1, input.getY () - 2, input.getZ () + 1);
		result [5] = new HexCoordinates (input.getX () - 1, input.getY () + 2, input.getZ () - 1);

		return result;
	}

	// returns the distance between 2 hex coordinates a and b
	public static int DistanceBetween (HexCoordinates a, HexCoordinates b)
	{
		int xDif = Mathf.Abs (a.getX () - b.getX ());
		int yDif = Mathf.Abs (a.getY () - b.getY ());
		int zDif = Mathf.Abs (a.getZ () - b.getZ ());

		return Mathf.Max (xDif, yDif, zDif);
	}

	private static float lerp (float a, float b, float t)
	{ // for floats
		return a + (b - a) * t;
	}

	private static HexCoordinates HexLerp (HexCoordinates a, HexCoordinates b, float t)
	{ // for hexes
		int[] coordinates = HexCoordinateRounding (lerp (a.getX (), b.getX (), t),
			                    lerp (a.getY (), b.getY (), t),
			                    lerp (a.getZ (), b.getZ (), t));
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
		for (int i = -N; i <= N; i++) { //-N ≤ dx ≤ N:
			for (int j = Mathf.Max (-N, - i - N); j <= Mathf.Min (N, -i + N); j++) {  //(-N, -dx-N) ≤ dy ≤ min(N, -dx+N):
				int k = -i - j;//var dz = -dx-dy
				result [count] = new HexCoordinates (input.getX () + i, input.getY () + j, input.getZ () + k);//results.append(cube_add(center, Cube(dx, dy, dz)))
				count++;

			}
		}
		return result;
	}



}
