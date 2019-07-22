from __future__ import absolute_import
from __future__ import division
from __future__ import print_function
#from matching import get_match
import sys


#import argparse
import sys
import time
import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'
import numpy as np
import tensorflow as tf


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

imgname=''
from flask import Flask, jsonify

from flask import abort
app = Flask(__name__)

@app.route('/todo/api/v1.0/get_name/<string:img_name>', methods=['GET'])
def get_name(img_name):
    imgname=img_name
    return jsonify({'name': img_name})
@app.route('/todo/api/v1.0/ddd/<string:x>', methods=['GET'])
def get_result(x):
    imgname=x
    return jsonify({'name': imgname})






@app.route('/todo/api/v1.0/get_images/<string:img_name>', methods=['GET'])
def get_images(img_name):
  imgname=img_name
  

  image=imgname
  file_name ="C:/Users/khale/Desktop/test/souqZone/A/content/images/"+image
  model_file = "C:/Users/khale/Desktop/test/souqZone/A/tensorflow-for-poets-2/tf_files/retrained_graph.pb"
  label_file = "C:/Users/khale/Desktop/test/souqZone/A/tensorflow-for-poets-2/tf_files/retrained_labels.txt"
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
  return jsonify({'name': img_name})

  #print('\nEvaluation time (1-image): {:.3f}s\n'.format(end-start))
  #template = "{} (score={:0.5f})"
  #for i in top_k:
  #  print(template.format(labels[i], results[i]))
  f="{:0.5f}"
  a=float(f.format(results[0]))
  b=float(f.format(results[1]))
  c=float(f.format(results[2]))
#print(a)
#print(b)
#print(c)
  max=max(a,b,c)
  category_name=""
  if max==a:
    print(labels[0])
    category_name=labels[0]
  elif max==b:
    print(labels[1])
    category_name=labels[1]
  else:
    print(labels[2])
    category_name=labels[2]

  print(file_name , category_name)
  return jsonify({'result': category_name})


if __name__ == "__main__":
  
  app.run(debug=True)
#file_name = "F:/project/ConsoleApplication2/tensorflow-for-poets-2/tf_files/v.jpg"
  image=imgname
  file_name ="C:/Users/khale/Desktop/test/souqZone/A/content/images/"+image
#get_match(file_name , category_name)

#-----------------------------------------------------------------------------------------------
import cv2
import numpy as np
import pyodbc 
server = 'tcp:souqzone.database.windows.net' 
database = 'SouqZone' 
username = 'souqzone_admin' 
password = 'Az@00000' 
cnxn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER='+server+';DATABASE='+database+';UID='+username+';PWD='+ password)
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

    
    
     
   # print(list_name)
    path="C:/Users/khale/Desktop/test/souqZone/A/content/images/"
    




    #l = [path+"1.jpg",path+"2.jpg",path+"3.jpg",path+"4.png",path+"5.jpg",path+"6.jpg",path+"7.jpg",path+"8.png",path+"9.jpg",path+"10.jpg",path+"11.jpg",path+"12.jpg",path+"13.jpg",path+"14.jpg",path+"15.jpg",path+"56.jpg",path+"57.jpg",path+"58.jpg",path+"59.jpg",path+"60.jpg",path+"61.jpg",path+"62.jpg",path+"63.jpg"]
    #l2 = [path+"16.jpg",path+"17.jpg",path+"18.png",path+"19.jpg",path+"20.jpg",path+"21.jpg",path+"22.jpg",path+"23.jpg",path+"24.jpg",path+"25.jpg",path+"26.jpg",path+"27.jpg",path+"28.jpg",path+"29.jpg",path+"30.png"]
    #l3 = [path+"32.jpg",path+"34.jpg",path+"36.jpg",path+"37.jpg",path+"38.jpg",path+"39.jpg",path+"40.jpg",path+"42.png",path+"43.png",path+"44.png",path+"45.jpg",path+"47.jpg",path+"48.jpg",path+"50.jpg",path+"55.jpg"]
    #l=["F:/4'th year/pattern recognition/project_G/birds/1.jpg","F:/4'th year/pattern recognition/project_G/birds/2.jpg","F:/4'th year/pattern recognition/project_G/birds/3.jpg","F:/4'th year/pattern recognition/project_G/birds/4.jpg","F:/4'th year/pattern recognition/project_G/birds/5.jpg","F:/4'th year/pattern recognition/project_G/birds/6.jpg","F:/4'th year/pattern recognition/project_G/birds/7.jpg","F:/4'th year/pattern recognition/project_G/birds/8.jpg","F:/4'th year/pattern recognition/project_G/birds/9.jpg","F:/4'th year/pattern recognition/project_G/birds/10.jpg","F:/4'th year/pattern recognition/project_G/birds/11.jpg","F:/4'th year/pattern recognition/project_G/birds/12.jpg","F:/4'th year/pattern recognition/project_G/birds/13.jpg","F:/4'th year/pattern recognition/project_G/birds/14.jpg","F:/4'th year/pattern recognition/project_G/birds/15.jpg","F:/4'th year/pattern recognition/project_G/birds/16.jpg","F:/4'th year/pattern recognition/project_G/birds/16.jpg","F:/4'th year/pattern recognition/project_G/birds/17.jpg"]
    orb = cv2.ORB_create()
    kp1, des1 = orb.detectAndCompute(img_resize, None)
    #print(des1)
    i=0
    sum=0
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
      if((len(matches)*100)/len(kp1)>=50):
        similarity.append(LIST[i])

      print((len(matches)*100)/len(kp1),'%',LIST[i])
      sum=sum+len(matches)
      
      #image_name.append(LIST[i])
      #percent.append((len(matches)*100)/len(kp1))

      matching_result = cv2.drawMatches(img_resize, kp1, img_resize2, kp2, matches, None, flags=2)
      i=i+1


        
    





   

    #avg=sum/len(similarity)
    #print(avg)
    #print('-------------------------------')
    #result=[]
    #i=0
    
    #while i<len(similarity)-1:
    #  if similarity[i]> avg:
    #    print(image_name[i],LIST.index(image_name[i]))
    #    result.append(LIST.index(image_name[i]))
    #    i=i+1
        
    print("------by percent---------")


    file_name = r'C:\Users\khale\Desktop\test\souqZone\A\images.txt'
    with open(file_name, "r") as f:

        file_data = f.read()
        raw = open(file_name, "w")

    for i in similarity:
      print(i)
      with open(file_name, 'a') as x_file:
          x_file.writelines(i+'\n')
      

       

    #j=0
    
    #while j<len(similarity):
     # if percent[j]>=50:
      #  print(image_name[j],LIST.index(image_name[j]))
       # result.append(LIST.index(image_name[j]))
        #with open(file_name, 'a') as x_file:
         # x_file.writelines(image_name[j]+'\n')
      #j=j+1
    
    
    

    

    


    

get_match(file_name , category_name)







