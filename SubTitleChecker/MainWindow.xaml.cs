using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SubtitleChecker.Domain;
using SubtitleChecker.Domain.Rules;
using SubtitleChecker.Parser;

namespace SubtitleChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Classes

        public class ValidationData
        {
            public string Time { get; set; }
            public string Number { get; set; }
            public string Severity { get; set; }
            public string Text { get; set; }
            public string Rule { get; set; }
            public string Violation { get; set; }
        }

        #endregion

        #region Constants

        const int Maxtextsize = 83;
        const string Delimiter = ",";

        #endregion

        #region Fields

        private readonly ObservableCollection<ValidationData> _validationCollection = new ObservableCollection<ValidationData>();

        #endregion

        #region Properties

        public ObservableCollection<ValidationData> ValidationCollection
        {
            get { return _validationCollection; }
        }

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            StatusBar.Items[0] = Properties.Resources.StatusBar__Browse_for_a_valid_subtitle;
        }

        #endregion

        #region Methods

        private void BrowseButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog {DefaultExt = ".sub", Filter = "DVD subtitle files (.sub)|*.sub"};
            var result = dialog.ShowDialog();

            if (result != true) return;

            var filename = dialog.FileName;
            FileNameTextBox.Text = filename;
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((0 == e.PreviousSize.Height) && (0 == e.PreviousSize.Width)) return;
            SubtitleCheckerGrid.Width = e.NewSize.Width - 15;
            SubtitleCheckerGrid.Height = e.NewSize.Height - 40;
        }

        private void FileNameTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var filename = FileNameTextBox.Text;
            if (!File.Exists(filename)) StatusBar.Items[0] = Properties.Resources.StatusBar__Browse_for_a_valid_subtitle;
            var result = TryParseSubtitleFile(filename);
            ViewValidationResult(result);
            WriteValidationResultToCsv(filename);
        }

        private void WriteValidationResultToCsv(string originalFilename)
        {
            var fileInfo = new FileInfo(originalFilename);
            var directoryInfo = fileInfo.Directory;
            var i = 0;
            string generatedFile;
            do
            {
                if (i == 0)
                {
                    generatedFile = directoryInfo.FullName + Path.DirectorySeparatorChar + 
                        Path.GetFileNameWithoutExtension(fileInfo.Name) +
                        ".csv";
                }
                else
                {
                    generatedFile = directoryInfo.FullName + Path.DirectorySeparatorChar + 
                        Path.GetFileNameWithoutExtension(fileInfo.Name) +
                        "_" + i + ".csv";
                }
                i++;
            } while (File.Exists(generatedFile));

            using (var fs = new FileStream(generatedFile, FileMode.CreateNew))
            {
                using (var sw = new StreamWriter(fs))
                {
                    foreach (var data in _validationCollection)
                    {
                        sw.Write(data.Number);
                        sw.Write(Delimiter);
                        sw.Write(data.Time);
                        sw.Write(Delimiter);
                        sw.Write(data.Text);
                        sw.Write(Delimiter);
                        sw.Write(data.Severity);
                        sw.Write(Delimiter);
                        sw.Write(data.Rule);
                        sw.Write(Delimiter);
                        sw.Write(data.Violation);
                        sw.WriteLine();
                    }
                }
            }
        }

        private static IEnumerable<RuleValidationResult> TryParseSubtitleFile(string filename)
        {
            RuleValidationResult[] result = null;
            var video = new Video();
            try
            {
                using (var stream = new FileStream(filename, FileMode.Open))
                {
                    var parser = new DvdSubtitleParser(stream);
                    parser.Parse(video);
                }
            }
            catch (Exception exception)
            {
                result = new RuleValidationResult[1];
                result[0] = new RuleValidationResult(RuleValidationResult.SeverityLevel.Exception, "Parse subtitle file.", exception.Message, null);
                video.Subtitles.Validate();
            }
            return result ?? (video.Subtitles.Validate());
        }

        private void ViewValidationResult(IEnumerable<RuleValidationResult> results)
        {
            _validationCollection.Clear();
            foreach (var result in results)
            {
                var data = new ValidationData();
                var subtitle = result.Subtitle;
                if (subtitle != null)
                {
                    data.Text = string.Empty;
                    var first = true;
                    foreach (var text in subtitle.Text)
                    {
                        if (!first) data.Text += "|";
                        data.Text += text;
                        if (data.Text.Length > Maxtextsize) break;
                        first = false;
                    }
                    if (data.Text.Length > Maxtextsize)
                    {
                        data.Text.Substring(0, Maxtextsize-3);
                        data.Text += "...";
                    }

                    data.Number = subtitle.Number.ToString();

                    data.Time = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", subtitle.Offset.Hours, subtitle.Offset.Minutes, subtitle.Offset.Seconds, subtitle.Offset.Milliseconds);
                }
                data.Rule = result.RuleDescription;
                data.Violation = result.ViolationDescription;
                data.Severity = result.Severity.ToString();
                
                _validationCollection.Add(data);
            }
        }

        #endregion
    }
}
