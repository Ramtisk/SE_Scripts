using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronPython.Hosting;
using System.Threading;
using static IronPython.Modules._ast;

namespace WindowsForm_replika360
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Paths to Meshroom and folders
            string meshroomPath = @"C:\Users\Ramos\Documents\Meshroom-2023.3.0\meshroom_batch.exe"; // Update path
            string inputFolder = @"C:\Users\Ramos\Pictures\meshroom";
            string outputFolder = @"C:\Users\Ramos\Documents\OutputMeshroomProjeto";

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
                    Arguments = "/k" + arguments, // /c tells cmd.exe to execute the command and then terminate                   
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Paths
            string blenderPath = @"C:\Program Files\Blender Foundation\Blender 4.2\blender.exe"; // Update this to your Blender path
            string scriptFile = @"C:\Users\Ramos\Desktop\escola\Replika360\scripts\cript_original.py"; // Path to your Python script

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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(fetch_files);

            thread1.Start();

        }

        private void fetch_files()
        {
            // Caminho para o diretório onde os scripts estão localizados
            string scriptsPath = @"C:\Users\Ramos\Desktop\escola\Replika360\scripts\fetch_files.py";
            string pythonPath = @"C:\Users\Ramos\AppData\Local\Programs\Python\Python312\python.exe"; // Caminho para o Python

            var processStartInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = scriptsPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBox1.AppendText(e.Data + Environment.NewLine);
                    });
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBox1.AppendText($"{e.Data}" + Environment.NewLine);
                    });
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread thread2 = new Thread(production);

            thread2.Start();

        }


        private void production()
        {
            // Caminho para o diretório onde os scripts estão localizados
            string scriptsPath = @"C:\Users\Ramos\Desktop\escola\Replika360\scripts\production_script.py";
            string pythonPath = @"C:\Users\Ramos\AppData\Local\Programs\Python\Python312\python.exe"; // Caminho para o Python


            var processStartInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = scriptsPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBox1.AppendText(e.Data + Environment.NewLine);
                    });
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBox1.AppendText($"{e.Data}" + Environment.NewLine);
                    });
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            ////////////////////////////////////////////////Meshroom/////////////////////////////////////////////////////////////////////

            this.Invoke((MethodInvoker)delegate
                {
                    textBox1.AppendText($" ##################### - Pode comecar Meshroom - ################ " + Environment.NewLine);
                    textBox1.AppendText($" ##################### - O processo Meshroom vai comecar - ################ " + Environment.NewLine);
                });

            // Caminhos para o Meshroom e diretórios
            string meshroomPath = @"C:\Users\Ramos\Documents\Meshroom-2023.3.0\meshroom_batch.exe"; // Atualizar caminho
            string inputFolder = @"C:\Users\Ramos\Pictures\meshroom";
            string baseOutputFolder = @"C:\Users\Ramos\Documents\OutputMeshroomProjeto";

            // Validar caminhos
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

            // Criar a pasta base se não existir
            if (!Directory.Exists(baseOutputFolder))
            {
                Directory.CreateDirectory(baseOutputFolder);
            }

            // Gerar nova subpasta incremental
            string outputFolder;
            int projectNumber = 1;
            do
            {
                outputFolder = Path.Combine(baseOutputFolder, $"Projeto{projectNumber}");
                projectNumber++;
            } while (Directory.Exists(outputFolder));

            // Criar a nova pasta para o projeto
            Directory.CreateDirectory(outputFolder);

            // Start Meshroom processing
            try
            {
                // Build the command-line arguments for Meshroom
                //string arguments = $"--input \"{inputFolder}\" --output \"{outputFolder}\" --forceCompute --params \"Texturing.textureFileType=png\"";
                string arguments = $"{meshroomPath} --input \"{inputFolder}\" --output \"{outputFolder}\" --forceCompute --paramOverrides Texturing_1.colorMapping.colorMappingFileType=png";

                ProcessStartInfo startInfo1 = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\System32\cmd.exe",
                    Arguments = "/k" + arguments, // /c tells cmd.exe to execute the command and then terminate                   
                    UseShellExecute = true,
                    // RedirectStandardOutput = true,
                    // RedirectStandardError = true,
                    CreateNoWindow = false
                };

                using (Process process2 = Process.Start(startInfo1))
                {

                    this.Invoke((MethodInvoker)delegate
                    {
                        textBox1.AppendText($" ## Meshroom -> Processing started. Please wait... - ################ " + Environment.NewLine);
                    });

                    process2.WaitForExit();

                    if (process2.ExitCode == 0)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            textBox1.AppendText($" ## Meshroom -> Processing completed successfully! - ################ " + Environment.NewLine);
                        });
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            textBox1.AppendText($" ## Meshroom -> Processing failed. Check error logs. - ################ " + Environment.NewLine);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /////////////////////////////////////////////////Blender/////////////////////////////////////////////////////////////////////

            this.Invoke((MethodInvoker)delegate
            {
                textBox1.AppendText("##################### - Starting Blender - ################" + Environment.NewLine);
            });

            string blenderPath = @"C:\Program Files\Blender Foundation\Blender 4.2\blender.exe";
            string blenderScriptFile = @"C:\Users\Ramos\Desktop\escola\Replika360\scripts\cript.py";

            if (!File.Exists(blenderPath))
            {
                MessageBox.Show("Error: Blender executable not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string objFilePath = Path.Combine(outputFolder, "texturedMesh.obj"); // Meshroom OBJ output
            if (!File.Exists(objFilePath))
            {
                MessageBox.Show("Error: OBJ file not found in Meshroom output folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string blendFilePath = Path.Combine(outputFolder, "scene.blend"); // Output .blend file

            try
            {
                string arguments = $"--background --python \"{blenderScriptFile}\" -- \"{objFilePath}\" \"{blendFilePath}\"";

                ProcessStartInfo startInfo2 = new ProcessStartInfo
                {
                    FileName = blenderPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process3 = Process.Start(startInfo2))
                {
                    process3.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                textBox1.AppendText($"Blender: {e.Data}" + Environment.NewLine);
                            });
                        }
                    };
                    process3.BeginOutputReadLine();
                    process3.WaitForExit();

                    if (process3.ExitCode == 0)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            textBox1.AppendText("Blender: Scene created successfully." + Environment.NewLine);
                        });
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            textBox1.AppendText("Blender: Failed to create the scene." + Environment.NewLine);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during Blender processing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



    }

        private void Reset_Motor_Click(object sender, EventArgs e)
        {
            Thread thread3 = new Thread(Motor_reset);

            thread3.Start();
        }

        private void Motor_reset()
        {
            // Caminho para o diretório onde os scripts estão localizados
            string scriptsPath = @"C:\Users\Ramos\Desktop\escola\Replika360\scripts\motor_reset.py";
            string pythonPath = @"C:\Users\Ramos\AppData\Local\Programs\Python\Python312\python.exe"; // Caminho para o Python


            var processStartInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = scriptsPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBox1.AppendText(e.Data + Environment.NewLine);
                    });
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        textBox1.AppendText($"{e.Data}" + Environment.NewLine);
                    });
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }



    }
}
