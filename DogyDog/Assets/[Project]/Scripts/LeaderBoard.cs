using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public static class LeaderBoard
{
    public delegate void OnLeaderBoardEvent(Player[] players);
    public static event OnLeaderBoardEvent OnLeaderBoardFetched;
    private static Player[] _playerArray = null;
    private static Task _taskLeaderBordFetch;

    public static async void FetchLeaderBoard()
    {
        _playerArray = null;
        GetLeaderBoard();
        float time = 0;
        float fetchDelay = 5;

        while (Application.isPlaying)
        {
            time += Time.deltaTime;
            // Debug.Log("FetchLeaderBoard while loop");
            if (time >= fetchDelay)
            {
                time = 0;
                GetLeaderBoard();
            }
            await Task.Yield();
        }
    }

    private static async void GetLeaderBoard()
    {
        string url = "https://gamesapi.bienvu.net/doggypeak/list";
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching leaderboard: " + request.error);
            return;
        }
        // else
        //     Debug.Log("Leaderboard data: " + request.downloadHandler.text);


        string json = request.downloadHandler.text;
        // if (json.TrimStart().StartsWith("["))
        // {
        //     json = "{\"Items\":" + json + "}";
        // }
        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
        Array.Sort(wrapper.Items, (a, b) => a.time.CompareTo(b.time));

        if (_playerArray == null)
        {
            SetNewData(wrapper.Items);
            return;
        }

        if (_playerArray == null || _playerArray.Length != wrapper.Items.Length)
        {
            SetNewData(wrapper.Items);
            return;
        }

        for (int i = 0; i < wrapper.Items.Length; i++)
        {
            if (_playerArray[i].player != wrapper.Items[i].player || _playerArray[i].time != wrapper.Items[i].time)
            {
                SetNewData(wrapper.Items);
                return;
            }
        }
        Debug.Log("Data is same, nothing done !");
    }

    private static void SetNewData(Player[] newPlayerArray)
    {
        _playerArray = newPlayerArray;
        OnLeaderBoardFetched.Invoke(_playerArray);
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