using System.Collections.Generic;
using System.Linq;
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
    public enum State {
        Building,
        Playing,
        CalculatingScore,
        ReadyToBuild
    }
    public State state = State.Building;

    public void Awake()
    {
        StartRun();
    }

    public void StartRun()
    {
        state = State.Building;
        roundIndex = -1;
        NextRound();
    }

    public void NextRound()
    {
        ScoreDisplay.Instance.Show(false, () => {
            roundIndex++;
            score = 0;

            if (roundIndex > runDefinition.rounds.Count) {
                EndRun();
                return;
            }

            CoduckStudio.Utils.Async.Instance.WaitForSeconds(0.5f, () => {
                RelicChoice.Instance.Show(false, () => {
                    CardChoice.Instance.Show(false, () => {
                        handsPlayedThisRound = -1;
                        NextHand();
                    }, 2);
                });
            });
        });
    }

    public void NextHand()
    {
        handsPlayedThisRound++;

        // No more hands this round
        if (handsPlayedThisRound >= runDefinition.rounds[roundIndex].handsPerRound) {
            NextRound();
            return;
        }

        EnergyPointManager.i.ResetValue();
        DrawHand();
    }

    private void EndRun()
    {
        // TODO: check score and display game over if loose
        isRunOver = true;
    }

    public void PlayHand()
    {
        state = State.Playing;

        SlotContainerManager.i.ShowActionContainer(true);
        HandManager.i.Hide();

        CoduckStudio.Utils.Async.Instance.WaitForSeconds(1, () => {
            SequenceManager.i.Play();
            CameraManager.Instance.SetPlayerTarget();
            SlotContainerManager.i.EnableAllSortingGroup(false);
        });
    }

    public void SetStateCalculateScore()
    {
        state = State.CalculatingScore;
        BoardScoreCalculator.Instance.ShowAndCalculateScore();
    }

    public void SetStateReadyToBuild()
    {
        state = State.ReadyToBuild;
    }

    public void OnEndHand()
    {
        state = State.Building;

        SlotContainerManager.i.EnableAllSortingGroup(true);
        SlotContainerManager.i.ShowActionContainer(false);
        CameraManager.Instance.SetLevelBuildingTarget();
        BoardScoreCalculator.Instance.Hide();

        CoduckStudio.Utils.Async.Instance.WaitForSeconds(1, () => {
            NextHand();

            BoardManager.i.slotContainer.RemoveAllCards();
            CoduckStudio.Utils.Async.Instance.WaitForEndOfFrame(() => {
                BoardManager.i.slotContainer.cardCountMax = runDefinition.rounds[roundIndex].cardCountOnBoard;
                if (HasRelic("Board Wheel")) {
                    BoardManager.i.slotContainer.cardCountMax++;
                }

                BoardManager.i.slotContainer.AddEmptySlotsIfNeeded();
            });
        });
    }
#endregion

#region Hand
    public List<CardDefinition> inventory = new();
    public SlotContainer handSlotContainer;
    public void DrawHand() {
        RideButton.Instance.Show();

        int cardAmountToDraw = runDefinition.rounds[roundIndex].cardCountOnBoard + 2;
        if (HasRelic("Hand Wheel")) {
            cardAmountToDraw++;
        }

        handSlotContainer.AddCards(GetRandomCardsFromInventory(cardAmountToDraw));
        
        HandManager.i.Show();
        BoardManager.i.Show();
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

#region Items
    public Transform relicUIList;
    public RelicUI relicUIPrefab;
    private List<RelicDefinition> relics = new List<RelicDefinition>();

    public void AddRelic(RelicDefinition relicDefinition)
    {
        relics.Add(relicDefinition);

        RelicUI relicUI = Instantiate(relicUIPrefab, relicUIList);
        relicUI.relicDefinition = relicDefinition;
    }

    public bool HasRelic(string name)
    {
        return relics.Any((relic) => relic.name.ToLower().Trim() == name.ToLower().Trim());
    }

    public List<RelicDefinition> GetRelics()
    {
        return relics;
    }
#endregion
}
