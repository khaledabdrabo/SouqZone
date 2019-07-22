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


if __name__ == "__main__":
  image=sys.argv[1]
  file_name ="C:/Users/khale/Desktop/test/souqZone/A/content/images/"+image
  model_file = "C:/Users/khale/Desktop/test/souqZone/A/tensorflow-for-poets-2/tf_files/retrained_graph.pb"
  label_file = "C:/Users/khale/Desktop/test/souqZone/A/tensorflow-for-poets-2/tf_files/retrained_labels.txt"
  input_height = 224
  input_width = 224
  input_mean = 128
  input_std = 128
  input_layer = "input"
  output_layer = "final_result"

  #parser = argparse.ArgumentParser()
  #parser.add_argument("--image", help="image to be processed")
  #parser.add_argument("--graph", help="graph/model to be executed")
  #parser.add_argument("--labels", help="name of file containing labels")
  #parser.add_argument("--input_height", type=int, help="input height")
  #parser.add_argument("--input_width", type=int, help="input width")
  #parser.add_argument("--input_mean", type=int, help="input mean")
  #parser.add_argument("--input_std", type=int, help="input std")
  #parser.add_argument("--input_layer", help="name of input layer")
  #parser.add_argument("--output_layer", help="name of output layer")
  #args = parser.parse_args()

  #if args.graph:
  #  model_file = args.graph
  #if args.image:
  #  file_name = args.image
  #if args.labels:
  #  label_file = args.labels
  #if args.input_height:
  #  input_height = args.input_height
  #if args.input_width:
  #  input_width = args.input_width
  #if args.input_mean:
  #  input_mean = args.input_mean
  #if args.input_std:
  #  input_std = args.input_std
  #if args.input_layer:
  #  input_layer = args.input_layer
  #if args.output_layer:
  #  output_layer = args.output_layer

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
#file_name = "F:/project/ConsoleApplication2/tensorflow-for-poets-2/tf_files/v.jpg"
image=sys.argv[1]
file_name ="C:/Users/khale/Desktop/test/souqZone/A/content/images/"+image
#get_match(file_name , category_name)

#-----------------------------------------------------------------------------------------------
import cv2
import numpy as np


