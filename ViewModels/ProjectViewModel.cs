using Models.Interfaces;

namespace ViewModels
{
    public class ProjectViewModel
    {
        private IProject project;

        public IProject Project { get => project; }
        public string Name { get => project.Name; }

        public ProjectViewModel(IProject project)
        {
            this.project = project;
        }
    }
}
