using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using System.Linq;

public class FB_ScoreController : MonoBehaviour
{
    [SerializeField] Text scoreText;
    DatabaseReference mDatabase;
    [SerializeField] Text listNames, listScoresText;
    string UserId;
    int highScore;
    // Start is called before the first frame update
    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        GetUserScore();
    }

    public void WriteNewScore(int score)
    {
        if (highScore < score)
        {

            UserData data = new UserData();
            data.score = score;
            string json = JsonUtility.ToJson(data);

            mDatabase.Child("users").Child(UserId).Child("score").SetValueAsync(score);
            highScore = score;
        }


    }

    public void GetUserScore()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users/" + UserId)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    Debug.Log(snapshot.Value);

                    var data = ((Dictionary<string, object>)snapshot.Value);
                    Debug.Log("Puntaje: " + data["score"]);
                    scoreText.text = data["score"].ToString();

                    highScore = int.Parse(data["score"].ToString());
                    //foreach (var userDoc in (Dictionary<string,object>)snapshot.Value)
                    //{
                    //    Debug.Log(userDoc.Key);
                    //    Debug.Log(userDoc.Value);

                    //}
                    // Do something with snapshot...
                }
            });
    }

    public void GetUsersHighestScores()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").LimitToLast(10)
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        //Handle the error...
                    }
                    else if (task.IsCompleted)
                    {
                        listNames.text = "";
                        listScoresText.text = "";
                        DataSnapshot snapshot = task.Result;
                        Debug.Log(snapshot);
                        foreach (DataSnapshot dataList in snapshot.Children.Reverse<DataSnapshot>())
                        {
                            listNames.text += dataList.Child("username").Value.ToString() + "\n";
                            listScoresText.text += dataList.Child("score").Value.ToString() + "\n";
                        }

                    }
                });
    }
}

public class UserData
{
    public int score;
    public string username;
}
