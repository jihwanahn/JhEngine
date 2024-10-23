using CoreEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace CoreEditor.GameProject
{
    [DataContract]
    public class ProjectTemplate
    {
        [DataMember] 
        public string ProjectType { get; set; }
        [DataMember] 
        public string ProjectFile { get; set; }
        [DataMember] 
        public List<string> Folders{ get; set; }
        [DataMember]
        public byte[] Icon { get; set; }
        [DataMember]
        public byte[] Preview { get; set; }
        [DataMember]
        public string IconFilePath { get; set; }
        [DataMember]
        public string PreviewFilePath { get; set; }
        [DataMember]
        public string ProjectFilePath { get; set; }
    }
    class NewProject : ViewModelBase
    {
        private readonly string _templatePath = @"..\..\CoreEditor\ProjectTemplates";
        //private readonly string _logPath = @"..\..\CoreEditor\Logs\";
        private string _projectName = "NewProject";
        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }
        private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\AhnrealProject\";
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (_projectPath != value)
                {
                    _projectPath = value;
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }


        public NewProject()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);
            try {
                var templateFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                Debug.Assert(templateFiles.Any());
                foreach (var templateFile in templateFiles) 
                {
                    var template = Serializer.FromFile<ProjectTemplate>(templateFile);
                    template.IconFilePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(templateFile), "Logo.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.PreviewFilePath= System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(templateFile), "Preview.png"));
                    template.Preview = File.ReadAllBytes(template.PreviewFilePath);
                    template.ProjectFilePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(templateFile), template.ProjectFile));

                    _projectTemplates.Add(template);
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                //File.WriteAllText(_logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt", e.Message);
            }
        }
    }
}
