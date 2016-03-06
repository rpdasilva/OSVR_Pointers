using UnityEngine;
using System.Diagnostics;
using System.Threading;

class OSVRServer: MonoBehaviour
{
    private Process proc;
    private Thread thread;
    
    private void ExecuteCommand(string command)
    {
        proc = new Process();
        proc.StartInfo.FileName = "/bin/bash";
        proc.StartInfo.Arguments = "-c \" " + command + " \"";
        proc.StartInfo.UseShellExecute = false; 
        proc.StartInfo.RedirectStandardOutput = true;
        
        ThreadStart threadStart = new ThreadStart(() => {
            proc.Start();
            proc.WaitForExit();
        });
        
        thread = new Thread(threadStart);
        thread.Start();
    }

    void Start()
    {
        var configFile = "osvr_server_config.json";
        ExecuteCommand("/usr/local/Cellar/osvr-core/HEAD/bin/osvr_server " + Application.dataPath + "/OSVRConfig/" + configFile);
    }

    void OnApplicationQuit()
    {
        proc.Kill();
        thread.Join();
    }
}
