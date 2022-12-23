using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class MatchSystemTests
{
    private IContainer game;
    private ActionSystem actionSystem;
    private DataSystem dataSystem;
    private MatchSystem matchSystem;
    private TestSkipSystem testSkipSystem;

    private class TestSkipSystem : Aspect, IObserve
    {
        public bool enabled;

        public void Awake()
        {
            this.AddObserver(OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction>(), container);
        }

        public void Destroy()
        {
            this.RemoveObserver(OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction>(), container);
        }

        private void OnPrepareChangeTurn(object sender, object args)
        {
            if (!enabled)
                return;

            var action = args as ChangeTurnAction;
            action.targetPlayerIndex = container.GetMatch().currentPlayerIndex;
        }
    }

    [SetUp]
    public void TestSetup()
    {
        game = new Container();
        actionSystem = game.AddAspect<ActionSystem>();
        dataSystem = game.AddAspect<DataSystem>();
        matchSystem = game.AddAspect<MatchSystem>();
        testSkipSystem = game.AddAspect<TestSkipSystem>();
        game.Awake();
    }

    [TearDown]
    public void TestTearDown()
    {
        game.Destroy();
    }

    [Test]
    public void TestChangeTurnAddsAction()
    {
        Assert.IsFalse(actionSystem.IsActive);
        matchSystem.ChangeTurn(1);
        Assert.IsTrue(actionSystem.IsActive);
    }

    [Test]
    public void TestChangeTurnAppliesAction()
    {
        dataSystem.match.currentPlayerIndex = 0;
        var targetIndex = 1;
        matchSystem.ChangeTurn(targetIndex);
        RunToCompletion();
        Assert.AreEqual(dataSystem.match.currentPlayerIndex, targetIndex);
    }

    [Test]
    public void TestDefaultChangeTurnIsOpponentTurn()
    {
        for (var i = 0; i < 2; ++i)
        {
            var lastIndex = dataSystem.match.currentPlayerIndex;
            matchSystem.ChangeTurn();
            RunToCompletion();
            Assert.AreNotEqual(dataSystem.match.currentPlayerIndex, lastIndex);
        }
    }

    [Test]
    public void TestChangeTurnCanBeModified()
    {
        testSkipSystem.enabled = true;
        var startIndex = dataSystem.match.currentPlayerIndex;
        matchSystem.ChangeTurn();
        RunToCompletion();
        Assert.AreEqual(dataSystem.match.currentPlayerIndex, startIndex);
    }

    private void RunToCompletion()
    {
        while (actionSystem.IsActive) actionSystem.Update();
    }
}