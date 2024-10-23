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
                    ValidateProjectPath();
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
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        private string _errorMsg;
        public string ErrorMsg
        {
            get => _errorMsg;
            set
            {
                if (_errorMsg != value)
                {
                    _errorMsg = value;
                    OnPropertyChanged(nameof(ErrorMsg));
                }
            }
        }

        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }

        private bool ValidateProjectPath()
        {
            var path = ProjectPath;
            if (!Path.EndsInDirectorySeparator(path))
            {
                path += @"\";
                path += "$@{ProjectName}";

                IsValid = false;
                if (string.IsNullOrWhiteSpace(ProjectName))
                {
                    ErrorMsg = "Project name is empty";
                }
                else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                {
                    ErrorMsg = "Project name contains invalid characters";
                }
                else if (string.IsNullOrWhiteSpace(ProjectPath.Trim()))
                {
                    ErrorMsg = "Project path is empty";
                }
                else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                { 
                    ErrorMsg = "Project path contains invalid characters";
                }
                else if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
                {
                    ErrorMsg = "Project path already exists and is not empty";
                }
                else
                {
                    IsValid = true;
                    ErrorMsg = string.Empty;
                }
            }
            return IsValid;
        }

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
                ValidateProjectPath();
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                //File.WriteAllText(_logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt", e.Message);
            }
        }
    }
}
