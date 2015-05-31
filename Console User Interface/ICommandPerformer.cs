namespace Console_User_Interface
{
    internal interface ICommandPerformer
    {
        bool TryPerform(string command);
    }
}
