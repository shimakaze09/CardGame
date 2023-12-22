using System.Collections;
using NUnit.Framework;
using TheLiquidFire.AspectContainer;

public class PhaseTests
{
    [Test]
    public void PhaseRunsCompleteViewerFlow()
    {
        var action = new TestAction(10, 5);
        var phase = new Phase(action, action.KeyFrameHandler);
        phase.viewer = action.KeyFrameViewer;

        var flow = phase.Flow(null);
        while (flow.MoveNext())
        {
        }

        Assert.AreEqual(action.step, action.loopCount);
    }

    [Test]
    public void PhaseTriggersHandler()
    {
        var action = new TestAction(10, 5);
        var phase = new Phase(action, action.KeyFrameHandler);
        phase.viewer = action.KeyFrameViewer;

        var flow = phase.Flow(null);
        while (flow.MoveNext())
        {
        }

        Assert.IsTrue(action.isComplete);
    }

    [Test]
    public void PhaseTriggersHandlerWithoutKeyFrame()
    {
        var action = new TestAction(10, -1);
        var phase = new Phase(action, action.KeyFrameHandler);
        phase.viewer = action.KeyFrameViewer;

        var flow = phase.Flow(null);
        while (flow.MoveNext())
        {
        }

        Assert.IsTrue(action.isComplete);
    }

    [Test]
    public void PhaseTriggersOnKeyFrame()
    {
        var action = new TestAction(10, 5);
        var phase = new Phase(action, action.KeyFrameHandler);
        phase.viewer = action.KeyFrameViewer;

        var flow = phase.Flow(null);
        while (flow.MoveNext())
            if (action.step < action.keyframe)
                Assert.IsFalse(action.isComplete);
            else if (action.step > action.keyframe) Assert.IsTrue(action.isComplete);
    }

    private class TestAction : GameAction
    {
        public readonly int keyframe;
        public readonly int loopCount;

        public TestAction(int loopCount, int keyFrame)
        {
            this.loopCount = loopCount;
            keyframe = keyFrame;
        }

        public int step { get; private set; }
        public bool isComplete { get; private set; }

        public IEnumerator KeyFrameViewer(IContainer container, GameAction action)
        {
            for (step = 0; step < loopCount; ++step)
                if (step == keyframe)
                    yield return true;
                else
                    yield return false;
        }

        public void KeyFrameHandler(IContainer container)
        {
            isComplete = true;
        }
    }
}