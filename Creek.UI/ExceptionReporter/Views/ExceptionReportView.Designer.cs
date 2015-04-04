namespace Creek.UI.ExceptionReporter.Views
{
	public partial class ExceptionReportView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionReportView));
           listviewAssemblies = new System.Windows.Forms.ListView();
           tabControl = new System.Windows.Forms.TabControl();
           tabGeneral = new System.Windows.Forms.TabPage();
           picGeneral = new System.Windows.Forms.PictureBox();
           txtExceptionMessage = new System.Windows.Forms.TextBox();
           lblExplanation = new System.Windows.Forms.Label();
           txtUserExplanation = new System.Windows.Forms.TextBox();
           lblRegion = new System.Windows.Forms.Label();
           txtRegion = new System.Windows.Forms.TextBox();
           lblDate = new System.Windows.Forms.Label();
           txtDate = new System.Windows.Forms.TextBox();
           lblTime = new System.Windows.Forms.Label();
           txtTime = new System.Windows.Forms.TextBox();
           lblApplication = new System.Windows.Forms.Label();
           txtApplicationName = new System.Windows.Forms.TextBox();
           lblVersion = new System.Windows.Forms.Label();
           txtVersion = new System.Windows.Forms.TextBox();
           tabExceptions = new System.Windows.Forms.TabPage();
           tabAssemblies = new System.Windows.Forms.TabPage();
           tabConfig = new System.Windows.Forms.TabPage();
           webBrowserConfig = new System.Windows.Forms.WebBrowser();
           tabSysInfo = new System.Windows.Forms.TabPage();
           lblMachine = new System.Windows.Forms.Label();
           txtMachine = new System.Windows.Forms.TextBox();
           lblUsername = new System.Windows.Forms.Label();
           txtUserName = new System.Windows.Forms.TextBox();
           treeEnvironment = new System.Windows.Forms.TreeView();
           tabContact = new System.Windows.Forms.TabPage();
           lblContactMessageTop = new System.Windows.Forms.Label();
           txtFax = new System.Windows.Forms.TextBox();
           lblFax = new System.Windows.Forms.Label();
           txtPhone = new System.Windows.Forms.TextBox();
           lblPhone = new System.Windows.Forms.Label();
           lblWebSite = new System.Windows.Forms.Label();
           urlWeb = new System.Windows.Forms.LinkLabel();
           lblEmail = new System.Windows.Forms.Label();
           urlEmail = new System.Windows.Forms.LinkLabel();
           btnSave = new System.Windows.Forms.Button();
           progressBar = new System.Windows.Forms.ProgressBar();
           btnEmail = new System.Windows.Forms.Button();
           lblProgressMessage = new System.Windows.Forms.Label();
           btnCopy = new System.Windows.Forms.Button();
           btnDetailToggle = new System.Windows.Forms.Button();
           txtExceptionMessageLarge = new System.Windows.Forms.TextBox();
           btnClose = new System.Windows.Forms.Button();
           lessDetailPanel = new System.Windows.Forms.Panel();
           lessDetail_optionsPanel = new System.Windows.Forms.Panel();
           lblContactCompany = new System.Windows.Forms.Label();
           btnSimpleEmail = new System.Windows.Forms.Button();
           btnSimpleDetailToggle = new System.Windows.Forms.Button();
           btnSimpleCopy = new System.Windows.Forms.Button();
           txtExceptionMessageLarge2 = new System.Windows.Forms.TextBox();
           lessDetail_alertIcon = new System.Windows.Forms.PictureBox();
           label1 = new System.Windows.Forms.Label();
           tabControl.SuspendLayout();
           tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGeneral)).BeginInit();
           tabAssemblies.SuspendLayout();
           tabConfig.SuspendLayout();
           tabSysInfo.SuspendLayout();
           tabContact.SuspendLayout();
           lessDetailPanel.SuspendLayout();
           lessDetail_optionsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lessDetail_alertIcon)).BeginInit();
           SuspendLayout();
            // 
            // listviewAssemblies
            // 
           listviewAssemblies.Activation = System.Windows.Forms.ItemActivation.OneClick;
           listviewAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
           listviewAssemblies.FullRowSelect = true;
           listviewAssemblies.HotTracking = true;
           listviewAssemblies.HoverSelection = true;
           listviewAssemblies.Location = new System.Drawing.Point(0, 0);
           listviewAssemblies.Name = "listviewAssemblies";
           listviewAssemblies.Size = new System.Drawing.Size(364, 131);
           listviewAssemblies.TabIndex = 21;
           listviewAssemblies.UseCompatibleStateImageBehavior = false;
           listviewAssemblies.View = System.Windows.Forms.View.Details;
            // 
            // tabControl
            // 
           tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           tabControl.Controls.Add(this.tabGeneral);
           tabControl.Controls.Add(this.tabExceptions);
           tabControl.Controls.Add(this.tabAssemblies);
           tabControl.Controls.Add(this.tabConfig);
           tabControl.Controls.Add(this.tabSysInfo);
           tabControl.Controls.Add(this.tabContact);
           tabControl.HotTrack = true;
           tabControl.Location = new System.Drawing.Point(6, 6);
           tabControl.MinimumSize = new System.Drawing.Size(200, 0);
           tabControl.Multiline = true;
           tabControl.Name = "tabControl";
           tabControl.SelectedIndex = 0;
           tabControl.ShowToolTips = true;
           tabControl.Size = new System.Drawing.Size(372, 157);
           tabControl.TabIndex = 6;
            // 
            // tabGeneral
            // 
           tabGeneral.Controls.Add(this.picGeneral);
           tabGeneral.Controls.Add(this.txtExceptionMessage);
           tabGeneral.Controls.Add(this.lblExplanation);
           tabGeneral.Controls.Add(this.txtUserExplanation);
           tabGeneral.Controls.Add(this.lblRegion);
           tabGeneral.Controls.Add(this.txtRegion);
           tabGeneral.Controls.Add(this.lblDate);
           tabGeneral.Controls.Add(this.txtDate);
           tabGeneral.Controls.Add(this.lblTime);
           tabGeneral.Controls.Add(this.txtTime);
           tabGeneral.Controls.Add(this.lblApplication);
           tabGeneral.Controls.Add(this.txtApplicationName);
           tabGeneral.Controls.Add(this.lblVersion);
           tabGeneral.Controls.Add(this.txtVersion);
           tabGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           tabGeneral.Location = new System.Drawing.Point(4, 22);
           tabGeneral.Name = "tabGeneral";
           tabGeneral.Size = new System.Drawing.Size(364, 131);
           tabGeneral.TabIndex = 0;
           tabGeneral.Text = "General";
           tabGeneral.UseVisualStyleBackColor = true;
            // 
            // picGeneral
            // 
           picGeneral.Image = ((System.Drawing.Image)(resources.GetObject("picGeneral.Image")));
           picGeneral.Location = new System.Drawing.Point(8, 7);
           picGeneral.Name = "picGeneral";
           picGeneral.Size = new System.Drawing.Size(64, 64);
           picGeneral.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
           picGeneral.TabIndex = 25;
           picGeneral.TabStop = false;
            // 
            // txtExceptionMessage
            // 
           txtExceptionMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           txtExceptionMessage.BackColor = System.Drawing.Color.White;
           txtExceptionMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           txtExceptionMessage.Location = new System.Drawing.Point(78, 7);
           txtExceptionMessage.Multiline = true;
           txtExceptionMessage.Name = "txtExceptionMessage";
           txtExceptionMessage.ReadOnly = true;
           txtExceptionMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
           txtExceptionMessage.Size = new System.Drawing.Size(271, 68);
           txtExceptionMessage.TabIndex = 0;
           txtExceptionMessage.Text = "No message";
            // 
            // lblExplanation
            // 
           lblExplanation.AutoSize = true;
           lblExplanation.Location = new System.Drawing.Point(6, 191);
           lblExplanation.Name = "lblExplanation";
           lblExplanation.Size = new System.Drawing.Size(334, 13);
           lblExplanation.TabIndex = 14;
           lblExplanation.Text = "Please enter a brief explanation of events leading up to this exception";
            // 
            // txtUserExplanation
            // 
           txtUserExplanation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           txtUserExplanation.BackColor = System.Drawing.Color.Cornsilk;
           txtUserExplanation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           txtUserExplanation.Location = new System.Drawing.Point(8, 210);
           txtUserExplanation.Multiline = true;
           txtUserExplanation.Name = "txtUserExplanation";
           txtUserExplanation.Size = new System.Drawing.Size(341, 0);
           txtUserExplanation.TabIndex = 6;
            // 
            // lblRegion
            // 
           lblRegion.AutoSize = true;
           lblRegion.Location = new System.Drawing.Point(254, 127);
           lblRegion.Name = "lblRegion";
           lblRegion.Size = new System.Drawing.Size(41, 13);
           lblRegion.TabIndex = 7;
           lblRegion.Text = "Region";
            // 
            // txtRegion
            // 
           txtRegion.BackColor = System.Drawing.Color.Snow;
           txtRegion.Location = new System.Drawing.Point(310, 124);
           txtRegion.Name = "txtRegion";
           txtRegion.ReadOnly = true;
           txtRegion.Size = new System.Drawing.Size(141, 20);
           txtRegion.TabIndex = 3;
            // 
            // lblDate
            // 
           lblDate.AutoSize = true;
           lblDate.Location = new System.Drawing.Point(14, 159);
           lblDate.Name = "lblDate";
           lblDate.Size = new System.Drawing.Size(30, 13);
           lblDate.TabIndex = 9;
           lblDate.Text = "Date";
            // 
            // txtDate
            // 
           txtDate.BackColor = System.Drawing.Color.Snow;
           txtDate.Location = new System.Drawing.Point(78, 156);
           txtDate.Name = "txtDate";
           txtDate.ReadOnly = true;
           txtDate.Size = new System.Drawing.Size(152, 20);
           txtDate.TabIndex = 4;
            // 
            // lblTime
            // 
           lblTime.AutoSize = true;
           lblTime.Location = new System.Drawing.Point(254, 159);
           lblTime.Name = "lblTime";
           lblTime.Size = new System.Drawing.Size(30, 13);
           lblTime.TabIndex = 11;
           lblTime.Text = "Time";
            // 
            // txtTime
            // 
           txtTime.BackColor = System.Drawing.Color.Snow;
           txtTime.Location = new System.Drawing.Point(310, 156);
           txtTime.Name = "txtTime";
           txtTime.ReadOnly = true;
           txtTime.Size = new System.Drawing.Size(141, 20);
           txtTime.TabIndex = 5;
            // 
            // lblApplication
            // 
           lblApplication.AutoSize = true;
           lblApplication.Location = new System.Drawing.Point(14, 94);
           lblApplication.Name = "lblApplication";
           lblApplication.Size = new System.Drawing.Size(59, 13);
           lblApplication.TabIndex = 3;
           lblApplication.Text = "Application";
            // 
            // txtApplicationName
            // 
           txtApplicationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           txtApplicationName.BackColor = System.Drawing.Color.Snow;
           txtApplicationName.Location = new System.Drawing.Point(78, 92);
           txtApplicationName.Name = "txtApplicationName";
           txtApplicationName.ReadOnly = true;
           txtApplicationName.Size = new System.Drawing.Size(271, 20);
           txtApplicationName.TabIndex = 1;
            // 
            // lblVersion
            // 
           lblVersion.Location = new System.Drawing.Point(14, 127);
           lblVersion.Name = "lblVersion";
           lblVersion.Size = new System.Drawing.Size(48, 16);
           lblVersion.TabIndex = 5;
           lblVersion.Text = "Version";
            // 
            // txtVersion
            // 
           txtVersion.BackColor = System.Drawing.Color.Snow;
           txtVersion.Location = new System.Drawing.Point(78, 124);
           txtVersion.Name = "txtVersion";
           txtVersion.ReadOnly = true;
           txtVersion.Size = new System.Drawing.Size(152, 20);
           txtVersion.TabIndex = 2;
            // 
            // tabExceptions
            // 
           tabExceptions.Location = new System.Drawing.Point(4, 22);
           tabExceptions.Name = "tabExceptions";
           tabExceptions.Size = new System.Drawing.Size(364, 131);
           tabExceptions.TabIndex = 1;
           tabExceptions.Text = "Exceptions";
           tabExceptions.UseVisualStyleBackColor = true;
            // 
            // tabAssemblies
            // 
           tabAssemblies.Controls.Add(this.listviewAssemblies);
           tabAssemblies.Location = new System.Drawing.Point(4, 22);
           tabAssemblies.Name = "tabAssemblies";
           tabAssemblies.Size = new System.Drawing.Size(364, 131);
           tabAssemblies.TabIndex = 6;
           tabAssemblies.Text = "Assemblies";
           tabAssemblies.UseVisualStyleBackColor = true;
            // 
            // tabConfig
            // 
           tabConfig.Controls.Add(this.webBrowserConfig);
           tabConfig.Location = new System.Drawing.Point(4, 22);
           tabConfig.Name = "tabConfig";
           tabConfig.Size = new System.Drawing.Size(364, 131);
           tabConfig.TabIndex = 5;
           tabConfig.Text = "Configuration";
           tabConfig.UseVisualStyleBackColor = true;
            // 
            // webBrowserConfig
            // 
           webBrowserConfig.AllowNavigation = false;
           webBrowserConfig.AllowWebBrowserDrop = false;
           webBrowserConfig.Dock = System.Windows.Forms.DockStyle.Fill;
           webBrowserConfig.IsWebBrowserContextMenuEnabled = false;
           webBrowserConfig.Location = new System.Drawing.Point(0, 0);
           webBrowserConfig.MinimumSize = new System.Drawing.Size(20, 20);
           webBrowserConfig.Name = "webBrowserConfig";
           webBrowserConfig.Size = new System.Drawing.Size(364, 131);
           webBrowserConfig.TabIndex = 21;
           webBrowserConfig.WebBrowserShortcutsEnabled = false;
            // 
            // tabSysInfo
            // 
           tabSysInfo.Controls.Add(this.lblMachine);
           tabSysInfo.Controls.Add(this.txtMachine);
           tabSysInfo.Controls.Add(this.lblUsername);
           tabSysInfo.Controls.Add(this.txtUserName);
           tabSysInfo.Controls.Add(this.treeEnvironment);
           tabSysInfo.Location = new System.Drawing.Point(4, 22);
           tabSysInfo.Name = "tabSysInfo";
           tabSysInfo.Size = new System.Drawing.Size(364, 131);
           tabSysInfo.TabIndex = 3;
           tabSysInfo.Text = "System";
           tabSysInfo.UseVisualStyleBackColor = true;
            // 
            // lblMachine
            // 
           lblMachine.AutoSize = true;
           lblMachine.Location = new System.Drawing.Point(5, 15);
           lblMachine.Name = "lblMachine";
           lblMachine.Size = new System.Drawing.Size(46, 13);
           lblMachine.TabIndex = 16;
           lblMachine.Text = "Machine";
            // 
            // txtMachine
            // 
           txtMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           txtMachine.BackColor = System.Drawing.SystemColors.Control;
           txtMachine.Location = new System.Drawing.Point(59, 12);
           txtMachine.Name = "txtMachine";
           txtMachine.ReadOnly = true;
           txtMachine.Size = new System.Drawing.Size(270, 21);
           txtMachine.TabIndex = 0;
            // 
            // lblUsername
            // 
           lblUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
           lblUsername.AutoSize = true;
           lblUsername.Location = new System.Drawing.Point(351, 15);
           lblUsername.Name = "lblUsername";
           lblUsername.Size = new System.Drawing.Size(55, 13);
           lblUsername.TabIndex = 1;
           lblUsername.Text = "Username";
            // 
            // txtUserName
            // 
           txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
           txtUserName.BackColor = System.Drawing.SystemColors.Control;
           txtUserName.Location = new System.Drawing.Point(412, 14);
           txtUserName.Name = "txtUserName";
           txtUserName.ReadOnly = true;
           txtUserName.Size = new System.Drawing.Size(169, 21);
           txtUserName.TabIndex = 1;
            // 
            // treeEnvironment
            // 
           treeEnvironment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           treeEnvironment.BackColor = System.Drawing.SystemColors.Window;
           treeEnvironment.HotTracking = true;
           treeEnvironment.Location = new System.Drawing.Point(8, 40);
           treeEnvironment.Name = "treeEnvironment";
           treeEnvironment.Size = new System.Drawing.Size(573, 301);
           treeEnvironment.TabIndex = 2;
            // 
            // tabContact
            // 
           tabContact.Controls.Add(this.lblContactMessageTop);
           tabContact.Controls.Add(this.txtFax);
           tabContact.Controls.Add(this.lblFax);
           tabContact.Controls.Add(this.txtPhone);
           tabContact.Controls.Add(this.lblPhone);
           tabContact.Controls.Add(this.lblWebSite);
           tabContact.Controls.Add(this.urlWeb);
           tabContact.Controls.Add(this.lblEmail);
           tabContact.Controls.Add(this.urlEmail);
           tabContact.Location = new System.Drawing.Point(4, 22);
           tabContact.Name = "tabContact";
           tabContact.Size = new System.Drawing.Size(364, 131);
           tabContact.TabIndex = 4;
           tabContact.Text = "Contact";
           tabContact.UseVisualStyleBackColor = true;
            // 
            // lblContactMessageTop
            // 
           lblContactMessageTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           lblContactMessageTop.Location = new System.Drawing.Point(8, 24);
           lblContactMessageTop.MinimumSize = new System.Drawing.Size(200, 0);
           lblContactMessageTop.Name = "lblContactMessageTop";
           lblContactMessageTop.Size = new System.Drawing.Size(533, 24);
           lblContactMessageTop.TabIndex = 27;
           lblContactMessageTop.Text = "The following details can be used to obtain support for this application.";
            // 
            // txtFax
            // 
           txtFax.BackColor = System.Drawing.SystemColors.Control;
           txtFax.Location = new System.Drawing.Point(72, 168);
           txtFax.MinimumSize = new System.Drawing.Size(200, 0);
           txtFax.Name = "txtFax";
           txtFax.ReadOnly = true;
           txtFax.Size = new System.Drawing.Size(249, 21);
           txtFax.TabIndex = 3;
            // 
            // lblFax
            // 
           lblFax.AutoSize = true;
           lblFax.Location = new System.Drawing.Point(18, 168);
           lblFax.Name = "lblFax";
           lblFax.Size = new System.Drawing.Size(25, 13);
           lblFax.TabIndex = 34;
           lblFax.Text = "Fax";
            // 
            // txtPhone
            // 
           txtPhone.Location = new System.Drawing.Point(72, 142);
           txtPhone.MinimumSize = new System.Drawing.Size(200, 0);
           txtPhone.Name = "txtPhone";
           txtPhone.ReadOnly = true;
           txtPhone.Size = new System.Drawing.Size(249, 21);
           txtPhone.TabIndex = 2;
            // 
            // lblPhone
            // 
           lblPhone.AutoSize = true;
           lblPhone.Location = new System.Drawing.Point(16, 144);
           lblPhone.Name = "lblPhone";
           lblPhone.Size = new System.Drawing.Size(37, 13);
           lblPhone.TabIndex = 32;
           lblPhone.Text = "Phone";
            // 
            // lblWebSite
            // 
           lblWebSite.AutoSize = true;
           lblWebSite.Location = new System.Drawing.Point(16, 80);
           lblWebSite.Name = "lblWebSite";
           lblWebSite.Size = new System.Drawing.Size(29, 13);
           lblWebSite.TabIndex = 30;
           lblWebSite.Text = "Web";
            // 
            // urlWeb
            // 
           urlWeb.ActiveLinkColor = System.Drawing.Color.Orange;
           urlWeb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           urlWeb.AutoSize = true;
           urlWeb.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           urlWeb.Location = new System.Drawing.Point(72, 77);
           urlWeb.Margin = new System.Windows.Forms.Padding(5);
           urlWeb.MinimumSize = new System.Drawing.Size(200, 0);
           urlWeb.Name = "urlWeb";
           urlWeb.Size = new System.Drawing.Size(200, 18);
           urlWeb.TabIndex = 1;
           urlWeb.TabStop = true;
           urlWeb.Text = "NA";
            // 
            // lblEmail
            // 
           lblEmail.AutoSize = true;
           lblEmail.Location = new System.Drawing.Point(16, 56);
           lblEmail.Name = "lblEmail";
           lblEmail.Size = new System.Drawing.Size(31, 13);
           lblEmail.TabIndex = 28;
           lblEmail.Text = "Email";
            // 
            // urlEmail
            // 
           urlEmail.ActiveLinkColor = System.Drawing.Color.Orange;
           urlEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           urlEmail.AutoSize = true;
           urlEmail.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           urlEmail.Location = new System.Drawing.Point(72, 53);
           urlEmail.Margin = new System.Windows.Forms.Padding(5);
           urlEmail.MinimumSize = new System.Drawing.Size(200, 0);
           urlEmail.Name = "urlEmail";
           urlEmail.Size = new System.Drawing.Size(200, 18);
           urlEmail.TabIndex = 0;
           urlEmail.TabStop = true;
           urlEmail.Text = "NA";
            // 
            // btnSave
            // 
           btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
           btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
           btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
           btnSave.Location = new System.Drawing.Point(151, 166);
           btnSave.Name = "btnSave";
           btnSave.Size = new System.Drawing.Size(72, 32);
           btnSave.TabIndex = 2;
           btnSave.Text = "Save";
           btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar
            // 
           progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
           progressBar.Location = new System.Drawing.Point(5, 182);
           progressBar.Name = "progressBar";
           progressBar.Size = new System.Drawing.Size(141, 16);
           progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
           progressBar.TabIndex = 53;
            // 
            // btnEmail
            // 
           btnEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
           btnEmail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           btnEmail.Image = ((System.Drawing.Image)(resources.GetObject("btnEmail.Image")));
           btnEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
           btnEmail.Location = new System.Drawing.Point(228, 166);
           btnEmail.Name = "btnEmail";
           btnEmail.Size = new System.Drawing.Size(72, 32);
           btnEmail.TabIndex = 1;
           btnEmail.Text = "Email";
           btnEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProgressMessage
            // 
           lblProgressMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
           lblProgressMessage.AutoSize = true;
           lblProgressMessage.BackColor = System.Drawing.Color.Transparent;
           lblProgressMessage.Location = new System.Drawing.Point(3, 166);
           lblProgressMessage.Name = "lblProgressMessage";
           lblProgressMessage.Size = new System.Drawing.Size(150, 13);
           lblProgressMessage.TabIndex = 52;
           lblProgressMessage.Text = "Loading system information...";
            // 
            // btnCopy
            // 
           btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
           btnCopy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
           btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
           btnCopy.Location = new System.Drawing.Point(74, 166);
           btnCopy.Name = "btnCopy";
           btnCopy.Size = new System.Drawing.Size(72, 32);
           btnCopy.TabIndex = 3;
           btnCopy.Text = "Copy";
           btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDetailToggle
            // 
           btnDetailToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
           btnDetailToggle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           btnDetailToggle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
           btnDetailToggle.Location = new System.Drawing.Point(-3, 166);
           btnDetailToggle.Name = "btnDetailToggle";
           btnDetailToggle.Size = new System.Drawing.Size(72, 32);
           btnDetailToggle.TabIndex = 4;
           btnDetailToggle.Text = "Less Detail";
            // 
            // txtExceptionMessageLarge
            // 
           txtExceptionMessageLarge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           txtExceptionMessageLarge.BackColor = System.Drawing.Color.White;
           txtExceptionMessageLarge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           txtExceptionMessageLarge.Location = new System.Drawing.Point(6, 6);
           txtExceptionMessageLarge.Multiline = true;
           txtExceptionMessageLarge.Name = "txtExceptionMessageLarge";
           txtExceptionMessageLarge.ReadOnly = true;
           txtExceptionMessageLarge.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
           txtExceptionMessageLarge.Size = new System.Drawing.Size(371, 154);
           txtExceptionMessageLarge.TabIndex = 5;
           txtExceptionMessageLarge.Text = "No message";
            // 
            // btnClose
            // 
           btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
           btnClose.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
           btnClose.Location = new System.Drawing.Point(305, 166);
           btnClose.Name = "btnClose";
           btnClose.Size = new System.Drawing.Size(72, 32);
           btnClose.TabIndex = 0;
           btnClose.Text = "Close";
            // 
            // lessDetailPanel
            // 
           lessDetailPanel.BackColor = System.Drawing.Color.White;
           lessDetailPanel.Controls.Add(this.lessDetail_optionsPanel);
           lessDetailPanel.Controls.Add(this.txtExceptionMessageLarge2);
           lessDetailPanel.Controls.Add(this.lessDetail_alertIcon);
           lessDetailPanel.Controls.Add(this.label1);
           lessDetailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
           lessDetailPanel.Location = new System.Drawing.Point(0, 0);
           lessDetailPanel.Name = "lessDetailPanel";
           lessDetailPanel.Size = new System.Drawing.Size(384, 202);
           lessDetailPanel.TabIndex = 54;
            // 
            // lessDetail_optionsPanel
            // 
           lessDetail_optionsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(245)))));
           lessDetail_optionsPanel.Controls.Add(this.lblContactCompany);
           lessDetail_optionsPanel.Controls.Add(this.btnSimpleEmail);
           lessDetail_optionsPanel.Controls.Add(this.btnSimpleDetailToggle);
           lessDetail_optionsPanel.Controls.Add(this.btnSimpleCopy);
           lessDetail_optionsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
           lessDetail_optionsPanel.Location = new System.Drawing.Point(0, 123);
           lessDetail_optionsPanel.Name = "lessDetail_optionsPanel";
           lessDetail_optionsPanel.Padding = new System.Windows.Forms.Padding(8);
           lessDetail_optionsPanel.Size = new System.Drawing.Size(384, 79);
           lessDetail_optionsPanel.TabIndex = 26;
            // 
            // lblContactCompany
            // 
           lblContactCompany.AutoSize = true;
           lblContactCompany.ForeColor = System.Drawing.Color.SlateGray;
           lblContactCompany.Location = new System.Drawing.Point(13, 11);
           lblContactCompany.Name = "lblContactCompany";
           lblContactCompany.Size = new System.Drawing.Size(0, 13);
           lblContactCompany.TabIndex = 3;
            // 
            // btnSimpleEmail
            // 
           btnSimpleEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
           btnSimpleEmail.FlatAppearance.BorderSize = 0;
           btnSimpleEmail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(230)))));
           btnSimpleEmail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
           btnSimpleEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           btnSimpleEmail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           btnSimpleEmail.Image = ((System.Drawing.Image)(resources.GetObject("btnSimpleEmail.Image")));
           btnSimpleEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
           btnSimpleEmail.Location = new System.Drawing.Point(255, 36);
           btnSimpleEmail.Name = "btnSimpleEmail";
           btnSimpleEmail.Size = new System.Drawing.Size(118, 32);
           btnSimpleEmail.TabIndex = 1;
           btnSimpleEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSimpleDetailToggle
            // 
           btnSimpleDetailToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
           btnSimpleDetailToggle.FlatAppearance.BorderSize = 0;
           btnSimpleDetailToggle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(230)))));
           btnSimpleDetailToggle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
           btnSimpleDetailToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           btnSimpleDetailToggle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           btnSimpleDetailToggle.Image = ((System.Drawing.Image)(resources.GetObject("btnSimpleDetailToggle.Image")));
           btnSimpleDetailToggle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
           btnSimpleDetailToggle.Location = new System.Drawing.Point(12, 36);
           btnSimpleDetailToggle.Name = "btnSimpleDetailToggle";
           btnSimpleDetailToggle.Size = new System.Drawing.Size(96, 32);
           btnSimpleDetailToggle.TabIndex = 4;
           btnSimpleDetailToggle.Text = "More detail";
           btnSimpleDetailToggle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSimpleCopy
            // 
           btnSimpleCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
           btnSimpleCopy.FlatAppearance.BorderSize = 0;
           btnSimpleCopy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(230)))));
           btnSimpleCopy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
           btnSimpleCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           btnSimpleCopy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           btnSimpleCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnSimpleCopy.Image")));
           btnSimpleCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
           btnSimpleCopy.Location = new System.Drawing.Point(144, 36);
           btnSimpleCopy.Name = "btnSimpleCopy";
           btnSimpleCopy.Size = new System.Drawing.Size(109, 32);
           btnSimpleCopy.TabIndex = 3;
           btnSimpleCopy.Text = "Copy details";
           btnSimpleCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExceptionMessageLarge2
            // 
           txtExceptionMessageLarge2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
           txtExceptionMessageLarge2.BackColor = System.Drawing.Color.White;
           txtExceptionMessageLarge2.BorderStyle = System.Windows.Forms.BorderStyle.None;
           txtExceptionMessageLarge2.Location = new System.Drawing.Point(86, 62);
           txtExceptionMessageLarge2.Multiline = true;
           txtExceptionMessageLarge2.Name = "txtExceptionMessageLarge2";
           txtExceptionMessageLarge2.ReadOnly = true;
           txtExceptionMessageLarge2.Size = new System.Drawing.Size(283, 55);
           txtExceptionMessageLarge2.TabIndex = 0;
           txtExceptionMessageLarge2.Text = "No message";
            // 
            // lessDetail_alertIcon
            // 
           lessDetail_alertIcon.Image = ((System.Drawing.Image)(resources.GetObject("lessDetail_alertIcon.Image")));
           lessDetail_alertIcon.Location = new System.Drawing.Point(14, 13);
           lessDetail_alertIcon.Name = "lessDetail_alertIcon";
           lessDetail_alertIcon.Size = new System.Drawing.Size(64, 64);
           lessDetail_alertIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
           lessDetail_alertIcon.TabIndex = 25;
           lessDetail_alertIcon.TabStop = false;
            // 
            // label1
            // 
           label1.AutoSize = true;
           label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           label1.Location = new System.Drawing.Point(84, 33);
           label1.Name = "label1";
           label1.Size = new System.Drawing.Size(147, 23);
           label1.TabIndex = 14;
           label1.Text = "Operation Failed";
            // 
            // ExceptionReportView
            // 
           AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
           ClientSize = new System.Drawing.Size(384, 202);
           Controls.Add(this.lessDetailPanel);
           Controls.Add(this.btnClose);
           Controls.Add(this.btnDetailToggle);
           Controls.Add(this.tabControl);
           Controls.Add(this.btnSave);
           Controls.Add(this.progressBar);
           Controls.Add(this.btnEmail);
           Controls.Add(this.lblProgressMessage);
           Controls.Add(this.btnCopy);
           Controls.Add(this.txtExceptionMessageLarge);
           Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
           MaximizeBox = false;
           Name = "ExceptionReportView";
           StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
           tabControl.ResumeLayout(false);
           tabGeneral.ResumeLayout(false);
           tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGeneral)).EndInit();
           tabAssemblies.ResumeLayout(false);
           tabConfig.ResumeLayout(false);
           tabSysInfo.ResumeLayout(false);
           tabSysInfo.PerformLayout();
           tabContact.ResumeLayout(false);
           tabContact.PerformLayout();
           lessDetailPanel.ResumeLayout(false);
           lessDetailPanel.PerformLayout();
           lessDetail_optionsPanel.ResumeLayout(false);
           lessDetail_optionsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lessDetail_alertIcon)).EndInit();
           ResumeLayout(false);
           PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listviewAssemblies;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.TextBox txtUserExplanation;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.TextBox txtRegion;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label lblApplication;
        private System.Windows.Forms.TextBox txtApplicationName;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.TabPage tabExceptions;
        private System.Windows.Forms.TabPage tabAssemblies;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.TabPage tabSysInfo;
        private System.Windows.Forms.Label lblMachine;
        private System.Windows.Forms.TextBox txtMachine;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TreeView treeEnvironment;
        private System.Windows.Forms.TabPage tabContact;
        private System.Windows.Forms.Label lblContactMessageTop;
        private System.Windows.Forms.TextBox txtFax;
        private System.Windows.Forms.Label lblFax;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblWebSite;
        private System.Windows.Forms.LinkLabel urlWeb;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.LinkLabel urlEmail;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.Label lblProgressMessage;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox txtExceptionMessage;
        private System.Windows.Forms.PictureBox picGeneral;
        private System.Windows.Forms.Button btnDetailToggle;
        private System.Windows.Forms.TextBox txtExceptionMessageLarge;
        private System.Windows.Forms.WebBrowser webBrowserConfig;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel lessDetailPanel;
        private System.Windows.Forms.PictureBox lessDetail_alertIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel lessDetail_optionsPanel;
        private System.Windows.Forms.TextBox txtExceptionMessageLarge2;
        private System.Windows.Forms.Label lblContactCompany;
        private System.Windows.Forms.Button btnSimpleEmail;
        private System.Windows.Forms.Button btnSimpleCopy;
        private System.Windows.Forms.Button btnSimpleDetailToggle;
    }
}