using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{

[ActionCategory("Heist PlayMaker Kit")]
[Tooltip("The Game Object must have a NavMeshAgentcomponent attached.")]
public class NPCPatrol : FsmStateAction
{
	public enum Operation
	{
		Circular,
		ToandFro
	}

	[RequiredField]
	[Tooltip("NPC")]
	[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
	public FsmOwnerDefault gameObject;

	[RequiredField]
	[Tooltip("Waypoints")]
	public FsmGameObject[] waypoints;

	public float checkingDistance = 1;

	public FsmBool skipPatrolling = false;

	public FsmGameObject currentWaypoint;

	private UnityEngine.AI.NavMeshAgent _agent;
	private GameObject go;
	private int i = 0;
	private bool flag = true;
	private int inc = -1;
	
	private void _getAgent()
	{
		go = Fsm.GetOwnerDefaultTarget(gameObject);
		if (go == null) 
		{
			return;
		}
		
		_agent =  go.GetComponent<UnityEngine.AI.NavMeshAgent>();
	}	


	public Operation operation = Operation.Circular;
		
	// Code that runs on entering the state.
	public override void OnEnter()
	{			

		_getAgent();
		
	}

	// Code that runs every frame.
	public override void OnUpdate()
	{
		if(!skipPatrolling.Value)
		{
		switch (operation)
			{
				case Operation.Circular:
				{
					if(flag)
						{
						_agent.SetDestination(waypoints[i].Value.transform.position);
						currentWaypoint.Value=waypoints[i].Value;
						}

					var dist = (go.transform.position - waypoints[i].Value.transform.position).sqrMagnitude;

					if (dist < checkingDistance)
					{
						flag = true;
						if(i< waypoints.Length - 1)
						{
						i++;
						
						}
						else
						{
							i=0;
						}
					}
					else
						flag = false;
					break;
				}
				case Operation.ToandFro:
				{
					if(flag)
						_agent.SetDestination(waypoints[i].Value.transform.position);
					
					var dist = (go.transform.position - waypoints[i].Value.transform.position).sqrMagnitude;

					if (dist < checkingDistance)
					{
						flag = true;

						if((i== waypoints.Length - 1) || (i==0))
							inc=inc*(-1);

						i+=inc;

					}
					else
						flag = false;

					break;
				}

			}

		}
		
	}


}


}