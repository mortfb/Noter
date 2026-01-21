namespace MyNotePad;

public partial class MyForm : Form
{
    private RichTextBox _editor;
    private string _currentFile = "";

    public MyForm()
    {
        Text = "My Notepad";
        //These can be altered
        Width = 1000;
        Height = 800;

        _editor = new RichTextBox
        {
            Dock = DockStyle.Fill, Font = new Font("Times New Roman", 11), WordWrap = true
        };

        var menu = new MenuStrip();
        var menuFiles = new ToolStripMenuItem("File");
        FileMenuSetup(menuFiles);
        //add the filetab to the menu
        menu.Items.Add(menuFiles);
        
        //setting menu
        var menuSetting = new ToolStripMenuItem("Settings");
        SettingMenuSetup(menuSetting);
        menu.Items.Add(menuSetting);
        
        //edit menu
        var menuEdit = new ToolStripMenuItem("Edit");
        EditMenuSetup(menuEdit);
        menu.Items.Add(menuEdit);


        MainMenuStrip = menu;
        Controls.Add(_editor);
        Controls.Add(menu);
        //InitializeComponent();
    }
    /// <summary>
    /// Sets up the File menu
    /// </summary>
    /// <param name="menuFiles">the File Menu</param>
    private void FileMenuSetup(ToolStripMenuItem menuFiles)
    {
        var menuNewFile = new ToolStripMenuItem("New File", null, NewFile);
        var menuOpenFile = new ToolStripMenuItem("Open", null, OpenFile);
        var menuSaveFile = new ToolStripMenuItem("Save", null, SaveFile);
        var menuSaveAsItem = new ToolStripMenuItem("Save As", null, SaveAsFile);
        var menuExit = new ToolStripMenuItem("Exit", null, (_, _) => Close());

        //Add the options to the file tab
        menuFiles.DropDownItems.Add(menuNewFile);
        menuFiles.DropDownItems.Add(menuOpenFile);
        menuFiles.DropDownItems.Add(menuSaveFile);
        menuFiles.DropDownItems.Add(menuSaveAsItem);
        menuFiles.DropDownItems.Add(menuExit);
    }
    
    /// <summary>
    /// Sets up the Settings menu
    /// </summary>
    /// <param name="menuSetting">The settings menu</param>
    private void SettingMenuSetup(ToolStripMenuItem menuSetting)
    {   
        //Setting up dark mode
        var menuDarkMode = new ToolStripMenuItem("Dark Mode");
        menuDarkMode.CheckOnClick = true;
        menuDarkMode.CheckedChanged += SetDarkMode;
        
        //changing fonts
        var menuChangeFonts = new ToolStripMenuItem("Change Font");

        menuChangeFonts.Click += ChangeFont;
        menuSetting.DropDownItems.Add(menuChangeFonts);
        menuSetting.DropDownItems.Add(menuDarkMode);

    }

    private void EditMenuSetup(ToolStripMenuItem menuEdit)
    {
        var menuUndo = new ToolStripMenuItem("Undo", null, (_, _) => _editor.Undo());
        var menuRedo = new ToolStripMenuItem("Redo", null, (_, _) => _editor.Redo());
        menuUndo.ForeColor = Color.Red;
        menuRedo.ForeColor = Color.Red;
        menuEdit.DropDownItems.Add(menuRedo);
        menuEdit.DropDownItems.Add(menuUndo);
        


        _editor.TextChanged += (_, _) =>
        {
            menuUndo.Enabled = _editor.CanUndo;
            if (!_editor.CanUndo)
            {
                menuUndo.ForeColor = Color.Red;
            }
            else
            {
                menuUndo.ForeColor = Color.Black;
            }

            menuRedo.Enabled = _editor.CanRedo;
            if (!_editor.CanRedo)
            {
                menuRedo.ForeColor = Color.Red;
            }
            else
            {
                menuRedo.ForeColor = Color.Black;
            }

        };
    }
    

    /// <summary>
    /// Creates a new File
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="ea">event handler</param>
    private void NewFile(object sender, EventArgs ea)
    {
        _editor.Clear(); //removes all text
        _currentFile = ""; //reset current file
    }

    /// <summary>
    /// Open a File.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ea"></param>
    private void OpenFile(object sender, EventArgs ea)
    {
        OpenFileDialog ofd = new OpenFileDialog { Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*" };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            _currentFile = ofd.FileName;
            _editor.Text = File.ReadAllText(_currentFile);
        }
        else
        {
            Console.WriteLine("Open File Dialog failed"); // this is just for me :)
        }
    }

    /// <summary>
    /// Saves a file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ea"></param>
    private void SaveFile(object sender, EventArgs ea)
    {
        if (string.IsNullOrEmpty(_currentFile))
        {
            SaveAsFile(sender, ea);
        }
        else
        {
            File.WriteAllText(_currentFile, _editor.Text); //Overwrites what is already in the file
        }
    }
    
    /// <summary>
    /// Saves a file in a certain format.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ea"></param>
    private void SaveAsFile(object sender, EventArgs ea)
    {
        SaveFileDialog sfd = new SaveFileDialog()
        {
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        };

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            _currentFile = sfd.FileName;
            File.WriteAllText(_currentFile, _editor.Text);
        }
        else
        {
            Console.WriteLine("Saving File Dialog failed");
        }

    }
    /// <summary>
    /// Toggle darkmode
    /// </summary>
    /// <param name="sender">The checkbox</param>
    /// <param name="ea"></param>
    private void SetDarkMode(object? sender, EventArgs ea)
    {
        var dm = sender as ToolStripMenuItem;
        if (dm != null && dm.Checked) //checks if the box in the dark mode item is checked
        {
            _editor.ForeColor = Color.White;
            _editor.BackColor = Color.FromArgb(30, 30, 50);

            MainMenuStrip.BackColor = Color.FromArgb(45, 45, 45);
            MainMenuStrip.ForeColor = Color.White;
            
            BackColor = Color.FromArgb(30, 30, 50);
        }
        else //if it is already in darkmode
        {
            _editor.BackColor = Color.White;
            _editor.ForeColor = Color.Black;
            
            MainMenuStrip.BackColor = Color.LightGray;
            MainMenuStrip.ForeColor = Color.Black;

            BackColor = Color.LightGray;
            
        }
    }

    private void ChangeFont(object? sender, EventArgs ea)
    {
        FontDialog fd = new FontDialog();
        fd.Font = _editor.Font;

        if (fd.ShowDialog() == DialogResult.OK)
        {
            _editor.Font = fd.Font;
        }
    }
}