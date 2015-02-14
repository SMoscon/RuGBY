using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	private const string typeName = "com.dillongu.rugby";
	private const string gameName = "CARL";
	 
	private HostData[] hostList;

	public GameObject playerPrefab;

	void OnGUI()
	{
	    if (!Network.isClient && !Network.isServer)
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
	    }
	}

	//Server
	private void StartServer()
	{
		//Max 4 playesr with port 25000
	    Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
	    MasterServer.RegisterHost(typeName, gameName);
	}
	void OnServerInitialized()
	{
		SpawnPlayer();
	    Debug.Log("Server Initializied");
	}


	//Host 
	private void RefreshHostList()
	{
	    MasterServer.RequestHostList(typeName);
	}
	 
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
	    if (msEvent == MasterServerEvent.HostListReceived)
	        hostList = MasterServer.PollHostList();
	}
	private void JoinServer(HostData hostData)
	{
	    Network.Connect(hostData);
	}
	 
	void OnConnectedToServer()
	{
		SpawnPlayer();
	    Debug.Log("Server Joined");
	}
	private void SpawnPlayer()
	{
	    Network.Instantiate(playerPrefab, new Vector3(0f, 2f, 0f), Quaternion.identity, 0);
	}

}
