
from __future__ import absolute_import
from __future__ import division
from __future__ import print_function


#from matching import get_match
#import argparse
import sys
import time
import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'
import numpy as np
import tensorflow as tf

from flask import Flask, jsonify
app = Flask(__name__)
from flask import abort

def load_graph(model_file):
  graph = tf.Graph()
  graph_def = tf.GraphDef()

  with open(model_file, "rb") as f:
    graph_def.ParseFromString(f.read())
  with graph.as_default():
    tf.import_graph_def(graph_def)
    

  return graph

def read_tensor_from_image_file(file_name, input_height=299, input_width=299,
				input_mean=0, input_std=255):
  input_name = "file_reader"
  output_name = "normalized"
  file_reader = tf.read_file(file_name, input_name)
  if file_name.endswith(".png"):
    image_reader = tf.image.decode_png(file_reader, channels = 3,
                                       name='png_reader')
  elif file_name.endswith(".gif"):
    image_reader = tf.squeeze(tf.image.decode_gif(file_reader,
                                                  name='gif_reader'))
  elif file_name.endswith(".bmp"):
    image_reader = tf.image.decode_bmp(file_reader, name='bmp_reader')
  else:
    image_reader = tf.image.decode_jpeg(file_reader, channels = 3,
                                        name='jpeg_reader')
  float_caster = tf.cast(image_reader, tf.float32)
  dims_expander = tf.expand_dims(float_caster, 0);
  resized = tf.image.resize_bilinear(dims_expander, [input_height, input_width])
  normalized = tf.divide(tf.subtract(resized, [input_mean]), [input_std])
  sess = tf.Session()
  result = sess.run(normalized)

  return result


def load_labels(label_file):
  label = []
  proto_as_ascii_lines = tf.gfile.GFile(label_file).readlines()
  for l in proto_as_ascii_lines:
    label.append(l.rstrip())
  return label
import simplejson as json
import cv2
import numpy as np
import pyodbc 
server = 'tcp:souqzone.database.windows.net' 
database = 'SouqZone'
username = 'souqzone_admin' 
password = 'Az@00000' 
#cnxn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER='+server+';DATABASE='+database+';UID='+username+';PWD='+ password)
cnxn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER=DESKTOP-J0TTRBH\SQLEXPRESS ;DATABASE=test99;Trusted_Connection=yes;')
cursor = cnxn.cursor()
LIST=[]

def get_match(imagePath,category_name):
    
    img1 = cv2.imread(imagePath, cv2.IMREAD_GRAYSCALE)
    img_resize=cv2.resize(img1,(256,197))
    
    
    
    cat=category_name
    list_name=""
    if cat=="watch":
      
      cursor.execute("select picture from product where categoryID=1") 
      row = cursor.fetchone() 
      while row:
        LIST.append(row[0])
        row = cursor.fetchone()
    elif cat=="shoes":
        cursor.execute("select picture from product where categoryID=2") 
        row = cursor.fetchone() 
        while row:
            

            LIST.append(row[0])
            row = cursor.fetchone()
    elif cat=="bag":
        cursor.execute("select picture from product where categoryID=3") 
        row = cursor.fetchone() 
        while row:
            

            LIST.append(row[0])
            row = cursor.fetchone()
    print(LIST)
    path="C:/Users/khale/Desktop/todo-api/todo-api/content/images/"
    orb = cv2.ORB_create()
    kp1, des1 = orb.detectAndCompute(img_resize, None)
    i=0
    summ=0
    similarity=[]
    image_name=[]
    percent=[]
    
    i=0
    while i<len(LIST):
        img2 = cv2.imread(path+LIST[i], cv2.IMREAD_GRAYSCALE)
        img_resize2 = cv2.resize(img2, (256, 197))
        kp2, des2 = orb.detectAndCompute(img_resize2, None)
        #cv2.imshow(l[i], img_resize2)
        # Brute Force Matching
        bf = cv2.BFMatcher(cv2.NORM_HAMMING, crossCheck=True)
        matches = bf.match(des1, des2)
        matches = sorted(matches, key=lambda x: x.distance)
        if((len(matches)*200)/len(kp1)>=50):
            similarity.append(LIST[i])
        if((len(matches)*200)/len(kp1)>=50):
          
           percent.append((len(matches)*100)/len(kp1))
        else:
          percent.append((len(matches)*200)/len(kp1))
          

        print((len(matches)*100)/len(kp1),'%',LIST[i])
        summ=summ+len(matches)
      
      #image_name.append(LIST[i])
      #percent.append((len(matches)*100)/len(kp1))

        matching_result = cv2.drawMatches(img_resize, kp1, img_resize2, kp2, matches, None, flags=2)
        i=i+1

    matched_imgs=[]
    for i in similarity:
      matched_imgs.append(i)





    ID=""
    if cat=="watch":
      ID="1"
      
    elif cat=="shoes":
        ID="2"
    elif cat=="bag":
        ID="3"
    cursor.execute("select picture ,price,shopOwnerID from product where categoryID="+ID)

    row = cursor.fetchone()
