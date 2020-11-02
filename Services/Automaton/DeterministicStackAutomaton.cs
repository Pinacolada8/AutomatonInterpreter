using System.Collections.Generic;
using System.Linq;
using Services.Automaton.Internals;

namespace Services.Automaton
{
    public class DeterministicStackAutomaton : IAutomaton
    {
        private readonly Dictionary<State, List<StackTransition>> _transitions;
        private readonly State                                    _initialState;
        public const     char                                     LAMBDA_SYMBOL = '#';

        public DeterministicStackAutomaton(State initialState, IEnumerable<StackTransition> transitions)
        {
            _initialState = initialState;
            _transitions = transitions
                           .ToLookup(transition => transition.InitialState)
                           .ToDictionary(
                               transitionsByState => transitionsByState.Key,
                               transitionsByState => transitionsByState
                                                     .OrderByDescending(transition =>
                                                         transition.Symbol)
                                                     .ThenByDescending(transition =>
                                                         transition.StackCost)
                                                     .ThenByDescending(transition =>
                                                         transition.StackIncrement)
                                                     .ToList()
                           );
        }

        private IEnumerable<StackTransition> GetAvailableTransitions(State state, char? symbol)
        {
            if (!_transitions.TryGetValue(state, out var availableTransitions))
                return new List<StackTransition>();

            return availableTransitions.Where(x => x.Symbol == symbol || x.Symbol == LAMBDA_SYMBOL);
        }

        public bool TestWord(string word)
        {
            var wordQueue = new Queue<char>(word.Where(x => x != LAMBDA_SYMBOL));
            var stack = new Stack<char>();
            var currentState = _initialState;
            char? targetSymbol = null;

            while (wordQueue.Any() || stack.Any())
            {
                targetSymbol ??= wordQueue.TryDequeue(out var symbol) ? symbol : (char?)null;
                var currentStackTop = stack.TryPeek(out var resultStackPeak) ? resultStackPeak : LAMBDA_SYMBOL;

                var availableTransitions = GetAvailableTransitions(currentState, targetSymbol);

                // Gets the best transition that removes a value from the stack
                var bestTransition = availableTransitions
                    .FirstOrDefault(x => x.StackCost == currentStackTop);

                // Case no transition that removes a value from the stack is found, it tries to
                //  find a transition that doesn't require any stack removal
                bestTransition ??= availableTransitions
                    .FirstOrDefault(x => x.StackCost == LAMBDA_SYMBOL);

                // Case no transition is found it returns false meaning that the word is not
                //  valid for the automaton
                if (bestTransition == default)
                    return false;

                if (bestTransition.StackCost != LAMBDA_SYMBOL)
                    if (!stack.TryPop(out var trash))
                        return false;

                if (bestTransition.StackIncrement != LAMBDA_SYMBOL)
                    stack.Push(bestTransition.StackIncrement);

                if (bestTransition.Symbol == targetSymbol && !bestTransition.TargetState.IsVirtualState)
                    targetSymbol = null;

                currentState = bestTransition.TargetState;
            }

            return currentState.IsFinalState;
        }
    }
}