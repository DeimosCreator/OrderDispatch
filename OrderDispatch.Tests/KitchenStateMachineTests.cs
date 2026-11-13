using KitchenService.Models.Entities.Enums;
using KitchenService.StateMachine;

namespace OrderDispatch.Tests;

public class KitchenStateMachineTests
{
    [TestCase(KitchenStatus.Queued, KitchenStatus.Accepted, true)]
    [TestCase(KitchenStatus.Queued, KitchenStatus.Rejected, true)]
    [TestCase(KitchenStatus.Accepted, KitchenStatus.Preparing, true)]
    [TestCase(KitchenStatus.Preparing, KitchenStatus.Ready, true)]
    [TestCase(KitchenStatus.Queued, KitchenStatus.Ready, false)]
    [TestCase(KitchenStatus.Ready, KitchenStatus.Cancelled, false)]
    [TestCase(KitchenStatus.Cancelled, KitchenStatus.Accepted, false)]
    public void CanTransition_ShouldReturnExpectedResult(KitchenStatus current, KitchenStatus next, bool expected)
    {
        var result = KitchenStateMachine.CanTransition(current, next);

        Assert.That(result, Is.EqualTo(expected));
    }
}