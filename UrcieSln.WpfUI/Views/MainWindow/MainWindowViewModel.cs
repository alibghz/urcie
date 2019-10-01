using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UrcieSln.Domain;
using UrcieSln.Drawer.PvcItems;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.ViewModels;
using UrcieSln.WpfUI.Views;
using UrcieSln.WpfUI.Views.Materials;
using UrcieSln.WpfUI.Views.Materials.Mullions;
using UrcieSln.WpfUI.Extensions;
using UrcieSln.Domain.Entities;
using UrcieSln.Domain.Extensions;
using UrcieSln.Domain.Models;
using iTextSharp.text.pdf;
using UrcieSln.Domain.Reports;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UMD.HCIL.Piccolo.Util;

namespace UrcieSln.WpfUI
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(FileStorage storage)
            : base(storage) {}

        #region Dependencies

        [Dependency]
        public TabControlViewModel TabControlViewModel
        {
            get;
            protected set;
        }

        [Dependency]
        protected SashesViewViewModel SashesViewViewModel
        {
            get;
            set;
        }

        [Dependency]
        protected ProfilesViewViewModel ProfilesViewViewModel
        {
            get;
            set;
        }

        [Dependency]
        protected MullionsViewViewModel MullionsViewViewModel
        {
            get;
            set;
        }

        [Dependency]
        protected FillingsViewViewModel FillingsViewViewModel
        {
            get;
            set;
        }

        [Dependency]
        protected MuntinsViewViewModel MuntinsViewViewModel
        {
            get;
            set;
        }

        [Dependency]
        protected AccessoriesViewViewModel AccessoriesViewViewModel
        {
            get;
            set;
        }

        #endregion

        #region ProfilesViewCommand

        private ICommand profilesViewCommand;
        public ICommand ProfilesViewCommand
        {
            get
            {
                if (profilesViewCommand == null)
                {
                    profilesViewCommand = new BaseCommand(i => this.ShowProfilesView(), null);
                }
                return profilesViewCommand;
            }
        }
        private void ShowProfilesView()
        {
            TabControlViewModel.OpenTab(
                new ProfilesView(ProfilesViewViewModel),
                "Profile Types",
                ProfilesViewViewModel,
                ProfilesViewViewModel);
        }

        #endregion ProfilesViewCommand

        #region MullionsViewCommand

        private ICommand mullionsViewCommand;
        public ICommand MullionsViewCommand
        {
            get
            {
                if (mullionsViewCommand == null)
                {
                    mullionsViewCommand = new BaseCommand(i => this.ShowMullionsView(), null);
                }
                return mullionsViewCommand;
            }
        }
        private void ShowMullionsView()
        {
            TabControlViewModel.OpenTab(
                new MullionsView(MullionsViewViewModel),
                "Mullion Types",
                MullionsViewViewModel,
                MullionsViewViewModel);
        }

        #endregion MullionsViewCommand

        #region FillingsViewCommand

        private ICommand fillingsViewCommand;
        public ICommand FillingsViewCommand
        {
            get
            {
                if (fillingsViewCommand == null)
                {
                    fillingsViewCommand = new BaseCommand(i => this.ShowFillingsView(), null);
                }
                return fillingsViewCommand;
            }
        }
        private void ShowFillingsView()
        {
            TabControlViewModel.OpenTab(
                new FillingsView(FillingsViewViewModel),
                "Filling Types",
                FillingsViewViewModel,
                FillingsViewViewModel);
        }

        #endregion FillingsViewCommand

        #region SashesViewCommand

        private ICommand sashesViewCommand;
        public ICommand SashesViewCommand
        {
            get
            {
                if (sashesViewCommand == null)
                {
                    sashesViewCommand = new BaseCommand(i => this.ShowSashesView(), null);
                }
                return sashesViewCommand;
            }
        }
        private void ShowSashesView()
        {
            TabControlViewModel.OpenTab(
                new SashesView(SashesViewViewModel),
                "Sash Types",
                SashesViewViewModel,
                SashesViewViewModel);
        }

        #endregion

        #region AccessoriesViewCommand

        private ICommand accessoriesViewCommand;
        public ICommand AccessoriesViewCommand
        {
            get
            {
                if (accessoriesViewCommand == null)
                {
                    accessoriesViewCommand = new BaseCommand(i => this.ShowAccessories(), null);
                }
                return accessoriesViewCommand;
            }
        }
        private void ShowAccessories()
        {
            TabControlViewModel.OpenTab(
                new AccessoriesView(AccessoriesViewViewModel),
                "Accessory Types",
                AccessoriesViewViewModel,
                AccessoriesViewViewModel);
        }

        #endregion

        #region MuntinsViewCommand

        //private ICommand muntinsViewCommand;
        //public ICommand MuntinsViewCommand
        //{
        //	get
        //	{
        //		if (muntinsViewCommand == null)
        //		{
        //			muntinsViewCommand = new BaseCommand(i => this.ShowMuntins(), null);
        //		}
        //		return muntinsViewCommand;
        //	}
        //}
        //private void ShowMuntins()
        //{
        //	TabControlViewModel.OpenTab(
        //		new MuntinsView(MuntinsViewViewModel),
        //		"Muntin Types",
        //		MuntinsViewViewModel,
        //		MuntinsViewViewModel);
        //}

        #endregion

        #region SelectedProject

        private ProjectViewModel selectedProject;
        public ProjectViewModel SelectedProject
        {
            get { return selectedProject; }
            set
            {
                if (selectedProject == null || !selectedProject.Equals(value))
                {
                    selectedProject = value;
                    OnPropertyChanged("SelectedProject");
                }
            }
        }

        #endregion

        #region Selected Unit

        private UnitViewModel selectedUnit;
        public UnitViewModel SelectedUnit
        {
            get { return selectedUnit; }
            set
            {
                if (selectedUnit == null || !selectedUnit.Equals(value))
                {
                    selectedUnit = value;
                    OnPropertyChanged("SelectedUnit");
                }
            }
        }

        #endregion

        #region NewProjectCommand

        private ICommand newProjectCommand;
        public ICommand NewProjectCommand
        {
            get
            {
                if (newProjectCommand == null)
                {
                    newProjectCommand = new BaseCommand(i => this.NewProject(), null);
                }
                return newProjectCommand;
            }
        }
        private void NewProject()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Urcie Project"; // Default file name
            dlg.DefaultExt = ".urc1"; // Default file extension
            dlg.Filter = "Urcie Project (.urc1)|*.urc1"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                ProjectViewModel viewModel = new ProjectViewModel();
                viewModel.Code = dlg.FileName;
                try
                {
                    viewModel.Code = Path.GetFileNameWithoutExtension(dlg.FileName);
                    viewModel.Path = dlg.FileName;
                    Storage.SaveProject(viewModel.ToModel());
                    OpenProjects.Add(viewModel);
                    SelectedProject = viewModel;
                    SelectedProject.Original = viewModel.ToModel();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region OpenProjectCommand

        private ICommand openProjectCommand;
        public ICommand OpenProjectCommand
        {
            get
            {
                if (openProjectCommand == null)
                {
                    openProjectCommand = new BaseCommand(i => this.OpenProject(), null);
                }
                return openProjectCommand;
            }
        }
        private void OpenProject()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Project";
            dialog.DefaultExt = ".urc1";
            dialog.Filter = "UPVC Designer Projects (.urc1)|*.urc1";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                OpenProject(dialog.FileName);
            }
        }

        public void OpenProject(string fileName)
        {
            Project project = Storage.GetProject(fileName);
            if (project == null)
            {
                MessageBox.Show(
                    "An error occured while opening project file.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (OpenProjects.ToList().Find(i => i.Id == project.Id) != null)
            {
                SelectedProject = OpenProjects.ToList().Find(i => i.Id == project.Id);
            }
            else
            {
                //if (dialog.FileName != project.Path)
                //{
                project.Path = fileName;
                project.Code = Path.GetFileNameWithoutExtension(fileName);
                foreach (Unit unit in project.Units)
                {
                    unit.Project.Path = fileName;
                    unit.Project.Code = Path.GetFileNameWithoutExtension(fileName);
                }
                //}
                var projectViewModel = new ProjectViewModel(project);
                OpenProjects.Add(projectViewModel);
                SelectedProject = projectViewModel;
            }
        }

        #endregion

        #region CloseProjectCommand

        private ICommand closeProjectCommand;
        public ICommand CloseProjectCommand
        {
            get
            {
                if (closeProjectCommand == null)
                {
                    closeProjectCommand = new BaseCommand(i => this.CloseProject(), i => this.SelectedProject != null);
                }
                return closeProjectCommand;
            }
        }
        private void CloseProject()
        {
            foreach (UnitViewModel uvm in SelectedProject.Units)
            {
                uvm.CloseCommand.Execute(null);
            }
            OpenProjects.Remove(SelectedProject);
            SelectedProject = null;
        }

        #endregion

        #region NewUnitCommand

        private ICommand newUnitCommand;
        public ICommand NewUnitCommand
        {
            get
            {
                if (newUnitCommand == null)
                {
                    newUnitCommand = new BaseCommand(i => this.NewUnit(), i => this.SelectedProject != null);
                }
                return newUnitCommand;
            }
        }
        private void NewUnit()
        {
            if (SelectedProject == null) return;
            if (Storage.GetAll<ProfileType>().Find(p => p.Shape == ProfileShape.L) == null)
            {
                MessageBox.Show("There is no L profile defined.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Storage.GetAll<ProfileType>().Find(p => p.Shape == ProfileShape.U) == null)
            {
                MessageBox.Show("There is no U profile defined.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Unit unit = new Unit();
            unit.Project = SelectedProject.Original as Project;
            unit.Width = 1000;
            unit.Height = 600;
            unit.Quantity = 1;
            UnitViewModel viewModel = new UnitViewModel(unit);
            EditUnitView view = new EditUnitView(viewModel);
            bool? result = view.ShowDialog();
            if (result.HasValue && result.Value)
            {
                SelectedProject.Units.Add(viewModel);
            }
        }

        #endregion

        #region EditUnitCommand

        private ICommand editUnitCommand;
        public ICommand EditUnitCommand
        {
            get
            {
                if (editUnitCommand == null)
                {
                    editUnitCommand = new BaseCommand(i => this.EditUnit(), i => this.SelectedUnit != null);
                }
                return editUnitCommand;
            }
        }
        private void EditUnit()
        {
            if (SelectedProject == null) return;

            if (TabControlViewModel.IsOpen(SelectedUnit))
            {
                TabControlViewModel.SelectTab(SelectedUnit);
            }
            else
            {
                TabControlViewModel.OpenTab(new UnitView(SelectedUnit, Storage), SelectedUnit.Code, SelectedUnit, SelectedUnit);
            }
        }

        #endregion

        #region DeleteUnitCommand

        private ICommand deleteUnitCommand;
        public ICommand DeleteUnitCommand
        {
            get
            {
                if (deleteUnitCommand == null)
                {
                    deleteUnitCommand = new BaseCommand(i => this.DeleteUnit(), i => this.SelectedUnit != null);
                }
                return deleteUnitCommand;
            }
        }
        private void DeleteUnit()
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete the selected unit?", "Confirm Delete",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Storage.DeleteUnit(SelectedUnit.ToModel());
                SelectedProject.Units.Remove(SelectedUnit);
            }
        }

        #endregion

        #region OpenProjects

        private ObservableCollection<ProjectViewModel> openProjects = new ObservableCollection<ProjectViewModel>();
        public ObservableCollection<ProjectViewModel> OpenProjects
        {
            get { return openProjects; }
            set
            {
                if (openProjects != value)
                {
                    openProjects = value;
                    OnPropertyChanged("OpenProjects");
                }
            }
        }

        #endregion

        #region GenerateReportCommand

        private ICommand generateReportCommand;
        public ICommand GenerateReportCommand
        {
            get
            {
                if (generateReportCommand == null)
                {
                    generateReportCommand = new BaseCommand(i => this.GenerateReport(), i => this.SelectedProject != null);
                }
                return generateReportCommand;
            }
        }
        private void GenerateReport()
        {
            try
            {
                var projName = SelectedProject.Code;
                var logoPath = System.IO.Directory.GetCurrentDirectory() + "\\logo.png";
                var path = Path.GetDirectoryName(SelectedProject.Path);
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                //export images
                foreach (var item in SelectedProject.Units)
                {
                    if (item.Image != null) ((System.Drawing.Image)item.Image).Save(path + "\\" + item.Code + ".png"); //item.Image.ToImage().Save(path + "\\" + item.Code + ".png");
                }

                //initialize and detailed report
                var profiles = new List<Profile>();
                var fillings = new List<Domain.Models.Filling>();
                var accessories = new List<Accessory>();
                foreach (var item in SelectedProject.Units)
                {
                    if (item.Frame == null) //|| item.Frame.Length == 0)
                        continue;
                    byte[] frameByte = item.Frame;
                    if (frameByte == null || frameByte.Length == 0)
                        continue;

                    var formatter = new BinaryFormatter();
                    formatter.SurrogateSelector = PUtil.FrameworkSurrogateSelector;
                    var stream = new MemoryStream(frameByte);
                    var pStream = new PStream(stream);
                    var frame = (PVCFrame)pStream.ReadObjectTree(formatter);
                    stream.Close();

                    for (int i = 0; i < item.Quantity; i++)
                    {
                        profiles.AddRange(frame.Model.GetProfiles(true, true));
                        fillings.AddRange(frame.Model.GetFillings(true));
                        accessories.AddRange(frame.Model.GetAccessories());
                    }
                }

                var profileTables = new List<PdfPTable>();
                var fillingTables = new List<PdfPTable>();
                var accessoryTables = new List<PdfPTable>();
                var tables = new List<PdfPTable>();

                if (profiles != null && profiles.Count > 0) profileTables = Reporter.ProfilesTable(profiles, includesPrice: true).ToList();
                if (fillings != null && fillings.Count > 0) fillingTables = Reporter.FillingsTable(fillings, includesPrice: true).ToList();
                if (accessories != null && fillings.Count > 0) accessoryTables = Reporter.AccessoriesTable(accessories, includesPrice: true).ToList();

                if (profileTables != null && profileTables.Count > 0) tables.AddRange(profileTables);
                if (fillingTables != null && fillingTables.Count > 0) tables.AddRange(fillingTables);
                if (accessoryTables != null && accessoryTables.Count > 0) tables.AddRange(accessoryTables);

                var fileName = path + "\\" + projName + "-Details.pdf";
                if (tables != null && tables.Count > 0) Reporter.ItemsReport(fileName, tables, projName, logoPath);

                //total report
                tables = new List<PdfPTable>();
                if (profiles != null && profiles.Count > 0) profileTables = Reporter.ProfilesTable(profiles, includesPrice: true, onlyTotal: true).ToList();
                if (fillings != null && fillings.Count > 0) fillingTables = Reporter.FillingsTable(fillings, includesPrice: true, onlyTotal: true).ToList();
                if (accessories != null && fillings.Count > 0) accessoryTables = Reporter.AccessoriesTable(accessories, includesPrice: true, onlyTotal: true).ToList();

                if (profileTables != null && profileTables.Count > 0) tables.AddRange(profileTables);
                if (fillingTables != null && fillingTables.Count > 0) tables.AddRange(fillingTables);
                if (accessoryTables != null && accessoryTables.Count > 0) tables.AddRange(accessoryTables);

                fileName = path + "\\" + projName + "-Total.pdf";
                if (tables != null && tables.Count > 0) Reporter.ItemsReport(fileName, tables, projName, logoPath);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() { FileName = path, UseShellExecute = true, Verb = "open" });
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}