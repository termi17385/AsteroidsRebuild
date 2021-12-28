using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonCreation : MonoBehaviour
{
	[SerializeField] private int numberOfSides = 6;
	[SerializeField] private float polygonRadius = 5;
	[SerializeField, Range(0, 360)] private float angle;

	void Update() => DebugDrawPolygon(transform.position, polygonRadius, numberOfSides);

	/// <summary> Draw a polygon in the XY plane with a specified position,
	/// number of sides and radius. </summary>
	/// <param name="_centre">The centre of the polygon</param>
	/// <param name="_radius">the radius of the polygon</param>
	/// <param name="_numSides">the number of sides of the polygon</param>
	private void DebugDrawPolygon(Vector2 _centre, float _radius, int _numSides)
	{
		// The corner that is used to start the polygon (parallel to the X axis).
		Vector2 startCorner = new Vector2(_radius, 0) + _centre;
		// The "previous" corner point, initialised to the starting corner.
		Vector2 previousCorner = startCorner;
		// For each corner after the starting corner
		for(int i = 1; i < _numSides; i++)
		{
			// Calculate the angle of the corner in radians.
			float cornerAngle = 2f * Mathf.PI / (float) _numSides * i;// + angle * Mathf.Deg2Rad;
			// Get the X and Y coordinates of the corner point.
			Vector2 currentCorner = new Vector2(Mathf.Cos(cornerAngle) * _radius, Mathf.Sin(cornerAngle) * _radius) + _centre;
			// Draw a side of the polygon by connecting the current corner to the previous one.
			Debug.DrawLine(currentCorner, previousCorner);
			// Having used the current corner, it now becomes the prievous corner.
			previousCorner = currentCorner;
		}
		// Draw the final side by connecting the last corner to the start corner.
		Debug.DrawLine(startCorner, previousCorner);
	}
}
