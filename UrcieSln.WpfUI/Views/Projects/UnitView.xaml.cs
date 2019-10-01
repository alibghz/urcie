using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UrcieSln.Domain;
using UrcieSln.Domain.Entities;
using UrcieSln.Drawer.PvcItems;
using UrcieSln.WpfUI.Extensions;
using UrcieSln.Domain.Extensions;
using System.Collections.ObjectModel;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.Views.Materials;

namespace UrcieSln.WpfUI.Views
{
    public partial class UnitView : UserControl, INotifyPropertyChanged
    {
        public UnitView(UnitViewModel viewModel, FileStorage storage)
        {
            ViewModel = viewModel;
            Storage = storage;
            InitializeComponent();
            AddHotKeys();
            KeyUp += UnitView_KeyUp;
        }

        void UnitView_KeyUp(object sender, KeyEventArgs e)
        {
            if (MullionToolActive)
            {
                if (e.Key == Key.H)
                {
                    HorizontalMullionChecked = true;
                    return;
                }
                else if (e.Key == Key.V)
                {
                    VerticalMullionChecked = true;
                    return;
                }
            }
            if (SelectToolActive)
            {
                if (e.Key == Key.Delete)
                {
                    var selectedNode = SelectionEventHandler.SelectedNode;

                    if (selectedNode is Mullion)
                    {
                        PItemRemove.RemoveShiftLeftMullion((Mullion)selectedNode);
                    }
                    else if (selectedNode is Sash || selectedNode is Filling)
                    {
                        PItemRemove.RemoveItem(selectedNode);
                    }
                    return;
                }
            }
        }
        void AddHotKeys()
        {
            try
            {
                // CTRL + S ==> SAVE
                RoutedCommand saveTool = new RoutedCommand();
                saveTool.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(saveTool, SaveToolBtn_Click));

                // CTRL + T ==> SELECT TOOL
                RoutedCommand selectTool = new RoutedCommand();
                selectTool.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(selectTool, SelectToolBtn_Click));

                // CTRL + M ==> MULLION TOOL
                RoutedCommand mullionTool = new RoutedCommand();
                mullionTool.InputGestures.Add(new KeyGesture(Key.M, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(mullionTool, MullionToolBtn_Click));

                // CTRL + H ==> SASH TOOL
                RoutedCommand sashTool = new RoutedCommand();
                sashTool.InputGestures.Add(new KeyGesture(Key.H, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(sashTool, SashToolBtn_Click));

                // CTRL + F ==> FILLING TOOL
                RoutedCommand fillingTool = new RoutedCommand();
                fillingTool.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(fillingTool, FillingToolBtn_Click));

                // CTRL + P ==> PAN TOOL
                RoutedCommand panTool = new RoutedCommand();
                panTool.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(panTool, PanToolBtn_Click));

