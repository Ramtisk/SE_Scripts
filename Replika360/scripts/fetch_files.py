import paramiko
import os

def fetch_files(remote_directory, local_directory, hostname, username, password):
    # Estabelecer conexão SSH com o Raspberry Pi
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    ssh.connect(hostname, username=username, password=password)

    # Executar comando 'pwd' para verificar o diretório atual no Raspberry Pi
    stdin, stdout, stderr = ssh.exec_command("pwd")
    current_directory = stdout.read().decode().strip()
    print(f"Current directory on Raspberry Pi: {current_directory}")

    # Abrir sessão SFTP
    sftp = ssh.open_sftp()
    
    # Verificar se o diretório remoto existe
    try:
        sftp.listdir(remote_directory)
        print(f"Remote directory exists: {remote_directory}")
    except IOError:
        print(f"Remote directory does not exist: {remote_directory}")
        sftp.close()
        ssh.close()
        return
    
    # Criar o diretório local se não existir
    if not os.path.exists(local_directory):
        os.makedirs(local_directory)
    
    # Listar arquivos no diretório remoto e transferi-los para o diretório local
    for file in sftp.listdir(remote_directory):
        remote_file = os.path.join(remote_directory, file).replace("\\", "/")
        local_file = os.path.join(local_directory, file)
        print(f"Fetching {remote_file} to {local_file}")
        sftp.get(remote_file, local_file)
    
    # Fechar sessão SFTP e conexão SSH
    sftp.close()
    ssh.close()

if __name__ == "__main__":
    # Diretório remoto no Raspberry Pi contendo as fotos
    remote_directory = "/home/user/projeto/Imagens"
    
    # Diretório local no PC onde as fotos serão armazenadas
    local_directory = "C:\\Users\\Ramos\\Pictures\\meshroom"
    
    # Informações de conexão SSH do Raspberry Pi
    hostname = "192.168.242.100"
    username = "user"
    password = "123456789"

    # Buscar arquivos
    fetch_files(remote_directory, local_directory, hostname, username, password)