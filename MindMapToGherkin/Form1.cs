using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text;

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

            txtGherkin.Text = "Feature: TST-" + txtTSTID.Text + " - Enter your feature title here\n\n" + string.Join(Environment.NewLine + Environment.NewLine, scenarios);
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

    }
}