def get_match(imagePath,category_name):
    
    img1 = cv2.imread(imagePath, cv2.IMREAD_GRAYSCALE)
    img_resize=cv2.resize(img1,(256,197))
    
    
    cat=category_name
    list_name=""
    if cat=="watch":
        list_name="l"
    elif cat=="bag":
        list_name="l2"
    else:
        list_name="l3"
   # print(list_name)
    path="C:/Users/khale/Desktop/test/souqZone/A/content/images/"




    l = [path+"1.jpg",path+"2.jpg",path+"3.jpg",path+"4.png",path+"5.jpg",path+"6.jpg",path+"7.jpg",path+"8.png",path+"9.jpg",path+"10.jpg",path+"11.jpg",path+"12.jpg",path+"13.jpg",path+"14.jpg",path+"15.jpg",path+"56.jpg",path+"57.jpg",path+"58.jpg",path+"59.jpg",path+"60.jpg",path+"61.jpg",path+"62.jpg",path+"63.jpg"]
    l2 = [path+"16.jpg",path+"17.jpg",path+"18.png",path+"19.jpg",path+"20.jpg",path+"21.jpg",path+"22.jpg",path+"23.jpg",path+"24.jpg",path+"25.jpg",path+"26.jpg",path+"27.jpg",path+"28.jpg",path+"29.jpg",path+"30.png"]
    l3 = [path+"32.jpg",path+"34.jpg",path+"36.jpg",path+"37.jpg",path+"38.jpg",path+"39.jpg",path+"40.jpg",path+"42.png",path+"43.png",path+"44.png",path+"45.jpg",path+"47.jpg",path+"48.jpg",path+"50.jpg",path+"55.jpg"]
    #l=["F:/4'th year/pattern recognition/project_G/birds/1.jpg","F:/4'th year/pattern recognition/project_G/birds/2.jpg","F:/4'th year/pattern recognition/project_G/birds/3.jpg","F:/4'th year/pattern recognition/project_G/birds/4.jpg","F:/4'th year/pattern recognition/project_G/birds/5.jpg","F:/4'th year/pattern recognition/project_G/birds/6.jpg","F:/4'th year/pattern recognition/project_G/birds/7.jpg","F:/4'th year/pattern recognition/project_G/birds/8.jpg","F:/4'th year/pattern recognition/project_G/birds/9.jpg","F:/4'th year/pattern recognition/project_G/birds/10.jpg","F:/4'th year/pattern recognition/project_G/birds/11.jpg","F:/4'th year/pattern recognition/project_G/birds/12.jpg","F:/4'th year/pattern recognition/project_G/birds/13.jpg","F:/4'th year/pattern recognition/project_G/birds/14.jpg","F:/4'th year/pattern recognition/project_G/birds/15.jpg","F:/4'th year/pattern recognition/project_G/birds/16.jpg","F:/4'th year/pattern recognition/project_G/birds/16.jpg","F:/4'th year/pattern recognition/project_G/birds/17.jpg"]
    orb = cv2.ORB_create()
    kp1, des1 = orb.detectAndCompute(img_resize, None)
    #print(des1)
    i=0
    sum=0
    similarity=[]
    image_name=[]
    percent=[]
    if(list_name=="l"):


        while i<len(l):

            img2 = cv2.imread(l[i], cv2.IMREAD_GRAYSCALE)
            img_resize2 = cv2.resize(img2, (256, 197))
            kp2, des2 = orb.detectAndCompute(img_resize2, None)
        #cv2.imshow(l[i], img_resize2)
        # Brute Force Matching
            bf = cv2.BFMatcher(cv2.NORM_HAMMING, crossCheck=True)
            matches = bf.match(des1, des2)
            matches = sorted(matches, key=lambda x: x.distance)

            #print((len(matches)*100)/len(kp1),'%',l[i])
            sum=sum+len(matches)
            similarity.append(len(matches))
            image_name.append(l[i])
            percent.append((len(matches)*100)/len(kp1))

            matching_result = cv2.drawMatches(img_resize, kp1, img_resize2, kp2, matches, None, flags=2)
            i=i+1


        
    elif list_name=="l2":


        while i<len(l2):

            img2 = cv2.imread(l2[i], cv2.IMREAD_GRAYSCALE)
            img_resize2 = cv2.resize(img2, (256, 197))
            kp2, des2 = orb.detectAndCompute(img_resize2, None)
        #cv2.imshow(l[i], img_resize2)
        # Brute Force Matching
            bf = cv2.BFMatcher(cv2.NORM_HAMMING, crossCheck=True)
            matches = bf.match(des1, des2)
            matches = sorted(matches, key=lambda x: x.distance)
            #print((len(matches)*100)/len(kp1),'%',l2[i])
            sum=sum+len(matches)
            similarity.append(len(matches))
            image_name.append(l2[i])
            percent.append((len(matches)*100)/len(kp1))

            matching_result = cv2.drawMatches(img_resize, kp1, img_resize2, kp2, matches, None, flags=2)

            i=i+1
    else:

        while i<len(l3):


            img2 = cv2.imread(l3[i], cv2.IMREAD_GRAYSCALE)
            img_resize2 = cv2.resize(img2, (256, 197))
            kp2, des2 = orb.detectAndCompute(img_resize2, None)
        #cv2.imshow(l[i], img_resize2)
        # Brute Force Matching
            bf = cv2.BFMatcher(cv2.NORM_HAMMING, crossCheck=True)
            matches = bf.match(des1, des2)
            matches = sorted(matches, key=lambda x: x.distance)
            #print((len(matches)*100)/len(kp1),'%',l3[i])
            sum=sum+len(matches)
            similarity.append(len(matches))
            image_name.append(l3[i])
            percent.append((len(matches)*100)/len(kp1))
            matching_result = cv2.drawMatches(img_resize, kp1, img_resize2, kp2, matches, None, flags=2)
            i=i+1
        







   

    avg=sum/len(similarity)
    print(avg)
    print('-------------------------------')
    result=[]
    i=0
    if list_name=="l":
        while i<len(similarity)-1:


            if similarity[i]> avg:


                print(image_name[i],l.index(image_name[i]))
                result.append(l.index(image_name[i]))
            i=i+1
        #cv2.waitKey(0)
    elif list_name=="l2":
        while i<len(similarity)-1:

             
            if similarity[i]> avg:

                

                print(image_name[i],l2.index(image_name[i]))
                result.append(l2.index(image_name[i]))
            i=i+1
        #cv2.waitKey(0)
    else:
        while i<len(similarity)-1:

             
            if similarity[i]> avg:

                

                print(image_name[i],l3.index(image_name[i]))
                result.append(l3.index(image_name[i]))
            i=i+1
        #cv2.waitKey(0)
    print("------by percent---------")


    file_name = r'C:\Users\khale\Desktop\test\souqZone\A\images.txt'
    with open(file_name, "r") as f:

        file_data = f.read()
        raw = open(file_name, "w")

       

    j=0
    if list_name=="l":
        while j<len(percent):
            if percent[j]>50:
                print(image_name[j],l.index(image_name[j]))
                result.append(l.index(image_name[j]))
                with open(file_name, 'a') as x_file:
                    x_file.writelines(image_name[j]+'\n')
            j=j+1
    elif list_name=="l2":

        while j<len(percent):
            if percent[j]>50:
                print(image_name[j],l2.index(image_name[j]))
                result.append(l2.index(image_name[j]))
                with open(file_name, 'a') as x_file:
                    x_file.writelines(image_name[j]+'\n')
            j=j+1
    elif list_name=="l3":

        while j<len(percent):
            if percent[j]>50:
                print(image_name[j],l3.index(image_name[j]))
                result.append(l3.index(image_name[j]))
                with open(file_name, 'a') as x_file:
                    x_file.writelines(image_name[j]+'\n')
            j=j+1


        

    #print("---------------")
   
    #    cv2.waitKey(0)
    #elif list_name=="l2":
    #    while percent[i]>50:
    #        print(image_name[i],l2.index(image_name[i]))
    #        result.append(l2.index(image_name[i]))
    #        i=i+1
   
    #    cv2.waitKey(0)
    #else:
    #    while percent[i]>50:
    #        print(image_name[i],l3.index(image_name[i]))
    #        result.append(l3.index(image_name[i]))
    #        i=i+1
   
    #    cv2.waitKey(0)
    #for i in percent:
    #    print(i)



    #cv2.imshow("image",img_resize)
    #cv2.waitKey(0)
    
    
    

    

    


    

get_match(file_name , category_name)







