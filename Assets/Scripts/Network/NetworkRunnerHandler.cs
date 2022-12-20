using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;


public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefab;

    NetworkRunner networkRunner;

    void Start()
    {
        // instantiate networkRunner from Prefab and set name
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network runner";

        // run the task to start the game
        InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
        Debug.Log("Server NetworkRunner started.");

    }

    // Task for NetworkRunner-Initialisation
    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        // get sceneManager
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            // set scenemanager from existing stuff (fallback)
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        // Start game with arguments
        runner.ProvideInput = true;

        StartGameArgs sga = new StartGameArgs {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "TestRoom",
            Initialized = initialized,
            SceneManager = sceneManager
        }; 

        return runner.StartGame(sga);
    }
}
