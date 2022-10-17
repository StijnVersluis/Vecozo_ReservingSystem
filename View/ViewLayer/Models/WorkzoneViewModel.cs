
using BusinessLayer;

namespace ViewLayer.Models
{
    public class WorkzoneViewModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Workspaces { get; private set; }
        public int Floor { get; private set; }
        public bool TeamOnly { get; private set; }

        public WorkzoneViewModel(Workzone workzone)
        {
            Id = workzone.Id;
            Name = workzone.Name;
            Workspaces = workzone.Workspaces;
            Floor = workzone.Floor;
            TeamOnly = workzone.TeamOnly;
        }
    }
}
