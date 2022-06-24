using Abraham.Compression;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ZipArchiveTest
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Form { Text = "ZipArchiveTest", StartPosition = FormStartPosition.WindowsDefaultBounds };
            var listbox = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false };
            listbox.MouseDoubleClick += (object p, MouseEventArgs e) =>
            {
                var path = listbox.SelectedItem as string; if (path == null) return;
                try
                {
                    using (var zip = ZipArchive.OpenOnFile((string)listbox.Tag))
                    {
                        var file = zip.GetFile(path);
                        if (file.FolderFlag) return;
                        var text = new StreamReader(file.GetStream()).ReadToEnd();

                        var dlg = new Form { Text = Path.Combine((string)listbox.Tag, path), StartPosition = FormStartPosition.WindowsDefaultBounds };
                        var textbox = new TextBox { Text = text, MaxLength = 0, Multiline = true, ScrollBars = System.Windows.Forms.ScrollBars.Vertical, Dock = DockStyle.Fill };
                        textbox.Select(0, 0); dlg.Controls.Add(textbox);
                        dlg.ShowDialog();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
            };

            form.Controls.Add(listbox);
            form.Controls.Add(new ToolStrip(new ToolStripMenuItem("Open Zip Archive...", null, (object p, EventArgs e) =>
            {
                try
                {
                    var dlg = new OpenFileDialog { DefaultExt = "zip", Filter = "Zip Archives (*.zip)|*.zip" };
                    if (dlg.ShowDialog() != DialogResult.OK) return;

                    listbox.Items.Clear(); listbox.Tag = dlg.FileName;
                    using (var zip = ZipArchive.OpenOnFile(dlg.FileName))
                        listbox.Items.AddRange(zip.Files.OrderBy(file => file.Name).Select(file => file.Name).ToArray());
                    form.Text = dlg.FileName + " - " + "ZipArchiveTest";
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
            })));
            Application.Run(form);
        }
    }


}
