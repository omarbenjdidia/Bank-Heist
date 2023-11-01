using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Character)]
	[Tooltip("Gets the velocity of the attached character controller")]
	public class GetCharacterControllerVelocity : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(CharacterController))]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeResult;
		public bool everyFrame;
		
		GameObject previousGo;
		CharacterController controller;
		
		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			doGetVelocity();
			if (!everyFrame) Finish();
		}
		
		public override void OnUpdate()
		{
			doGetVelocity();	
		}
		
		public void doGetVelocity()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			if (go != previousGo)
			{
				controller = go.GetComponent<CharacterController>();
				previousGo = go;
			}
			
			if (controller != null)
			{
				storeResult.Value = controller.velocity;
			}
		}
	}
}