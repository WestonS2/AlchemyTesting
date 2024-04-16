using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
	public static IDictionary<KeyCode, KeyCode> keyCoordination = new Dictionary<KeyCode, KeyCode>();
	public static IDictionary<int, int> mouseCoordination = new Dictionary<int, int>();
	
	#region Directional Movement
	public static KeyCode forwardMove = KeyCode.W;
	public static KeyCode backwardMove = KeyCode.S;
	public static KeyCode rightStrafe = KeyCode.D;
	public static KeyCode leftStrafe = KeyCode.A;
	#endregion
	
	public static KeyCode jumpKey = KeyCode.Space;
	
	public static KeyCode generalInteraction = KeyCode.E;
	public static KeyCode inventoryKey = KeyCode.Tab;
	public static KeyCode exitKey = KeyCode.Escape;
	
	public static int pickUpMouseKey = 0;
	
	void Start()
	{
		//Keys
		keyCoordination.Add(forwardMove, KeyCode.W);
		keyCoordination.Add(backwardMove, KeyCode.S);
		keyCoordination.Add(leftStrafe, KeyCode.A);
		keyCoordination.Add(rightStrafe, KeyCode.D);
		
		keyCoordination.Add(generalInteraction, KeyCode.E);
		keyCoordination.Add(inventoryKey, KeyCode.Tab);
		keyCoordination.Add(exitKey, KeyCode.Escape);
		
		//Mouse
		mouseCoordination.Add(pickUpMouseKey, 0);
	}
}
