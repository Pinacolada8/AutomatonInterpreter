using System.Collections.Generic;
using System.Linq;
using Business.Input;
using Services.Automaton;
using Services.Automaton.Internals;

namespace App.Extensions
{
    public static class StackAutomatonInputExtensions
    {
        public static DeterministicStackAutomaton ToDeterministicStackAutomaton(this StackAutomatonInput input)
        {
            if (!input.CheckIfValidInput())
                return default;

            var states = input.States.Select(x => new State()
                              {
                                  Name           = x,
                                  IsInitialState = x == input.InitialState,
                                  IsFinalState   = input.FinalState.Contains(x)
                              })
                              .ToList();

            // Checks if a initial state exists
            if (!states.Any(x => x.IsInitialState))
                return default;

            // Checks if a final state exists
            if (!states.Any(x => x.IsFinalState))
                return default;

            var transitions = new List<StackTransition>();

            var alphabet = input.Alphabet
                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                .Select(x => x[0])
                                .Append(DeterministicStackAutomaton.LAMBDA_SYMBOL)
                                .ToList();
            var stackAlphabet = input.StackAlphabet
                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                     .Select(x => x[0])
                                     .Append(DeterministicStackAutomaton.LAMBDA_SYMBOL)
                                     .ToList();

            foreach (var inputTransition in input.Transitions)
            {
                if (inputTransition.Length != 5)
                    return default;

                var transitionInitialState = states.Find(x => x.Name == inputTransition[0]);

                var transitionSymbol = string.IsNullOrEmpty(inputTransition[1])
                                           ? DeterministicStackAutomaton.LAMBDA_SYMBOL
                                           : inputTransition[1][0];
                var transitionCosts = inputTransition[2].Any()
                                          ? inputTransition[2].Reverse().ToArray()
                                          : new[] { DeterministicStackAutomaton.LAMBDA_SYMBOL };
                var transitionFinalState = states.Find(x => x.Name == inputTransition[3]);
                var transitionIncrements = inputTransition[4].Any()
                                               ? inputTransition[4].Reverse().ToArray()
                                               : new[] { DeterministicStackAutomaton.LAMBDA_SYMBOL };

                if (transitionInitialState  == null
                    || transitionFinalState == null
                    || !alphabet.Contains(transitionSymbol)
                    || !transitionCosts.All(x => stackAlphabet.Contains(x))
                    || !transitionIncrements.All(x => stackAlphabet.Contains(x)))
                    return default;

                // Creates new "virtual(fake)" transitions where the cost or increment is more than one value
                var transitionsInsideLine = new List<StackTransition>();
                for (var i = 0; i < transitionCosts.Length; i++)
                {
                    transitionsInsideLine = new List<StackTransition>();
                    if (i == transitionCosts.Length - 1)
                    {
                        for (var j = 0; j < transitionIncrements.Length; j++)
                        {
                            if (j == 0)
                            {
                                var targetState = new State()
                                {
                                    Name           = transitionInitialState.Name + $"^{j}",
                                    IsVirtualState = true
                                };
                                transitionsInsideLine.Add(new StackTransition()
                                {
                                    InitialState   = transitionInitialState,
                                    Symbol         = transitionSymbol,
                                    TargetState    = targetState,
                                    StackIncrement = transitionIncrements[j],
                                    StackCost      = transitionCosts[i],
                                });
                                transitionInitialState = targetState;
                            }
                            else
                            {
                                var targetState = new State()
                                {
                                    Name           = transitionInitialState.Name + $"^{j}",
                                    IsVirtualState = true
                                };
                                transitionsInsideLine.Add(new StackTransition()
                                {
                                    InitialState   = transitionInitialState,
                                    Symbol         = transitionSymbol,
                                    TargetState    = targetState,
                                    StackIncrement = transitionIncrements[j],
                                    StackCost      = DeterministicStackAutomaton.LAMBDA_SYMBOL,
                                });
                                transitionInitialState = targetState;
                            }
                        }
                    }
                    else
                    {
                        var targetState = new State()
                        {
                            Name           = transitionInitialState.Name + $"_{i}",
                            IsVirtualState = true
                        };
                        transitionsInsideLine.Add(new StackTransition()
                        {
                            InitialState   = transitionInitialState,
                            Symbol         = transitionSymbol,
                            TargetState    = targetState,
                            StackIncrement = DeterministicStackAutomaton.LAMBDA_SYMBOL,
                            StackCost      = transitionCosts[i]
                        });
                        transitionInitialState = targetState;
                    }
                }

                transitionsInsideLine[^1].TargetState = transitionFinalState;

                transitions.AddRange(transitionsInsideLine);
            }

            return new DeterministicStackAutomaton(states.Find(x => x.IsInitialState), transitions);
        }
    }
}