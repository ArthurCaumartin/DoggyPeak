using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public static class LeaderBoard
{
    public delegate void OnLeaderBoardEvent(Player[] players);
    public static event OnLeaderBoardEvent OnLeaderBoardFetched;
    private static Player[] _playerArray = null;
    private static Task _taskLeaderBordFetch;
    private static float _fetchTime = 0;

    public static string currentPlayerName = "";
    public static float currentPlayerPB = -1;


    public static async void FetchLeaderBoard()
    {
        _playerArray = null;
        GetLeaderBoard();
        _fetchTime = 0;
        float fetchDelay = 5;


        while (Application.isPlaying)
        {
            _fetchTime += Time.deltaTime;
            // Debug.Log("FetchLeaderBoard while loop");
            if (_fetchTime >= fetchDelay)
            {
                GetLeaderBoard();
            }
            await Task.Yield();
        }
    }

    private static async void GetLeaderBoard()
    {
        // Debug.Log("Fetching LeaderBoard Data...");
        _fetchTime = 0;
        string url = "https://gamesapi.bienvu.net/doggypeak/list";
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching leaderboard: " + request.error);
            return;
        }
        // else
        //     Debug.Log("Leaderboard data get: " + request.downloadHandler.text);


        string json = request.downloadHandler.text;
        // if (json.TrimStart().StartsWith("["))
        // {
        //     json = "{\"Items\":" + json + "}";
        // }
        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
        Array.Sort(wrapper.Items, (a, b) => a.time.CompareTo(b.time));

        if (_playerArray == null || _playerArray.Length != wrapper.Items.Length)
        {
            SetNewData(wrapper.Items);
        }

        for (int i = 0; i < wrapper.Items.Length; i++)
        {
            if (_playerArray[i].player != wrapper.Items[i].player || _playerArray[i].time != wrapper.Items[i].time)
            {
                SetNewData(wrapper.Items);
            }
        }

        currentPlayerPB = TestPB(new Player(currentPlayerName, 1001), wrapper.Items);
        Debug.Log("Set Current PB to : " + currentPlayerPB);

        // Debug.Log("Data is same, nothing done !"); 
    }

    private static void SetNewData(Player[] newPlayerArray)
    {
        _playerArray = newPlayerArray;
        OnLeaderBoardFetched.Invoke(_playerArray);
    }

    public static async void PostTimeData(Player newPlayer)
    {
        if (currentPlayerName == "")
            return;

        newPlayer.player = newPlayer.player.ToLower();

        newPlayer.time = TestPB(newPlayer, _playerArray, false);

        // Debug.Log("Posting Time Data for player : " + currentPlayerName);
        string playerData = JsonUtility.ToJson(newPlayer);
        UnityWebRequest request =
        UnityWebRequest.Post("https://gamesapi.bienvu.net/doggypeak/add", playerData, "application/json");
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error posting time data : " + request.error);
            return;
        }
        // Debug.Log("Successfully posted time data for : " + currentPlayerName);
        GetLeaderBoard();
    }

    private static float TestPB(Player player, Player[] playerData, bool skipFX = true)
    {
        Debug.Log("Testing PB for player: " + player.player);
        if (playerData == null)
        {
            Debug.LogWarning("[TestPB] _playerArray is null !");
            return player.time;
        }

        // Player playerInData = playerData.FirstOrDefault(p => p.player == player.player);
        Player playerInData = null;
        for (int i = 0; i < playerData.Length; i++)
        {
            if (playerData[i].player == player.player)
            {
                playerInData = playerData[i];
                break;
            }
        }
        if (playerInData == null)
        {
            Debug.LogWarning("[TestPB] Joueur non trouvé dans le leaderboard: '" + player.player + "'");
            return player.time;
        }

        if (player.time < playerInData.time)
        {
            if (!skipFX)
                AudioManager.Instance.PlaySound(AudioManager.Instance.GoodBoySound);
            currentPlayerPB = player.time;
            return player.time;
        }
        else
            return playerInData.time;
    }
}

[Serializable]
public class Player
{
    public string player;
    public float time;
    public string date;

    public Player(string name, float time)
    {
        this.player = name;
        this.time = time;
    }
}

[Serializable]
public class Wrapper
{
    public Player[] Items;
}
