using System;

namespace Services.Automaton.Internals
{
    public class State : IEquatable<State>
    {
        public string Name { get; set; }
        public bool IsInitialState { get; set; } = false;
        public bool IsFinalState { get; set; } = false;
        public bool IsVirtualState { get; set; } = false;

        public static implicit operator State(string str)
         => new State() {Name = str};

        public override string ToString() => $"{Name}";

        public bool Equals(State other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && IsVirtualState == other.IsVirtualState;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((State)obj);
        }

        public override int GetHashCode() => HashCode.Combine(Name, IsVirtualState);
    }
}