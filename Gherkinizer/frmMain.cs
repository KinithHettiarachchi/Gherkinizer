using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using Gherkinizer;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace MindMapToGherkin
{

    public partial class frmMain : Form
    {
        AppSettings settings;
        private XNamespace ns = "urn:xmind:xmap:xmlns:content:2.0";
        private string lastLoadedFilePath = "";

        public frmMain()
        {
            InitializeComponent();
            txtGherkin.AllowDrop = true;
            txtGherkin.DragEnter += lblDrop_DragEnter;
            txtGherkin.DragDrop += lblDrop_DragDrop;


            var configPath = Path.Combine(Environment.CurrentDirectory, "settings.ini");
            settings = AppSettings.Load(configPath);

            lblTestCasePrefix.Text = settings.TestPrefix;
            lblTestCasePrefix.Text = settings.TestPrefix;
            lblMindMapMainEpicPrefix.Text = settings.MindmapPrefix;
            lblMindMapTaskPrefix.Text = settings.MindmapPrefix;

            foreach (var domain in settings.DomainList)
            {
                drpDomain.Items.Add(domain);
            }

            drpDomain.SelectedIndex = 0;

            txtRequirements.Text = settings.ReqPrefix;
            txtParent.Text = settings.ParentNode;
            txtSequence.Text = settings.SequenceStart.ToString();
            txtLevel.Text = settings.ScenarioLevel.ToString();
            chkClearOnDrop.Checked = settings.ClearTextArea;


            LoadFolderHierarchy(settings.MindmapFindRoot);

        }

        private void lblDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void HighlightGherkinSyntax()
        {
            int selectionStart = txtGherkin.SelectionStart;
            int selectionLength = txtGherkin.SelectionLength;

            txtGherkin.SuspendLayout();
            txtGherkin.ForeColor = Color.White;

            // Indent table lines
            string[] lines = txtGherkin.Lines;
            for (int i = 0; i < lines.Length; i++)
            {
                if (Regex.IsMatch(lines[i], @"^\s*\|"))
                {
                    lines[i] = lines[i].Trim();
                    lines[i] = new string(' ', 12) + lines[i];
                }
            }
            txtGherkin.Lines = lines;

            // Reset all formatting
            txtGherkin.SelectAll();
            txtGherkin.SelectionColor = Color.Black;
            txtGherkin.SelectionFont = new System.Drawing.Font(txtGherkin.Font, FontStyle.Regular);
            txtGherkin.SelectionBackColor = txtGherkin.BackColor;

            string text = txtGherkin.Text;

            // Basic syntax highlights
            var formattingRules = new List<(string pattern, Color color, bool bold)>
    {
        (@"\bFeature\b", Color.DeepPink, true),
        (@"\bScenario|Scenario Ouline:\b", Color.DeepPink, true),
        (@"\b(Given|When|Then|And|But)\b", Color.LimeGreen, true),
        (@"#.*?$", Color.Gray, false),
        ("\"[^\"]*\"", Color.Blue, false),
        ("<[^>]+>", Color.Purple, false),
        (@"@\w+", Color.DodgerBlue, false),
        ("\"\"\"[\\s\\S]*?\"\"\"", Color.MediumAquamarine, true),
    };

            foreach (var (pattern, color, bold) in formattingRules)
            {
                foreach (Match match in Regex.Matches(text, pattern, RegexOptions.Multiline))
                {
                    txtGherkin.Select(match.Index, match.Length);
                    txtGherkin.SelectionColor = color;
                    txtGherkin.SelectionFont = new System.Drawing.Font(txtGherkin.Font, bold ? FontStyle.Bold : FontStyle.Regular);
                }
            }

            // Highlight Scenario/Scenario Outline/Background/Examples with black bg + blue keyword
            var keywordBlocks = new List<(string keyword, Regex pattern)>
{
    ("Scenario Outline", new Regex(@"^\s*Scenario Outline:.*", RegexOptions.Multiline)),
    ("Scenario",         new Regex(@"^\s*Scenario:\b.*", RegexOptions.Multiline)),
    ("Background",       new Regex(@"^\s*Background\b.*", RegexOptions.Multiline)),
    ("Examples",         new Regex(@"^\s*Examples\b.*", RegexOptions.Multiline)),
};

            // Must process longer keywords first (Scenario Outline before Scenario)
            foreach (var (keyword, pattern) in keywordBlocks)
            {
                foreach (Match match in pattern.Matches(text))
                {
                    // Apply black background to the whole line
                    txtGherkin.Select(match.Index, match.Length);
                    txtGherkin.SelectionBackColor = Color.White;
                    txtGherkin.SelectionColor = Color.Black;
                    txtGherkin.SelectionFont = new System.Drawing.Font(txtGherkin.Font, FontStyle.Regular);

                    // Directly highlight the keyword at the start of the line
                    int keywordStart = match.Index + match.Value.IndexOf(keyword);
                    txtGherkin.Select(keywordStart, keyword.Length);
                    txtGherkin.SelectionColor = Color.DeepSkyBlue;
                    txtGherkin.SelectionFont = new System.Drawing.Font(txtGherkin.Font, FontStyle.Bold);
                }
            }



            // Highlight table header rows (first row of a table block)
            foreach (Match tableMatch in Regex.Matches(text, @"((^\s*\|.*\|.*\r?\n)+)", RegexOptions.Multiline))
            {
                string[] tableLines = tableMatch.Value.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                if (tableLines.Length > 0)
                {
                    var headerLine = tableLines[0];
                    int headerStart = text.IndexOf(headerLine, tableMatch.Index);
                    if (headerStart >= 0)
                    {
                        txtGherkin.Select(headerStart, headerLine.Length);
                        txtGherkin.SelectionColor = Color.MediumPurple;
                        txtGherkin.SelectionFont = new System.Drawing.Font(txtGherkin.Font, FontStyle.Bold);
                    }
                }
            }

            // Restore selection
            txtGherkin.Select(selectionStart, selectionLength);
            txtGherkin.SelectionColor = Color.Black;
            txtGherkin.SelectionFont = new System.Drawing.Font(txtGherkin.Font, FontStyle.Regular);
            txtGherkin.SelectionBackColor = txtGherkin.BackColor;

            txtGherkin.ResumeLayout();
        }




        public void MergeGherkinScenariosWithExamples()
        {
            string text = txtGherkin.Text;
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            // Extract feature header (first line starting with "Feature:")
            int featureLineIndex = lines.FindIndex(l => l.TrimStart().StartsWith("Feature:"));
            string featureHeader = featureLineIndex >= 0 ? lines[featureLineIndex] : "";
            var scenarioBlocks = new List<(List<string> Tags, string TitleLine, List<string> Steps)>();

            // Parse scenarios and tags
            // Tags lines start with spaces then @, followed by a Scenario or Scenario Outline line
            List<string> currentTags = new();
            string currentTitle = null;
            List<string> currentSteps = new();
            bool insideScenario = false;

            for (int i = featureLineIndex + 1; i < lines.Count; i++)
            {
                string line = lines[i];
                string trimmed = line.TrimStart();

                if (trimmed.StartsWith("@"))
                {
                    // Tag line, accumulate
                    if (insideScenario)
                    {
                        // New scenario start detected, save previous scenario
                        if (currentTitle != null)
                            scenarioBlocks.Add((new List<string>(currentTags), currentTitle, new List<string>(currentSteps)));

                        currentTags.Clear();
                        currentSteps.Clear();
                        currentTitle = null;
                    }

                    currentTags.Add(trimmed);
                    insideScenario = false; // not yet scenario line
                }
                else if (trimmed.StartsWith("Scenario:") || trimmed.StartsWith("Scenario Outline:"))
                {
                    // Scenario start line
                    if (insideScenario)
                    {
                        // Save previous scenario
                        if (currentTitle != null)
                            scenarioBlocks.Add((new List<string>(currentTags), currentTitle, new List<string>(currentSteps)));

                        currentTags.Clear();
                        currentSteps.Clear();
                    }
                    currentTitle = trimmed;
                    insideScenario = true;
                }
                else
                {
                    if (insideScenario)
                    {
                        currentSteps.Add(line);
                    }
                    else
                    {
                        // Between feature and scenarios or other lines - ignore or keep if needed
                    }
                }
            }

            // Save last scenario if any
            if (insideScenario && currentTitle != null)
            {
                scenarioBlocks.Add((new List<string>(currentTags), currentTitle, new List<string>(currentSteps)));
            }

            // Now merge example tables that are separated scenarios with only example tables,
            // typically the next scenario is actually example data for the previous scenario.
            // We'll detect those by looking for steps starting with "Given Examples" etc.

            var mergedScenarios = new List<(List<string> Tags, string TitleLine, List<string> Steps)>();

            for (int i = 0; i < scenarioBlocks.Count; i++)
            {
                var (tags, title, steps) = scenarioBlocks[i];

                // Check if this scenario looks like just an Examples block
                // We'll consider it an example block if:
                // - Steps contain a "Given Examples" line
                // - Steps mostly are example tables (lines starting with "|" after "Given Examples")
                bool isExamplesOnly = false;
                int givenExamplesIndex = -1;
                for (int si = 0; si < steps.Count; si++)
                {
                    if (steps[si].Trim() == "Given Examples")
                    {
                        givenExamplesIndex = si;
                        break;
                    }
                }

                if (givenExamplesIndex >= 0)
                {
                    // Check if following lines are example table rows or "And |" header
                    // We'll gather the example lines from steps[givenExamplesIndex+1 ...]
                    var exampleLines = new List<string>();

                    int exLineIndex = givenExamplesIndex + 1;
                    // Handle optional "And | ..." header line (strip "And ")
                    if (exLineIndex < steps.Count && steps[exLineIndex].TrimStart().StartsWith("And |"))
                    {
                        string headerLine = steps[exLineIndex].TrimStart().Substring(4);
                        exampleLines.Add(headerLine);
                        exLineIndex++;
                    }
                    else if (exLineIndex < steps.Count && steps[exLineIndex].TrimStart().StartsWith("|"))
                    {
                        exampleLines.Add(steps[exLineIndex].TrimStart());
                        exLineIndex++;
                    }

                    // Add remaining table rows starting with |
                    while (exLineIndex < steps.Count && steps[exLineIndex].TrimStart().StartsWith("|"))
                    {
                        exampleLines.Add(steps[exLineIndex].TrimStart());
                        exLineIndex++;
                    }

                    // If we got some example lines, treat this scenario as example-only
                    if (exampleLines.Count > 0)
                    {
                        isExamplesOnly = true;

                        // Attach these example lines to the previous scenario as Examples:
                        if (mergedScenarios.Count > 0)
                        {
                            var prev = mergedScenarios[mergedScenarios.Count - 1];
                            var prevSteps = new List<string>(prev.Steps);

                            // Insert Examples: block with proper indentation (match previous step indentation)
                            // Find indentation of first step line (usually spaces at start)
                            string indent = "";
                            foreach (var stepLine in prevSteps)
                            {
                                if (!string.IsNullOrWhiteSpace(stepLine))
                                {
                                    int firstNonSpace = stepLine.TakeWhile(c => c == ' ').Count();
                                    indent = stepLine.Substring(0, firstNonSpace);
                                    break;
                                }
                            }
                            // Add Examples: line + example table lines indented one level more (4 spaces)
                            prevSteps.Add(indent + "Examples:");
                            string exampleIndent = indent + "    ";
                            foreach (var exLine in exampleLines)
                            {
                                prevSteps.Add(exampleIndent + exLine.TrimEnd());
                            }

                            mergedScenarios[mergedScenarios.Count - 1] = (prev.Tags, prev.TitleLine, prevSteps);
                        }
                        else
                        {
                            // No previous scenario to attach to, keep as is
                            mergedScenarios.Add((tags, title, steps));
                        }
                    }
                    else
                    {
                        mergedScenarios.Add((tags, title, steps));
                    }
                }
                else
                {
                    mergedScenarios.Add((tags, title, steps));
                }
            }

            // Now renumber scenario tags and titles sequentially
            // Extract feature id from featureHeader like "Feature: TST-00000 - ..."
            string featureId = settings.TestPrefix + "00000";
            var featureMatch = System.Text.RegularExpressions.Regex.Match(featureHeader, @"Feature:\s*(\S+)");
            if (featureMatch.Success)
            {
                featureId = featureMatch.Groups[1].Value.Replace("-", "");
            }

            int startNumber = int.Parse(txtSequence.Text); // Read starting number from the textbox

            for (int i = 0; i < mergedScenarios.Count; i++)
            {
                int scenarioNumber = startNumber + i;
                var (tags, title, steps) = mergedScenarios[i];

                // Update tags: replace @TSTxxxxx_XX style with new number padded 2 digits
                var updatedTags = new List<string>();
                foreach (var tagLine in tags)
                {
                    string updatedTagLine = System.Text.RegularExpressions.Regex.Replace(tagLine, $@"@{featureId}_(\d+)", $"@{featureId}_{scenarioNumber:00}");
                    updatedTags.Add(updatedTagLine);
                }

                // Update scenario title line, replace EX.TSTxxxxx_XX with current number
                string updatedTitle = System.Text.RegularExpressions.Regex.Replace(title, $@"EX\.{featureId}_(\d+)", $"EX.{featureId}_{scenarioNumber:00}");

                // Update the scenario in the list
                mergedScenarios[i] = (updatedTags, updatedTitle, steps);
            }


            // Rebuild the full text
            var output = new List<string>();
            if (!string.IsNullOrEmpty(featureHeader))
            {
                output.Add(featureHeader);
                output.Add("");
            }

            foreach (var (tags, title, steps) in mergedScenarios)
            {
                // Add tags
                foreach (var tagLine in tags)
                {
                    output.Add("    " + tagLine);
                }
                // Add scenario title
                output.Add("    " + title);

                // Add scenario steps
                foreach (var stepLine in steps)
                {
                    output.Add(stepLine);
                }
                output.Add("");
            }

            // Set back to txtGherkin.Text
            txtGherkin.Text = string.Join("\r\n", output);
            ConvertToScenarioOutlineIfExamplesExist();
        }

        private string ExtractFeatureId(string featureText)
        {
            string prefix = settings.TestPrefix; // e.g., "TST-"
            var match = Regex.Match(featureText, $@"Feature:\s*({Regex.Escape(prefix)}\d+)");
            string fallbackId = prefix.Replace("-", "") + "XXXXX"; // e.g., "TSTXXXXX"
            return match.Success ? match.Groups[1].Value.Replace("-", "") : fallbackId;
        }


        public void ConvertToScenarioOutlineIfExamplesExist()
        {
            string featureText = txtGherkin.Text;
            var lines = featureText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var updatedLines = new List<string>();

            int i = 0;
            int scenarioCounter = 1;
            string featureId = ExtractFeatureId(featureText); // e.g., "TST25325"

            while (i < lines.Length)
            {
                if (lines[i].TrimStart().StartsWith("@manual"))
                {
                    int tagLineIndex = i;
                    int scenarioLineIndex = i + 1;

                    // Look ahead to find scenario line
                    while (scenarioLineIndex < lines.Length && !lines[scenarioLineIndex].TrimStart().StartsWith("Scenario"))
                        scenarioLineIndex++;

                    if (scenarioLineIndex < lines.Length)
                    {
                        string scenarioLine = lines[scenarioLineIndex];
                        string newIndex = scenarioCounter.ToString("D2");
                        string newTagId = $"{featureId}_{newIndex}";

                        // Update tag line: replace only the TST tag (not the whole line)
                        lines[tagLineIndex] = Regex.Replace(
                            lines[tagLineIndex],
                            @$"\b{featureId}_\d+\b",
                            newTagId
                        );

                        // Ensure only one @ before each tag (fix @@)
                        lines[tagLineIndex] = Regex.Replace(lines[tagLineIndex], @"@+", "@");

                        // Update Scenario title ID
                        lines[scenarioLineIndex] = Regex.Replace(
                            scenarioLine,
                            @$"\b{featureId}_\d+\b",
                            newTagId
                        );

                        // Check for Examples block and convert to Scenario Outline if needed
                        bool hasExamples = false;
                        for (int j = scenarioLineIndex + 1; j < lines.Length; j++)
                        {
                            string trimmed = lines[j].TrimStart();
                            if (trimmed.StartsWith("Examples:"))
                            {
                                hasExamples = true;
                                break;
                            }
                            if (trimmed.StartsWith("Scenario") || trimmed.StartsWith("@")) break;
                        }

                        if (hasExamples && lines[scenarioLineIndex].TrimStart().StartsWith("Scenario:"))
                        {
                            lines[scenarioLineIndex] = lines[scenarioLineIndex].Replace("Scenario:", "Scenario Outline:");
                        }

                        scenarioCounter++;
                    }
                }

                updatedLines.Add(lines[i]);
                i++;
            }

            txtGherkin.Text = string.Join("\n", updatedLines);
        }




        private void TxtTSTID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void TxtRequirements_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow basic control keys
            if (char.IsControl(e.KeyChar)) return;

            // Auto-capitalize r/e/q
            if (char.ToLower(e.KeyChar) == 'r' || char.ToLower(e.KeyChar) == 'e' || char.ToLower(e.KeyChar) == 'q')
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }

            var tb = sender as TextBox;
            int pos = tb.SelectionStart;

            // Simulate resulting text
            string before = tb.Text.Substring(0, pos);
            string after = tb.Text.Substring(pos);
            string simulated = before + e.KeyChar + after;

            // Allow digits, R/E/Q, hyphen, comma, space
            if (!char.IsDigit(e.KeyChar) && !(settings.ReqPrefix + ", ").Contains(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Auto-insert " REQ-" after comma
            if (e.KeyChar == ',')
            {
                e.Handled = true; // Block default comma behavior

                // Insert ", REQ-" at the current position
                string insertText = ", " + settings.ReqPrefix;
                tb.Text = before + insertText + after;

                // Move cursor after the inserted "REQ-"
                tb.SelectionStart = (before + insertText).Length;
            }
        }


        private void lblDrop_DragDrop(object sender, DragEventArgs e)
        {

            if (!int.TryParse(txtTSTID.Text.Trim(), out int tstId) || tstId <= 0 || txtTSTID.Text == "00000")
            {
                MessageBox.Show("Please enter a valid TST ID!", "Incomplete Information!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTSTID.Focus();
                txtTSTID.SelectAll();
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            scenarioSequence = int.Parse(txtSequence.Text.Trim());


            if (files != null && files.Length > 0)
            {
                if (files[0].EndsWith(".mm"))
                {
                    lastLoadedFilePath = files[0];

                    //Preset values
                    string featureTitleText = GetCentralNodeText(lastLoadedFilePath.ToString());

                    // Only prompt if there is a mismatch and the current textbox is not empty
                    if (txtFeatureTitle.Text != featureTitleText && txtFeatureTitle.Text != "")
                    {
                        DialogResult result = MessageBox.Show(
                            "The feature title in the mind map differs from the current text.\n\n" +
                            "Do you want to replace the current text with the one from the mind map?\n\n" +
                            "Yes - Replace with mind map title\n" +
                            "No - Keep current title\n" +
                            "Cancel - Do nothing",
                            "Confirm Title Replacement",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question
                        );

                        if (result == DialogResult.Yes)
                        {
                            txtFeatureTitle.Text = featureTitleText; // Replace
                        }
                        else if (result == DialogResult.No)
                        {
                            // Keep existing text — do nothing
                        }
                        else if (result == DialogResult.Cancel)
                        {
                            return; // Stop further processing
                        }
                    }
                    else
                    {
                        // No mismatch or textbox is empty — safe to auto-assign
                        txtFeatureTitle.Text = featureTitleText;
                    }

                    string requirementList = GetReqIdsUnderRequirements(lastLoadedFilePath.ToString());

                    // Only prompt if there is a mismatch and the current textbox is not empty
                    if (txtRequirements.Text != requirementList && txtRequirements.Text != "" && txtRequirements.Text != settings.ReqPrefix)
                    {
                        DialogResult result = MessageBox.Show(
                            "The requirements in the mind map differ from the current text.\n\n" +
                            "Do you want to replace the current requirements with the ones from the mind map?\n\n" +
                            "Yes - Replace with mind map requirements\n" +
                            "No - Keep current requirements\n" +
                            "Cancel - Do nothing",
                            "Confirm Requirements Replacement",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question
                        );

                        if (result == DialogResult.Yes)
                        {
                            txtRequirements.Text = requirementList; // Replace
                        }
                        else if (result == DialogResult.No)
                        {
                            // Keep existing text — do nothing
                        }
                        else if (result == DialogResult.Cancel)
                        {
                            return; // Stop further processing
                        }
                    }
                    else
                    {
                        // No mismatch or textbox is empty — safe to auto-assign
                        txtRequirements.Text = requirementList;
                    }


                    if (txtFeatureTitle.Text.Trim() == "Enter your feature title here" || txtFeatureTitle.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter a valid feature title!", "Incomplete Information!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtFeatureTitle.Focus();
                        txtFeatureTitle.SelectAll();
                        return;
                    }

                    string requirements = txtRequirements.Text.Trim();
                    string reqPrefix = settings.ReqPrefix; // e.g., "REQ-"
                    string pattern = $@"^({Regex.Escape(reqPrefix)}\d{{4,}})(,\s*{Regex.Escape(reqPrefix)}\d{{4,}})*$";


                    if (string.IsNullOrEmpty(requirements) || requirements == settings.ReqPrefix || !Regex.IsMatch(requirements, pattern))
                    {
                        MessageBox.Show($"Please enter valid requirement IDs (e.g., {settings.ReqPrefix}1234, {settings.ReqPrefix}5678).", "Incomplete Information!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtRequirements.Focus();
                        txtRequirements.SelectAll();
                        return;
                    }


                    if (txtParent.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter valid functional test parent node!", "Incomplete Information!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtParent.Focus();
                        txtParent.SelectAll();
                        return;
                    }

                    if (!int.TryParse(txtLevel.Text.Trim(), out int level) || level <= 0)
                    {
                        MessageBox.Show("Please enter valid functional test parent node level!", "Incomplete Information!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtLevel.Focus();
                        txtLevel.SelectAll();
                        return;
                    }

                    if (!int.TryParse(txtSequence.Text.Trim(), out int sequence) || sequence <= 0)
                    {
                        MessageBox.Show("Please enter valid scenario start sequence!", "Incomplete Information!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSequence.Focus();
                        txtSequence.SelectAll();
                        return;
                    }

                    if (chkClearOnDrop.Checked)
                    {
                        txtGherkin.Clear();
                    }

                    ProcessMmFile(lastLoadedFilePath, int.Parse(txtLevel.Text));
                    MergeGherkinScenariosWithExamples();
                    HighlightGherkinSyntax();
                    lblFilePath.Text = lastLoadedFilePath;
                }
                else
                {
                    MessageBox.Show("Please provide a valid mind map file!", "Invalid File!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        private void txtLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (e.g., Backspace), and digits
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void txtSequence_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /*=========================================================================================================*/
        public class Node
        {
            public string Text { get; set; }
            public List<Node> Children { get; set; } = new List<Node>();
        }

        public Node FunctionalTestsNode { get; private set; }
        public List<Node> SelectedNodes { get; private set; } = new List<Node>();

        // Method to read and process the .mm file
        public void ProcessMmFile(string filePath, int level)
        {
            XElement mmFile = XElement.Load(filePath);
            FunctionalTestsNode = FindFunctionalTestsNode(mmFile);

            if (FunctionalTestsNode != null)
            {
                FindSubLevelNodes(FunctionalTestsNode, level);
                DisplayScenariosInGherkin();
            }
            else
            {
                MessageBox.Show($"'{txtParent.Text}' node was not found!.");
            }
        }

        // Find the 'Functional Tests' node
        private Node FindFunctionalTestsNode(XElement root)
        {
            return Traverse(root, txtParent.Text);
        }

        // Recursive function to traverse and find a node by its text
        private Node Traverse(XElement element, string searchText)
        {
            string text = element.Attribute("TEXT")?.Value;
            if (text == searchText)
            {
                return new Node { Text = text, Children = GetChildren(element) };
            }

            foreach (var child in element.Elements("node"))
            {
                Node result = Traverse(child, searchText);
                if (result != null) return result;
            }

            return null;
        }

        // Get children nodes from an XML element
        private List<Node> GetChildren(XElement element)
        {
            return element.Elements("node")
                          .Select(child => new Node
                          {
                              Text = child.Attribute("TEXT")?.Value,
                              Children = GetChildren(child)
                          })
                          .ToList();
        }

        // Find sub-level nodes based on the level passed
        private void FindSubLevelNodes(Node parentNode, int level)
        {
            if (level == 1)
            {
                SelectedNodes = parentNode.Children;
            }
            else
            {
                foreach (var child in parentNode.Children)
                {
                    GetNodesAtLevel(child, level - 1);
                }
            }
        }

        // Recursive method to find nodes at a specific level
        private void GetNodesAtLevel(Node node, int level)
        {
            if (level == 1)
            {
                SelectedNodes.AddRange(node.Children);
            }
            else
            {
                foreach (var child in node.Children)
                {
                    GetNodesAtLevel(child, level - 1);
                }
            }
        }

        // Generate and display Gherkin scenarios in txtGherkin
        private void DisplayScenariosInGherkin()
        {
            List<string> scenarios = GenerateScenarios();

            txtGherkin.Text = string.Empty;

            txtGherkin.Text = $"Feature: {settings.TestPrefix}{txtTSTID.Text} - {txtFeatureTitle.Text}\n\n" + string.Join(Environment.NewLine + Environment.NewLine, scenarios);
        }

        // Method to generate Gherkin scenarios from the nodes
        private List<string> GenerateScenarios()
        {
            List<string> scenarios = new List<string>();

            foreach (var node in SelectedNodes)
            {
                GenerateScenarioPaths(node, new List<string>(), scenarios);
            }

            return scenarios;
        }

        // Recursively create scenarios based on node paths
        private void GenerateScenarioPaths(Node node, List<string> currentPath, List<string> scenarios, List<Node> siblings = null)
        {
            currentPath.Add(node.Text);

            if (node.Children.Count == 0)
            {
                // No children, check for siblings
                if (siblings != null && siblings.Any())
                {
                    // Add sibling nodes to the current path for the scenario
                    foreach (var sibling in siblings.Where(s => s != node))
                    {
                        currentPath.Add(sibling.Text);
                    }
                }

                // End of the path, create a scenario
                scenarios.Add(CreateScenarioFromPath(currentPath));
            }
            else
            {
                // If there are children, continue traversing each child
                foreach (var child in node.Children)
                {
                    // Pass sibling nodes to handle grouping
                    GenerateScenarioPaths(child, new List<string>(currentPath), scenarios, node.Children);
                }
            }
        }

        // Create a Gherkin scenario from a path of nodes
        int scenarioSequence;
        private string CreateScenarioFromPath(List<string> path)
        {
            string domainPart = string.IsNullOrWhiteSpace(drpDomain.Text) ? "" : $"{drpDomain.Text}.";
            string scenarioTitle = $"    @manual @{txtTSTID.Text} @TST{txtTSTID.Text}_{FormatScenarioSequence(scenarioSequence)}{Environment.NewLine}    Scenario: {domainPart}TST{txtTSTID.Text}_{FormatScenarioSequence(scenarioSequence)}.{path.First()} : {txtRequirements.Text}";

            // Map each step based on the starting text
            string lastGeneratedStep = "";
            string lastMainStepKeyword = "";

            try
            {
                // Skip the first element in the path
                string steps = string.Join(Environment.NewLine, path
                    .Skip(1) // Skip the first element
                    .Where(p => p != null && !p.StartsWith("~"))  // Filter out null values and elements starting with '~'
                    .Select(p =>
                    {
                        // When
                        if (p.ToUpper().StartsWith("USER", StringComparison.OrdinalIgnoreCase))
                        {
                            if (lastGeneratedStep.Trim().StartsWith("When") || (lastGeneratedStep.Trim().StartsWith("And") && lastMainStepKeyword == "When"))
                            {
                                lastGeneratedStep = $"        And {FormatResrouceStep(p)}";
                            }
                            else
                            {
                                lastMainStepKeyword = "When";
                                lastGeneratedStep = $"        {lastMainStepKeyword} {FormatResrouceStep(p)}";
                            }
                        }
                        // Then
                        else if (p.ToUpper().StartsWith("SYSTEM", StringComparison.OrdinalIgnoreCase))
                        {
                            if (lastGeneratedStep.Trim().StartsWith("Then") || (lastGeneratedStep.Trim().StartsWith("And") && lastMainStepKeyword == "Then"))
                            {
                                lastGeneratedStep = $"        And {FormatResrouceStep(p)}";
                            }
                            else
                            {
                                lastMainStepKeyword = "Then";
                                lastGeneratedStep = $"        {lastMainStepKeyword} {FormatResrouceStep(p)}";
                            }
                        }
                        // Given
                        else
                        {
                            if (lastGeneratedStep.Trim().StartsWith("Given") || (lastGeneratedStep.Trim().StartsWith("And") && lastMainStepKeyword == "Given"))
                            {
                                lastGeneratedStep = $"        And {p}";
                            }
                            else
                            {
                                lastMainStepKeyword = "Given";
                                lastGeneratedStep = $"        {lastMainStepKeyword} {p}";
                            }
                        }

                        return lastGeneratedStep;
                    }));

                scenarioSequence++;
                return $"{scenarioTitle}{Environment.NewLine}{steps}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"'{ex.Message}'!.");
                return "";
            }
        }

        private string FormatScenarioSequence(int scenarioSequence)
        {
            return scenarioSequence < 10 ? $"0{scenarioSequence}" : scenarioSequence.ToString();
        }

        private string FormatResrouceStep(string p)
        {
            // Define the possible prefixes and suffix
            string[] prefixes = { "system confirms user with '",
                                  "system alerts user with '" };
            const string suffix = "'";

            //Make first letter simple
            if (!string.IsNullOrEmpty(p))
            {
                p = char.ToLower(p[0]) + p.Substring(1);
            }

            // Check if the string starts with any of the expected prefixes
            foreach (var prefix in prefixes)
            {
                if (p.StartsWith(prefix) && p.EndsWith(suffix))
                {
                    // Extract the content between the quotes
                    int startIndex = p.IndexOf('\'') + 1;
                    int endIndex = p.LastIndexOf('=');

                    if (endIndex > startIndex) // Ensure valid indices
                    {
                        // Extract the key and the full message
                        string key = p.Substring(startIndex, endIndex - startIndex).Trim();
                        string fullMessage = p.Substring(endIndex + 1).Trim().TrimEnd('\'');

                        // Format and return the desired result
                        return $"{prefix}{key}'{Environment.NewLine}            \"\"\"{Environment.NewLine}            {key}={fullMessage}{Environment.NewLine}            \"\"\"";
                    }
                }
            }

            // If the format is not as expected, return the original string or an error message
            return p; // or you can return a specific error message
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Instructions for Mind Map\n\n" +
                "🛟 Place your scenarios under a node named 'Functional Tests'.\n\n" +
                "🛟 Include your preconditions in the first (left most) steps. These will be taken as 'Given' steps and should not start with words 'user' or 'system'.\n\n" +
                "🛟 Every user action must start with the word 'user'. These will be converted as 'When' steps.\n\n" +
                "🛟 Every system result must start with the word 'system'. These will be converted as 'Then' steps.\n\n" +
                "🛟 If you want to skip any node in the mind map path, start those with '~'. Such steps will not be converted to a Gherkin step, and will simply be skipped.\n\n" +
                "🛟 Enter the domain, TST ID, and Requirements before dropping the file so the tool will automatically handle the scenario naming and default tags.\n\n" +
                "🛟 Once converted, copy and paste it into your Gherkin editor and do the modifications needed.",
                "Help", MessageBoxButtons.OK, MessageBoxIcon.Information
            );
        }

        private void txtLevel_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTSTID_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtGherkin.Text))
            {
                Clipboard.SetText(txtGherkin.Text);
                MessageBox.Show("Gherkin text copied to clipboard!", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("There is no text to copy.", "Empty Text", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLocate_Click(object sender, EventArgs e)
        {
            string idToFind = settings.TestPrefix.Replace("-", "") + txtTSTID.Text + "_";
            string pathToFind = settings.FeatureFindRoot;

            try
            {
                var matchingFile = Directory.EnumerateFiles(pathToFind, "*", SearchOption.AllDirectories)
                                            .FirstOrDefault(file => Path.GetFileName(file).StartsWith(idToFind, StringComparison.OrdinalIgnoreCase));

                if (matchingFile != null)
                {
                    // Optional: Show file path
                    //MessageBox.Show("Found: " + matchingFile, "File Located", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open the file with default associated application
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = matchingFile,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("There is no feature file for " + settings.TestPrefix + txtTSTID.Text + " in your test case repository path " + settings.FeatureFindRoot + ".\nPlease add the file first and then try again!", "File Not Found!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching or opening file:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnWriteToFeatureFile_Click(object sender, EventArgs e)
        {
            string idToFind = settings.TestPrefix.Replace("-", "") + txtTSTID.Text + "_";
            string pathToFind = settings.FeatureFindRoot;

            try
            {
                var matchingFile = Directory.EnumerateFiles(pathToFind, "*", SearchOption.AllDirectories)
                                            .FirstOrDefault(file => Path.GetFileName(file).StartsWith(idToFind, StringComparison.OrdinalIgnoreCase));

                if (matchingFile == null)
                {
                    MessageBox.Show("There is no feature file for " + settings.TestPrefix + txtTSTID.Text +
                                    " in your test case repository path " + settings.FeatureFindRoot + "!\nPlease add the file first and then try again!",
                                    "File Not Found!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the file has content
                string existingContent = File.ReadAllText(matchingFile);
                if (!string.IsNullOrWhiteSpace(existingContent))
                {
                    var result = MessageBox.Show("The feature file " + settings.TestPrefix + txtTSTID.Text + " already contains content.\nDo you want to overwrite the file with the new content?",
                                                 "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                    {
                        return; // Stop if user does not want to overwrite
                    }
                }

                // Write the content from txtGherkin to the file
                File.WriteAllText(matchingFile, txtGherkin.Text);

                // Open the file with the default application
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = matchingFile,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing or opening the file:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadMindMap_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the ID of the mind map that you want to load.\n(e.g., " + settings.MindmapPrefix + "1234):", "Load Mind Map", settings.MindmapPrefix);

            if (string.IsNullOrWhiteSpace(input))
                return;

            string pattern = @"^DEV-(\d{4,})$";
            Match match = Regex.Match(input.Trim(), pattern);

            if (!match.Success || !int.TryParse(match.Groups[1].Value, out int devNumber) || devNumber <= 0)
            {
                MessageBox.Show("Please enter a valid mind map ID in the format " + settings.MindmapPrefix + "-1234.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string devIdNoDash = input.Replace("-", ""); // e.g., DEV1234
            string searchPattern = devIdNoDash + "_*.mm";

            try
            {
                var fileToDrop = Directory.EnumerateFiles(settings.MindmapFindRoot, "*.mm", SearchOption.AllDirectories)
                                          .FirstOrDefault(file => Path.GetFileName(file).StartsWith(devIdNoDash + "_", StringComparison.OrdinalIgnoreCase));

                if (fileToDrop == null)
                {
                    MessageBox.Show("No matching .mm mind map file found for " + input + "!", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Simulate drag-and-drop onto txtGherkin (specifically lblDrop control)
                string[] files = new string[] { fileToDrop };
                DataObject data = new DataObject(DataFormats.FileDrop, files);

                DragEventArgs args = new DragEventArgs(data, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
                lblDrop_DragDrop(txtGherkin, args); // Trigger your existing handler

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while locating or loading the mind map file:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string GetCentralNodeText(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var rootNode = doc.Descendants("node").FirstOrDefault();
            var rawText = rootNode?.Attribute("TEXT")?.Value ?? string.Empty;

            // Remove text within square brackets including brackets and trim
            string cleanedText = Regex.Replace(rawText, @"\[[^\]]*\]", "").Trim();
            return cleanedText;
        }

        public string GetReqIdsUnderRequirements(string filePath)
        {
            var doc = XDocument.Load(filePath);

            // Find the "Requirements" or "Requirement" node (case-insensitive)
            var requirementsNode = doc.Descendants("node")
                .FirstOrDefault(n =>
                    string.Equals((string)n.Attribute("TEXT"), "Requirements", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals((string)n.Attribute("TEXT"), "Requirement", StringComparison.OrdinalIgnoreCase));

            if (requirementsNode == null)
                return string.Empty;

            // Collect all descendant nodes (recursive) under the Requirements node
            var descendantTexts = requirementsNode
                .Descendants("node")
                .Select(n => (string)n.Attribute("TEXT"))
                .Where(text => !string.IsNullOrEmpty(text));

            // Extract all matches like [REQ-12345]
            string reqPrefix = settings.ReqPrefix.ToString(); // e.g., "REQ-"
            var reqIdPattern = new Regex(@"\[" + Regex.Escape(reqPrefix) + @"\d+\]");

            var reqIds = new HashSet<string>();

            foreach (var text in descendantTexts)
            {
                foreach (Match match in reqIdPattern.Matches(text))
                {
                    string id = match.Value.Trim('[', ']');
                    reqIds.Add(id);
                }
            }

            return string.Join(", ", reqIds.OrderBy(id => id));
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (lastLoadedFilePath == null || lastLoadedFilePath.Length == 0) return;

            string[] files = new string[] { lastLoadedFilePath };
            DataObject data = new DataObject(DataFormats.FileDrop, files);

            DragEventArgs args = new DragEventArgs(data, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
            lblDrop_DragDrop(txtGherkin, args); // Trigger your existing handler
        }

        private void btnOpenMindmap_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lastLoadedFilePath))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = lastLoadedFilePath,
                        UseShellExecute = true // Opens with default associated program
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open the file.\n\nDetails: {ex.Message}",
                        "Error Opening File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No mind map file has been loaded yet.",
                    "Open Mind Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static string ValidateGherkinFeatureFile(string[] lines)
        {
            var issues = new List<string>();

            bool featureSeen = false;
            string currentScenarioType = null;
            bool examplesSeen = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                int lineNumber = i + 1;

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("Feature", StringComparison.OrdinalIgnoreCase))
                {
                    featureSeen = true;
                    currentScenarioType = null;
                    examplesSeen = false;
                }
                else if (line.StartsWith("Scenario Outline", StringComparison.OrdinalIgnoreCase))
                {
                    if (!featureSeen)
                        issues.Add($"Line {lineNumber}: 'Scenario Outline' found before 'Feature'");

                    currentScenarioType = "Scenario Outline";
                    examplesSeen = false;
                }
                else if (line.StartsWith("Scenario", StringComparison.OrdinalIgnoreCase))
                {
                    if (!featureSeen)
                        issues.Add($"Line {lineNumber}: 'Scenario' found before 'Feature'");

                    currentScenarioType = "Scenario";
                    examplesSeen = false;
                }
                else if (line.StartsWith("Examples", StringComparison.OrdinalIgnoreCase))
                {
                    if (currentScenarioType == "Scenario")
                        issues.Add($"Line {lineNumber}: 'Examples' found for 'Scenario', should be 'Scenario Outline'?");

                    if (currentScenarioType == null)
                        issues.Add($"Line {lineNumber}: 'Examples' found outside of a scenario");

                    examplesSeen = true;
                }
                else if (line.StartsWith("|")) // table line
                {
                    if (currentScenarioType == "Scenario" && !examplesSeen)
                    {
                        issues.Add($"Line {lineNumber}: Table found under 'Scenario' without 'Examples'");
                    }
                }
            }

            // Post-scan: check for Scenario Outline without Examples
            if (currentScenarioType == "Scenario Outline" && !examplesSeen)
            {
                issues.Add($"End of file: 'Scenario Outline' declared without any 'Examples'");
            }

            if (!featureSeen)
            {
                issues.Insert(0, "Line 1: No 'Feature' keyword found in file");
            }

            return issues.Count > 0 ? string.Join(Environment.NewLine, issues) : "No issues found.";
        }

        private void bttValidate_Click(object sender, EventArgs e)
        {
            string[] lines = txtGherkin.Lines;
            string validationResult = ValidateGherkinFeatureFile(lines);
            MessageBox.Show(validationResult, "Gherkin Validation Results");

        }



        //=========================================================================================================================
        // MIND MAP PAGE
        //=========================================================================================================================
        private void LoadFolderHierarchy(string rootPath)
        {
            tree.Nodes.Clear();

            TreeNode rootNode = new TreeNode(Path.GetFileName(rootPath))
            {
                Tag = rootPath
            };
            tree.Nodes.Add(rootNode);

            LoadSubDirectories(rootNode, rootPath, 1);

            ExpandToLevel(tree, 3); // Expand only up to 3rd level
        }

        /// <summary>
        /// Recursively loads all subdirectories, skipping folders starting with .svn.
        /// </summary>
        private void LoadSubDirectories(TreeNode parentNode, string path, int currentLevel)
        {
            try
            {
                foreach (string directory in Directory.GetDirectories(path))
                {
                    string folderName = Path.GetFileName(directory);
                    if (folderName.StartsWith(".svn", StringComparison.OrdinalIgnoreCase))
                        continue;

                    TreeNode node = new TreeNode(folderName)
                    {
                        Tag = directory
                    };
                    parentNode.Nodes.Add(node);

                    // Recurse into subdirectories regardless of depth
                    LoadSubDirectories(node, directory, currentLevel + 1);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Optionally log or ignore access issues
            }
        }

        /// <summary>
        /// Expands the tree nodes up to a specific level.
        /// </summary>
        private void ExpandToLevel(TreeView treeView, int level)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                ExpandToLevel(node, level, 1);
            }
        }

        private void ExpandToLevel(TreeNode node, int maxLevel, int currentLevel)
        {
            if (currentLevel < maxLevel)
                node.Expand();

            foreach (TreeNode child in node.Nodes)
            {
                ExpandToLevel(child, maxLevel, currentLevel + 1);
            }
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is string path)
            {
                txtMindMapPath.Text = path;
            }
        }

        string mindMapperVersion = "1.12.9.0";
        private void btnCreateMindMap_Click(object sender, EventArgs e)
        {
            string centralNodeText = $"{txtMindMapEpicTitle.Text}&#xa;[{settings.MindmapPrefix + txtMindMapMainEpicID.Text}]";
            string mindMapID = txtMindMapTaskID.Text;
            string mindMapFilePath = $"{txtMindMapPath.Text}";
            string sanitizedTitle = SanitizeAndFormatTitle(txtMindMapTitle.Text);
            string mindMapFileName = $"{settings.MindmapPrefix.Replace("-", "")}{mindMapID}_{sanitizedTitle}";
            string mindMapVersion = GetMindMapperVersion();
            WriteFreeplaneMindMap(centralNodeText, mindMapFilePath, mindMapFileName, mindMapVersion);
        }

        private string GetMindMapperVersion()
        {
            string exePath = @"C:\Program Files\Freeplane\freeplane.exe"; // or the actual path
            if (File.Exists(exePath))
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(exePath);
                mindMapperVersion = versionInfo.FileVersion; // or ProductVersion
            }

            return mindMapperVersion;
        }
        private string SanitizeAndFormatTitle(string input)
        {
            // Remove non-letter characters (keep only A-Z, a-z, and spaces)
            string cleaned = new string(input.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());

            // Capitalize each word
            string[] words = cleaned.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }

            // Join without spaces
            return string.Join("", words);
        }


        public void WriteFreeplaneMindMap(string centralNodeText, string mindMapFilePath, string mindMapFileName, string mindMapVersion)
        {
            string xmlContent = $@"<map version=""freeplane {mindMapVersion}"">
    <node TEXT=""{centralNodeText}"" FOLDED=""false"" ID=""ID_0"">
        <font NAME=""Consolas"" SIZE=""16"" BOLD=""true""/>
    
        <node TEXT=""Functional Tests"" POSITION=""bottom_or_right"" ID=""ID_1"">
            <icon BUILTIN=""emoji-1F489""/>
            <font NAME=""Consolas"" SIZE=""12"" BOLD=""true""/>
        
            <node TEXT=""Scenario title here"" ID=""ID_2"">
                <icon BUILTIN=""emoji-1F4C1""/>
                <font NAME=""Consolas"" SIZE=""10""/>
            
                <node TEXT=""precondition here"" ID=""ID_3"">
                    <icon BUILTIN=""idea""/>
                    <font NAME=""Consolas"" SIZE=""10""/>
                
                    <node TEXT=""user performs action"" ID=""ID_4"">
                        <icon BUILTIN=""male2""/>
                        <font NAME=""Consolas"" SIZE=""10""/>
                    
                        <node TEXT=""system gives result"" ID=""ID_5"">
                            <icon BUILTIN=""button_ok""/>
                            <font NAME=""Consolas"" SIZE=""10""/>
                        </node>
                    </node>
                </node>
            </node>
        </node>

        <node TEXT=""Regression Tests"" POSITION=""bottom_or_right"" ID=""ID_6"">
            <icon BUILTIN=""emoji-1FA79""/>
            <font NAME=""Consolas"" SIZE=""12"" BOLD=""true""/>
        </node>

        <node TEXT=""Requirements"" POSITION=""top_or_left"" ID=""ID_7"">
            <icon BUILTIN=""list""/>
            <font NAME=""Consolas"" SIZE=""12"" BOLD=""true""/>
            <node TEXT=""[REQ-#####] Requirement Title Here&#xa;[REQ-#####] Requirement Title Here"" ID=""ID_8"">
                <icon BUILTIN=""list""/>
                <font NAME=""Consolas"" SIZE=""10""/>
            </node>
        </node>

        <node TEXT=""Analysis"" POSITION=""top_or_left"" ID=""ID_9"">
            <icon BUILTIN=""emoji-1F52C""/>
            <font NAME=""Consolas"" SIZE=""12"" BOLD=""true""/>
        </node>

        <node TEXT=""Database"" POSITION=""top_or_left"" ID=""ID_10"">
            <icon BUILTIN=""emoji-1F9EE""/>
            <font NAME=""Consolas"" SIZE=""12"" BOLD=""true""/>
        </node>

        <node TEXT=""GUI"" POSITION=""top_or_left"" ID=""ID_11"">
            <icon BUILTIN=""emoji-1F5A5""/>
            <font NAME=""Consolas"" SIZE=""12"" BOLD=""true""/>
        </node>

        <node TEXT=""Tracers"" POSITION=""top_or_left"" ID=""ID_12"">
            <icon BUILTIN=""emoji-1FA7A""/>
            <font NAME=""Consolas"" SIZE=""12"" BOLD=""true""/>
        </node>

        <node TEXT=""MD Files"" POSITION=""top_or_left"" ID=""ID_13"">
            <icon BUILTIN=""emoji-1F4DD""/>
            <font NAME=""Consolas"" SIZE=""12"" BOLD=""true""/>
        </node>

        <node TEXT=""Resources"" POSITION=""top_or_left"" ID=""ID_14"">
            <icon BUILTIN=""links/sysadmin/shell_script""/>
            <font NAME=""Consolas"" SIZE=""12"" BOLD=""true""/>
        </node>
    </node>
</map>";

            // Ensure file path ends with directory separator
            if (!mindMapFilePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                mindMapFilePath += Path.DirectorySeparatorChar;

            // Full file path
            string fullPath = Path.Combine(mindMapFilePath, $"{mindMapFileName}.mm");

            // Write XML to file
            File.WriteAllText(fullPath, xmlContent, Encoding.UTF8);

            if (chkOpenMindMapFolder.Checked)
            {
                if (Directory.Exists(mindMapFilePath))
                {
                    System.Diagnostics.Process.Start("explorer.exe", mindMapFilePath);
                }
                else
                {
                    MessageBox.Show("The specified mind map folder does not exist.", "Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (chkOpenMindMapFile.Checked)
            {
                if (File.Exists(fullPath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = fullPath,
                        UseShellExecute = true // Required to open with default associated app
                    });
                }
                else
                {
                    MessageBox.Show("The mind map file was not found.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
    }
}
