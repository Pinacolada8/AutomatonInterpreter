using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace Business.Input
{
    public class StackAutomatonInput : Collection<object>
    {
        public string[] States =>((JArray)base[0]).ToObject<string[]>();
        public string[] Alphabet => ((JArray)base[1]).ToObject<string[]>();
        public string[] StackAlphabet => ((JArray)base[2]).ToObject<string[]>();
        public IEnumerable<string[]> Transitions => ((JArray)base[3]).ToObject<IEnumerable<string[]>>();
        public string InitialState => (string)base[4];
        public string[] FinalState => ((JArray)base[5]).ToObject<string[]>();

        public bool CheckIfValidInput() => base.Count >= 6;
    }
}