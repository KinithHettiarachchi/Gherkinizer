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


namespace MindMapToGherkin
{
    public partial class Form1 : Form
    {
        private XNamespace ns = "urn:xmind:xmap:xmlns:content:2.0";

        public Form1()
        {
            InitializeComponent();
            txtGherkin.AllowDrop = true;
            txtGherkin.DragEnter += lblDrop_DragEnter;
            txtGherkin.DragDrop += lblDrop_DragDrop;
            drpDomain.SelectedIndex = 0;
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

            // Get lines and indent table lines with 12 spaces
            string[] lines = txtGherkin.Lines;
            for (int i = 0; i < lines.Length; i++)
            {
                if (Regex.IsMatch(lines[i], @"^\s*\|")) // Table line
                {
                    lines[i] = lines[i].Trim(); // Remove any existing leading whitespace
                    lines[i] = new string(' ', 12) + lines[i]; // Add 12 spaces
                }
            }

            // Update text with indented tables
            txtGherkin.Lines = lines;

            // Reset all formatting
            txtGherkin.SelectAll();
            txtGherkin.SelectionColor = Color.Black;
            txtGherkin.SelectionFont = new Font(txtGherkin.Font, FontStyle.Regular);

            string text = txtGherkin.Text;

            // Patterns and formatting
            var formattingRules = new List<(string pattern, Color color, bool bold)>
    {
        // Gherkin keywords
        (@"\b(Feature|Scenario Outline|Scenario|Background|Examples|Given|When|Then|And|But)\b", Color.DarkBlue, true),

        // Comments
        (@"#.*?$", Color.DarkGray, false),

        // Quoted strings
        ("\"[^\"]*\"", Color.Brown, false),

        // Parameters in angle brackets
        ("<[^>]+>", Color.Purple, false),

        // Tags
        (@"@\w+", Color.Teal, false),

        // Docstrings (""" multiline content """)
        ("\"\"\"[\\s\\S]*?\"\"\"", Color.DarkGreen, true),
    };

            foreach (var (pattern, color, bold) in formattingRules)
            {
                foreach (Match match in Regex.Matches(text, pattern, RegexOptions.Multiline))
                {
                    txtGherkin.Select(match.Index, match.Length);
                    txtGherkin.SelectionColor = color;
                    txtGherkin.SelectionFont = new Font(txtGherkin.Font, bold ? FontStyle.Bold : FontStyle.Regular);
                }
            }

            // Restore original selection
            txtGherkin.Select(selectionStart, selectionLength);
            txtGherkin.SelectionColor = Color.Black;
            txtGherkin.SelectionFont = new Font(txtGherkin.Font, FontStyle.Regular);

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
            string featureId = "TST00000";
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

        public void ConvertToScenarioOutlineIfExamplesExist()
        {
            string featureText = txtGherkin.Text;
            var lines = featureText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var updatedLines = new List<string>();

            int i = 0;
            while (i < lines.Length)
            {
                if (lines[i].TrimStart().StartsWith("Scenario:"))
                {
                    int scenarioStartIndex = i;
                    bool hasExamples = false;
                    var scenarioBlock = new List<string>();

                    // Add the original scenario title line temporarily
                    scenarioBlock.Add(lines[i]);
                    i++;

                    // Collect all scenario lines until the next scenario or tag
                    while (i < lines.Length &&
                           !lines[i].TrimStart().StartsWith("Scenario") &&
                           !lines[i].TrimStart().StartsWith("@"))
                    {
                        if (lines[i].TrimStart().StartsWith("Examples:"))
                        {
                            hasExamples = true;
                        }

                        scenarioBlock.Add(lines[i]);
                        i++;
                    }

                    // Update the scenario title if examples were found
                    if (hasExamples)
                    {
                        string originalLine = scenarioBlock[0];
                        int indent = originalLine.Length - originalLine.TrimStart().Length;
                        string updatedLine = new string(' ', indent) + originalLine.TrimStart().Replace("Scenario:", "Scenario Outline:");
                        scenarioBlock[0] = updatedLine;
                    }

                    updatedLines.AddRange(scenarioBlock);
                }
                else
                {
                    updatedLines.Add(lines[i]);
                    i++;
                }
            }

            txtGherkin.Text = string.Join("\n", updatedLines);
        }




        private void lblDrop_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            scenarioSequence = int.Parse(txtSequence.Text.Trim());

            if (!ConfirmAction($"Please confirm that the following information is correct!\n\nMindmap File :\n{files[0]}\n\nParent node of tests:\n{txtParent.Text}\n\nDomain:\n{drpDomain.Text}\n\nTestable Functionality ID :\nTST-{txtTSTID.Text}\n\nSequence Start :\n{scenarioSequence}\n\nRequirements :\n{txtRequirements.Text}", $"Confirm Conversion!"))
            {
                return;
            }
            if (files != null && files.Length > 0)
            {
                if (files[0].EndsWith(".xmind"))
                {
                    MessageBox.Show("*.xmind files have limited support. Please use Freeplane files instead!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    string xmindFile = files[0];
                    string tempFolder = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(xmindFile)) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    Directory.CreateDirectory(tempFolder);

                    // Extract .xmind file using 7-Zip
                    ExtractXmindFile(xmindFile, tempFolder);

                    // Read content.xml from the extracted files
                    string contentFilePath = Path.Combine(tempFolder, "content.xml");
                    if (File.Exists(contentFilePath))
                    {
                        XDocument xmlDoc = XDocument.Load(contentFilePath);

                        // Convert the XML content to Gherkin scenarios
                        string gherkinScenarios = ConvertToGherkin(xmlDoc);

                        // Display the Gherkin output in the txtGherkin text field
                        txtGherkin.Text = gherkinScenarios;
                        MergeGherkinScenariosWithExamples();
                        HighlightGherkinSyntax();
                    }
                    else
                    {
                        MessageBox.Show("content.xml file not found in the extracted .xmind archive.");
                    }

                    Directory.Delete(tempFolder, true);
                }
                else if (files[0].EndsWith(".mm"))
                {
                    ProcessMmFile(files[0], int.Parse(txtLevel.Text));
                    MergeGherkinScenariosWithExamples();
                    HighlightGherkinSyntax();
                }
                else
                {
                    MessageBox.Show("File is not an .xmind or .mm");
                }

            }
        }

