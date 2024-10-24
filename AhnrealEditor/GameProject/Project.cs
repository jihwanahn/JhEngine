using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AhnrealEditor.GameProject
{
    [DataContract(Name = "Game")]
    public class Project : ViewModelBase
    {
        public static string Extension { get; } = ".ahnreal";
        [DataMember]
        public string ProjectName { get; private set; }
        [DataMember]
        public string ProjectPath { get; private set; }

        public string FullPath => $"{ProjectPath}{ProjectName}{Extension}";
        
        [DataMember(Name ="Scenes")]
        private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();
        public ReadOnlyObservableCollection<Scene> Scenes { get; }

        public Project(string projectName, string projectPath)
        {
            ProjectName = projectName;
            ProjectPath = projectPath;
            
            _scenes.Add(new Scene(this, "Default Scene"));
        }
    }
}
