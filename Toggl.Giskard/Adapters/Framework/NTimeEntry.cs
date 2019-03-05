using System;
using Toggl.Foundation.MvvmCross.Interfaces;

namespace Toggl.Giskard.Adapters
{
    public class NTimeEntry 
    {
        public NTimeEntry(string id)
        {
            Description = $"{id} Time Entry";
            ID = id;
        }

        public string Description { get; set; }
        public string ID { get; set; }
    }
}
