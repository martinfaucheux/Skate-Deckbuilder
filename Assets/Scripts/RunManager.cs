using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class RunManager : CoduckStudio.Utils.Singleton<RunManager>
{
    public RunDefinition runDefinition;

#region Stats
    public RunStats runStats;
    private int _roundIndex = -1;
    public int roundIndex {
        get { return _roundIndex; }
        set {
            _roundIndex = value;

            runStats.UpdateRound(_roundIndex + 1, runDefinition.rounds.Count);
        }
    }
    private int _handsPlayedThisRound = 0;
    public int handsPlayedThisRound {
        get { return _handsPlayedThisRound; }
        set {
            _handsPlayedThisRound = value;

            runStats.UpdateHand(_handsPlayedThisRound + 1, runDefinition.rounds[_roundIndex].handsPerRound);
        }
    }
    private int _score = 0;
    public int score {
        get { return _score; }
        set {
            _score = value;

            runStats.UpdateScore(_score, runDefinition.rounds[_roundIndex].scoreGoal);
        }
    }
#endregion

#region Gameloop
    public bool isRunOver = false;

    public void Awake()
    {
        StartRun();
    }

    public void StartRun()
    {
        roundIndex = -1;
        NextRound();
    }

    public void NextRound()
    {
        roundIndex++;

        // Round is over
        if (roundIndex > runDefinition.rounds.Count) {
            EndRun();
            return;
        }

        // TODO: display rewards before calling NextHand()

        handsPlayedThisRound = -1;
        NextHand();
    }

    public void NextHand()
    {
        handsPlayedThisRound++;

        // No more hands this round
        if (handsPlayedThisRound >= runDefinition.rounds[roundIndex].handsPerRound) {
            NextRound();
            return;
        }

        DrawHand();
    }

    private void EndRun()
    {
        // TODO: check score and display game over if loose
        isRunOver = true;
    }

    // TODO: do button instead of key
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !SequenceManager.i.isPlaying && !isRunOver) {
            if (BoardManager.i.CanAddCard()) {
                Debug.LogWarning("cannot play if not all cards are placed");
            }
            else {
                PlayHand();
            }
        }
    }

    public void PlayHand()
    {
        SlotContainerManager.i.ShowActionContainer(true);
        HandManager.i.Hide();

        CoduckStudio.Utils.Async.Instance.WaitForSeconds(1, () => {
            SequenceManager.i.Play();
            CameraManager.Instance.SetPlayerTarget();
            SlotContainerManager.i.EnableAllSortingGroup(false);
        });
    }

    public void OnEndHand()
    {
        SlotContainerManager.i.EnableAllSortingGroup(true);
        SlotContainerManager.i.ShowActionContainer(false);
        CameraManager.Instance.SetLevelBuildingTarget();

        CoduckStudio.Utils.Async.Instance.WaitForSeconds(1, () => {
            NextHand();
        });
    }
#endregion

#region Hand
    public List<CardDefinition> inventory = new();
    public SlotContainer handSlotContainer;
    public void DrawHand() {
        int cardAmountToDraw = runDefinition.rounds[roundIndex].cardCountOnBoard + 2;
        handSlotContainer.AddCards(GetRandomCardsFromInventory(cardAmountToDraw));
        
        HandManager.i.Show();
    }

    private List<CardDefinition> GetRandomCardsFromInventory(int amount)
    {
        List<CardDefinition> cardDefinitions = new();
        for (int i = 0; i < amount; i++) {
            cardDefinitions.Add(inventory[Random.Range(0, inventory.Count)]);
        }

        return cardDefinitions;
    }
#endregion
}
