using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
	private const string typeName = "com.dillongu.rgby.something";
	private const string gameName = "CARL";
	private bool blue;
	private bool green;
	private bool yellow;
	private bool red;
	
	private bool isRefreshingHostList = false;
	private HostData[] hostList;
	
	public GameObject playerPrefab;
	
	void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

	void OnGUI()
	{
		/*if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}*/
	}

	void Awake(){
		blue = false;
		green = false;
		yellow = false;
		red = false;
	}

	public void RefreshFunction(){
		//if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
		/*RefreshHostList();
		Debug.Log ("refreshing...");
		if (hostList != null)
		{
			Debug.Log ("HostList not null");
			for (int i = 0; i < hostList.Length; i++)
			{
				Debug.Log ("generating button");
				//if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
				//JoinServer(hostList[i]);
			}
		}*/
		PhotonNetwork.JoinRoom(gameName);
	}
	
	public void StartServer()
	{
		Debug.Log ("StartServer");
		//Create this room. 
		// PhotonNetwork.CreateRoom(gameName); 
		PhotonNetwork.CreateRoom(gameName, true, true, 5);

		// Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
		// MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnJoinedRoom()
	{
	    Debug.Log("Connected to Room");
		Screen.lockCursor = true;
		SpawnPlayer();
	}
	
/*	void Update()
	{
		if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
		{
			isRefreshingHostList = false;
			hostList = MasterServer.PollHostList();
		}
	}
	
	public void RefreshHostList()
	{
		if (!isRefreshingHostList)
		{
			isRefreshingHostList = true;
			MasterServer.RequestHostList(typeName);
		}
	}
	
	private void JoinServer(HostData hostData)
	{
		//Network.Connect(hostData);
	}
	
	void OnConnectedToServer()
	{
		//Debug.Log ("spawn character select");
		SpawnPlayer();
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	*/
	private void SpawnPlayer()
	{
		GameObject temp = (GameObject) PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up * 5, Quaternion.identity, 0);
		//Network.Instantiate(cameraPrefab, Vector3.up * 5, Quaternion.identity, 0);

		GameObject.FindWithTag("HUD").GetComponent<Canvas>().enabled = true;
		temp.GetComponent<PlayerHealth>().healthSlider = GameObject.FindWithTag("HUD").GetComponentInChildren<Slider>();
		temp.GetComponent<PlayerHealth>().damageImage = GameObject.FindWithTag("HUD").GetComponentInChildren<Image>();

		var original = GameObject.FindWithTag("MainCamera");
		Debug.Log ("found main camera");
		//Camera _cam = (Camera) Camera.Instantiate(original.camera, new Vector3(0, 0, 0), 
		//								Quaternion.FromToRotation(new Vector3(0, 0, 0), new Vector3(0, 0, 1)));
		//DestroyImmediate(Camera.main.gameObject);


		//GameObject.FindWithTag("MainCamera").GetComponent<SmoothLookAt>().target = temp.GetComponentInChildren<Transform>().Find("Head_Target");
		original.GetComponent<MouseOrbitImproved>().target = temp.GetComponentInChildren<Transform>().Find("Head_Target"); 
		//original.GetComponent<SmoothFollow>().target = temp.GetComponentInChildren<Transform>().Find("Head_Target"); 
		
		original.AddComponent<CameraRaycast>();
		original.GetComponent<CameraRaycast>().Player = temp;
		//temp.GetComponent<PlayerInput>().enabled=true;
	}
}
