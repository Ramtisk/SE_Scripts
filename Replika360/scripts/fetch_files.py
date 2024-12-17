import paramiko
import os
import sys
import logging


def fetch_files(remote_directory, local_directory, hostname, username, password):
    logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')

    # Estabelecer conexão SSH com o Raspberry Pi
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(hostname, username=username, password=password)

    # Executar comando 'pwd' para verificar o diretório atual no Raspberry Pi
    stdin, stdout, stderr = ssh.exec_command("pwd")
    current_directory = stdout.read().decode().strip()
    logging.info(f"Current directory on Raspberry Pi: {current_directory}")
    sys.stderr.flush()

    # Abrir sessão SFTP
    sftp = ssh.open_sftp()
    
    # Verificar se o diretório remoto existe
    try:
        sftp.listdir(remote_directory)
        logging.info(f"Remote directory exists: {remote_directory}")
        sys.stderr.flush()
    except IOError:
        logging.error(f"Remote directory does not exist: {remote_directory}")
        sys.stderr.flush()
        sftp.close()
        ssh.close()
        return
    
    # Criar o diretório local se não existir
    if not os.path.exists(local_directory):
        os.makedirs(local_directory)
        logging.info(f"Created local directory: {local_directory}")
    
    # Listar arquivos no diretório remoto e transferi-los para o diretório local
    for file in sftp.listdir(remote_directory):
        remote_file = os.path.join(remote_directory, file).replace("\\", "/")
        local_file = os.path.join(local_directory, file)
        logging.info(f"Fetching {remote_file} to {local_file}")
        sys.stderr.flush()
        sftp.get(remote_file, local_file)
    
    # Fechar sessão SFTP e conexão SSH
    sftp.close()
    ssh.close()
    logging.info("File transfer completed and SSH connection closed")

if __name__ == "__main__":
    # Diretório remoto no Raspberry Pi contendo as fotos
    remote_directory = "/home/user/projeto/Imagens"
    
    # Diretório local no PC onde as fotos serão armazenadas
    local_directory = "C:\\Users\\Ramos\\Pictures\\meshroom"
    
    # Informações de conexão SSH do Raspberry Pi
    hostname = "192.168.0.110"
    username = "user"
    password = "123456789"

    # Buscar arquivos
    fetch_files(remote_directory, local_directory, hostname, username, password)