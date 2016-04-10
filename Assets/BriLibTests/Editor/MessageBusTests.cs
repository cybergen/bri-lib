using NUnit.Framework;
using BriLib;

[TestFixture]
public class MessageBusTests
{
    private MessageBus _bus;
    private Message _messageOne;
    private Message _messageTwo;
    private MockMessage _mockOne;
    private MockMessage _mockTwo;
    private Message _receivedMessage;
    private MockMessage _receivedMock;
    private int _messOneTriggerCount;
    private int _messTwoTriggerCount;
    private int _mockOneTriggerCount;
    private int _mockTwoTriggerCount;

    [SetUp]
    public void Setup()
    {
        _bus = new MessageBus();
        _messOneTriggerCount = _messTwoTriggerCount = 0;
        _mockOneTriggerCount = _mockTwoTriggerCount = 0;
        _receivedMock = null;
        _receivedMessage = null;
    }

    [Test]
    public void SubscriberGetsMessage()
    {
        var message = new Message();
        Assert.AreEqual(0, _messOneTriggerCount, "No messages received yet");
        _bus.Subscribe<Message>(OnMessageOne);
        Assert.AreEqual(0, _messOneTriggerCount, "No messages received yet");
        _bus.Broadcast(message);
        Assert.AreEqual(1, _messOneTriggerCount, "One message received");
        Assert.AreEqual(message, _receivedMessage, "The same message reference should be passed");
    }

    [Test]
    public void MultipleSubscribers()
    {
        var message = new Message();
        Assert.AreEqual(0, _messOneTriggerCount, "No messages received yet");
        Assert.AreEqual(0, _messTwoTriggerCount, "No messages received yet");
        _bus.Subscribe<Message>(OnMessageOne);
        _bus.Subscribe<Message>(OnMessageTwo);
        Assert.AreEqual(0, _messOneTriggerCount, "No messages received yet");
        Assert.AreEqual(0, _messTwoTriggerCount, "No messages received yet");
        _bus.Broadcast(message);
        Assert.AreEqual(1, _messOneTriggerCount, "Subscriber one should get message");
        Assert.AreEqual(1, _messTwoTriggerCount, "Subscriber two should also get message");
    }

    [Test]
    public void UnsubscriberGetsNoMessage()
    {
        var message = new Message();
        _bus.Subscribe<Message>(OnMessageOne);
        _bus.Subscribe<Message>(OnMessageTwo);
        _bus.Unsubscribe<Message>(OnMessageOne);
        _bus.Broadcast(message);
        Assert.AreEqual(0, _messOneTriggerCount, "Subscriber one should not get message");
        Assert.AreEqual(1, _messTwoTriggerCount, "Subscriber two should also get message");
    }

    [Test]
    public void MultipleSubscriberTypes()
    {
        var message = new MockMessage();
        _bus.Subscribe<Message>(OnMessageOne);
        _bus.Subscribe<Message>(OnMessageTwo);
        _bus.Subscribe<MockMessage>(OnMockOne);
        _bus.Subscribe<MockMessage>(OnMockTwo);
        _bus.Broadcast(message);
        Assert.AreEqual(0, _messOneTriggerCount, "No messages for different subscriber type");
        Assert.AreEqual(0, _messTwoTriggerCount, "No messages for different subscriber type");
        Assert.AreEqual(1, _mockOneTriggerCount, "Subscriber for MockMessage should get triggered");
        Assert.AreEqual(1, _mockTwoTriggerCount, "Subscriber for MockMessage should get triggered");
        Assert.AreEqual(message, _receivedMock, "Same message reference should have been passed");
    }

    private void OnMessageOne(Message m) { _messOneTriggerCount++; _receivedMessage = m; }
    private void OnMessageTwo(Message m) { _messTwoTriggerCount++; }
    private void OnMockOne(MockMessage m) { _mockOneTriggerCount++; _receivedMock = m; }
    private void OnMockTwo(MockMessage m) { _mockTwoTriggerCount++; }

    private class MockMessage : Message { }
}
