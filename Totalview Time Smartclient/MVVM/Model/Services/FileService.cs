namespace Totalview_Time_Smartclient.MVVM.Model.Services
{
    public interface IFileService
    {
        string[] GetFontFilenames();
    }
    public class FileService : IFileService
    {
        private readonly string _workingDirectory;
        private readonly string _projectDirectory;
        public FileService()
        {
            _workingDirectory = Environment.CurrentDirectory;
            _projectDirectory = Directory.GetParent(_workingDirectory).Parent.Parent.FullName;
        }

        public string[] GetFontFilenames()
        {
            string fontsDirectory = _projectDirectory + "\\Resources\\Fonts";
            return Directory.GetFiles(fontsDirectory);
        }
    }
}
