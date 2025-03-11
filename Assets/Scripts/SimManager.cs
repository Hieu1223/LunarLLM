using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SimManager : MonoBehaviour
{
    public GameObject agentPrefab;
    public UnityEvent OnWorldReset;
    GameObject currentAgent;
    Lander landerScript;

    public UDPSend udpSend ;
    public UDPReceive udpReceive ;



    public float send_interval = 0.2f;
    
    bool agentDestroyed = false;

    private void Start()
    {
        //Setup lander
        currentAgent = Instantiate(agentPrefab, transform.position, Quaternion.identity);
        landerScript = currentAgent.GetComponent<Lander>();
        
        //Setup sender
        

        //Run method
        InvokeRepeating("ProcessData", 0, send_interval);

    }




    private void Update()
    {
        agentDestroyed = currentAgent.IsDestroyed();
        if (agentDestroyed)
        {
            currentAgent = Instantiate(agentPrefab, transform.position, Quaternion.identity);
            landerScript = currentAgent.GetComponent<Lander>();
            OnWorldReset?.Invoke();
            return;
        }

        if (udpReceive.data != null && udpReceive.data != "") {
            Lander.ThrottleInput input = JsonUtility.FromJson<Lander.ThrottleInput>(udpReceive.data);
            if(!input.IsUnityNull())landerScript.ApplyThrottleInput(input);
        }
    }

    public void ProcessData()
    {
        if (agentDestroyed)
            return;
        Lander.State state =  landerScript.GetState();
        udpSend.Send(JsonUtility.ToJson(state));
        
    }
    
}
