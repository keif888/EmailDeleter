using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Identity.Client;
using MimeKit;
using System.Security.Cryptography;

namespace EmailDeleter
{
    public partial class MainForm : Form
    {
        private ListViewColumnSorter lvwColumnSorter;

        private string emailAddress = string.Empty;
        private string password = string.Empty;
        private string IMAPServer = string.Empty;
        private bool UseSSL = true;
        private int IMAPPort = 993;
        private string LogonUsing = string.Empty;
        private static string username = string.Empty;
        private static IntPtr hWnd;
        private static string folderPath = "INBOX";
        private static IMailFolder? folder;
        private static ImapClient client = new ImapClient();
        private static bool connected = false;
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();

        /// <summary>
        /// Method to initialise the form.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            hWnd = this.Handle;
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvEmails.ListViewItemSorter = lvwColumnSorter;
        }

        /// <summary>
        /// Method to close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Method to show the connection dialog box, and then connect.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectForm connectForm = new ConnectForm();
            if (connectForm.ShowDialog() == DialogResult.OK)
            {
                emailAddress = connectForm.EmailAddress;
                password = connectForm.Password;
                IMAPServer = connectForm.IMAPServer;
                UseSSL = connectForm.UseSSL;
                IMAPPort = connectForm.IMAPPort;
                LogonUsing = connectForm.LogonUsing;
                tsslStatus.Text = IMAPServer;

                client.Connect(IMAPServer, IMAPPort, UseSSL);
                username = emailAddress;
                try
                {
                    if (LogonUsing == "Outlook OAUTH")
                    {
                        if (client.AuthenticationMechanisms.Contains("OAUTHBEARER") || client.AuthenticationMechanisms.Contains("XOAUTH2"))
                            await OutlookAuthenticateAsync(client);
                    }
                    else if (LogonUsing == "Google OAUTH")
                    {
                        if (client.AuthenticationMechanisms.Contains("OAUTHBEARER") || client.AuthenticationMechanisms.Contains("XOAUTH2"))
                            await GoogleAuthenticateAsync(client);
                    }
                    else
                    {
                        client.Authenticate(emailAddress, password);
                    }
                }
                catch (Exception ex)
                {
                    connectToolStripMenuItem.Enabled = true;
                    findToolStripMenuItem.Enabled = false;
                    MessageBox.Show(ex.Message, "Exception Encountered");
                    return;
                }

                connectToolStripMenuItem.Enabled = false;
                findToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// Method to display the folder selection dialog and save the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderForm folderForm = new FolderForm();

            foreach (FolderNamespace folderNamespace in client.PersonalNamespaces)
            {
                var folders = client.GetFolders(folderNamespace, false);
                foreach (var folder in folders)
                {
                    folderForm.AddItem(folder.FullName, folder);
                    var subFolders = folder.GetSubfolders(false);
                    foreach (var subFolder in subFolders)
                    {
                        folderForm.AddItem(subFolder.FullName, subFolder);
                    }
                }
            }
            if (folderForm.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderForm.folderPath;
                folder = folderForm.folder;
                searchToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// method to process the search menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // make sure that a folder has been selected
            if (folder != null)
            {
                FilterForm filterForm = new FilterForm();
                if (filterForm.ShowDialog() == DialogResult.OK)
                {
                    lvEmails.BeginUpdate();
                    // Collect and reset the sort order so sort doesn't throw an error.
                    int sortColumn = lvwColumnSorter.SortColumn;
                    System.Windows.Forms.SortOrder sortOrder = lvwColumnSorter.Order;
                    lvwColumnSorter.SortColumn = 0;
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;

                    lvEmails.Items.Clear();

                    folder.Open(FolderAccess.ReadOnly);

                    // Build the search query from the dialog box results
                    SearchQuery? query = null;
                    if (!filterForm.FromDateIsNull)
                    {
                        query = SearchQuery.DeliveredAfter(filterForm.FromDate);
                    }
                    if (!filterForm.ToDateIsNull)
                    {
                        if (query == null) { query = SearchQuery.DeliveredAfter(filterForm.ToDate); }
                        else { query = query.And(SearchQuery.DeliveredBefore(filterForm.ToDate)); }
                    }
                    if (filterForm.BodyContains != string.Empty)
                    {
                        if (query == null) { query = SearchQuery.BodyContains(filterForm.BodyContains); }
                        else { query = query.And(SearchQuery.BodyContains(filterForm.BodyContains)); }
                    }
                    if (filterForm.FromContains != string.Empty)
                    {
                        if (query == null) { query = SearchQuery.FromContains(filterForm.FromContains); }
                        else { query = query.And(SearchQuery.FromContains(filterForm.FromContains)); }
                    }
                    if (filterForm.ToContains != string.Empty)
                    {
                        if (query == null) { query = SearchQuery.ToContains(filterForm.ToContains); }
                        else { query = query.And(SearchQuery.ToContains(filterForm.ToContains)); }
                    }
                    if (filterForm.SubjectContains != string.Empty)
                    {
                        if (query == null) { query = SearchQuery.SubjectContains(filterForm.SubjectContains); }
                        else { query = query.And(SearchQuery.SubjectContains(filterForm.SubjectContains)); }
                    }

                    if (query == null)
                    {
                        // If nothing was selected, then just search for every email
                        query = SearchQuery.All;
                    }

                    IList<MailKit.UniqueId> searchIds = folder.Search(query);

                    // Force the display of the number of messages.
                    tsRecords.Text = String.Format("{0} Records Listed for Deletion", searchIds.Count);
                    statusStrip1.Refresh();

                    // Reset the tokenSource
                    tokenSource.Dispose();
                    tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;

                    // Setup the progress bar function
                    var progress = new Progress<int>(v =>
                    {
                        tsProgress.Value = v;
                    });

                    // Execute the inserts (and catch if cancel is hit)
                    try
                    {
                        cancelToolStripMenuItem.Enabled = true;
                        await Task.Run(() => ExecuteInsert(token, searchIds, progress));
                    }
                    catch (OperationCanceledException)
                    {
                        MessageBox.Show("Search Processing Cancelled!");
                    }

                    if (lvEmails.Items.Count > 0)
                        deleteToolStripMenuItem.Enabled = true;
                    else
                        deleteToolStripMenuItem.Enabled = false; 
                    tsRecords.Text = String.Format("{0} Records Listed for Deletion", lvEmails.Items.Count);
                    // Reset the sort order back again
                    lvwColumnSorter.SortColumn = sortColumn;
                    lvwColumnSorter.Order = sortOrder;
                    lvEmails.Sort();
                    lvEmails.EndUpdate();
                    cancelToolStripMenuItem.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Async authentication to Microsoft OAUTH
        /// </summary>
        /// <param name="client">The Imap client to use to connect with</param>
        /// <returns>The task from the Async authentication attempt</returns>
        private async Task OutlookAuthenticateAsync(ImapClient client)
        {
            var options = new PublicClientApplicationOptions
            {
                ClientId = getProperty(Properties.Settings.Default.AppString4, Properties.Settings.Default.AppString1),
                TenantId = "consumers",
                RedirectUri = "http://localhost"
            };

            var publicClientApplication = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(options)
                .Build();

            var scopes = new string[] {
                "email",
                "offline_access",
                "https://outlook.office.com/IMAP.AccessAsUser.All", // Only needed for IMAP
                //"https://outlook.office.com/POP.AccessAsUser.All",  // Only needed for POP
                //"https://outlook.office.com/SMTP.AccessAsUser.All", // Only needed for SMTP
            };
            AuthenticationResult? result;

            try
            {
                var accounts = await publicClientApplication.GetAccountsAsync();
                // First, check the cache for an auth token.
                result = await publicClientApplication.AcquireTokenSilent(scopes, accounts.FirstOrDefault() /*username*/).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                // If that fails, then try getting an auth token interactively.
                result = await publicClientApplication.AcquireTokenInteractive(scopes).WithLoginHint(username).ExecuteAsync(); // .WithParentActivityOrWindow(hWnd)
            }

            // Note: We use result.Account.Username here instead of ExchangeAccount because the user *may* have chosen a
            // different Microsoft Exchange account when presented with the browser window during the authentication process.
            SaslMechanism oauth2;

            if (client.AuthenticationMechanisms.Contains("OAUTHBEARER"))
                oauth2 = new SaslMechanismOAuthBearer(result.Account.Username, result.AccessToken);
            else
                oauth2 = new SaslMechanismOAuth2(result.Account.Username, result.AccessToken);

            await client.AuthenticateAsync(oauth2);
        }

        /// <summary>
        /// Disconnect from the email client and dispose of the cancellation token
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connected)
                client.Disconnect(true);
            tokenSource.Dispose();
        }

        /// <summary>
        /// Method when the delete menu item is clicked to initiate the deletion of the emails left in the email list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvEmails.Items.Count > 0)
            {
                MessageBoxButtons button = MessageBoxButtons.YesNo;
                string message = String.Format("There are {0} items to delete, are you sure?", lvEmails.Items.Count);
                if (MessageBox.Show(message, "Deletion Starting", button) == DialogResult.Yes)
                {
                    cancelToolStripMenuItem.Enabled = true;
                    List<MailKit.UniqueId> uniqueIds = new List<MailKit.UniqueId>();
                    foreach (ListViewItem item in lvEmails.Items)
                    {
                        if (item.Tag != null)
                            uniqueIds.Add((MailKit.UniqueId)item.Tag);
                    }
                    // Reset the tokenSource
                    tokenSource.Dispose();
                    tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;

                    var progress = new Progress<int>(v =>
                    {
                        tsProgress.Value = v;
                    });

                    try
                    {
                        await Task.Run(() => ExecuteDelete(token, uniqueIds, progress));
                    }
                    catch (OperationCanceledException)
                    {
                        MessageBox.Show("Delete Processing Cancelled!");
                    }

                    tsRecords.Text = String.Format("{0} Records Listed for Deletion", lvEmails.Items.Count);
                    if (lvEmails.Items.Count == 0)
                        deleteToolStripMenuItem.Enabled = false;
                    cancelToolStripMenuItem.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Cancellable async thread to delete items into the email list
        /// </summary>
        /// <param name="ct">Cancellation Token to allow thread to cancel cleanly</param>
        /// <param name="uniqueIds">List of email UniqueId's to be deleted</param>
        /// <param name="progress">The feedback for the progress bar</param>
        private void ExecuteDelete(CancellationToken ct, List<MailKit.UniqueId> uniqueIds, IProgress<int> progress)
        {
            if (folder != null && uniqueIds.Count > 0)
            {
                int itemsToProcess = uniqueIds.Count;
                // Close and reopen the folder in Read Write so we can update and delete messages.
                folder.Close();
                folder.Open(FolderAccess.ReadWrite);
                StoreFlagsRequest request = new(StoreAction.Add, MessageFlags.Deleted) { Silent = true };
                int i = 0, itemsProcessed = 0;
                foreach (MailKit.UniqueId item in uniqueIds)
                {
                    // Mark the email message as deleted
                    folder.Store(item, request);
                    // Update the email list (safely)
                    DeleteEmailListItem(item.ToString());
                    i++;
                    if (i >= 10)
                    {
                        // Tell email server to remove the marked as deleted messages.
                        folder.Expunge();
                        i = 0;
                    }
                    if (progress != null)
                        progress.Report((++itemsProcessed) * 100 / itemsToProcess);
                    if (ct.IsCancellationRequested)
                    {
                        // Close and reopen in ReadOnly, cause it's safer.
                        folder.Close();
                        folder.Open(FolderAccess.ReadOnly);
                        // Cleanly kill off the thread.
                        ct.ThrowIfCancellationRequested();
                    }
                }
                if (i > 0)
                {
                    // Tell email server to remove the marked as deleted messages.
                    folder.Expunge();
                }
                // Close and reopen in ReadOnly, cause it's safer.
                folder.Close();
                folder.Open(FolderAccess.ReadOnly);
            }
            if (progress != null)
                progress.Report(100);
        }

        /// <summary>
        /// Detect the click on the Cancel menu item, and use the Cancellation Token to let the thread know to stop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
            cancelToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Thread safe method to delete items from the email list
        /// </summary>
        /// <param name="itemKey"></param>
        private void DeleteEmailListItem(string itemKey)
        {
            if (lvEmails.InvokeRequired)
            {
                Action safeRemove = delegate { DeleteEmailListItem(itemKey); };
                lvEmails.Invoke(safeRemove);
            }
            else
                lvEmails.Items.RemoveByKey(itemKey);
        }

        /// <summary>
        /// Cancellable async thread to insert items into the email list
        /// </summary>
        /// <param name="ct">CancellationToken to allow this thread to be cancelled</param>
        /// <param name="searchIds">The list of Unique identifiers of messages to list</param>
        /// <param name="progress">The feedback for the progress bar</param>
        private void ExecuteInsert(CancellationToken ct, IList<MailKit.UniqueId> searchIds, IProgress<int> progress)
        {
            if (folder != null && searchIds.Count > 0)
            {
                int itemsToProcess = searchIds.Count;
                int itemsProcessed = 0;
                foreach (var uid in searchIds)
                {
                    var message = folder.GetMessage(uid);
                    InsertEmailListItem(message, uid);
                    if (progress != null)
                        progress.Report((++itemsProcessed) * 100 / itemsToProcess);
                    if (ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                }
            }
            if (progress != null)
                progress.Report(100);
        }

        /// <summary>
        /// Thread Safe - Insert an item into the email list
        /// </summary>
        /// <param name="message">A MimeMessage object that contains the message details to add</param>
        /// <param name="uid">The unique identifier of the message</param>
        private void InsertEmailListItem(MimeMessage message, MailKit.UniqueId uid)
        {
            if (lvEmails.InvokeRequired)
            {
                Action safeRemove = delegate { InsertEmailListItem(message, uid); };
                lvEmails.Invoke(safeRemove);
            }
            else
            {
                ListViewItem lvItem = lvEmails.Items.Add(uid.ToString(), message.Date.ToString("yyyy-MM-dd hh:mm:ss tt"), 0);
                // Tag the message with the unique identifier so the message can be deleted later.
                lvItem.Tag = uid;
                lvItem.SubItems.Add(message.From.ToString());
                lvItem.SubItems.Add(message.To.ToString());
                lvItem.SubItems.Add(message.Subject);
            }
        }

        /// <summary>
        /// Detect a delete key press in the email list, and if so, delete the selected items from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvEmails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ListView.SelectedListViewItemCollection tobedeleted = this.lvEmails.SelectedItems;
                lvEmails.BeginUpdate();
                foreach (ListViewItem item in tobedeleted)
                {
                    lvEmails.Items.Remove(item);
                }
                lvEmails.EndUpdate();
                tsRecords.Text = String.Format("{0} Records Listed for Deletion", lvEmails.Items.Count);
            }
        }

        /// <summary>
        /// Enable changing of the sorting order of the list of emails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvEmails_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvEmails.Sort();
        }

        /// <summary>
        /// Async authentication to Google OAUTH
        /// </summary>
        /// <param name="client">The Imap client to use to connect with</param>
        /// <returns>The task from the Async authentication attempt</returns>
        private async Task GoogleAuthenticateAsync(ImapClient client)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = getProperty(Properties.Settings.Default.AppString3, Properties.Settings.Default.AppString1),
                ClientSecret = getProperty(Properties.Settings.Default.AppString2, Properties.Settings.Default.AppString1)
            };

            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                DataStore = new FileDataStore("CredentialCacheFolder", false),
                Scopes = new[] { "https://mail.google.com/" },
                ClientSecrets = clientSecrets
            });

            // Note: For a web app, you'll want to use AuthorizationCodeWebApp instead.
            var codeReceiver = new LocalServerCodeReceiver();
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

            var credential = await authCode.AuthorizeAsync(username, CancellationToken.None);

            if (credential.Token.IsStale)
                await credential.RefreshTokenAsync(CancellationToken.None);

            // Note: We use credential.UserId here instead of GMailAccount because the user *may* have chosen a
            // different GMail account when presented with the browser window during the authentication process.
            SaslMechanism oauth2;

            if (client.AuthenticationMechanisms.Contains("OAUTHBEARER"))
                oauth2 = new SaslMechanismOAuthBearer(credential.UserId, credential.Token.AccessToken);
            else
                oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

            await client.AuthenticateAsync(oauth2);
        }

        /// <summary>
        /// Retreive a value from a property string
        /// </summary>
        /// <param name="inbound">The Base64 value to retreive</param>
        /// <param name="theotherone">The other Base64 value to combine with it</param>
        /// <returns>A string with the property value</returns>
        private string getProperty(string inbound, string theotherone)
        {
            byte[] value = Convert.FromBase64String(inbound);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(theotherone);
                byte[] iv = new byte[aes.IV.Length];
                int numBytesToRead = aes.IV.Length, encryptLength = value.Length - aes.IV.Length;
                Array.Copy(value, 0, iv, 0, numBytesToRead);
                aes.IV = iv;
                byte[] encryptedBytes = new byte[encryptLength];
                Array.Copy(value, numBytesToRead, encryptedBytes, 0, encryptLength);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return System.Text.Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

    }
}
