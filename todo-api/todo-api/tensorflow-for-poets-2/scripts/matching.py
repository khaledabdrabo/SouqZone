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
    path="~/content/images/"




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
    #print(avg)
    #print('-------------------------------')
    result=[]
    i=0
    if list_name=="l":
        while i<len(similarity)-1:


            if similarity[i]> avg:


                #print(image_name[i],l.index(image_name[i]))
                result.append(l.index(image_name[i]))
            i=i+1
        #cv2.waitKey(0)
    elif list_name=="l2":
        while i<len(similarity)-1:

             
            if similarity[i]> avg:

                

                #print(image_name[i],l2.index(image_name[i]))
                result.append(l2.index(image_name[i]))
            i=i+1
        #cv2.waitKey(0)
    else:
        while i<len(similarity)-1:

             
            if similarity[i]> avg:

                

                #print(image_name[i],l3.index(image_name[i]))
                result.append(l3.index(image_name[i]))
            i=i+1
        #cv2.waitKey(0)
   # print("------by percent---------")


    file_name = r'C:\Users\khale\Desktop\test\souqZone\A\images.txt'
    with open(file_name, "r") as f:

        file_data = f.read()
        raw = open(file_name, "w")

       

    j=0
    if list_name=="l":
        while j<len(percent):
            if percent[j]>50:
                #print(image_name[j],l.index(image_name[j]))
                result.append(l.index(image_name[j]))
                with open(file_name, 'a') as x_file:
                    x_file.writelines(image_name[j]+'\n')
            j=j+1
    elif list_name=="l2":

        while j<len(percent):
            if percent[j]>50:
                #print(image_name[j],l2.index(image_name[j]))
                result.append(l2.index(image_name[j]))
                with open(file_name, 'a') as x_file:
                    x_file.writelines(image_name[j]+'\n')
            j=j+1
    elif list_name=="l3":

        while j<len(percent):
            if percent[j]>50:
                #print(image_name[j],l3.index(image_name[j]))
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
    
    
    

    

    


    
if __name__ == "__main__":
    get_match(SystemError.args[0],SystemError.args[1])






