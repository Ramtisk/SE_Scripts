import paramiko

def execute_remote_script(hostname, username, password, script_path):
    # Estabelecer conexão SSH com o Raspberry Pi
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(hostname, username=username, password=password)

    # Executar o script remoto
    stdin, stdout, stderr = ssh.exec_command(f"python3 {script_path}")
    
    # Ler a saída do comando
    output = stdout.read().decode()
    error = stderr.read().decode()
    
    # Fechar a conexão SSH
    ssh.close()
    
    return output, error

if __name__ == "__main__":
    # Informações de conexão SSH do Raspberry Pi
    hostname = "192.168.242.100"
    username = "user"
    password = "123456789"
    
    # Caminho para o script no Raspberry Pi
    script_path = "/home/user/projeto/motor.py"
    
    # Executar o script remoto
    output, error = execute_remote_script(hostname, username, password, script_path)
    
    # Exibir a saída e os erros
    print("Output:")
    print(output)
    print("Error:")
    print(error)