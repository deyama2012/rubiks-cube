using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cubie : MonoBehaviour
{
	public MeshRenderer[] panels;
	private Dictionary<MeshRenderer, Vector3> dictVectorFromMesh = new Dictionary<MeshRenderer, Vector3>();
	private Facelet[] facelets = new Facelet[6];

	/// //////////////////////////////////////////////////////////

	void Awake()
	{
		Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.left, Vector3.right, Vector3.up, Vector3.down };

		for (int i = 0; i < 6; i++)
			dictVectorFromMesh.Add(panels[i], directions[i]);
	}


	void Start()
	{
		RefreshPanelsColor();
	}


	public void SetAllSides(Color color)
	{
		Vector3[] normals = { Vector3.back, Vector3.forward, Vector3.left, Vector3.right, Vector3.up, Vector3.down };

		for (int i = 0; i < 6; i++)
			facelets[i] = new Facelet(normals[i], color);
	}


	public void SetColor(int side, Color color)
	{
		facelets[side].color = color;

		RefreshPanelsColor();
	}


	private void RefreshPanelsColor()
	{
		foreach (var rendVectPair in dictVectorFromMesh)
		{
			Color color = facelets.First(x => x.normal == rendVectPair.Value).color;
			rendVectPair.Key.material.color = color;
		}
	}


	public class Facelet
	{
		public Vector3 normal;
		public Color color;

		public Facelet(Vector3 normal, Color color)
		{
			this.normal = normal;
			this.color = color;
		}
	}
}