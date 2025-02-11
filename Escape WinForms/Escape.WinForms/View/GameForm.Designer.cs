namespace Escape.View
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _menuStrip = new MenuStrip();
            _menuFile = new ToolStripMenuItem();
            _menuNewGame = new ToolStripMenuItem();
            _menuLoadGame = new ToolStripMenuItem();
            _menuSaveGame = new ToolStripMenuItem();
            _menuExitGame = new ToolStripMenuItem();
            _menuSettings = new ToolStripMenuItem();
            _settingsGameEasy = new ToolStripMenuItem();
            _settingsGameNormal = new ToolStripMenuItem();
            _settingsGameHard = new ToolStripMenuItem();
            _pause = new ToolStripMenuItem();
            _statusStrip = new StatusStrip();
            _toolLabel1 = new ToolStripStatusLabel();
            _toolLabelGameTime = new ToolStripStatusLabel();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();
            _menuStrip.SuspendLayout();
            _statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.Items.AddRange(new ToolStripItem[] { _menuFile, _menuSettings, _pause });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new Size(950, 24);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _menuFile
            // 
            _menuFile.DropDownItems.AddRange(new ToolStripItem[] { _menuNewGame, _menuLoadGame, _menuSaveGame, _menuExitGame });
            _menuFile.Name = "_menuFile";
            _menuFile.Size = new Size(37, 20);
            _menuFile.Text = "File";
            // 
            // _menuNewGame
            // 
            _menuNewGame.Name = "_menuNewGame";
            _menuNewGame.Size = new Size(133, 22);
            _menuNewGame.Text = "New game";
            _menuNewGame.Click += MenuNewGame_Click;
            // 
            // _menuLoadGame
            // 
            _menuLoadGame.Name = "_menuLoadGame";
            _menuLoadGame.Size = new Size(133, 22);
            _menuLoadGame.Text = "Load game";
            _menuLoadGame.Click += new System.EventHandler(MenuLoadGame_Click);
            // 
            // _menuSaveGame
            // 
            _menuSaveGame.Name = "_menuSaveGame";
            _menuSaveGame.Size = new Size(133, 22);
            _menuSaveGame.Text = "Save game";
            _menuSaveGame.Click += new System.EventHandler(MenuSaveGame_Click);
            // 
            // _menuExitGame
            // 
            _menuExitGame.Name = "_menuExitGame";
            _menuExitGame.Size = new Size(133, 22);
            _menuExitGame.Text = "Exit";
            _menuExitGame.Click += MenuExit_Click;
            // 
            // _menuSettings
            // 
            _menuSettings.DropDownItems.AddRange(new ToolStripItem[] { _settingsGameEasy, _settingsGameNormal, _settingsGameHard });
            _menuSettings.Name = "_menuSettings";
            _menuSettings.Size = new Size(61, 20);
            _menuSettings.Text = "Settings";
            // 
            // _settingsGameEasy
            // 
            _settingsGameEasy.Name = "_settingsGameEasy";
            _settingsGameEasy.Size = new Size(148, 22);
            _settingsGameEasy.Text = "Easy mode";
            _settingsGameEasy.Click += SettingsGameEasy_Click;
            // 
            // _settingsGameNormal
            // 
            _settingsGameNormal.Name = "_settingsGameNormal";
            _settingsGameNormal.Size = new Size(148, 22);
            _settingsGameNormal.Text = "Normal mode";
            _settingsGameNormal.Click += SettingsGameMedium_Click;
            // 
            // _settingsGameHard
            // 
            _settingsGameHard.Name = "_settingsGameHard";
            _settingsGameHard.Size = new Size(148, 22);
            _settingsGameHard.Text = "Hard mode";
            _settingsGameHard.Click += SettingsGameHard_Click;
            // 
            // _pause
            // 
            _pause.Name = "_pause";
            _pause.Size = new Size(50, 20);
            _pause.Text = "Pause";
            _pause.Click += Pause_Click;
            // 
            // _statusStrip
            // 
            _statusStrip.Items.AddRange(new ToolStripItem[] { _toolLabel1, _toolLabelGameTime });
            _statusStrip.Location = new Point(0, 978);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new Size(950, 22);
            _statusStrip.TabIndex = 1;
            _statusStrip.Text = "statusStrip1";
            // 
            // _toolLabel1
            // 
            _toolLabel1.Name = "_toolLabel1";
            _toolLabel1.Size = new Size(36, 17);
            _toolLabel1.Text = "Time:";
            // 
            // _toolLabelGameTime
            // 
            _toolLabelGameTime.Name = "_toolLabelGameTime";
            _toolLabelGameTime.Size = new Size(43, 17);
            _toolLabelGameTime.Text = "0:00:00";
            // 
            // _openFileDialog
            // 
            _openFileDialog.Filter = "Escape table (*.escp)|*.escp";
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.Filter = "Escape table (*.escp)|*.escp";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 1000);
            Controls.Add(_statusStrip);
            Controls.Add(_menuStrip);
            MainMenuStrip = _menuStrip;
            Name = "GameForm";
            Text = "Escape";
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private MenuStrip _menuStrip;
        private ToolStripMenuItem _menuFile;
        private ToolStripMenuItem _menuNewGame;
        private ToolStripMenuItem _menuLoadGame;
        private ToolStripMenuItem _menuSaveGame;
        private ToolStripMenuItem _menuExitGame;
        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _toolLabel1;
        private ToolStripStatusLabel _toolLabelGameTime;
        private ToolStripMenuItem _menuSettings;
        private ToolStripMenuItem _settingsGameEasy;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
        private ToolStripMenuItem _settingsGameNormal;
        private ToolStripMenuItem _settingsGameHard;
        private ToolStripMenuItem _pause;
    }
}
