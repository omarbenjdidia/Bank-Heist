using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	
	[ActionCategory("Heist PlayMaker Kit")]
	public class HeistFOVMesh : FsmStateAction
	{
		[RequiredField]
		[Tooltip("FOV Mesh")]
		public FsmOwnerDefault FovMeshGameObject;

		[RequiredField]
		[Tooltip("The Player GameObject Which uses the Mesh")]
		public FsmGameObject playerGameObject;

		[Tooltip("Range of FOV")]
		public FsmFloat range = 1;

		[Tooltip("Resolution of FOV Mesh")]
		public FsmInt levelOfDetails = 1;

		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;


		private Vector3 direction;
		private int index = 0;
		private int triIndex = 0;
		private int lod = 1;
		private float width;
		private GameObject go;
		private GameObject pGo;
		private Mesh mesh;
		private Vector3 worldPos;
		
		private Vector3[] verts;
		private int[] tris;
		private Vector2[] uvs;
		private GameObject didHit;
		

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			go = Fsm.GetOwnerDefaultTarget(FovMeshGameObject);
			pGo = playerGameObject.Value;
			
			mesh = new Mesh();
			go.GetComponent<MeshFilter>().mesh = mesh;
			
			lod = levelOfDetails.Value;
			width = range.Value;
			
			verts = new Vector3[(360/lod) + 1] ;
			tris = new int[(360/lod)*3];
			
			uvs = new Vector2[verts.Length];
		}
		
		// Code that runs every frame.
		public override void OnUpdate()
		{
			index = 0;
			triIndex = 0;
			
			worldPos = pGo.transform.position;

			verts[index]= worldPos;
			
			index++;
			
			for(var a=0; a<360; a += lod)
			{
				var direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * a), 0, Mathf.Cos(Mathf.Deg2Rad * a));
				
				direction = direction * width;
				
				RaycastHit hit;
				
				if (Physics.Raycast (worldPos, direction, out hit, width, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value))) 
				{
							
						verts[index]= new Vector3 (hit.point.x, 0, hit.point.z);
										
				}
				else
				{
					
					verts[index]= new Vector3 (direction.x + worldPos.x, 0, direction.z + worldPos.z);
				}
				
				index++;
			}
			
			for(var i=1; i<(360/lod); i++)
			{
				tris[triIndex] = 0;
				tris[triIndex + 1] = i;
				tris[triIndex + 2] = i + 1;
				triIndex += 3;
				
			}
			
			tris[((360/lod)*3)-3] = 0;
			tris[((360/lod)*3)-2] = 360/lod;
			tris[((360/lod)*3)-1] = 1;
			
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
			
			
		}
		

	}
	
}