#while row:
 #   print(row[0])
    
  #  row = cursor.fetchone()


    i=0
    songs_as_dict = []
    while row:
      
      song_as_dict = {
            'picture' : row[0],
            'price' : json.dumps(row[1], use_decimal=True),
            'shopOwnerID' : row[2],'ratio':percent[i]}
      i=i+1
    
      songs_as_dict.append(song_as_dict)
      row = cursor.fetchone()


    end_result=[]
    for i in matched_imgs:
      
      for j in songs_as_dict:
        
        if i== j['picture']:
            end_result.append(j)


    #return jsonify({'result':end_result})
    #return jsonify(end_result)
    return end_result
      

    
    
    
    




@app.route('/todo/api/v1.0/name/<string:name>', methods=['GET'])
def get_name(name):
    
    return jsonify({'task': name})


def add(x,y):
    return x+y
@app.route('/todo/api/v1.0/xx/<string:name>', methods=['GET'])
def get_n(name):
    s=add(2,3)
    return jsonify({'sum': s})




#@app.route('/todo/api/v1.0/get_images/<string:img_name>', methods=['GET'])
#def get_images(img_name):
#if __name__ == '__main__':
    
imgname='3MC.jpg'
  

image=imgname
file_name ="C:/Users/khale/Desktop/todo-api/todo-api/content/images/"+image
model_file = "C:/Users/khale/Desktop/todo-api/todo-api/tensorflow-for-poets-2/tf_files/retrained_graph.pb"
label_file = "C:/Users/khale/Desktop/todo-api/todo-api/tensorflow-for-poets-2/tf_files/retrained_labels.txt"
input_height = 224
input_width = 224
input_mean = 128
input_std = 128
input_layer = "input"
output_layer = "final_result"

  

graph = load_graph(model_file)
t = read_tensor_from_image_file(file_name,
                                  input_height=input_height,
                                  input_width=input_width,
                                  input_mean=input_mean,
                                  input_std=input_std)



  

input_name = "import/" + input_layer
output_name = "import/" + output_layer
input_operation = graph.get_operation_by_name(input_name);
output_operation = graph.get_operation_by_name(output_name);

with tf.Session(graph=graph) as sess:
    start = time.time()
    results = sess.run(output_operation.outputs[0],
                      {input_operation.outputs[0]: t})
    end=time.time()
results = np.squeeze(results)

top_k = results.argsort()[-5:][::-1]
labels = load_labels(label_file)
f="{:0.5f}"
a=float(f.format(results[0]))
b=float(f.format(results[1]))
c=float(f.format(results[2]))
#print(a)
#print(b)
#print(c)
maxx=max(a,b,c)
category_name=""
if maxx==a:
    print(labels[0])
    category_name=labels[0]
elif maxx==b:
    
    print(labels[1])
    category_name=labels[1]
else:
    print(labels[2])
    category_name=labels[2]

print(file_name , category_name)
list_images=get_match(file_name,category_name)
  



  
print(list_images)




