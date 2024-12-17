import RPi.GPIO as GPIO
import time
from picamera2 import Picamera2
from time import sleep

#Define GPIO pins
IN1 = 17
IN2 = 18
IN3 = 27
IN4 = 22
dire =True
IN11 = 23
IN21 = 24
IN31 = 25
IN41 = 26
i = 1
#Set up GPIO mode
GPIO.setmode(GPIO.BCM)
GPIO.setup(IN1, GPIO.OUT)
GPIO.setup(IN2, GPIO.OUT)
GPIO.setup(IN3, GPIO.OUT)
GPIO.setup(IN4, GPIO.OUT)
GPIO.setup(IN11, GPIO.OUT)
GPIO.setup(IN21, GPIO.OUT)
GPIO.setup(IN31, GPIO.OUT)
GPIO.setup(IN41, GPIO.OUT)

#Incializa a câmara
picam2 = Picamera2()
#configura
config = picam2.create_still_configuration()
picam2.configure(config)
#Pré
picam2.start()

#Define step sequence
step_sequence = [
	#[1, 0, 0, 0],
	[1, 1, 0, 0],
	#[0, 1, 0, 0],
	[0, 1, 1, 0],
	#[0, 0, 1, 0],
	[0, 0, 1, 1],
	#[0, 0, 0, 1],
	[1, 0, 0, 1],
]

def set_step(pins):
	GPIO.output(IN1, pins[0])
	GPIO.output(IN2, pins[1])
	GPIO.output(IN3, pins[2])
	GPIO.output(IN4, pins[3])
def set_stepBRACO(pins):
	GPIO.output(IN11,  pins[0])
	GPIO.output(IN21,  pins[1])
	GPIO.output(IN31,  pins[2])
	GPIO.output(IN41,  pins[3])

def move_stepper_base(steps, delay,dire):
	for _ in range(steps):
		for step in (step_sequence if dire else reversed(step_sequence)):			
			set_step(step)
			time.sleep(delay)			
def move_stepper_braco(steps, delay,dire):
	for _ in range(steps):
		for step in (step_sequence if dire else reversed(step_sequence)):			
			set_stepBRACO(step)
			time.sleep(delay)			
			
try:
		
		#foto inicial			
		picam2.capture_file(f"/home/user/projeto/Imagens/foto{i}.jpg")
		print(f"Foto {i} tirada")
		i = i+1	

		for descida in range(1):
			#desce
			move_stepper_braco(605, 0.005,False) # desce
			for volta in range(20):
				#tira foto
				sleep(0.1)
				#Captura a imagem		
				picam2.capture_file(f"/home/user/projeto/Imagens/foto{i}.jpg")
				print(f"Foto {i} tirada")
				i = i+1
				#rodabase
				move_stepper_base(60, 0.005,True) #roda melhor aqui 1024 total
				time.sleep(1)	
		
		
		print("Acabou!")
		
		
		
		
		#move_stepper_braco(3024, 0.005,True) # sobe

except KeyboardInterrupt:
	GPIO.cleanup()
	

