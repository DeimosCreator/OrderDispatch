using OrderDispatch.Contracts.Enums;
using OrderService.StateMachine;

namespace OrderDispatch.Tests;

public class OrderStateMachineTests
{
    [TestCase(OrderStatus.Created, OrderStatus.Paid, true)]
    [TestCase(OrderStatus.Created, OrderStatus.PendingKitchen, true)]
    [TestCase(OrderStatus.Created, OrderStatus.Cancelled, true)]
    [TestCase(OrderStatus.Paid, OrderStatus.Preparing, true)]
    [TestCase(OrderStatus.OutForDelivery, OrderStatus.Delivered, true)]
    [TestCase(OrderStatus.Delivered, OrderStatus.Cancelled, false)]
    [TestCase(OrderStatus.Cancelled, OrderStatus.Paid, false)]
    [TestCase(OrderStatus.Created, OrderStatus.Delivered, false)]
    public void CanTransition_ShouldReturnExpectedResult(OrderStatus current, OrderStatus next, bool expected)
    {
        var result = OrderStateMachine.CanTransition(current, next);

        Assert.That(result, Is.EqualTo(expected));
    }
}