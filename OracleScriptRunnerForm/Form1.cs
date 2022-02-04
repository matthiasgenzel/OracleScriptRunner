using OracleScriptRunner.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OracleScriptRunnerForm
{
    public partial class Form1 : Form
    {
        private readonly IDbConnectionStringCollection _connManager = Factory.CreateDbConnectionStringCollection();
        private readonly IAddOnScriptsManager _addOnScriptsManager = Factory.CreateAddOnScriptsManager();

        public Form1()
        {
            InitializeComponent();

            InitConnections();
            InitAddOnScripts();
        }

        private void InitAddOnScripts()
        {
            txtSettingsPreScripts.Text = _addOnScriptsManager.PreScript.Text;
            txtSettingsPostScripts.Text = _addOnScriptsManager.PostScript.Text;
        }

        private void InitConnections()
        {
            _connManager.LoadFromSetting();
            SetConnectionsToSettingsList();
            SetConnectionsToTreeView();
        }

        private void SetConnectionsToTreeView()
        {
            tvConnections.Nodes.Clear();

            TreeNode lastTreeNode = null;
            foreach (var c in _connManager.DbConnectionStrings)
            {
                if (!c.IsValidConnection())
                {
                    // invalid connections are headers
                    lastTreeNode = new TreeNode(c.ToString());
                    tvConnections.Nodes.Add(lastTreeNode);
                }
                else
                {
                    // valid connections contain object in tag
                    var newNode = new TreeNode()
                    {
                        Text = c.ToString(),
                        Tag = c
                    };

                    if (lastTreeNode == null)
                    {
                        tvConnections.Nodes.Add(newNode);
                    }
                    else
                    {
                        lastTreeNode.Nodes.Add(newNode);
                    }
                }
            }
        }

        private void SetConnectionsToSettingsList()
        {
            txtSettingsConnectionStrings.Text = string.Join(Environment.NewLine, _connManager.DbConnectionStrings.Select((c) => c.ConnectionString));
        }

        private void btnAddScript_Click(object sender, EventArgs e)
        {
            try
            {
                AddScripts();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AddScripts()
        {
            var ofd = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    lbFiles.Items.Add(fileName);
                }
            }
        }

        private void btnRemoveScript_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveScripts();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void RemoveScripts()
        {
            if (lbFiles.Items.Count > 0)
                lbFiles.Items.RemoveAt(lbFiles.SelectedIndex);
        }

        private void btnScriptUp_Click(object sender, EventArgs e)
        {
            try
            {
                MoveScriptUp();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void MoveScriptUp()
        {
            if (lbFiles.SelectedIndex > 0)
            {
                // move file one up
                var temp = lbFiles.SelectedItem;
                lbFiles.Items[lbFiles.SelectedIndex] = lbFiles.Items[lbFiles.SelectedIndex - 1];
                lbFiles.Items[lbFiles.SelectedIndex - 1] = temp;

                // select the previous selected item again
                lbFiles.SelectedIndex -= 1;
            }
        }

        private void btnScriptDown_Click(object sender, EventArgs e)
        {
            try
            {
                MoveScriptDown();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void MoveScriptDown()
        {
            if (lbFiles.SelectedIndex < lbFiles.Items.Count - 1)
            {
                // move file one up
                var temp = lbFiles.SelectedItem;
                lbFiles.Items[lbFiles.SelectedIndex] = lbFiles.Items[lbFiles.SelectedIndex + 1];
                lbFiles.Items[lbFiles.SelectedIndex + 1] = temp;

                // select the previous selected item again
                lbFiles.SelectedIndex += 1;
            }
        }

        private void OpenDirInExplorer(string dir)
        {
            Process.Start("explorer", dir);
        }

        private bool CheckDirectoryIsEmptyOrForce(string dir)
        {
            if (Directory.EnumerateFileSystemEntries(dir).Any())
            {
                if (MessageBox.Show("Folder is not empty! Do you want to delete all files in the folder?",
                    "Error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var filePaths = Directory.GetFiles(dir);
                    foreach (var filePath in filePaths)
                        File.Delete(filePath);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void BuildBatchFile(string dir)
        {
            var builder = Factory.CreateTerminExecFileBuilder(
                addOnScriptsManager: _addOnScriptsManager,
                connectionStrings: getSelectedConnections(),
                emptyDirectory: dir,
                sqlFilePaths: lbFiles.Items.Cast<string>());

            builder.BuildExecutionFiles();
        }

        private IEnumerable<IDbConnectionString> getSelectedConnections()
        {
            var connections = new List<IDbConnectionString>();

            void addAllConnectionsInNodes(TreeNodeCollection nodes)
            {
                foreach (TreeNode node in nodes)
                {
                    if (node.Checked && node.Tag != null)
                        connections.Add(node.Tag as IDbConnectionString);

                    if (node.Nodes.Count > 0)
                        addAllConnectionsInNodes(node.Nodes);
                }
            }

            addAllConnectionsInNodes(tvConnections.Nodes);

            return connections;
        }

        private static void HandleException(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettingsChanges();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SaveSettingsChanges()
        {
            _addOnScriptsManager.SetPreScript(txtSettingsPreScripts.Text);
            _addOnScriptsManager.SetPostScript(txtSettingsPostScripts.Text);

            _connManager.LoadFromString(txtSettingsConnectionStrings.Text);
            _connManager.SaveToSetting();
            InitConnections();
        }

        private void btnBuildBatch_Click(object sender, EventArgs e)
        {
            try
            {
                var workDir = SelectEmptyFolder();

                if (!string.IsNullOrEmpty(workDir) && CheckDirectoryIsEmptyOrForce(workDir))
                {
                    BuildBatchFile(workDir);
                    OpenDirInExplorer(workDir);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static string SelectEmptyFolder()
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                return fbd.SelectedPath;

            return null;
        }

        private void tvConnections_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                checkAllNodes(e.Node.Nodes, e.Node.Checked);
            }
        }

        private static void checkAllNodes(TreeNodeCollection nodes, bool isChecked)
        {
            if (nodes.Count > 0)
            {
                foreach (TreeNode node in nodes)
                {
                    node.Checked = isChecked;
                }
            }
        }
    }
}