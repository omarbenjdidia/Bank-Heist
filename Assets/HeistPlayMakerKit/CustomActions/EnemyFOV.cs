using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{

[ActionCategory("Heist PlayMaker Kit")]
public class EnemyFOV : FsmStateAction
{
	[RequiredField]
	[Tooltip("FOV Mesh")]
	public FsmOwnerDefault FovMeshGameObject;

	[Tooltip("Angle of FOV")]
	public FsmFloat angle = 45;

	[Tooltip("Range of FOV")]
	public FsmFloat range = 10;
	
	private GameObject go;
	private Mesh mesh;
	private Vector3[] verts;
	private int[] tris;
	private Vector2[] uvs;

	// Code that runs on entering the state.
	public override void OnEnter()
	{
		go = Fsm.GetOwnerDefaultTarget(FovMeshGameObject);
		mesh = new Mesh();
		go.GetComponent<MeshFilter>().mesh = mesh;
		
		verts = new Vector3[3];
		tris = new int[3];
		uvs = new Vector2[verts.Length];


		var vert1 = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle.Value), 0, Mathf.Cos(Mathf.Deg2Rad * angle.Value));
		var vert2 = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle.Value * (-1)), 0, Mathf.Cos(Mathf.Deg2Rad * angle.Value * (-1)));

		vert1 = vert1 * range.Value;
		vert2 = vert2 * range.Value;

		verts[1]= vert1;
	
		verts[2]= vert2;

		Debug.Log(verts[0]);
		Debug.Log(verts[1]);
		Debug.Log(verts[2]);

		tris[0] = 0;
		tris[1] = 1;
		tris[2] = 2;

		int j = 0;
		while (j < uvs.Length) {
			uvs[j] = new Vector2(verts[j].x, verts[j].z);
			j++;
		}
		
		
		
		mesh.Clear();
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		
		Finish();
	}


}

}