        private bool ConfirmAction(string message, string title)
        {
            // Display a confirmation message box with Yes and No buttons
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Check the result of the dialog
            return result == DialogResult.Yes; // Returns true if Yes is clicked, false otherwise
        }

        private void ExtractXmindFile(string xmindFile, string extractTo)
        {
            string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe";  // Update this path if needed

            // Run 7-Zip to extract the .xmind file
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = sevenZipPath,
                Arguments = $"x \"{xmindFile}\" -o\"{extractTo}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processInfo))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    string errorOutput = process.StandardError.ReadToEnd();
                    MessageBox.Show($"Error extracting .xmind file: {errorOutput}");
                }
            }
        }

        private string ConvertToGherkin(XDocument xmlDoc)
        {
            int level = int.Parse(txtLevel.Text);  // Get the level from txtLevel
            return ConvertXmlToGherkinScenarios(xmlDoc, level);
        }

        private string ConvertXmlToGherkinScenarios(XDocument xmlDoc, int level)
        {
            var scenarios = new StringBuilder();

            // Extract the main topic (center node, feature title)
            XElement rootTopic = xmlDoc.Descendants(ns + "topic").FirstOrDefault(e => e.Parent.Name.LocalName == "sheet");

            if (rootTopic == null)
                return "Root topic not found.";

            string featureTitle = rootTopic.Element(ns + "title")?.Value ?? "Untitled Feature";

            // Find the "Functional Tests" topic
            XElement functionalTestsTopic = rootTopic.Descendants(ns + "topic").FirstOrDefault(e => e.Element(ns + "title")?.Value == "Functional Tests");

            if (functionalTestsTopic == null)
                return "Functional Tests topic not found.";

            // Generate paths from the Functional Tests node, based on the level provided
            var paths = GetAllPaths(functionalTestsTopic);

            // Dictionary to track and group scenarios by title
            var scenarioDictionary = new Dictionary<string, List<List<string>>>();

            // Group paths by scenario title, which is determined by the specified level
            foreach (var path in paths)
            {
                if (path.Count <= level)
                    continue;  // Skip if the path is not deep enough for the specified level

                string scenarioTitle = path[level];  // The title is determined by the node at the specified level
                if (!scenarioDictionary.ContainsKey(scenarioTitle))
                {
                    scenarioDictionary[scenarioTitle] = new List<List<string>>();
                }

                // Add the path to the corresponding scenario title group
                scenarioDictionary[scenarioTitle].Add(path);
            }

            // Dictionary to handle duplicate scenario titles and append sequence numbers
            var scenarioCountDictionary = new Dictionary<string, int>();

            foreach (var scenarioGroup in scenarioDictionary)
            {
                string baseTitle = scenarioGroup.Key;
                int scenarioIndex = scenarioCountDictionary.ContainsKey(baseTitle) ? scenarioCountDictionary[baseTitle] : 0;

                // Group paths by common last-level nodes
                var groupedPaths = new Dictionary<string, List<string>>();
                foreach (var path in scenarioGroup.Value)
                {
                    // Convert the last level nodes (leaf nodes) into Then steps
                    string pathKey = string.Join(" -> ", path.Skip(level + 1));
                    if (!groupedPaths.ContainsKey(pathKey))
                    {
                        groupedPaths[pathKey] = new List<string>(path.Skip(level + 1));
                    }
                }

                foreach (var pathGroup in groupedPaths)
                {
                    // Increment the sequence number for duplicate scenario titles
                    scenarioIndex++;
                    string scenarioTitle = $"{baseTitle}#{scenarioIndex}";

                    scenarios.AppendLine($"    Scenario: {scenarioTitle}");

                    // Append the steps for the scenario
                    foreach (var step in pathGroup.Value)
                    {
                        if (step.StartsWith("user") && !step.StartsWith("user '"))
                        {
                            scenarios.AppendLine($"        When {step}");
                        }
                        else if (step.StartsWith("system"))
                        {
                            scenarios.AppendLine($"        Then {step}");
                        }
                        else
                        {
                            scenarios.AppendLine($"        Given {step}");
                        }

                    }

                    scenarios.AppendLine();
                }

                // Update the scenario count for future titles
                scenarioCountDictionary[baseTitle] = scenarioIndex;
            }

            return $"Feature: {featureTitle}\n\n{scenarios.ToString()}";
        }

        private List<List<string>> GetAllPaths(XElement startTopic)
        {
            var paths = new List<List<string>>();
            var allPathsAtSameLevel = new Dictionary<string, List<string>>(); // Track all nodes at each level

            void Traverse(XElement topic, List<string> currentPath)
            {
                XElement childrenElement = topic.Element(ns + "children");
                XElement topicsElement = childrenElement?.Element(ns + "topics");

                var title = ExtractScenarioTitle(topic);
                currentPath.Add(title);

                if (topicsElement == null || !topicsElement.HasElements)
                {
                    // If there are no more children, add the path to the list
                    paths.Add(new List<string>(currentPath));

                    // Track all nodes at this level
                    var parentKey = string.Join(" -> ", currentPath.Take(currentPath.Count - 1));
                    if (!allPathsAtSameLevel.ContainsKey(parentKey))
                    {
                        allPathsAtSameLevel[parentKey] = new List<string>();
                    }
                    allPathsAtSameLevel[parentKey].Add(title);
                }
                else
                {
                    foreach (XElement child in topicsElement.Elements(ns + "topic"))
                    {
                        Traverse(child, currentPath);
                    }
                }

                currentPath.RemoveAt(currentPath.Count - 1);  // Backtrack
            }

            Traverse(startTopic, new List<string>());

            // Grouping paths based on the leaf nodes found at the same level
            var updatedPaths = new List<List<string>>();

            for (var p = 0; p < paths.Count; p++)
            {
                var parentPath = paths[p].Take(paths[p].Count - 1).ToList();
                var lastNode = paths[p].Last();

                if (allPathsAtSameLevel.ContainsKey(string.Join(" -> ", parentPath)))
                {
                    var siblingLeafs = allPathsAtSameLevel[string.Join(" -> ", parentPath)];
                    if (siblingLeafs.Count > 1 && siblingLeafs.Contains(lastNode))
                    {
                        // Add paths including all sibling leaves as steps
                        foreach (var siblingLeaf in siblingLeafs)
                        {
                            if (siblingLeaf != lastNode)
                            {
                                var extendedPath = new List<string>(parentPath) { siblingLeaf };
                                paths[p].Add(siblingLeaf);
                                paths.RemoveAt(p + 1);
                                // updatedPaths.Add(extendedPath);
                            }
                        }
                    }
                }

                updatedPaths.Add(paths[p]);
            }

            return updatedPaths;
        }



        private string ExtractScenarioTitle(XElement topic)
        {
            return topic.Element(ns + "title")?.Value ?? "Untitled";
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

            txtGherkin.Text = $"Feature: TST-{txtTSTID.Text} - {txtFeatureTitle.Text}\n\n" + string.Join(Environment.NewLine + Environment.NewLine, scenarios);
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
            string scenarioTitle = $"    @manual @{txtTSTID.Text} @TST{txtTSTID.Text}_{FormatScenarioSequence(scenarioSequence)}{Environment.NewLine}    Scenario: {drpDomain.Text.ToString()}.TST{txtTSTID.Text}_{FormatScenarioSequence(scenarioSequence)}.{path.First()} : {txtRequirements.Text}";

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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtGherkin.Text = string.Empty;
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
                " Instructions for Mind Map\n\n" +
                "• Place your scenarios under a node named 'Functional Tests'.\n\n" +
                "• Each node path must denote one scenario from left to right.\n\n" +
                "• Include your preconditions in the first (left most) steps. These will be taken as 'Given' steps and should not start with words 'user' or 'system'.\n\n" +
                "• Every user action must start with the word 'user'. These will be converted as 'When' steps.\n\n" +
                "• Every system result must start with the word 'system'. These will be converted as 'Then' steps.\n\n" +
                "• If you want to skip any node in the mind map path, start those with '~'. Such steps will not be converted to a Gherkin step, and will simply be skipped.\n\n" +
                "• Enter the domain, TST ID, and Requirements before dropping the file so the tool will automatically handle the scenario naming and default tags.\n\n" +
                "• Once converted, copy and paste it into your Gherkin editor and do the modifications needed.\n\n\n" +
                "Support: khettiarachchi@aexis-medical.com",
                "Help", MessageBoxButtons.OK, MessageBoxIcon.Information
            );
        }

        private void txtGherkin_TextChanged(object sender, EventArgs e)
        {
            //HighlightGherkinSyntax();
        }
    }
}
