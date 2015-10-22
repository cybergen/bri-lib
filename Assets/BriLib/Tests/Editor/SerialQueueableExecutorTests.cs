using UnityEngine;
using System.Collections;
using NUnit.Framework;

[TestFixture]
public class SerialQueueableExecutorTests
{
    private SerialQueueableExecutor _executor;

    [SetUp]
    public void Setup()
    {
        _executor = new SerialQueueableExecutor();
    }

    [Test]
    public void BeginStartsQueuedEntry()
    {

    }

    [Test]
    public void RepeatedBeginDoesNothing()
    {

    }

    [Test]
    public void KillKillsQueuedEntry()
    {

    }

    [Test]
    public void KillDoesntKillUnstartedEntry()
    {

    }

    [Test]
    public void FinishEntryAdvancesQueue()
    {

    }

    [Test]
    public void FinishAllEntries()
    {

    }
}
