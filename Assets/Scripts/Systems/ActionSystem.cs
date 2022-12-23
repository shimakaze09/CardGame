using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class ActionSystem : Aspect
{
    #region Notifications

    public const string beginSequenceNotification = "ActionSystem.beginSequenceNotification";
    public const string endSequenceNotification = "ActionSystem.endSequenceNotification";
    public const string deathReaperNotification = "ActionSystem.deathReaperNotification";
    public const string completeNotification = "ActionSystem.completeNotification";

    #endregion

    #region Fields & Properties

    private GameAction _rootAction;
    private IEnumerator _rootSequence;
    private List<GameAction> _openReactions;
    public bool IsActive => _rootSequence != null;

    #endregion

    #region Public

    public void Perform(GameAction action)
    {
        if (IsActive) return;
        _rootAction = action;
        _rootSequence = Sequence(action);
    }

    public void Update()
    {
        if (_rootSequence == null)
            return;

        if (_rootSequence.MoveNext() == false)
        {
            _rootAction = null;
            _rootSequence = null;
            _openReactions = null;
            this.PostNotification(completeNotification);
        }
    }

    public void AddReaction(GameAction action)
    {
        _openReactions?.Add(action);
    }

    #endregion

    #region Private

    private IEnumerator Sequence(GameAction action)
    {
        this.PostNotification(beginSequenceNotification, action);

        var phase = MainPhase(action.prepare);
        while (phase.MoveNext()) yield return null;

        phase = MainPhase(action.perform);
        while (phase.MoveNext()) yield return null;

        if (_rootAction == action)
        {
            phase = EventPhase(deathReaperNotification, action, true);
            while (phase.MoveNext()) yield return null;
        }

        this.PostNotification(endSequenceNotification, action);
    }

    private IEnumerator MainPhase(Phase phase)
    {
        if (phase.owner.isCanceled)
            yield break;

        var reactions = _openReactions = new List<GameAction>();
        var flow = phase.Flow(container);
        while (flow.MoveNext()) yield return null;

        flow = ReactPhase(reactions);
        while (flow.MoveNext()) yield return null;
    }

    private IEnumerator ReactPhase(List<GameAction> reactions)
    {
        reactions.Sort(SortActions);
        foreach (var subFlow in reactions.Select(Sequence))
            while (subFlow.MoveNext())
                yield return null;
    }

    private IEnumerator EventPhase(string notification, GameAction action, bool repeats = false)
    {
        List<GameAction> reactions;
        do
        {
            reactions = _openReactions = new List<GameAction>();
            this.PostNotification(notification, action);

            var phase = ReactPhase(reactions);
            while (phase.MoveNext()) yield return null;
        } while (repeats && reactions.Count > 0);
    }

    private int SortActions(GameAction x, GameAction y)
    {
        return x.priority != y.priority ? y.priority.CompareTo(x.priority) : x.orderOfPlay.CompareTo(y.orderOfPlay);
    }

    #endregion
}