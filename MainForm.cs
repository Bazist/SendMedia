using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMedia
{
    public partial class MainForm : Form
    {
        private KeyboardWatcher _kw = null;
        private GiphySearch _gs = new GiphySearch();

        public MainForm()
        {
            InitializeComponent();

            _kw = new KeyboardWatcher(new List<string> { "Notepad" },
                                      new List<string> { this.Text });

            //_kw.ActivatedWindow += _kw_Activated;
            //_kw.DeactivatedWindow += _kw_Deactivated;

            _kw.ActivatedKeyword += OnActivatedKeyword;
            _gs.DownloadFileCompleted += AddToGrid;
        }

        private string _keyword;
        private string _word;

        private void OnActivatedKeyword(string keyword, string word)
        {
            this.Invoke(new Action(() =>
            {
                _keyword = keyword;
                _word = word;

                this.Top = Cursor.Position.Y + 5;
                this.Left = Cursor.Position.X + 5;

                this.Visible = true;

                gridImages.Controls.Clear();
                
                _gs.Search(word);
            }));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Rectangle resolution = Screen.PrimaryScreen.Bounds;

            //this.Top = resolution.Height - this.Height;
            //this.Left = resolution.Width - this.Width;

            //ThreadPool.QueueUserWorkItem(x =>
            //                            {
            //                                Thread.Sleep(100);
            //                                this.Invoke(new Action(() => { this.Visible = false; }));
            //                            });
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            HideWindow();
        }

        

        private void AddToGrid(string fileName, string url)
        {
            PictureBox picBox = new PictureBox();
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox.Height = 200;
            picBox.Width = 200;
            picBox.WaitOnLoad = false;
            picBox.LoadAsync(fileName);
            picBox.Name = url;
            picBox.Cursor = Cursors.Hand;
            picBox.MouseClick += new MouseEventHandler(
                (object picSender, MouseEventArgs evntArgs) => {
                    PictureBox pic = (PictureBox)picSender;

                    HideWindow();

                    ClipboardHelper.ClearText(_keyword.Length + _word.Length + 1);

                    ClipboardHelper.PasteText(pic.Name);

                    SendKeys.Send("{Enter}");

                    System.Diagnostics.Debug.WriteLine("Click => " + pic.Name);
                }
            );

            gridImages.Controls.Add(picBox);
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            HideWindow();
        }

        private void MainForm_Leave(object sender, EventArgs e)
        {
            HideWindow();
        }

        private void HideWindow()
        {
            this.Visible = false;

            _kw.IsActivatedWindow = false;
        }
    }
}
