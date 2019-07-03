using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreChangedEventArgs : System.EventArgs
{
    public int AmountToChange { get; set; }
}

public class Scorekeeper : MonoBehaviour, IObserver<ScoreChangedEventArgs>
{
    [SerializeField]
    private Text _scoreDisplayText;

    private int _score;

    public int Score {
        get
        {
            return _score;
        }

        private set
        {
            _score = value;
            _scoreDisplayText.text = _score.ToString();
        }
    }

    private void Start()
    {
        Reset();
    }

    public void OnStateChanged(ScoreChangedEventArgs eventArgs)
    {
        Score += eventArgs.AmountToChange;
    }

    public void Reset()
    {
        Score = 0;
    }
}
