namespace FunctionalTest;

public interface ICommandHandler<in TCommand>
{
    Task Execute(TCommand command);
}