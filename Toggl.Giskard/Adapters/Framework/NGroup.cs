using System;

namespace Toggl.Giskard.Adapters
{
    public class NGroup
    {
        public NGroup(string id)
        {
            Title = $"{id}";
            ID = id;
        }

        public string Title { get; set; }
        public string ID { get; set; }
    }
}
