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
        
        //add the filetab to the menu
        menu.Items.Add(menuFiles);
        
        Controls.Add(_editor);
        Controls.Add(menu);
        //InitializeComponent();
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

    private void OpenFile()
    {
        OpenFileDialog ofd = new OpenFileDialog {Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"};

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
    
    
}