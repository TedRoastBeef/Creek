namespace Creek.Tools
{
    using System;

    public abstract class Command
    {
        protected Receiver receiver;

        // Constructor 
        public Command(Receiver receiver)
        {
            this.receiver = receiver;
        }

        public abstract void Execute();
    }
    public class Invoker
    {
        private Command command;

        public void SetCommand(Command command)
        {
            this.command = command;
        }

        public void ExecuteCommand()
        {
            command.Execute();
        }
    }
    // "ConcreteCommand" 
    public class ConcreteCommand : Command
    {
        // Constructor 
        public ConcreteCommand(Receiver receiver) :
            base(receiver)
        {
        }

        public override void Execute()
        {
            this.receiver.Do();
        }
    }
    public abstract class Receiver
    {
        public abstract void Do();
    }
}