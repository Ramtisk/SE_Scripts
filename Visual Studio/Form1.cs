using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Meshroom_auto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Paths to Meshroom and folders
            string meshroomPath = @"C:\Users\pedro\Downloads\Meshroom-2023.3.0\meshroom_batch.exe"; // Update path
            string desktopPath = @"C:\Users\pedro\Desktop";
            string inputFolder =@"C:\Users\pedro\Desktop\meh";
            string outputFolder = @"C:\Users\pedro\Desktop\meh";

            // Validate paths
            if (!File.Exists(meshroomPath))
            {
                MessageBox.Show("Error: Meshroom executable not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(inputFolder))
            {
                MessageBox.Show("Error: Input folder not found. Create a 'MeshroomInput' folder on your Desktop.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Start Meshroom processing
            try
            {
                // Build the command-line arguments for Meshroom
                //string arguments = $"--input \"{inputFolder}\" --output \"{outputFolder}\" --forceCompute --params \"Texturing.textureFileType=png\"";
                string arguments = $"{meshroomPath} --input \"{inputFolder}\" --output \"{outputFolder}\" --forceCompute --paramOverrides Texturing_1.colorMapping.colorMappingFileType=png";
               
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\System32\cmd.exe",
                     Arguments =  "/k" +arguments, // /c tells cmd.exe to execute the command and then terminate                   
                    UseShellExecute = true,
                   // RedirectStandardOutput = true,
                  // RedirectStandardError = true,
                    CreateNoWindow = false
                };

                using (Process process = Process.Start(startInfo))
                {
                    label1.Text = "Processing started. Please wait...";
                  
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        label1.Text = "Processing completed successfully!";
                    }
                    else
                    {
                        label1.Text = $"Processing failed. Check error logs.";                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            // Paths
            string blenderPath = @"C:\Program Files\Blender Foundation\Blender 4.2\blender.exe"; // Update this to your Blender path
            string scriptFile = @"C:\Users\pedro\Desktop\meh\cript.py"; // Path to your Python script

            // Validate paths
            if (!File.Exists(blenderPath))
            {
                Console.WriteLine("Blender executable not found.");
                return;
            }
            if (!File.Exists(scriptFile))
            {
                Console.WriteLine("Python script not found.");
                return;
            }

            // Build the command to execute
            string arguments = $" --python \"{scriptFile}\"";

            // Start Blender
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = blenderPath,
                Arguments = arguments,
                UseShellExecute = true,
                CreateNoWindow = false
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    Console.WriteLine("Blender script executed successfully.");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
