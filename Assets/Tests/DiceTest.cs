using NUnit.Framework;
using UnityEngine;
using PropertyTycoon;
using UnityEngine.TestTools;
using System.Reflection;
using System.Collections;

public class DiceTest
{
    [Test]
    public void TestInitialState()
    {
        var dice = new GameObject().AddComponent<DiceRoller>();
        Assert.IsTrue(dice.hide);
        Assert.IsFalse(dice.HasRolled);
        Assert.IsFalse(dice.IsWaitingForDiceRoll); // check that the dice is hiding when not needed and hasnt rolled
    }

    [UnityTest]
    public IEnumerator TestPrepareRoll()
    {
        var dice = new GameObject().AddComponent<DiceRoller>();
        dice.PrepareRoll();
        Assert.IsFalse(dice.hide);
        Assert.IsTrue(dice.isSpinning);
        Assert.IsFalse(dice.HasRolled);
        yield return null; // check that the dice is now shown, spinning and hasnt rolled yet
    }

    [UnityTest]
    public IEnumerator TestResults()
    {
        var dice = new GameObject().AddComponent<DiceRoller>();
        yield return dice.CompleteRoll();
        Assert.GreaterOrEqual(dice.Result, 1); // check result is >= 1
        Assert.LessOrEqual(dice.Result, 6); //check result is <= 6
    }

    [UnityTest]
    public IEnumerator TestFinalRotation()
    {
        var dice = new GameObject().AddComponent<DiceRoller>();
        dice.PrepareRoll();
        yield return dice.CompleteRoll();
        Quaternion expectedRotation = dice.faceRotations[dice.Result - 1]; // calculate what result should be shown
        Assert.IsTrue(Quaternion.Angle(expectedRotation, dice.transform.rotation) < 0.1f); // check the rotations displays the result of the roll
        Assert.IsTrue(dice.hide);
    }
}
