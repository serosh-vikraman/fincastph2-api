using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechnipFMC.Common;
using System.Collections.Generic;
using System.Linq;

namespace TechnipFMC.Finapp.Service.API.Helpers
{
    public class GeminiAIHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        public GeminiAIHelper()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(5); // Set timeout for AI processing
            _apiKey = System.Configuration.ConfigurationManager.AppSettings["GeminiApiKey"] ?? "";
        }

        public async Task<string> GenerateReportFromFileAsync(string filePath, string prompt)
        {
            int maxRetries = 3;
            int retryDelayMs = 2000; // 2 seconds

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    if (string.IsNullOrEmpty(_apiKey))
                    {
                        throw new Exception("Gemini AI API key is not configured. Please add 'GeminiApiKey' to your configuration.");
                    }

                    // Read the file content
                    string fileContent = ReadFileContent(filePath);
                    
                    // Format the content based on file type
                    string formattedContent = FormatFileContent(filePath, fileContent);
                    
                    // Log the original content size
                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", "GenerateReportFromFileAsync", 
                        $"Original content size: {formattedContent.Length} characters");
                    
                    // Check if content is too large (Gemini has limits)
                    if (formattedContent.Length > 150000) // Increased to 150K character limit for financial data
                    {
                        var originalLength = formattedContent.Length;
                        
                        // For JSON financial data, implement smart sampling
                        if (filePath.ToLower().EndsWith(".json"))
                        {
                            try
                            {
                                // Parse the JSON data
                                var jsonData = JsonConvert.DeserializeObject<List<dynamic>>(formattedContent);
                                
                                if (jsonData != null && jsonData.Count > 0)
                                {
                                    // Smart sampling: Keep all records but limit to most important data types
                                    var sampledData = SmartSampleFinancialData(jsonData);
                                    formattedContent = JsonConvert.SerializeObject(sampledData, Formatting.Indented);
                                    
                                    RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", "GenerateReportFromFileAsync", 
                                        $"Smart sampled financial data: {jsonData.Count} original records -> {sampledData.Count} sampled records");
                                }
                                else
                                {
                                    // Fallback to truncation if parsing fails
                                    formattedContent = formattedContent.Substring(0, 150000) + "\n\n[Content truncated due to size limits]";
                                }
                            }
                            catch
                            {
                                // Fallback to truncation if JSON parsing fails
                                formattedContent = formattedContent.Substring(0, 150000) + "\n\n[Content truncated due to size limits]";
                            }
                        }
                        else
                        {
                            // For non-JSON files, use truncation
                            formattedContent = formattedContent.Substring(0, 150000) + "\n\n[Content truncated due to size limits]";
                        }
                        
                        RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", "GenerateReportFromFileAsync", 
                            $"Content processed from {originalLength} to {formattedContent.Length} characters");
                    }
                    
                    // Create the request payload
                    var requestPayload = new
                    {
                        contents = new[]
                        {
                            new
                            {
                                parts = new[]
                                {
                                    new
                                    {
                                        text = $"{prompt}\n\nFile Data:\n{formattedContent}"
                                    }
                                }
                            }
                        },
                        generationConfig = new
                        {
                            temperature = 0.3,
                            topK = 40,
                            topP = 0.95,
                            maxOutputTokens = 8192
                        }
                    };

                    // Serialize the request
                    string jsonPayload = JsonConvert.SerializeObject(requestPayload);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Add API key to URL
                    string requestUrl = $"{_apiUrl}?key={_apiKey}";

                    // Send request to Gemini AI
                    var response = await _httpClient.PostAsync(requestUrl, content);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(responseContent);
                        
                        if (geminiResponse?.candidates?.Length > 0)
                        {
                            return geminiResponse.candidates[0].content.parts[0].text;
                        }
                        else
                        {
                            throw new Exception("No response generated from Gemini AI");
                        }
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        
                        // Check if it's a 503 overload error
                        if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable || 
                            errorContent.Contains("overloaded") || 
                            errorContent.Contains("UNAVAILABLE"))
                        {
                            if (attempt < maxRetries)
                            {
                                //RaintelsLogManager.Error(ex,"TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", 
                                //    "GenerateReportFromFileAsync", 
                                //    $"Gemini AI overloaded, retrying in {retryDelayMs}ms. Attempt {attempt}/{maxRetries}");
                                
                                await Task.Delay(retryDelayMs * attempt); // Exponential backoff
                                continue;
                            }
                            else
                            {
                                throw new Exception($"Gemini AI service is overloaded after {maxRetries} attempts. Please try again later.");
                            }
                        }
                        
                        throw new Exception($"Gemini AI API error: {response.StatusCode} - {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    if (attempt == maxRetries)
                    {
                        RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", "GenerateReportFromFileAsync", $"File: {filePath}, Final attempt failed");
                        throw;
                    }
                    else
                    {
                        RaintelsLogManager.Error(ex,"TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", 
                            "GenerateReportFromFileAsync", 
                            $"Attempt {attempt} failed, retrying. Error: {ex.Message}");
                        
                        await Task.Delay(retryDelayMs * attempt);
                    }
                }
            }
            
            throw new Exception("Failed to generate report after all retry attempts");
        }

        private string ReadFileContent(string fileName)
        {
            try
            {
                string sharedReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"].ToString();
                string tempReportPath = System.Configuration.ConfigurationManager.AppSettings["TempReportPath"].ToString();
                
                // Try to find the file in both locations
                string fullPath = Path.Combine(tempReportPath, fileName);
                

                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", "ReadFileContent", $"Successfully found file at: {fullPath}");
                return File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", "ReadFileContent", $"File: {fileName}");
                throw;
            }
        }

        private string FormatFileContent(string filePath, string content)
        {
            try
            {
                string extension = Path.GetExtension(filePath).ToLower();
                
                switch (extension)
                {
                    case ".json":
                        // Pretty print JSON for better readability
                        var jsonObj = JsonConvert.DeserializeObject(content);
                        return JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    
                    case ".txt":
                    case ".csv":
                        return content;
                    
                    default:
                        // Try to parse as JSON, if it fails, return as is
                        try
                        {
                            var obj = JsonConvert.DeserializeObject(content);
                            return JsonConvert.SerializeObject(obj, Formatting.Indented);
                        }
                        catch
                        {
                            return content;
                        }
                }
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", "FormatFileContent", $"File: {filePath}");
                return content; // Return original content if formatting fails
            }
        }

        // Smart sampling method for financial data
        private List<dynamic> SmartSampleFinancialData(List<dynamic> originalData)
        {
            try
            {
                // Priority data types to keep (most important for analysis)
                var priorityDataTypes = new[] { "Revenue", "Gross Margin", "Man Hours" };
                
                // Get all unique projects
                var projects = originalData
                    .Select(d => d.ProjectName?.ToString())
                    .Where(p => !string.IsNullOrEmpty(p))
                    .Distinct()
                    .ToList();
                
                // Get all unique scenarios
                var scenarios = originalData
                    .Select(d => d.ScenarioTypeName?.ToString())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .ToList();
                
                // Sample strategy:
                // 1. Keep ALL records for priority data types (Revenue, Gross Margin, Man Hours)
                // 2. For other data types, keep only a sample of projects and scenarios
                // 3. This ensures we have complete data for the most important metrics
                
                var sampledData = new List<dynamic>();
                
                // Add all priority data type records
                foreach (var record in originalData)
                {
                    var dataType = record.ScenarioDataType?.ToString();
                    if (priorityDataTypes.Any(p => p == dataType))
                    {
                        sampledData.Add(record);
                    }
                }
                
                // For non-priority data types, sample strategically
                var nonPriorityRecords = originalData
                    .Where(d => !priorityDataTypes.Any(p => p == d.ScenarioDataType?.ToString()))
                    .ToList();
                
                if (nonPriorityRecords.Count > 0)
                {
                    // Take a sample of non-priority records (up to 100 records)
                    var sampleSize = Math.Min(nonPriorityRecords.Count, 100);
                    var random = new Random();
                    var sampledNonPriority = nonPriorityRecords
                        .OrderBy(x => random.Next())
                        .Take(sampleSize)
                        .ToList();
                    
                    sampledData.AddRange(sampledNonPriority);
                }
                
                // Log sampling results
                RaintelsLogManager.Info("TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", "SmartSampleFinancialData", 
                    $"Smart sampling: {originalData.Count} original -> {sampledData.Count} sampled records");
                
                return sampledData;
            }
            catch (Exception ex)
            {
                RaintelsLogManager.Error(ex, "TechnipFMC.Finapp.Service.API.Helpers.GeminiAIHelper", "SmartSampleFinancialData", 
                    "Error in smart sampling, returning original data");
                return originalData;
            }
        }

        // Helper method to generate a default prompt for financial reports
        public string GetDefaultFinancialReportPrompt()
        {
            return @"Please analyze the financial data provided and generate a comprehensive HTML report that includes:

1. **Executive Summary**: Key financial highlights and trends
2. **Financial Performance Analysis**: 
   - Revenue analysis
   - Cost analysis
   - Profitability metrics
   - Key performance indicators
3. **Trend Analysis**: Quarter-over-quarter and year-over-year comparisons
4. **Risk Assessment**: Identify potential financial risks and opportunities
5. **Recommendations**: Actionable insights and recommendations for improvement
6. **Visual Insights**: Include HTML code for interactive charts using Chart.js

Please format the report as a complete HTML document with:
- Professional styling using CSS
- Interactive charts using Chart.js library
- Responsive design
- Executive-friendly layout
- Include the following chart types:
  * Line charts for revenue and gross margin trends
  * Bar charts for quarterly comparisons
  * Pie charts for revenue distribution
  * Scatter plots for efficiency analysis

Make sure the HTML is complete and self-contained with all necessary CSS and JavaScript included.";
        }

        // Helper method to generate a custom prompt
        public string GetCustomPrompt(string reportType, string additionalRequirements = "")
        {
            string basePrompt = GetDefaultFinancialReportPrompt();
            
            if (!string.IsNullOrEmpty(additionalRequirements))
            {
                basePrompt += $"\n\nAdditional Requirements:\n{additionalRequirements}";
            }
            
            return basePrompt;
        }

        // Helper method to generate Word document prompt
        public string GetWordDocumentPrompt()
        {
            return @"Please analyze the financial data provided and generate a comprehensive report in Markdown format that includes:

1. **Executive Summary**: Key financial highlights and trends
2. **Financial Performance Analysis**: 
   - Revenue analysis
   - Cost analysis
   - Profitability metrics
   - Key performance indicators
3. **Trend Analysis**: Quarter-over-quarter and year-over-year comparisons
4. **Risk Assessment**: Identify potential financial risks and opportunities
5. **Recommendations**: Actionable insights and recommendations for improvement
6. **Visual Insights**: Detailed descriptions of recommended charts and graphs

Please format the report in Markdown with:
- Clear headings and subheadings
- Bullet points and numbered lists
- Professional formatting
- Detailed chart descriptions that can be converted to visual charts
- Executive-friendly language

The report should be suitable for conversion to Word document format with embedded charts.";
        }

        // Helper method to generate enhanced HTML prompt with better chart integration
        public string GetEnhancedHTMLPrompt()
        {
            return @"Generate a comprehensive financial performance report in HTML format with embedded Chart.js visualizations. 

IMPORTANT: You MUST include the complete JavaScript code to initialize and create the charts using the provided financial data. The charts should be fully functional and interactive.

Requirements:
1. Create a PROFESSIONAL HTML report with EXECUTIVE-LEVEL styling including:
   - Professional header with navigation
   - Executive summary section with highlighted styling
   - Financial analysis sections with proper spacing and borders
   - Professional footer
   - Responsive design for all screen sizes
   - Print-friendly CSS
   - Professional color scheme (blues, grays, accent colors)
   - Proper typography and spacing
   - Box shadows and gradients for visual appeal
   - Professional layout with containers and sections

2. Include Chart.js library from CDN

3. Parse the provided JSON financial data

4. Create at least 4 interactive charts:
   - Revenue and Gross Margin trends over quarters (line chart)
   - Quarterly performance comparison across projects (bar chart)
   - Revenue distribution by project (pie chart)
   - Efficiency metrics scatter plot (scatter chart)

5. Include comprehensive financial analysis sections:
   - Executive Summary with professional styling
   - Financial Performance Analysis
   - Trend Analysis
   - Risk Assessment
   - Recommendations

6. The JavaScript code MUST:
   - Parse the JSON data properly
   - Extract quarterly data (Q1New through Q12New)
   - Group data by ScenarioDataType (Revenue, Gross Margin, Man Hours)
   - Create fully functional Chart.js charts with proper configurations
   - Include hover effects and legends
   - Handle the data structure correctly

7. Professional styling requirements:
   - Use professional color schemes (blues, grays, accent colors)
   - Include header with navigation menu
   - Add footer with company branding
   - Use proper CSS for sections, containers, and spacing
   - Make it responsive for mobile and desktop
   - Include print styles
   - Use professional fonts and typography
   - Add visual elements like gradients, shadows, and borders

Format the response as complete HTML with embedded JavaScript and CSS that will work immediately when opened in a browser and look professional for executive presentations.";
        }
    }

    // Response models for Gemini AI
    public class GeminiResponse
    {
        public Candidate[] candidates { get; set; }
        public PromptFeedback promptFeedback { get; set; }
    }

    public class Candidate
    {
        public Content content { get; set; }
        public string finishReason { get; set; }
        public int index { get; set; }
        public SafetyRating[] safetyRatings { get; set; }
    }

    public class Content
    {
        public Part[] parts { get; set; }
        public string role { get; set; }
    }

    public class Part
    {
        public string text { get; set; }
    }

    public class PromptFeedback
    {
        public SafetyRating[] safetyRatings { get; set; }
    }

    public class SafetyRating
    {
        public string category { get; set; }
        public string probability { get; set; }
    }
} 