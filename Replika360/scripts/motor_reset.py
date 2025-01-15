import paramiko
import threading
import time
import logging
import sys

# Configurar logging
logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(levelname)s - %(message)s')

def execute_remote_script(hostname, username, password, script_path):
    # Estabelecer conexão SSH com o Raspberry Pi
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(hostname, username=username, password=password)

    # Executar o script remoto
    stdin, stdout, stderr = ssh.exec_command(f"python3 {script_path}")
    
    # Função para monitorar a execução do script
    def monitor_execution():
        while not stdout.channel.exit_status_ready():
            if stdout.channel.recv_ready():
                output = stdout.channel.recv(1024).decode()
                logging.info(output)
                sys.stderr.flush()
            if stderr.channel.recv_stderr_ready():
                error = stderr.channel.recv_stderr(1024).decode()
                logging.error(error)
                sys.stderr.flush()
            
            time.sleep(1)
    
    # Iniciar a thread de monitoramento
    monitor_thread = threading.Thread(target=monitor_execution)
    monitor_thread.start()
    
    return ssh, monitor_thread

    

def main():
    # Informações de conexão SSH do Raspberry Pi
    hostname = "192.168.242.100"
    username = "user"
    password = "123456789"
    
    # Caminho para o script no Raspberry Pi
    script_path = "/home/user/projeto/motor-reset.py"

    logging.info("Iniciando execução do script remoto")
    ssh, monitor_thread = execute_remote_script(hostname, username, password, script_path)
    

if __name__ == "__main__":
    main()