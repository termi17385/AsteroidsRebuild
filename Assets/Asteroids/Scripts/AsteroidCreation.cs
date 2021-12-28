using UnityEngine;
using Random = UnityEngine.Random;

public static class GenerateVerts
{
	public static Vector2[] GenerateVertArray(int _numSides, float _radius, float _noise)
	{
		Vector2[] vertArray = new Vector2[_numSides];
		for(int i = 0; i < _numSides; i++)
		{
			float cornerAngle = 2f * Mathf.PI * i / (float) _numSides + Mathf.PI / 2;
			// cos -1 , 1                   sin -1 , +1
			vertArray[i] = new Vector2(Mathf.Cos(cornerAngle), Mathf.Sin(cornerAngle)) * (_radius + Random.Range(-_noise, _noise));
		}
		
		return vertArray;
	}
}

[RequireComponent(typeof(PolygonCollider2D))]
public class AsteroidCreation : MonoBehaviour
{
	[SerializeField, Min(3)] private int numberOfSides = 6;
	[SerializeField] private float polygonRadius = 5;
	[SerializeField, Range(0, 1)] private float noiseAmt;
	[SerializeField] private bool debug;

	[SerializeField] private LineRenderer drawLine;

	private PolygonCollider2D polyCol;
	private Vector2[] vertArray;

	private void Awake()
	{
		drawLine = GetComponent<LineRenderer>();
		polyCol = GetComponent<PolygonCollider2D>();
		vertArray = GenerateVerts.GenerateVertArray(numberOfSides, polygonRadius, noiseAmt);
	}

	private void Start()
	{
		drawLine.positionCount = vertArray.Length;
		polyCol.pathCount = 1;
		
		polyCol.SetPath(0, vertArray);
	}

	private void Update() => DrawPolygon();
	private void DrawPolygon()
	{
		Vector2 startCorner = transform.TransformPoint(vertArray[0]);
		Vector2 previousCorner = startCorner;

		for(int i = 0; i < vertArray.Length; i++)
		{
			Vector3 currentCorner = transform.TransformPoint(vertArray[i]);
			drawLine.SetPosition(i, currentCorner);

			int x = i + 1;
			if(x < drawLine.positionCount)
			{
				drawLine.SetPosition(x, previousCorner);
			}
			previousCorner = currentCorner;
		}
	}
	
	
#region Debugging
	/// <summary> Draw a polygon in the XY plane with a specified position,
	/// number of sides and radius. </summary>
	private void DebugDrawPolygon()
	{
		Vector2 startCorner = transform.TransformPoint(vertArray[0]);
		Vector2 previousCorner = startCorner;

		for(int i = 1; i < vertArray.Length; i++)
		{
			Vector3 currentCorner = transform.TransformPoint(vertArray[i]);

			Gizmos.DrawLine(currentCorner, previousCorner);
			previousCorner = currentCorner;
		}

		Gizmos.DrawLine(startCorner, previousCorner);
	}
#endregion
}