using ClipboardMasking.Win.Data;
using ClipboardMasking.Win.Services;

namespace ClipboardMasking.Win.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly AppSettings _settings;
        private readonly SettingsService _settingsService;

        public SettingsForm(AppSettings settings, SettingsService service)
        {
            InitializeComponent();
            _settings = settings;
            _settingsService = service;
            LoadSettingsIntoUI();
        }

        private void LoadSettingsIntoUI()
        {
            // General
            chkStartOnLaunch.Checked = _settings.StartOnLaunch;

            // Masking Options
            chkMaskIPs.Checked = _settings.MaskIPAddresses;
            chkMaskEmails.Checked = _settings.MaskEmails;
            chkMaskPhones.Checked = _settings.MaskPhoneNumbers;
            chkMaskCreditCards.Checked = _settings.MaskCreditCards;
            chkMaskSSNs.Checked = _settings.MaskSSN;
            chkMaskURLs.Checked = _settings.MaskURLs;
            chkMaskNames.Checked = _settings.MaskNames;
            
            // Custom Names
            lstCustomNames.Items.Clear();
            _settings.CustomNames.ForEach(n => lstCustomNames.Items.Add(n));

            // Custom Patterns
            dgvCustomPatterns.DataSource = null;
            dgvCustomPatterns.DataSource = _settings.CustomPatterns;
        }

        private void SaveSettingsFromUI()
        {
            // General
            _settings.StartOnLaunch = chkStartOnLaunch.Checked;

            // Masking Options
            _settings.MaskIPAddresses = chkMaskIPs.Checked;
            _settings.MaskEmails = chkMaskEmails.Checked;
            _settings.MaskPhoneNumbers = chkMaskPhones.Checked;
            _settings.MaskCreditCards = chkMaskCreditCards.Checked;
            _settings.MaskSSN = chkMaskSSNs.Checked;
            _settings.MaskURLs = chkMaskURLs.Checked;
            _settings.MaskNames = chkMaskNames.Checked;

            // Custom Names
            _settings.CustomNames = lstCustomNames.Items.Cast<string>().ToList();
            
            // Commit any pending grid edits and copy data out to avoid binding artifacts
            if (dgvCustomPatterns.IsCurrentCellInEditMode)
            {
                dgvCustomPatterns.EndEdit();
            }
            dgvCustomPatterns.CommitEdit(DataGridViewDataErrorContexts.Commit);
            var boundList = dgvCustomPatterns.DataSource as List<CustomPattern> ?? new List<CustomPattern>();
            _settings.CustomPatterns = boundList.ToList();

            _settingsService.SaveSettings(_settings);
        }

        private void btnAddName_Click(object sender, EventArgs e)
        {
            var name = txtNewCustomName.Text.Trim();
            if (!string.IsNullOrEmpty(name) && !lstCustomNames.Items.Contains(name))
            {
                lstCustomNames.Items.Add(name);
                txtNewCustomName.Clear();
            }
        }

        private void btnRemoveName_Click(object sender, EventArgs e)
        {
            if (lstCustomNames.SelectedItem != null)
            {
                lstCustomNames.Items.Remove(lstCustomNames.SelectedItem);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettingsFromUI();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 