using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class LeaderBoard
{

    public static async Task<Player[]> GetLeaderBoard()
    {
        Player[] playerArray = null;

        string url = "https://gamesapi.bienvu.net/doggypeak/list";
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching leaderboard: " + request.error);
            return playerArray;
        }
        else
            Debug.Log("Leaderboard data: " + request.downloadHandler.text);


        string json = request.downloadHandler.text;
        // if (json.TrimStart().StartsWith("["))
        // {
        //     json = "{\"Items\":" + json + "}";
        // }
        PlayerArrayWrapper wrapper = JsonUtility.FromJson<PlayerArrayWrapper>(json);
        playerArray = wrapper.Items;
        return playerArray;
    }


}

[Serializable]
public class Player
{
    public string player;
    public float time;
    public string date;
}

[Serializable]
public class PlayerArrayWrapper
{
    public Player[] Items;
}