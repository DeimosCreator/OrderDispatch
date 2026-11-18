namespace OrderDispatch.Contracts.Abstractions;

public interface IAuditable
{
    DateTime UpdatedAt { get; set; }
}