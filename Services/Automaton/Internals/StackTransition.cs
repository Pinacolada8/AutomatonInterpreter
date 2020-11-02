namespace Services.Automaton.Internals
{
    public class StackTransition : Transition
    {
        public char StackCost { get; set; }

        public char StackIncrement { get; set; }
    }
}