                // CTRL + R ==> RELOAD TOOL
                RoutedCommand reloadTool = new RoutedCommand();
                reloadTool.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
                CommandBindings.Add(new CommandBinding(reloadTool, RefreshButton_Click));
            }
            catch
            {
            }
        }
        public UnitViewModel ViewModel
        {
            get { return (UnitViewModel)DataContext; }
            set { DataContext = value; }
        }
        public FileStorage Storage
        {
            get;
            set;
        }
        private void Init()
        {
            ProfileTypes = new ObservableCollection<ProfileType>(Storage.GetAll<ProfileType>());
            MullionTypes = new ObservableCollection<MullionType>(Storage.GetAll<MullionType>());
            SashTypes = new ObservableCollection<SashType>(Storage.GetAll<SashType>());
            FillingTypes = new ObservableCollection<FillingType>(Storage.GetAll<FillingType>());

            if (ViewModel.Frame != null)
            {
                ViewModel.Modified = false;
            }

            InitCanvas();
            InitFrame();
            if (Frame == null)
            {
                SelectToolBtn.IsEnabled = false;
                PanToolBtn.IsEnabled = false;
                RefreshButton.IsEnabled = false;
                MullionToolBtn.IsEnabled = false;
                SashToolBtn.IsEnabled = false;
                FillingToolBtn.IsEnabled = false;
                zoomSlider.IsEnabled = false;
                FrameFontSize.IsEnabled = false;
                ShowCodesCheckBox.IsEnabled = false;
                return;
            }
            ViewModel.CloseRequested += ViewModel_CloseRequested;
            SaveToolEnabled = true;
            SelectToolBtn.IsChecked = true;
            Frame.AddInputEventListener(SelectionEventHandler);
            Frame.AddInputEventListener(DragEventHandler);
            SelectToolActive = true;
        }

        private void ViewModel_CloseRequested(object sender, CloseEventHandlerArgs args)
        {
            if (ViewModel.Modified)
            {
                ViewModel.CloseRequested -= ViewModel_CloseRequested;
                Save();
            }
        }

        #region Canvas

        private PCanvas Canvas
        {
            get;
            set;
        }
        private void InitCanvas()
        {
            Canvas = new PCanvas();
            Canvas.AnimatingRenderQuality = UMD.HCIL.Piccolo.Util.RenderQuality.HighQuality;
            Canvas.DefaultRenderQuality = UMD.HCIL.Piccolo.Util.RenderQuality.HighQuality;
            Canvas.InteractingRenderQuality = UMD.HCIL.Piccolo.Util.RenderQuality.HighQuality;
            Canvas.GridFitText = true;
            Canvas.PanEventHandler = null;
            Canvas.ZoomEventHandler = null;
        }

        #endregion

        #region Unit/Frame

        public float[] FontSizes
        {
            get
            {
                return new float[] { 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52, 54, 56, 58, 60 };
            }
        }
        private PVCFrame Frame
        {
            get;
            set;
        }
        FrameDecorator Decorator
        {
            get;
            set;
        }
        private void InitFrame()
        {
            if (ViewModel.Frame == null)
            {
                ProfileType p = Storage.GetAll<ProfileType>().Find(i => i.Shape == ProfileShape.L);
                if (p == null)
                {
                    MessageBox.Show("There is no profile type defined.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Frame = new PVCFrame(ViewModel.Width, ViewModel.Height, Storage.GetAll<ProfileType>()[0]);
                ViewModel.Modified = true;
                Frame.FontSize = (float)FrameFontSize.SelectedItem;
            }
            else
            {
                Frame = (PVCFrame)ViewModel.Frame.ToFrame();
                FrameFontSize.SelectedValue = Frame.FontSize;
            }
            Frame.Click += Frame_Click;
            Frame.Model.Code = ViewModel.Code;
            ShowCodesCheckBox.IsChecked = Frame.ShowCodes;
            Canvas.Layer.AddChild(Frame);
            SelectionEventHandler = new SelectionEventHandler(Frame);
            DivisionEventHandler = new DivisionEventHandler(Frame.Surface);
            SashCreationEventHandler = new SashCreationEventHandler(Frame.Surface);
            FillingCreationEventHandler = new FillingCreationEventHandler(Frame.Surface);
            DragEventHandler = new MullionDragEventHandler(Canvas, Frame);
            PanEventHandler = new PPanEventHandler();

            Decorator = new FrameDecorator(Frame, Canvas.Layer);
            Decorator.EnableFrameDecorator();

            SelectionEventHandler.SelectionChanged += SelectionEventHandler_SelectionChanged;
            LProfileComboBox.SelectedItem = Frame.Model.ProfileType;
            UProfileComboBox.SelectedItem = Frame.Model.UProfileType;
            OnPropertyChanged("UnitWidth");
            OnPropertyChanged("UnitHeight");

            Frame.DimensionChanged += Frame_DimensionChanged;
        }
        private void Frame_DimensionChanged()
        {
            ViewModel.Modified = true;
        }
        private void LProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProfileType p = LProfileComboBox.SelectedItem as ProfileType;
            if (p == null) return;

            Frame.Model.ProfileType = p;
            Frame.Repaint();
        }
        private void UProfileCombBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProfileType p = UProfileComboBox.SelectedItem as ProfileType;
            if (p == null) return;
            Frame.Model.UProfileType = p;
            Frame.Repaint();
        }
        public float UnitWidth
        {
            get
            {
                if (Frame == null) return 1000;
                return Frame.Model.Width;
            }

            set
            {
                if (Frame == null) return;
                if (Frame.Model.Width != Width)
                {
                    Frame.Model.Width = value;
                    Frame.Repaint();
                    OnPropertyChanged("UnitWidth");
                }
            }
        }
        public float UnitHeight
        {
            get
            {
                if (Frame == null) return 1000;
                return Frame.Model.Height;
            }

            set
            {
                if (Frame == null) return;
                if (Frame.Model.Height != value)
                {
                    Frame.Model.Height = value;
                    Frame.Repaint();
                    OnPropertyChanged("UnitHeight");
                }
            }
        }

        #endregion

        #region DrawerEventHandlers

        protected SelectionEventHandler SelectionEventHandler
        {
            get;
            set;
        }
        protected DivisionEventHandler DivisionEventHandler
        {
            get;
            set;
        }
        protected SashCreationEventHandler SashCreationEventHandler
        {
            get;
            set;
        }
        protected FillingCreationEventHandler FillingCreationEventHandler
        {
            get;
            set;
        }
        protected PPanEventHandler PanEventHandler
        {
            get;
            set;
        }
        protected MullionDragEventHandler DragEventHandler
        {
            get;
            set;
        }

        #endregion

        #region Toolbar - SaveToolButton

        private void SaveToolBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!SaveToolEnabled) return;
            Save();
        }
        private bool saveToolEnabled = false;
        public bool SaveToolEnabled
        {
            get { return saveToolEnabled; }
            set
            {
                if (saveToolEnabled != value)
                {
                    saveToolEnabled = value;
                    OnPropertyChanged("SaveToolEnabled");
                }
            }
        }
        public void Save()
        {
            ClearEventHandlers();
            Keyboard.FocusedElement.RaiseEvent(new RoutedEventArgs() { RoutedEvent = LostFocusEvent });

            RectangleF rect = Canvas.Layer.UnionOfChildrenBounds;
            System.Drawing.Image mainImage = Canvas.Layer.ToImage(
                (int)(rect.Width + rect.X + Frame.FontSize),
                (int)(rect.Height + rect.Y + Frame.FontSize),
                System.Drawing.Brushes.White);

            System.Drawing.Image img = new Bitmap(
                (int)(rect.Width + rect.X + Frame.FontSize),
                (int)(rect.Height + rect.Y + Frame.FontSize + 80));

            Graphics g = Graphics.FromImage(img);
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, img.Width, img.Height);
            g.DrawImage(mainImage, 5, 5, mainImage.Width, mainImage.Height);
            if (!string.IsNullOrEmpty(ViewModel.Description))
            {
                g.DrawString(ViewModel.Description, new Font("Arial", Frame.FontSize),
                    System.Drawing.Brushes.Black, new RectangleF(0, mainImage.Height + 15, mainImage.Width, 60));
            }
            ViewModel.Image = img; //.ToBytes();
            ViewModel.VersionInfo = "1.0.0.0";
            Decorator.DisableFrameDecorator();
            Frame.DimensionChanged -= Frame_DimensionChanged;
            ViewModel.Frame = Frame.ToBytes();
            Frame.DimensionChanged += Frame_DimensionChanged;
            Decorator.EnableFrameDecorator();
            Unit unit = ViewModel.ToModel();
            unit.Width = Frame.Width;
            unit.Height = Frame.Height;
            Storage.SaveUnit(unit);
            ViewModel.Original = unit;
            ViewModel.Modified = false;
            SelectToolActive = true;
            SelectToolBtn.IsChecked = true;
        }

        #endregion

        #region Toolbar - SelectToolButton

        private void SelectToolBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Frame == null) return;

            ClearEventHandlers();
            SelectToolBtn.IsChecked = true;
            Frame.AddInputEventListener(SelectionEventHandler);
            Frame.AddInputEventListener(DragEventHandler);
            SelectToolActive = true;
        }
        private bool selectToolActive = false;
        public bool SelectToolActive
        {
            get { return selectToolActive; }
            set
            {
                if (selectToolActive != value)
                {
                    selectToolActive = value;
                    OnPropertyChanged("SelectToolActive");
                }
            }
        }

        #endregion

        #region Toolbar - PanToolButton

        private void PanToolBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearEventHandlers();
            PanToolBtn.IsChecked = true;
            Canvas.PanEventHandler = PanEventHandler;
            PanToolActive = true;
        }
        private bool panToolActive = false;
        public bool PanToolActive
        {
            get { return panToolActive; }
            set
            {
                if (panToolActive != value)
                {
                    panToolActive = value;
                    OnPropertyChanged("PanToolActive");
                }
            }
        }

        #endregion

        #region Toolbar - MullionToolButton

        private bool mullionToolActive = false;
        public bool MullionToolActive
        {
            get { return mullionToolActive; }
            set
            {
                if (mullionToolActive != value)
                {
                    mullionToolActive = value;
                    OnPropertyChanged("MullionToolActive");
                }
            }
        }

        private bool verticalMullionChecked = true;
        public bool VerticalMullionChecked
        {
            get { return verticalMullionChecked; }
            set
            {
                if (verticalMullionChecked != value)
                {
                    verticalMullionChecked = value;
                    OnPropertyChanged("VerticalMullionChecked");
                    if (verticalMullionChecked) DivisionEventHandler.Orientation = Domain.Entities.Orientation.Vertical;
                }
            }
        }

        private bool horizontalMullionChecked = false;
        public bool HorizontalMullionChecked
        {
            get { return horizontalMullionChecked; }
            set
            {
                if (horizontalMullionChecked != value)
                {
                    horizontalMullionChecked = value;
                    OnPropertyChanged("HorizontalMullionChecked");
                    if (value)
                        DivisionEventHandler.Orientation = Domain.Entities.Orientation.Horizontal;
                }
            }
        }

        private void MullionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DivisionEventHandler == null) return;

            DivisionEventHandler.MullionType = (MullionType)MullionType.SelectedItem;
        }
        private void MullionToolBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Frame == null) return;
            ClearEventHandlers();
            MullionToolBtn.IsChecked = true;
            Frame.AddInputEventListener(DivisionEventHandler);
            MullionToolActive = true;

            if (VerticalMullionChecked)
                DivisionEventHandler.Orientation = Domain.Entities.Orientation.Vertical;
            else if (HorizontalMullionChecked)
                DivisionEventHandler.Orientation = Domain.Entities.Orientation.Horizontal;
            else
                VerticalMullionChecked = true;

            DivisionEventHandler.MullionType = MullionType.SelectedItem as MullionType;
        }

        #endregion

        #region Toolbar - SashToolButton

        private bool sashToolActive = false;
        public bool SashToolActive
        {
            get { return sashToolActive; }
            set
            {
                if (sashToolActive != value)
                {
                    sashToolActive = value;
                    OnPropertyChanged("SashToolActive");
                }
            }
        }
        private void SashTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SashCreationEventHandler == null) return;

            SashCreationEventHandler.SashType = SashTypeComboBox.SelectedItem as SashType;
        }
        private void SashDirectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SashCreationEventHandler == null) return;

            SashCreationEventHandler.Direction = SashDirectionComboBox.SelectedValue.ToString();
        }
        private void SashToolBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Frame == null) return;
            ClearEventHandlers();
            SashToolBtn.IsChecked = true;
            Frame.AddInputEventListener(SashCreationEventHandler);
            SashCreationEventHandler.SashType = SashTypeComboBox.SelectedItem as SashType;
            SashCreationEventHandler.Direction = SashDirectionComboBox.SelectedValue.ToString();
            SashToolActive = true;
        }

        #endregion

        #region Toolbar - FillingToolButton

        private bool fillingToolActive = false;
        public bool FillingToolActive
        {
            get { return fillingToolActive; }
            set
            {
                if (fillingToolActive != value)
                {
                    fillingToolActive = value;
                    OnPropertyChanged("FillingToolActive");
                }
            }
        }
        private void FillingTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FillingCreationEventHandler == null) return;

            FillingCreationEventHandler.FillingType = FillingTypeComboBox.SelectedItem as FillingType;
        }
        private void IProfileTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FillingCreationEventHandler == null) return;

            if (IncludeIProfileCheckBox.IsChecked.Value)
                FillingCreationEventHandler.ProfileType = IProfileTypeComboBox.SelectedItem as ProfileType;
            else
                FillingCreationEventHandler.ProfileType = null;
        }
        private void IncludeIProfileCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (FillingCreationEventHandler == null) return;

            if (IncludeIProfileCheckBox.IsChecked.Value)
                FillingCreationEventHandler.ProfileType = IProfileTypeComboBox.SelectedItem as ProfileType;
            else
                FillingCreationEventHandler.ProfileType = null;
        }
        private void FillingToolBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Frame == null) return;
            ClearEventHandlers();
            FillingToolBtn.IsChecked = true;
            Frame.AddInputEventListener(FillingCreationEventHandler);
            FillingCreationEventHandler.FillingType = FillingTypeComboBox.SelectedItem as FillingType;
            if (IProfileTypeComboBox.SelectedItem == null || (IProfileTypeComboBox.SelectedItem as ProfileType).Shape != ProfileShape.I)
            {
                var pType = ProfileTypes.FirstOrDefault(x => x.Shape == ProfileShape.I);
                if (pType != null)
                    IProfileTypeComboBox.SelectedItem = pType;
            }
            if (IncludeIProfileCheckBox.IsChecked.Value)
                FillingCreationEventHandler.ProfileType = IProfileTypeComboBox.SelectedItem as ProfileType;
            else
                FillingCreationEventHandler.ProfileType = null;
            FillingToolActive = true;
        }

        #endregion

        #region SelectionEventHandler

        private void SelectionEventHandler_SelectionChanged(object sender, PPropertyEventArgs e)
        {
            FrameSelected = false;
            MullionSelected = false;
            SashSelected = false;
            FillingSelected = false;
            IncreaseMiddlePoint.IsEnabled = false;
            DeacreaseMiddlePoint.IsEnabled = false;
            SelectedSashType.SelectedItem = null;
            SelectedSashDirectionComboBox.SelectedItem = null;
            MiddlePointTextBox.IsEnabled = false;
            SelectedFillingType.SelectedItem = null;
            SelectedIProfileType.SelectedItem = null;
            SelectedIncludeIProfileTypeCheckBox.IsChecked = false;
            if (SelectionEventHandler.SelectedNode is Mullion)
            {
                MullionSelected = true;
                Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
                VirtualMullion.IsChecked = mullion.Model.IsVirtual;
                SelectedMullionType.SelectedItem = mullion.Model.MullionType;
                IncreaseMiddlePoint.IsEnabled = true;
                DeacreaseMiddlePoint.IsEnabled = true;
                MiddlePointTextBox.IsEnabled = true;
                MullionMiddlePoint = mullion.Model.MiddlePoint;
                OnPropertyChanged("MullionMiddlePoint");
                mullion.Model.PropertyChanged += Model_PropertyChanged;
            }
            else if (SelectionEventHandler.SelectedNode is Sash)
            {
                SashSelected = true;
                Sash sash = SelectionEventHandler.SelectedNode as Sash;
                SelectedSashType.SelectedItem = sash.Model.SashType;
                SelectedSashDirectionComboBox.SelectedValue = sash.Direction;
                SashFixedCheckBox.IsChecked = sash.Fixed;
            }
            else if (SelectionEventHandler.SelectedNode is Filling)
            {
                FillingSelected = true;
                Filling filling = SelectionEventHandler.SelectedNode as Filling;
                SelectedFillingType.SelectedItem = filling.Model.FillingType;
                if (filling.Model.ProfileType != null)
                {
                    SelectedIncludeIProfileTypeCheckBox.IsChecked = true;
                    SelectedIProfileType.SelectedItem = filling.Model.ProfileType;
                }
                else
                {
                    SelectedIncludeIProfileTypeCheckBox.IsChecked = false;
                    SelectedIProfileType.SelectedItem = null;
                }
            }
            else if (SelectionEventHandler.SelectedNode is PVCFrame)
            {
                FrameSelected = true;
            }

            if (e.OldValue is Mullion)
            {
                Mullion oldMullion = (Mullion)e.OldValue;
                oldMullion.Model.PropertyChanged -= Model_PropertyChanged;
            }
        }
        void Model_PropertyChanged(object sender, PVCModelPropertyChangedEventArgs e)
        {
            if (e.PropertyCode == MullionModel.MIDDLE_POINT_PROPERTY_CODE)
            {
                Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
                if (mullion != null)
                {
                    MiddlePointTextBox.Text = mullion.Model.MiddlePoint.ToString();
                }
            }
        }

        private bool frameSelected = false;
        public bool FrameSelected
        {
            get { return frameSelected; }
            set
            {
                if (frameSelected != value)
                {
                    frameSelected = value;
                    OnPropertyChanged("FrameSelected");
                }
            }
        }

        private bool mullionSelected = false;
        public bool MullionSelected
        {
            get { return mullionSelected; }
            set
            {
                if (mullionSelected != value)
                {
                    mullionSelected = value;
                    OnPropertyChanged("MullionSelected");
                }
            }
        }

        private bool sashSelected = false;
        public bool SashSelected
        {
            get { return sashSelected; }
            set
            {
                if (sashSelected != value)
                {
                    sashSelected = value;
                    OnPropertyChanged("SashSelected");
                }
            }
        }

        private bool fillingSelected = false;
        public bool FillingSelected
        {
            get { return fillingSelected; }
            set
            {
                if (fillingSelected != value)
                {
                    fillingSelected = value;
                    OnPropertyChanged("FillingSelected");
                }
            }
        }

        #endregion

        private void ClearEventHandlers()
        {
            if (Frame == null) return;

            Canvas.RemoveInputEventListener(PanEventHandler);
            Frame.RemoveInputEventListener(SelectionEventHandler);
            Frame.RemoveInputEventListener(DivisionEventHandler);
            Frame.RemoveInputEventListener(SashCreationEventHandler);
            Frame.RemoveInputEventListener(FillingCreationEventHandler);
            Frame.RemoveInputEventListener(DragEventHandler);

            SelectToolBtn.IsChecked = false;
            MullionToolBtn.IsChecked = false;
            SashToolBtn.IsChecked = false;
            FillingToolBtn.IsChecked = false;
            PanToolBtn.IsChecked = false;

            SelectToolActive = false;
            MullionToolActive = false;
            SashToolActive = false;
            FillingToolActive = false;
            PanToolActive = false;

            FrameSelected = false;
            MullionSelected = false;
            SashSelected = false;
            FillingSelected = false;

            SelectionEventHandler.SelectedNode = null;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region UI Event Handlers

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            WindowsFormsHost host = new WindowsFormsHost();
            host.SnapsToDevicePixels = true;
            host.Child = Canvas;
            CanvasGrid.Children.Add(host);
        }
        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Canvas != null)
            {
                Canvas.Camera.ViewScale = (float)(zoomSlider.Value / 100);
                Canvas.Camera.Repaint();
                zoomLabel.Text = (int)(zoomSlider.Value) + " %";
                FitToScreen.IsChecked = false;
            }
        }
        private void Frame_Click(object sender, PInputEventArgs e)
        {
            if (e.PickedNode is Surface || e.PickedNode is PVCFrame) return;

            if (e.PickedNode.IsDescendentOf(Frame) && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                SelectionEventHandler.SelectedNode = e.PickedNode;
                if (SelectionEventHandler.SelectedNode is Mullion)
                {
                    GetMullionContextMenu(((Mullion)e.PickedNode).Model.Orientation).Show(System.Windows.Forms.Cursor.Position);
                }
                else
                    GetNormalContextMenu().Show(System.Windows.Forms.Cursor.Position);
            }
        }
        private System.Windows.Forms.ContextMenuStrip GetNormalContextMenu()
        {
            System.Windows.Forms.ContextMenuStrip ctxMenu = new System.Windows.Forms.ContextMenuStrip();
            System.Windows.Forms.ToolStripMenuItem menuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuItem.Text = "Delete Item";
            menuItem.Click += menuItem_Click;
            ctxMenu.Items.Add(menuItem);
            return ctxMenu;
        }
        private void menuItem_Click(object sender, EventArgs e)
        {
            PItemRemove.RemoveItem(SelectionEventHandler.SelectedNode);
        }
        private System.Windows.Forms.ContextMenuStrip GetMullionContextMenu(UrcieSln.Domain.Entities.Orientation orientation)
        {
            System.Windows.Forms.ContextMenuStrip ctxMenu = new System.Windows.Forms.ContextMenuStrip();
            if (orientation == UrcieSln.Domain.Entities.Orientation.Horizontal)
            {
                System.Windows.Forms.ToolStripMenuItem shiftUpMnuItem = new System.Windows.Forms.ToolStripMenuItem();
                System.Windows.Forms.ToolStripMenuItem shiftDownMnuItem = new System.Windows.Forms.ToolStripMenuItem();
                shiftUpMnuItem.Text = "Shit Up";
                shiftDownMnuItem.Text = "Shift Down";

                shiftUpMnuItem.Click += shiftUpMnuItem_Click;
                shiftDownMnuItem.Click += shiftDownMnuItem_Click;
                ctxMenu.Items.Add(shiftDownMnuItem);
                ctxMenu.Items.Add(shiftUpMnuItem);
            }
            else
            {
                System.Windows.Forms.ToolStripMenuItem shiftLeftMnuItem = new System.Windows.Forms.ToolStripMenuItem();
                System.Windows.Forms.ToolStripMenuItem shiftRightMnuItem = new System.Windows.Forms.ToolStripMenuItem();
                shiftLeftMnuItem.Text = "Shift Left";
                shiftRightMnuItem.Text = "Shift Right";

                shiftLeftMnuItem.Click += shiftLeftMnuItem_Click;
                shiftRightMnuItem.Click += shiftRightMnuItem_Click;
                ctxMenu.Items.Add(shiftLeftMnuItem);
                ctxMenu.Items.Add(shiftRightMnuItem);
            }
            return ctxMenu;
        }
        private void shiftRightMnuItem_Click(object sender, EventArgs e)
        {
            Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
            if (mullion == null || mullion.Model.Orientation != UrcieSln.Domain.Entities.Orientation.Vertical)
                throw new InvalidOperationException("Invalid mullion");

            PItemRemove.RemoveShiftRightMullion(mullion);
        }
        private void shiftLeftMnuItem_Click(object sender, EventArgs e)
        {
            Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
            if (mullion == null || mullion.Model.Orientation != UrcieSln.Domain.Entities.Orientation.Vertical)
                throw new InvalidOperationException("Invalid mullion");

            PItemRemove.RemoveShiftLeftMullion(mullion);
        }
        private void shiftUpMnuItem_Click(object sender, EventArgs e)
        {
            Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
            if (mullion == null || mullion.Model.Orientation != UrcieSln.Domain.Entities.Orientation.Horizontal)
                throw new InvalidOperationException("Invalid mullion");

            PItemRemove.RemoveShiftLeftMullion(mullion);
        }
        private void shiftDownMnuItem_Click(object sender, EventArgs e)
        {
            Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
            if (mullion == null || mullion.Model.Orientation != UrcieSln.Domain.Entities.Orientation.Horizontal)
                throw new InvalidOperationException("Invalid mullion");

            PItemRemove.RemoveShiftRightMullion(mullion);
        }
        private void SelectedMullionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MullionType mullionType = SelectedMullionType.SelectedItem as MullionType;
            Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
            if (mullion == null) return;
            if (!mullion.Model.MullionType.Equals(MullionType))
            {
                mullion.Model.MullionType = mullionType;
                mullion.Repaint();
            }
        }
        private void SelectedSashDirectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!SashSelected) return;
            if (SelectedSashDirectionComboBox.SelectedValue == null) return;
            Sash sash = SelectionEventHandler.SelectedNode as Sash;

            if (sash != null && sash.Direction != SelectedSashDirectionComboBox.SelectedValue.ToString())
            {
                sash.Direction = SelectedSashDirectionComboBox.SelectedValue.ToString();
                sash.Repaint();
            }
        }
        private void SelectedSashType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!SashSelected) return;
            SashType sashType = SelectedSashType.SelectedItem as SashType;
            Sash sash = SelectionEventHandler.SelectedNode as Sash;
            if (sash == null) return;
            if (!sash.Model.SashType.Equals(sashType))
            {
                sash.Model.SashType = sashType;
                sash.Repaint();
            }
        }
        private void SelectedFillingType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!FillingSelected) return;
            FillingType fillingType = SelectedFillingType.SelectedItem as FillingType;
            Filling filling = SelectionEventHandler.SelectedNode as Filling;
            if (filling == null) return;
            if (!filling.Model.FillingType.Equals(fillingType))
            {
                filling.Model.FillingType = fillingType;
                filling.Repaint();
            }
        }
        private void SelectedIProfileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!FillingSelected) return;
            ProfileType profileType = SelectedIProfileType.SelectedItem as ProfileType;
            Filling filling = SelectionEventHandler.SelectedNode as Filling;
            if (filling == null) return;
            if (SelectedIncludeIProfileTypeCheckBox.IsChecked.Value)
                filling.Model.ProfileType = SelectedIProfileType.SelectedItem as ProfileType;
            else
                filling.Model.ProfileType = null;
        }
        private void SelectedIncludeIProfileTypeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (!FillingSelected) return;
            ProfileType profileType = SelectedIProfileType.SelectedItem as ProfileType;
            Filling filling = SelectionEventHandler.SelectedNode as Filling;
            if (filling == null) return;
            if (SelectedIncludeIProfileTypeCheckBox.IsChecked.Value)
                filling.Model.ProfileType = SelectedIProfileType.SelectedItem as ProfileType;
            else
                filling.Model.ProfileType = null;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Frame == null) return;
            var cb = sender as ComboBox;
            float value = (float)cb.SelectedItem;
            Frame.FontSize = value;
            Frame.OnDimensionChanged();
        }
        private void ShowCodesCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (Frame != null)
            {
                Frame.ShowCodes = (bool)ShowCodesCheckBox.IsChecked;
            }
        }

        #endregion

        #region Collections

        private ObservableCollection<ProfileType> profileTypes;
        public ObservableCollection<ProfileType> ProfileTypes
        {
            get { return profileTypes; }
            set
            {
                if (profileTypes != value)
                {
                    profileTypes = value;
                    OnPropertyChanged("ProfileTypes");
                }
            }
        }

        private ObservableCollection<MullionType> mullionTypes;
        public ObservableCollection<MullionType> MullionTypes
        {
            get { return mullionTypes; }
            set
            {
                if (mullionTypes != value)
                {
                    mullionTypes = value;
                    OnPropertyChanged("MullionTypes");
                }
            }
        }

        private ObservableCollection<FillingType> fillingTypes;
        public ObservableCollection<FillingType> FillingTypes
        {
            get { return fillingTypes; }
            set
            {
                if (fillingTypes != value)
                {
                    fillingTypes = value;
                    OnPropertyChanged("FillingTypes");
                }
            }
        }

        private ObservableCollection<SashType> sashTypes;
        public ObservableCollection<SashType> SashTypes
        {
            get { return sashTypes; }
            set
            {
                if (sashTypes != value)
                {
                    sashTypes = value;
                    OnPropertyChanged("SashTypes");
                }
            }
        }

        #endregion

        #region Custom Event Handlers

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionEventHandler != null)
                SelectionEventHandler.SelectedNode = null;
            ProfileTypes = new ObservableCollection<ProfileType>(Storage.GetAll<ProfileType>());
            MullionTypes = new ObservableCollection<MullionType>(Storage.GetAll<MullionType>());
            SashTypes = new ObservableCollection<SashType>(Storage.GetAll<SashType>());
            FillingTypes = new ObservableCollection<FillingType>(Storage.GetAll<FillingType>());

            LProfileComboBox.SelectedItem = Frame.Model.ProfileType;
            UProfileComboBox.SelectedItem = Frame.Model.UProfileType;
        }

        private void SashFixedCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Sash sash = SelectionEventHandler.SelectedNode as Sash;
            if (sash == null) return;
            sash.Fixed = SashFixedCheckBox.IsChecked.Value;
            sash.Repaint();
        }

        private void VirtualMullion_Click(object sender, RoutedEventArgs e)
        {
            Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
            if (mullion == null) return;
            mullion.Model.IsVirtual = VirtualMullion.IsChecked.Value;
            Frame.Repaint();
        }

        private void FitToScreen_Click(object sender, RoutedEventArgs e)
        {
            if (Frame == null || Canvas == null) return;
            if (FitToScreen.IsChecked.Value)
            {
                float rate = 1;
                RectangleF union = Canvas.Layer.UnionOfChildrenBounds;
                if (Frame.Width > Frame.Height)
                {

                    rate = Canvas.Camera.ViewBounds.Width / (union.Width + 50);
                }
                else
                {
                    rate = Canvas.Camera.ViewBounds.Height / (union.Height + 50);
                }
                Canvas.Camera.ViewScale = rate;
            }
            else
            {
                Canvas.Camera.ViewScale = 1;
            }
            Canvas.Camera.Repaint();
        }

        private void CenterScreen_Click(object sender, RoutedEventArgs e)
        {
            if (Frame == null || Canvas == null) return;

            Canvas.Camera.ViewBounds = new RectangleF(0, 0, Canvas.Camera.ViewBounds.Width, Canvas.Camera.ViewBounds.Height);
            Canvas.Camera.Repaint();
        }

        #endregion

        public float MullionMiddlePoint
        {
            get
            {
                if (!SelectToolActive) return 0;
                Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
                if (mullion == null) return 0;
                return mullion.Model.MiddlePoint;
            }
            set
            {
                if (!SelectToolActive) return;
                Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
                if (mullion == null) return;
                if (mullion.Model.MiddlePoint != value)
                {
                    mullion.Model.MiddlePoint = value;
                    OnPropertyChanged("MullionMiddlePoint");
                }
            }
        }
        private void DeacreaseMiddlePoint_Click(object sender, RoutedEventArgs e)
        {
            if (!SelectToolActive) return;
            Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
            if (mullion == null) return;
            MullionMiddlePoint--;
        }
        private void IncreaseMiddlePoint_Click(object sender, RoutedEventArgs e)
        {
            if (!SelectToolActive) return;
            Mullion mullion = SelectionEventHandler.SelectedNode as Mullion;
            if (mullion == null) return;
            MullionMiddlePoint++;
        }
        private void MiddlePointTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                float val = 0;
                if (float.TryParse(MiddlePointTextBox.Text, out val))
                {
                    MullionMiddlePoint = val;
                }
                else
                {
                    MiddlePointTextBox.Text = MullionMiddlePoint.ToString();
                }

            }
        }

        private void editAccessories_Click(object sender, RoutedEventArgs e)
        {
            SelectionWizardViewModel selectionWizardViewModel = new SelectionWizardViewModel();
            selectionWizardViewModel.AllItems = new ObservableCollection<AccessoryType>(Storage.GetAll<AccessoryType>());
            selectionWizardViewModel.SelectedItems = new ObservableCollection<AccessoryViewModel>(ViewModel.Accessories.ToList());
            SelectionWizardView selectionWizard = new SelectionWizardView(selectionWizardViewModel);
            bool? result = selectionWizard.ShowDialog();
            if (result.Value)
            {
                ViewModel.Accessories = new ObservableCollection<AccessoryViewModel>(selectionWizardViewModel.SelectedItems.ToList());
            }
        }
    }
}
