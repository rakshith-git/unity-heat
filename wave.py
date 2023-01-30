from turtle import *
import numpy as np
import time
tracer(0)
dx=0.01
dt=0.001
c=1
a=100
SPEED=7
l=10
OFFSET=500
Stop_time=30
r=c*dt/dx
n=l/dx+1
current =np.arange(0,l,dx)
for i in range(current.size):
        current[i]=4*np.cos(0.005*i)*np.sin(0.03*i)



pensize(10)
past=current
future=current
future[0]=0
future[future.size-1]=0
choice=1
    


if(choice==0):
    for t in range(0,int(500*(1/dt)),1):
        if(t%(SPEED)==0):
            for j in range(current.size):
                if j%8==0:
                    color("black")
                elif j%4==0:
                    color("red")
                goto(-OFFSET+j,a*current[j])
            update()
            clear()
            pu()
            goto(-OFFSET,current[0])
            pd()
        
        for i in range(1,future.size-1):
            future[0]=0
            future[i]=c*(current[i-1]+current[i+1])/2
            future[future.size-1]=0
            past=current
            current=future
            #print(future[future.size-1])
if(choice==1):
    for t in range(0,int(50*(1/dt)),1):
        if(t%(SPEED)==0):
            for j in range(current.size):
                goto(-OFFSET+j,a*current[j])
            update()
            clear()
            pu()
            goto(-OFFSET,current[0])
            pd()
        
        for i in range(1,future.size-1):
            #future[0]=0.05*np.sin(t*0.09)
            if i==1:
                future[i]=(current[i-1]+current[i+1])/2
            future[i]=(current[i-1]+current[i+1])-past[i]
            #future[future.size-1]=0
            past=current
            current=future
            #print(future[future.size-1])

