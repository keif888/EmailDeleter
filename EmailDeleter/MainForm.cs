using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit.Cryptography;
using Microsoft.Identity.Client;

//ToDo: Implement Google Authentication
//using Google.Apis.Util;
//using Google.Apis.Util.Store;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Auth.OAuth2.Flows;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Runtime.CompilerServices;
using System.ComponentModel;

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

        public MainForm()
        {
            InitializeComponent();
            hWnd = this.Handle;
            lvwColumnSorter = new ListViewColumnSorter();
            this.lvEmails.ListViewItemSorter = lvwColumnSorter;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
                    //ToDo: Implement Google Authentication
                    //else if (LogonUsing == "Google OAUTH")
                    //{
                    //    if (client.AuthenticationMechanisms.Contains("OAUTHBEARER") || client.AuthenticationMechanisms.Contains("XOAUTH2"))
                    //        await GoogleAuthenticateAsync(client);
                    //}
                    else
                    {
                        client.Authenticate(emailAddress, password);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception Encountered");
                    return;
                }

                connectToolStripMenuItem.Enabled = false;
                findToolStripMenuItem.Enabled = true;
            }
        }

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


        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folder != null)
            {
                FilterForm filterForm = new FilterForm();
                if (filterForm.ShowDialog() == DialogResult.OK)
                {
                    lvEmails.BeginUpdate();
                    lvEmails.Items.Clear();

                    //var folder = client.Inbox;
                    folder.Open(FolderAccess.ReadOnly);

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
                        query = SearchQuery.All;
                    }

                    IList<UniqueId> searchIds = folder.Search(query);

                    int searchItems = searchIds.Count, searchProgress = 0;

                    foreach (var uid in searchIds)
                    {
                        var message = folder.GetMessage(uid);
                        ListViewItem lvItem = lvEmails.Items.Add(uid.ToString(), message.Date.ToString("yyyy-MM-dd hh:mm:ss tt"), 0);
                        lvItem.Tag = uid;
                        lvItem.SubItems.Add(message.From.ToString());
                        lvItem.SubItems.Add(message.To.ToString());
                        lvItem.SubItems.Add(message.Subject);
                        tsProgress.Value = (++searchProgress * 100) / searchItems;
                    }

                    if (lvEmails.Items.Count > 0)
                    {
                        deleteToolStripMenuItem.Enabled = true;
                    }
                    else
                    { deleteToolStripMenuItem.Enabled = false; }
                    tsRecords.Text = String.Format("{0} Records Listed for Deletion", lvEmails.Items.Count);
                    lvEmails.EndUpdate();
                }
            }
        }

        private async Task OutlookAuthenticateAsync(ImapClient client)
        {
            var options = new PublicClientApplicationOptions
            {
                ClientId = "d773b4a9-2da2-44c3-8c25-95df82284351", //"Application (client) ID",
                TenantId = "consumers", //"68d7521a-ae28-46a5-a2e5-bb6e2b6bcea5", //"Directory (tenant) ID",
                //RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient"
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connected)
                client.Disconnect(true);
            tokenSource.Dispose();
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvEmails.Items.Count > 0)
            {
                MessageBoxButtons button = MessageBoxButtons.YesNo;
                string message = String.Format("There are {0} items to delete, are you sure?", lvEmails.Items.Count);
                if (MessageBox.Show(message, "Deletion Starting", button) == DialogResult.Yes)
                {
                    cancelToolStripMenuItem.Enabled = true;
                    List<UniqueId> uniqueIds = new List<UniqueId>();
                    foreach (ListViewItem item in lvEmails.Items)
                    {
                        if (item.Tag != null)
                            uniqueIds.Add((UniqueId)item.Tag);
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

        private void ExecuteDelete(CancellationToken ct, List<UniqueId> uniqueIds, IProgress<int> progress)
        {
            if (folder != null && uniqueIds.Count > 0)
            {
                int itemsToProcess = uniqueIds.Count;
                folder.Close();
                folder.Open(FolderAccess.ReadWrite);
                StoreFlagsRequest request = new(StoreAction.Add, MessageFlags.Deleted) { Silent = true };
                int i = 0, itemsProcessed = 0;
                foreach (UniqueId item in uniqueIds)
                {
                    //Thread.Sleep(100);
                    folder.Store(item, request);
                    DeleteEmailListItem(item.ToString());
                    i++;
                    if (i >= 10)
                    {
                        //Thread.Sleep(100);
                        folder.Expunge();
                        i = 0;
                    }
                    if (progress != null)
                        progress.Report((++itemsProcessed) * 100 / itemsToProcess);
                    if (ct.IsCancellationRequested)
                    {
                        folder.Close();
                        folder.Open(FolderAccess.ReadOnly);
                        ct.ThrowIfCancellationRequested();
                    }
                }
                if (i > 0)
                {
                    folder.Expunge();
                }
                folder.Close();
                folder.Open(FolderAccess.ReadOnly);
            }
            if (progress != null)
                progress.Report(100);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
            cancelToolStripMenuItem.Enabled = false;
        }

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
        //ToDo: Implement Google Authentication
        //private async Task GoogleAuthenticateAsync(ImapClient client)
        //{
        //    var clientSecrets = new ClientSecrets
        //    {
        //        ClientId = "XXX.apps.googleusercontent.com",
        //        ClientSecret = "XXX"
        //    };

        //    var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        //    {
        //        DataStore = new FileDataStore("CredentialCacheFolder", false),
        //        Scopes = new[] { "https://mail.google.com/" },
        //        ClientSecrets = clientSecrets
        //    });

        //    // Note: For a web app, you'll want to use AuthorizationCodeWebApp instead.
        //    var codeReceiver = new LocalServerCodeReceiver();
        //    var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

        //    var credential = await authCode.AuthorizeAsync(GMailAccount, CancellationToken.None);

        //    if (credential.Token.IsStale)
        //        await credential.RefreshTokenAsync(CancellationToken.None);

        //    // Note: We use credential.UserId here instead of GMailAccount because the user *may* have chosen a
        //    // different GMail account when presented with the browser window during the authentication process.
        //    SaslMechanism oauth2;

        //    if (client.AuthenticationMechanisms.Contains("OAUTHBEARER"))
        //        oauth2 = new SaslMechanismOAuthBearer(credential.UserId, credential.Token.AccessToken);
        //    else
        //        oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

        //    await client.AuthenticateAsync(oauth2);
        //}

    }
}
