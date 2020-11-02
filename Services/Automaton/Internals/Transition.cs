namespace Services.Automaton.Internals
{
    public class Transition
    {
        public State InitialState { get; set; }
        public State TargetState { get; set; }
        public char Symbol { get; set; }
    }
}